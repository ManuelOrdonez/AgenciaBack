namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
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

    public class CallHistoryTraceBl : BusinessBase<CallHistoryTrace>, ICallHistoryTrace
    {
        private readonly IGenericRep<BusyAgent> _busyAgentRepository;

        private readonly IGenericRep<CallHistoryTrace> _callHistoryRepository;

        private readonly IGenericRep<User> _callerRepository;

        private readonly IGenericRep<User> _agentRepository;

        public CallHistoryTraceBl(IGenericRep<CallHistoryTrace> callHistoryRepository,
            IGenericRep<User> agentRepository, IGenericRep<BusyAgent> busyAgentRepository)
        {
            _callHistoryRepository = callHistoryRepository;
            _agentRepository = agentRepository;
            _callerRepository = agentRepository;
            _busyAgentRepository = busyAgentRepository;
        }

        public Response<CallHistoryTrace> GetCallInfo(GetCallRequest request)
        {
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

        public Response<List<CallHistoryTrace>> GetAllCallsNotManaged(GetCallRequest request)
        {
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
            return ResponseSuccess(new List<List<CallHistoryTrace>> { call });
        }

        public Response<List<CallHistoryTrace>> CallQuality(QualityCallRequest request)
        {
            var errorMessages = request.Validate().ToList();
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
            var messagesValidationEntity = callRequest.Validate().ToList();
            var stateInput = (CallStates)callRequest.State;

            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(messagesValidationEntity);
            }

            var existsCall = GetCallInfo(new GetCallRequest()
            {
                OpenTokSessionId = callRequest.OpenTokSessionId,
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
                    callInfo.DateAnswerCall = DateTime.Now;
                    callInfo.UserAnswerCall = callRequest.UserName;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    if (agent != null)
                    {
                        agent.Available = false;
                        //this.Aviable(agent.UserName);
                        agent.CountCallAttended = Int32.Parse(agent.CountCallAttended.ToString()) + 1;
                        _agentRepository.AddOrUpdate(agent);
                    }
                    break;
                case CallStates.EndByWeb:
                    if (callInfo.State != CallStates.EndByWeb.ToString())
                    {
                        callInfo.DateFinishCall = DateTime.Now;
                        callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;                       
                    }
                    callInfo.State = callInfo.State != (CallStates.Answered.ToString()) ?
                           CallStates.Lost.ToString() : stateInput.ToString();
                    if (agent != null)
                    {
                        agent.Available = false;                        
                        if (!_agentRepository.AddOrUpdate(agent).Result)
                        {
                            return ResponseFail();
                        }
                    }
                    if (callInfo.State == CallStates.Lost.ToString())
                    {
                        this.Aviable(callRequest.UserName);
                        callInfo.UserCall = callRequest.UserName;
                    }
                    break;
                case CallStates.EndByMobile:
                    if (callInfo.State != CallStates.EndByMobile.ToString())
                    {
                        callInfo.DateFinishCall = DateTime.Now;
                        callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
 
                    }
                    callInfo.State = callInfo.State != (CallStates.Answered.ToString()) ?
                           CallStates.Lost.ToString() : stateInput.ToString();
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
                    break;
                default:
                    callInfo.State = CallStates.Unknown.ToString();
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    break;
            }
            if (callInfo.Trace != "Logout")
            {
                if (!_callHistoryRepository.AddOrUpdate(callInfo).Result)
                {
                    return ResponseFail();
                }
            }
            return ResponseSuccess();
        }

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

        public Response<GetAllUserCallResponse> GetAllUserCall(GetAllUserCallRequest getAllUserCallRequest)
        {
            GetAllUserCallResponse response = new GetAllUserCallResponse();
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
            response.Calls = calls;
            return ResponseSuccess(new List<GetAllUserCallResponse> { response }); ;
        }

        private CallHistoryTrace GetCallForAnyManage(string OpenTokSessionId, string OpenTokAccessToken)
        {
            return _callHistoryRepository.GetByPartitionKeyAndRowKeyAsync(OpenTokSessionId, OpenTokAccessToken).Result
                .OrderByDescending(t => t.DateCall).FirstOrDefault();
        }

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
                var caller = _callerRepository.GetAsync(callInfo?.UserCall).Result;
                CallerInfoResponse response = new CallerInfoResponse();
                response.Caller = caller;
                response.OpenTokAccessToken = callInfo.OpenTokAccessToken;
                response.CallInfo = callInfo;
                return ResponseSuccess(new List<CallerInfoResponse> { response });
            }
        }
    }
}
