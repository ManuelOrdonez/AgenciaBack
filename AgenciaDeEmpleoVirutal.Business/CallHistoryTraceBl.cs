namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Call History Trace Business logic
    /// </summary>
    public class CallHistoryTraceBl : BusinessBase<CallHistoryTrace>, ICallHistoryTrace
    {
        /// <summary>
        /// Busy Agents Reposotory
        /// </summary>
        private readonly IGenericRep<BusyAgent> _busyAgentRepository;

        /// <summary>
        /// Call History Trace Repository
        /// </summary>
        private readonly IGenericRep<CallHistoryTrace> _callHistoryRepository;

        /// <summary>
        /// Users Repository
        /// </summary>
        private readonly IGenericRep<User> _callerRepository;

        /// <summary>
        /// Agents Repository
        /// </summary>
        private readonly IGenericRep<User> _agentRepository;

        /// <summary>
        /// The open tok service
        /// </summary>
        private readonly IOpenTokExternalService _openTokService;

        /// <summary>
        /// The user secret settings
        /// </summary>
        private readonly UserSecretSettings _UserSecretSettings;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="callHistoryRepository"></param>
        /// <param name="agentRepository"></param>
        /// <param name="busyAgentRepository"></param>
        public CallHistoryTraceBl(IGenericRep<CallHistoryTrace> callHistoryRepository,
            IGenericRep<User> agentRepository, IGenericRep<BusyAgent> busyAgentRepository,
            IOpenTokExternalService openTokService, IOptions<UserSecretSettings> options)
        {
            if (options != null)
            {
                _callHistoryRepository = callHistoryRepository;
                _agentRepository = agentRepository;
                _callerRepository = agentRepository;
                _busyAgentRepository = busyAgentRepository;
                _openTokService = openTokService;
                _UserSecretSettings = options.Value;
            }
        }

        /// <summary>
        /// Method to Get Call Info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<CallHistoryTrace> GetCallInfo(GetCallRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", nameof(request));
            }
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(errorMessages);
            }

            var parameters = new List<ConditionParameter>
            {
                new ConditionParameter{ColumnName="PartitionKey", Condition = "eq" ,Value = request.OpenTokSessionId.ToLower(new CultureInfo("es-CO")) },
                new ConditionParameter{ColumnName="State", Condition = "eq" ,Value= request.State }
            };
            var call = _callHistoryRepository.GetSomeAsync(parameters).Result?
                .OrderByDescending(t => t.DateCall).FirstOrDefault();
            return ResponseSuccess(new List<CallHistoryTrace>
            {
                call == null || string.IsNullOrWhiteSpace(call.OpenTokSessionId) ? null : call
            });
        }

        /// <summary>
        /// Method to Get All Calls Not Managed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<List<CallHistoryTrace>> GetAllCallsNotManaged(GetCallRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", nameof(request));
            }
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<List<CallHistoryTrace>>(errorMessages);
            }
            var parameters = new List<ConditionParameter> {
                    new ConditionParameter{ColumnName="PartitionKey", Condition = "eq" ,Value = request.OpenTokSessionId.ToLower(new CultureInfo("es-CO")) },
                    new ConditionParameter{ColumnName="State", Condition = "ne", Value = request.State }
                };
            var call = _callHistoryRepository.GetSomeAsync(parameters).Result;
            if (call is null)
            {
                return ResponseFail<List<CallHistoryTrace>>();
            }
            return ResponseSuccess(new List<List<CallHistoryTrace>> { call });
        }

        /// <summary>
        /// Method to Call Quality
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<List<CallHistoryTrace>> CallQuality(QualityCallRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", nameof(request));
            }
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseFail<List<CallHistoryTrace>>(ServiceResponseCode.BadRequest);
            }
            var callTrace = _callHistoryRepository.GetByPartitionKeyAndRowKeyAsync(request.SessionId, request.TokenId).Result;
            if (callTrace.Count == 0)
            {
                return ResponseFail<List<CallHistoryTrace>>();
            }

            callTrace.First().Score = request.Score;
            if (callTrace.First().State == CallStates.Lost.ToString())
            {
                callTrace.First().Score = 0;
            }

            if (!_callHistoryRepository.AddOrUpdate(callTrace.First()).Result)
            {
                return ResponseFail<List<CallHistoryTrace>>();
            }
            return ResponseSuccess(new List<List<CallHistoryTrace>>());
        }

        /// <summary>
        /// Register call date and  trace 
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        public Response<CallHistoryTrace> SetCallTrace(SetCallTraceRequest callRequest)
        {
            if (callRequest == null)
            {
                throw new ArgumentNullException("callRequest", nameof(callRequest));
            }
            var messagesValidationEntity = callRequest.Validate().ToList();
            var stateInput = (CallStates)callRequest.State;

            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(messagesValidationEntity);
            }

            var existsCall = GetCallInfo(new GetCallRequest
            {
                OpenTokSessionId = callRequest?.OpenTokSessionId,
                State = CallStates.Begun.ToString()
            }).Data.FirstOrDefault();

            var callInfo = existsCall == null || string.IsNullOrWhiteSpace(existsCall.UserCall) ?
                GetDefaultCallHistoryTrace(callRequest) : existsCall;

            var agent = _agentRepository.GetAsync(callRequest.UserName).Result;

            if (agent?.OpenTokSessionId == null)
            {
                agent = null;
            }
            switch (stateInput)
            {
                case CallStates.Begun:
                    callInfo.DateCall = DateTime.Now;
                    callInfo.UserCall = callRequest.UserName;
                    callInfo.State = stateInput.ToString();
                    callInfo.CallType = callRequest.CallType;
                    break;
                case CallStates.Answered:
                    var recordId = _openTokService.StartRecord(callRequest.OpenTokSessionId, callRequest.UserName);
                    callInfo.DateAnswerCall = DateTime.Now;
                    callInfo.UserAnswerCall = callRequest.UserName;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    callInfo.RecordId = recordId;
                    if (agent != null)
                    {
                        agent.Available = false;
                        agent.CountCallAttended = Int32.Parse(agent.CountCallAttended.ToString(new CultureInfo("es-CO")), new CultureInfo("es-CO")) + 1;
                        _agentRepository.AddOrUpdate(agent);
                    }
                    break;
                case CallStates.EndByWeb:
                    callInfo = this.CallEnded(CallStates.EndByWeb, callInfo, callRequest, stateInput);
                    if (agent != null)
                    {
                        agent.Available = false;
                        if (!_agentRepository.AddOrUpdate(agent).Result)
                        {
                            return ResponseFail();
                        }
                    }
                    this.Aviable(callRequest.UserName);
                    break;
                case CallStates.EndByMobile:
                    callInfo = this.CallEnded(CallStates.EndByMobile, callInfo, callRequest, stateInput);
                    if (agent != null)
                    {
                        agent.Available = false;
                        this.Aviable(agent.UserName);
                        if (!_agentRepository.AddOrUpdate(agent).Result)
                        {
                            return ResponseFail();
                        }
                    }
                    break;
                case CallStates.Managed:
                    callInfo.DateFinishCall = DateTime.Now;
                    callInfo.Observations = callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    callInfo.CallType = callRequest.CallType;
                    callInfo.UserAnswerCall = callRequest.UserName;
                    break;
                default:
                    callInfo.State = CallStates.Unknown.ToString();
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    break;
            }
            if (callInfo.Trace != "Logout" && !_callHistoryRepository.AddOrUpdate(callInfo).Result)
            {
                return ResponseFail();
            }
            return ResponseSuccess();
        }

        /// <summary>
        /// Method to trace Call Ended
        /// </summary>
        /// <param name="TypeCall"></param>
        /// <param name="callInfo"></param>
        /// <param name="callRequest"></param>
        /// <param name="stateInput"></param>
        /// <returns></returns>
        private CallHistoryTrace CallEnded(CallStates TypeCall, CallHistoryTrace callInfo, SetCallTraceRequest callRequest, CallStates stateInput)
        {
            if (callInfo.State != TypeCall.ToString())
            {
                callInfo.DateFinishCall = DateTime.Now;
                callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                try
                {
                    _openTokService.StopRecord(callInfo.RecordId);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            callInfo.State = callInfo.State != (CallStates.Answered.ToString()) ?
                           CallStates.Lost.ToString() : stateInput.ToString();
            return callInfo;
        }

        /// <summary>
        /// Methos to vacate agent
        /// </summary>
        /// <param name="UserName"></param>
        private void Aviable(string UserName)
        {
            string type = string.Empty;
            var user = _agentRepository.GetAsync(UserName).Result;
            type = user.UserType.ToLower(new CultureInfo("es-CO")).Equals(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO")), StringComparison.CurrentCulture)
                ? "UserNameAgent" : "UserNameCaller";

            var busy = _busyAgentRepository.GetSomeAsync(type, UserName).Result;
            if (busy.Any())
            {
                _busyAgentRepository.DeleteRowAsync(busy.FirstOrDefault());
            }
        }

        /// <summary>
        /// Method to Get Default Call History Trace
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        private CallHistoryTrace GetDefaultCallHistoryTrace(SetCallTraceRequest callRequest)
        {
            var call = GetCallForAnyManage(callRequest.OpenTokSessionId,
                    callRequest.OpenTokAccessToken);
            if (call != null)
            {
                return call;
            }
            var stateInput = (CallStates)callRequest.State;
            return new CallHistoryTrace
            {
                Trace = callRequest.Trace,
                State = stateInput.ToString(),
                PartitionKey = callRequest.OpenTokSessionId,
                RowKey = callRequest.OpenTokAccessToken,
                DateAnswerCall = DateTime.Now,
                DateCall = DateTime.Now,
                UserCall = string.Empty,
                UserAnswerCall = string.Empty,
                DateFinishCall = DateTime.Now
            };
        }

        public Response<ResponseUrlRecord> GetRecordUrl(string RecordId)
        {
            var response = new List<ResponseUrlRecord>();

            var blobContainer = _UserSecretSettings.BlobContainer;
            var openTokApiKey = _UserSecretSettings.OpenTokApiKey;

            response.Add(new ResponseUrlRecord
            {
                URL = this.GetContainerSasUri(blobContainer, openTokApiKey + "/" + RecordId + "/archive.zip")

            });
            return ResponseSuccess(response);

        }

        /// <summary>
        /// Method to Get All User Call
        /// </summary>
        /// <param name="getAllUserCallRequest"></param>
        /// <returns></returns>
        public Response<GetAllUserCallResponse> GetAllUserCall(GetAllUserCallRequest getAllUserCallRequest)
        {
            var response = new List<GetAllUserCallResponse>();
            string type = string.Empty;
            type = getAllUserCallRequest.UserType.ToLower(new CultureInfo("es-CO")).Equals(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO")), StringComparison.CurrentCulture)
                ? "UserAnswerCall" : "UserCall";

            var query = new List<ConditionParameter>();
            query = !string.IsNullOrEmpty(getAllUserCallRequest.UserName) ?
                new List<ConditionParameter>
                {
                     new ConditionParameter
                     {
                         ColumnName = "DateCall",
                         Condition = QueryComparisons.GreaterThanOrEqual,
                         ValueDateTime = getAllUserCallRequest.StartDate
                     },

                      new ConditionParameter
                     {
                         ColumnName = "DateCall",
                         Condition = QueryComparisons.LessThan,
                         ValueDateTime = getAllUserCallRequest.EndDate.AddDays(1)
                     },
                      new ConditionParameter
                      {
                         ColumnName = type,
                         Condition = QueryComparisons.Equal,
                         Value = getAllUserCallRequest.UserName
                      }
                } :
                new List<ConditionParameter>
                 {
                     new ConditionParameter
                     {
                         ColumnName = "DateCall",
                         Condition = QueryComparisons.GreaterThanOrEqual,
                         ValueDateTime = getAllUserCallRequest.StartDate
                     },

                      new ConditionParameter
                     {
                         ColumnName = "DateCall",
                         Condition = QueryComparisons.LessThan,
                         ValueDateTime = getAllUserCallRequest.EndDate.AddDays(1)
                     },
                 };

            var calls = _callHistoryRepository.GetListQuery(query).Result;


            if (calls.Count == 0 || calls is null)
            {
                return ResponseFail<GetAllUserCallResponse>(ServiceResponseCode.UserDoNotHaveCalls);
            }

            List<CallHistoryTrace> callsList = new List<CallHistoryTrace>();

            foreach (var cll in calls.OrderByDescending(cll => cll.Timestamp).ToList())
            {
                cll.RecordUrl = cll.RecordId;

                if (!string.IsNullOrEmpty(cll.UserAnswerCall))
                {
                    var agentInfo = _agentRepository.GetByPartitionKeyAndRowKeyAsync(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO")),
                        cll.UserAnswerCall.ToLower(new CultureInfo("es-CO"))).Result.First();
                    if (agentInfo is null || string.IsNullOrEmpty(agentInfo.Name))
                    {
                        cll.AgentName = string.Empty;
                    }
                    else
                    {
                        cll.AgentName = agentInfo.Name + " " + agentInfo.LastName;
                    }
                }
                else
                {
                    cll.AgentName = string.Empty;
                }
                string typeU = string.Empty;


                if (!string.IsNullOrEmpty(cll.UserCall))
                {
                    typeU = cll.UserCall.Substring(cll.UserCall.Length - 1) == "1" ?
                        UsersTypes.Empresa.ToString().ToLower(new CultureInfo("es-CO")) : UsersTypes.Cesante.ToString().ToLower(new CultureInfo("es-CO"));

                    var callerInfo = _agentRepository.GetByPartitionKeyAndRowKeyAsync(typeU, cll.UserCall.ToLower(new CultureInfo("es-CO"))).Result.First();
                    if (callerInfo is null || string.IsNullOrEmpty(callerInfo.Name))
                    {
                        continue;
                    }
                    else
                    {
                        cll.CallerName = typeU != UsersTypes.Empresa.ToString().ToLower(new CultureInfo("es-CO")) ?
                            callerInfo.Name + " " + callerInfo.LastName : callerInfo.ContactName + "-" + callerInfo.SocialReason;
                    }
                }

                cll.Minutes = (cll.DateFinishCall - cll.DateAnswerCall);
                /*  if ((string.IsNullOrEmpty(cll.UserAnswerCall) || string.IsNullOrEmpty(cll.UserCall) ) && cll.State == CallStates.Lost.ToString())
                  {
                      continue;
                  }
                  else
                  {
                      callsList.Add(cll);
                  }*/
                callsList.Add(cll);

            }

            response.Add(new GetAllUserCallResponse
            {
                CallInfo = callsList,
            });
            return ResponseSuccess(response);
        }


        /// <summary>
        /// Get secure Url Blob
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public string GetContainerSasUri(string containerName, string BlobName)
        {
            var StorageConnectionString = _UserSecretSettings.TableStorage;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(BlobName);


            //Set the expiry time and permissions for the blob.
            //In this case, the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }

        /// <summary>
        /// Method to Get Call For Any Manage
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <param name="OpenTokAccessToken"></param>
        /// <returns></returns>
        private CallHistoryTrace GetCallForAnyManage(string OpenTokSessionId, string OpenTokAccessToken)
        {
            return _callHistoryRepository.GetByPartitionKeyAndRowKeyAsync(OpenTokSessionId, OpenTokAccessToken).Result
                .OrderByDescending(t => t.DateCall).FirstOrDefault();
        }

        /// <summary>
        /// Method to Get Caller Info
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <returns></returns>
        public Response<CallerInfoResponse> GetCallerInfo(string OpenTokSessionId)
        {
            if (string.IsNullOrEmpty(OpenTokSessionId))
            {
                return ResponseFail<CallerInfoResponse>(ServiceResponseCode.BadRequest);
            }
            else
            {
                GetCallRequest getCallReq = new GetCallRequest
                {
                    OpenTokSessionId = OpenTokSessionId,
                    State = CallStates.Begun.ToString(),
                };
                var callInfo = GetCallInfo(getCallReq).Data.First();
                var caller = _callerRepository.GetAsync(callInfo.UserCall).Result;
                if (caller is null)
                {
                    return ResponseFail<CallerInfoResponse>();
                }
                CallerInfoResponse response = new CallerInfoResponse
                {
                    Caller = caller,
                    OpenTokAccessToken = callInfo?.OpenTokAccessToken,
                    CallInfo = callInfo
                };
                return ResponseSuccess(new List<CallerInfoResponse> { response });
            }
        }
    }
}
