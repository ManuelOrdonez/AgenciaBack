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
    using System;
    using System.Collections.Generic;
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

        private readonly IOpenTokExternalService _openTokService;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="callHistoryRepository"></param>
        /// <param name="agentRepository"></param>
        /// <param name="busyAgentRepository"></param>
        public CallHistoryTraceBl(IGenericRep<CallHistoryTrace> callHistoryRepository,
            IGenericRep<User> agentRepository, IGenericRep<BusyAgent> busyAgentRepository,
            IOpenTokExternalService openTokService)
        {
            _callHistoryRepository = callHistoryRepository;
            _agentRepository = agentRepository;
            _callerRepository = agentRepository;
            _busyAgentRepository = busyAgentRepository;
            _openTokService = openTokService;
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
                throw new ArgumentNullException("request");
            }
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(errorMessages);
            }

            var parameters = new List<ConditionParameter>
            {
                new ConditionParameter{ColumnName="PartitionKey", Condition = "eq" ,Value = request.OpenTokSessionId.ToLower() },
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
                throw new ArgumentNullException("request");
            }
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<List<CallHistoryTrace>>(errorMessages);
            }
            var parameters = new List<ConditionParameter> {
                    new ConditionParameter{ColumnName="PartitionKey", Condition = "eq" ,Value = request.OpenTokSessionId.ToLower() },
                    new ConditionParameter{ColumnName="State", Condition = "ne", Value = request.State }
                };
            var call = _callHistoryRepository.GetSomeAsync(parameters).Result;
            if(call is null)
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
                throw new ArgumentNullException("request");
            }
            var errorMessages = request.Validate().ToList();
            if(errorMessages.Count > 0)
            {
                return ResponseFail<List<CallHistoryTrace>>(ServiceResponseCode.BadRequest);
            }
            var callTrace = _callHistoryRepository.GetByPartitionKeyAndRowKeyAsync(request.SessionId, request.TokenId).Result;
            if (callTrace.Count == 0)
            {
                return ResponseFail<List<CallHistoryTrace>>();
            }
            callTrace.First().Score = request.Score;
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
                throw new ArgumentNullException("callRequest");
            }
            var messagesValidationEntity = callRequest.Validate().ToList();
            var stateInput = (CallStates)callRequest.State;

            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(messagesValidationEntity);
            }

            var existsCall = GetCallInfo(new GetCallRequest()
            {
                OpenTokSessionId = callRequest?.OpenTokSessionId,
                State = CallStates.Begun.ToString()
            }).Data.FirstOrDefault();

            var callInfo = existsCall == null || string.IsNullOrWhiteSpace(existsCall.UserCall) ?
                GetDefaultCallHistoryTrace(callRequest) : existsCall;

            /// ver Row Key de agentes
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
                        agent.CountCallAttended = Int32.Parse(agent.CountCallAttended.ToString()) + 1;
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
                    callInfo.UserCall = callRequest.UserName;
                    var resultR = _openTokService.StopRecord(callInfo.RecordId);
                    break;
                case CallStates.EndByMobile:
                    callInfo = this.CallEnded(CallStates.EndByWeb, callInfo, callRequest, stateInput);
                    if (agent != null)
                    {
                        agent.Available = false;
                        this.Aviable(agent.UserName);
                        if (!_agentRepository.AddOrUpdate(agent).Result)
                        {
                            return ResponseFail();
                        }
                    }
                    var resultRM = _openTokService.StopRecord(callInfo.RecordId);
                    break;
                case CallStates.Managed:
                    callInfo.DateFinishCall = DateTime.Now;
                    callInfo.Observations = callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    callInfo.CallType = callRequest.CallType;
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
            if (user.UserType.ToLower().Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                type = "UserNameAgent";
            }
            else
            {
                type = "UserNameCaller";
            }

            var busy = _busyAgentRepository.GetSomeAsync(type,UserName).Result;
            if (busy.Any())
            {
                var resultDelete = _busyAgentRepository.DeleteRowAsync(busy.FirstOrDefault()).Result;
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

        /// <summary>
        /// Method to Get All User Call
        /// </summary>
        /// <param name="getAllUserCallRequest"></param>
        /// <returns></returns>
        public Response<GetAllUserCallResponse> GetAllUserCall(GetAllUserCallRequest getAllUserCallRequest)
        {
            var response = new List<GetAllUserCallResponse>();
            string type = string.Empty;
            if (getAllUserCallRequest.UserType.ToLower().Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                type = "UserAnswerCall";
            }
            else
            {
                type = "UserCall";
            }
            var calls = _callHistoryRepository.GetSomeAsync(type, getAllUserCallRequest.UserName).Result;
            if (calls.Count == 0 || calls is null)
            {
                return ResponseFail<GetAllUserCallResponse>(ServiceResponseCode.UserDoNotHaveCalls);
            }
            foreach (var cll in calls.OrderByDescending(cll => cll.Timestamp).ToList())
            {                
                if (string.IsNullOrEmpty(cll.UserAnswerCall))
                {
                    continue;
                }
                var agentInfo = _agentRepository.GetByPartitionKeyAndRowKeyAsync(UsersTypes.Funcionario.ToString().ToLower(), cll.UserAnswerCall).Result.First();
                if (agentInfo is null || string.IsNullOrEmpty(agentInfo.Name))
                {
                    continue;
                }
                response.Add(new GetAllUserCallResponse()
                {
                    AgentName = agentInfo.Name + " " + agentInfo.LastName,
                    CallInfo = cll,
                    DateCall = cll.DateCall.ToString("dd/MM/yyyy")
                });
            }
            return ResponseSuccess(response);
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
                GetCallRequest getCallReq = new GetCallRequest()
                {
                    OpenTokSessionId = OpenTokSessionId,
                    State = CallStates.Begun.ToString(),
                };
                var callInfo = GetCallInfo(getCallReq).Data.First();
                var caller = _callerRepository.GetAsync(callInfo.UserCall).Result;
                if(caller is null)
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
