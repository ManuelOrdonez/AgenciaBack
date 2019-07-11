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
        /// ReportCall Repository
        /// </summary>
        private readonly IGenericRep<ReportCall> _reportCaller;

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
            IOpenTokExternalService openTokService, IOptions<UserSecretSettings> options,
            IGenericRep<ReportCall> reportCallRepository)
        {
            if (options != null)
            {
                _callHistoryRepository = callHistoryRepository;
                _agentRepository = agentRepository;
                _callerRepository = agentRepository;
                _busyAgentRepository = busyAgentRepository;
                _openTokService = openTokService;
                _UserSecretSettings = options.Value;
                _reportCaller = reportCallRepository;
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
                throw new ArgumentNullException(nameof(request));
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
                throw new ArgumentNullException(nameof(request));
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
                throw new ArgumentNullException(nameof(request));
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

            if (!SetUpdateCall(callTrace.First()))
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
                throw new ArgumentNullException(nameof(callRequest));
            }

            var messagesValidationEntity = callRequest.Validate().ToList();
            var stateInput = (CallStates)callRequest.State;

            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(messagesValidationEntity);
            }

            var existsCall = GetCallInfo(new GetCallRequest
            {
                OpenTokSessionId = callRequest.OpenTokSessionId,
                State = CallStates.Begun.ToString()
            }).Data.FirstOrDefault();

            var callInfo = existsCall == null || string.IsNullOrEmpty(existsCall.UserCall) ?
                GetDefaultCallHistoryTrace(callRequest) : existsCall;

            var agent = _agentRepository.GetAsync(callRequest.UserName).Result;

            if (agent?.OpenTokSessionId == null)
            {
                agent = null;
            }

            bool validateAgent = true;
            SetCallState(callRequest, stateInput, ref callInfo, agent, ref validateAgent);
            if ((callInfo.Trace != "Logout" || !validateAgent) &&
                !string.IsNullOrEmpty(callInfo.UserCall) && !SetUpdateCall(callInfo))
            {
                return ResponseFail();
            }

            return ResponseSuccess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callRequest"></param>
        /// <param name="stateInput"></param>
        /// <param name="callInfo"></param>
        /// <param name="agent"></param>
        /// <param name="validateAgent"></param>
        private void SetCallState(SetCallTraceRequest callRequest, CallStates stateInput,
            ref CallHistoryTrace callInfo, User agent, ref bool validateAgent)
        {
            switch (stateInput)
            {
                case CallStates.Begun:
                    if (agent == null)
                    {
                        callInfo.DateCall = DateTime.Now;
                        callInfo.UserCall = callRequest.UserName;
                        var user = UserName(callRequest.UserName);
                        callInfo.CallerName = user.CallerName;
                        callInfo.CallerPhone = user.CallerPhone;
                    }
                    else
                    {
                        callInfo.UserAnswerCall = callRequest.UserName;
                    }

                    callInfo.State = stateInput.ToString();
                    callInfo.Trace = string.IsNullOrEmpty(callInfo.Trace) ? callRequest.Trace :
                        callInfo.Trace + " - " + callRequest.Trace;

                    callInfo.CallType = callRequest.CallType;

                    break;
                case CallStates.Answered:
                    var recordId = _openTokService.StartRecord(callRequest.OpenTokSessionId, callRequest.UserName);
                    callInfo.DateAnswerCall = DateTime.Now;
                    callInfo.UserAnswerCall = callRequest.UserName;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    callInfo.AgentName = GetAgentName(callInfo.UserAnswerCall);
                    callInfo.RecordId = recordId;
                    callInfo.RecordUrl = recordId;
                    validateAgent = this.ValidateCallAgent(agent, CallStates.Answered);
                    break;
                case CallStates.EndByWeb:
                    callInfo = this.CallEnded(CallStates.EndByWeb, callInfo, callRequest, stateInput);
                    validateAgent = this.ValidateCallAgent(agent, CallStates.EndByWeb);
                    this.Aviable(callRequest.UserName, validateAgent);
                    break;
                case CallStates.EndByMobile:
                    callInfo = this.CallEnded(CallStates.EndByMobile, callInfo, callRequest, stateInput);
                    validateAgent = this.ValidateCallAgent(agent, CallStates.EndByMobile);
                    break;
                case CallStates.Redirected:
                    callInfo = this.CallEnded(CallStates.Redirected, callInfo, callRequest, stateInput);
                    validateAgent = this.ValidateCallAgent(agent, CallStates.Redirected);
                    break;
                case CallStates.Managed:
                    callInfo.DateFinishCall = DateTime.Now;
                    callInfo.AgentName = GetAgentName(callInfo.UserAnswerCall);
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
        }


        /// <summary>
        /// Register report call
        /// </summary>
        /// <param name="call"></param>
        private bool SetUpdateCall(CallHistoryTrace call)
        {
            call.Minutes = call.DateAnswerCall != DateTime.MaxValue ?
                (call.DateFinishCall - call.DateAnswerCall) : TimeSpan.Zero;
            bool responseCall = _callHistoryRepository.AddOrUpdate(call).Result;
            var reportCall = _reportCaller.GetByPartitionKeyAndRowKeyAsync(
                call.DateCall.Date.ToString("yyyyMMdd"), call.RowKey).Result.FirstOrDefault();

            reportCall = reportCall == null || string.IsNullOrWhiteSpace(reportCall.DateFilter) ?
                SetReportCall(call) : reportCall;

            reportCall.AgentName = call.AgentName;
            reportCall.CallerName = call.CallerName;
            reportCall.CallerPhone = call.CallerPhone;
            reportCall.CallType = call.CallType;
            reportCall.DateAnswerCall = call.DateAnswerCall;
            reportCall.DateCall = call.DateCall;
            reportCall.DateFinishCall = call.DateFinishCall;
            reportCall.Minutes = call.Minutes;
            reportCall.Observations = call.Observations;
            reportCall.OpenTokAccessToken = call.OpenTokAccessToken;
            reportCall.OpenTokSessionId = call.OpenTokSessionId;
            reportCall.RecordId = call.RecordId;
            reportCall.RecordUrl = call.RecordUrl;
            reportCall.Score = call.Score;
            reportCall.State = call.State;
            reportCall.Trace = call.Trace;
            reportCall.UserAnswerCall = call.UserAnswerCall;
            reportCall.UserCall = call.UserCall;

            return responseCall && _reportCaller.AddOrUpdate(reportCall).Result;

        }

        /// <summary>
        /// new report call
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        private ReportCall SetReportCall(CallHistoryTrace call)
        {
            return new ReportCall()
            {
                PartitionKey = call.DateCall.Date.ToString("yyyyMMdd"),
                RowKey = call.OpenTokAccessToken,
                AgentName = call.AgentName,
                CallerName = call.CallerName,
                CallerPhone = call.CallerPhone,
                CallType = call.CallType,
                DateAnswerCall = call.DateAnswerCall,
                DateCall = call.DateCall,
                DateFinishCall = call.DateFinishCall,
                Minutes = call.Minutes,
                Observations = call.Observations,
                OpenTokAccessToken = call.OpenTokAccessToken,
                OpenTokSessionId = call.OpenTokSessionId,
                RecordId = call.RecordId,
                RecordUrl = call.RecordUrl,
                Score = call.Score,
                State = call.State,
                UserAnswerCall = call.UserAnswerCall,
                UserCall = call.UserCall,
            };
        }

        /// <summary>
        /// Validate Agent to answered call
        /// </summary>
        /// <param name="agent"></param>
        private bool ValidateCallAgent(User agent, CallStates state)
        {
            if (agent != null)
            {
                agent.Available = false;
                if (state == CallStates.Answered)
                {
                    agent.CountCallAttended = Int32.Parse(agent.CountCallAttended.ToString(new CultureInfo("es-CO")), new CultureInfo("es-CO")) + 1;
                }
                if (state == CallStates.EndByMobile)
                {
                    this.Aviable(agent.UserName, true);
                }
                if (!_agentRepository.AddOrUpdate(agent).Result)
                {
                    return false;
                }
            }
            return true;
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
                _openTokService.StopRecord(callInfo.RecordId);
            }
            if (callInfo.State != CallStates.Managed.ToString())
            {
                callInfo.State = callInfo.State != (CallStates.Answered.ToString()) &&
                    !(string.IsNullOrEmpty(callInfo.UserAnswerCall)) ?
                               CallStates.Lost.ToString() : stateInput.ToString();
            }
            if (callInfo.State == (CallStates.Lost.ToString()))
            {
                callInfo.AgentName = GetAgentName(callInfo.UserAnswerCall);
            }
            callInfo.Minutes = callInfo.DateAnswerCall != DateTime.MaxValue ?
                (callInfo.DateFinishCall - callInfo.DateAnswerCall) : TimeSpan.Zero;

            return callInfo;
        }

        /// <summary>
        /// Methos to vacate agent
        /// </summary>
        /// <param name="UserName"></param>
        private void Aviable(string UserName, bool Validate)
        {
            string type = string.Empty;
            if (Validate)
            {
                var user = _agentRepository.GetAsync(UserName).Result;
                type = user.UserType.ToLower(new CultureInfo("es-CO")).Equals(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO")), StringComparison.CurrentCulture)
                    ? "UserNameAgent" : "UserNameCaller";

                var busy = _busyAgentRepository.GetSomeAsync(type, UserName).Result;
                if (busy.Any())
                {
                    _busyAgentRepository.DeleteRowAsync(busy.FirstOrDefault());
                }
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
                Trace = string.Empty,
                State = stateInput.ToString(),
                PartitionKey = callRequest.OpenTokSessionId,
                RowKey = callRequest.OpenTokAccessToken,
                DateCall = DateTime.Now,
                UserCall = string.Empty,
                DateAnswerCall = DateTime.MaxValue,
                UserAnswerCall = string.Empty,
                DateFinishCall = DateTime.MaxValue
            };
        }

        public Response<ResponseUrlRecord> GetRecordUrl(string RecordId)
        {
            const string nameRecord = "archive.zip";
            var response = new List<ResponseUrlRecord>();

            var blobContainer = _UserSecretSettings.BlobContainer;
            var openTokApiKey = _UserSecretSettings.OpenTokApiKey;

            response.Add(new ResponseUrlRecord
            {
                URL = this.GetContainerSasUri(blobContainer, openTokApiKey + "/" + RecordId + "/" + nameRecord).ToString()

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
            string type = getAllUserCallRequest.UserType.ToLower(new CultureInfo("es-CO")).Equals(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO")), StringComparison.CurrentCulture)
                ? "UserAnswerCall" : "UserCall";

            var query = !string.IsNullOrEmpty(getAllUserCallRequest.UserName) ?
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


            if (calls.Count == 0)
            {
                return ResponseFail<GetAllUserCallResponse>(ServiceResponseCode.UserDoNotHaveCalls);
            }

            foreach (var call in calls)
            {
                call.Minutes = call.DateAnswerCall != DateTime.MaxValue ?
                    (call.DateFinishCall - call.DateAnswerCall) : TimeSpan.Zero;

                if (string.IsNullOrEmpty(call.CallerName))
                {
                    var user = UserName(call.UserCall);                
                    call.CallerName = user.CallerName;
                    call.CallerPhone = user.CallerPhone;
                    SetUpdateCall(call);
                }
            }

            response.Add(new GetAllUserCallResponse
            {
                CallInfo = calls,
            });
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Get all calls
        /// </summary>
        /// <param name="calls"></param>
        /// <returns></returns>
        private List<CallHistoryTrace> GetListAllCallUser(List<CallHistoryTrace> calls)
        {
            List<CallHistoryTrace> callsList = new List<CallHistoryTrace>();

            foreach (var cll in calls.OrderByDescending(cll => cll.Timestamp).ToList())
            {
                cll.RecordUrl = cll.RecordId;
                GetNameAgent(cll);

                if (!string.IsNullOrEmpty(cll.UserCall))
                {
                    var callAux = UserName(cll.UserCall);
                    cll.CallerName = callAux.CallerName;
                    cll.CallerPhone = callAux.CallerPhone;
                }

                cll.Minutes = cll.DateAnswerCall != DateTime.MaxValue ?
                    (cll.DateFinishCall - cll.DateAnswerCall) : TimeSpan.Zero;
                callsList.Add(cll);
            }
            return callsList;
        }

        private CallHistoryTrace UserName(string userCall)
        {
            CallHistoryTrace call = new CallHistoryTrace();
            string typeU = string.Empty;

            typeU = userCall.Substring(userCall.Length - 1) == "1" ?
                  UsersTypes.Empresa.ToString().ToLower(new CultureInfo("es-CO")) : UsersTypes.Cesante.ToString().ToLower(new CultureInfo("es-CO"));

            var callerInfo = _agentRepository.GetAsyncAll(userCall.ToLower(new CultureInfo("es-CO"))).Result.FirstOrDefault();
            if (!string.IsNullOrEmpty(callerInfo?.Name))
            {
                call.CallerName = typeU != UsersTypes.Empresa.ToString().ToLower(new CultureInfo("es-CO")) ?
                    callerInfo.Name + " " + callerInfo.LastName : callerInfo.ContactName + "-" + callerInfo.SocialReason;
                call.CallerPhone = !string.IsNullOrEmpty(callerInfo?.CellPhone1) ?
               callerInfo?.CellPhone1 : string.Empty;
            }
            return call;
        }



        private void GetNameAgent(CallHistoryTrace cll)
        {
            if (!string.IsNullOrEmpty(cll.UserAnswerCall))
            {
                cll.AgentName = GetAgentName(cll.UserAnswerCall);
            }
            else
            {
                cll.AgentName = string.Empty;
            }
        }

        private string GetAgentName(string userName)
        {
             var agentInfo = _agentRepository.GetByPartitionKeyAndRowKeyAsync(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO")),
                    userName?.ToLower(new CultureInfo("es-CO"))).Result?.First();
            if (agentInfo is null || string.IsNullOrEmpty(agentInfo.Name))
            {
                return string.Empty;
            }
            else
            {
                return agentInfo.Name + " " + agentInfo.LastName;
            }
        }

        /// <summary>
        /// Get secure Url Blob
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private Uri GetContainerSasUri(string containerName, string BlobName)
        {
            var StorageConnectionString = _UserSecretSettings.TableStorage;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(BlobName);
            const int hours = -5;
            const int addHours = 1;

            //Set the expiry time and permissions for the blob.
            //In this case, the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(hours);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(addHours);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return new Uri(blob.Uri + sasBlobToken);
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
