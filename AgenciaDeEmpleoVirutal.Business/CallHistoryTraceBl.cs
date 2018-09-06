namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CallHistoryTraceBl : BusinessBase<CallHistoryTrace>, ICallHistoryTrace
    {

        private readonly IGenericRep<CallHistoryTrace> _callHistoryRepository;

        private readonly IGenericRep<Agent> _agentRepository;

        public CallHistoryTraceBl(IGenericRep<CallHistoryTrace> callHistoryRepository,
            IGenericRep<Agent> agentRepository)
        {
            _callHistoryRepository = callHistoryRepository;
            _agentRepository = agentRepository;
        }

        public Response<CallHistoryTrace> GetCallInfo(GetCallRequest request)
        {
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0) return ResponseBadRequest<CallHistoryTrace>(errorMessages);

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
            if (errorMessages.Count > 0) return ResponseBadRequest<List<CallHistoryTrace>>(errorMessages);

            var parameters = new List<ConditionParameter> {
                    new ConditionParameter{ColumnName="PartitionKey", Condition = "eq" ,Value = request.OpenTokSessionId.ToLower() },
                    new ConditionParameter{ColumnName="State", Condition = "ne", Value = request.State }
                };
            var call = _callHistoryRepository.GetSomeAsync(parameters).Result;
            return ResponseSuccess(new List<List<CallHistoryTrace>> { call });
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

            if (messagesValidationEntity.Count > 0) return ResponseBadRequest<CallHistoryTrace>(messagesValidationEntity);
            
            var existsCall = GetCallInfo(new GetCallRequest()
            {
                OpenTokSessionId = callRequest.OpenTokSessionId,
                State = CallStates.Begun.ToString()
            }).Data.FirstOrDefault();

            var callInfo = existsCall == null || string.IsNullOrWhiteSpace(existsCall.UserCall) ? 
                GetDefaultCallHistoryTrace(callRequest) : existsCall;

            /// ver Row Key de agentes
            var agent = _agentRepository.GetAsync(callRequest.UserName).Result;

            switch (stateInput)
            {
                case CallStates.Begun:
                    callInfo.DateCall = DateTime.Now;
                    callInfo.UserCall = callRequest.UserName;
                    callInfo.State = stateInput.ToString();
                    break;
                case CallStates.Answered:
                    callInfo.DateAnswerCall = DateTime.Now;
                    callInfo.UserAnswerCall = callRequest.UserName;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    if (agent != null)
                    {
                        agent.Available = false;
                        agent.CountCallAttended = agent.CountCallAttended++;
                        _agentRepository.AddOrUpdate(agent);
                    }
                    break;
                case CallStates.EndByWeb:
                    if (callInfo.State != CallStates.EndByWeb.ToString())
                    {
                        callInfo.DateFinishCall = DateTime.Now;
                        callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                        callInfo.State = stateInput.ToString();
                    }
                    if (agent != null)
                    {
                        agent.Available = false;
                        if (!_agentRepository.AddOrUpdate(agent).Result) return ResponseFail();
                    }
                    break;
                case CallStates.EndByMobile:
                    if (callInfo.State != CallStates.EndByMobile.ToString())
                    {
                        callInfo.DateFinishCall = DateTime.Now;
                        callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                        callInfo.State = callInfo.State == CallStates.Begun.ToString() ?
                            CallStates.Lost.ToString() : stateInput.ToString();
                    }
                    break;
                case CallStates.Managed:
                    callInfo.DateFinishCall = DateTime.Now;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    if (agent != null)
                    {
                        agent.Available = true;
                        if (!_agentRepository.AddOrUpdate(agent).Result) return ResponseFail();
                    }
                    break;
                default:
                    callInfo.State = CallStates.Unknown.ToString();
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    break;
            }
            if (callInfo.Trace != "Logout")
                if (!_callHistoryRepository.AddOrUpdate(callInfo).Result) return ResponseFail();
            return ResponseSuccess();
        }

        private CallHistoryTrace GetDefaultCallHistoryTrace(SetCallTraceRequest callRequest)
        {
            var call = GetCallForAnyManage(callRequest.OpenTokSessionId,
                    callRequest.OpenTokAccessToken);
            if (call != null) return call;
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

        private CallHistoryTrace GetCallForAnyManage(string OpenTokSessionId, string OpenTokAccessToken)
        {
            return _callHistoryRepository.GetByPartitionKeyAndRowKeyAsync(OpenTokSessionId, OpenTokAccessToken).Result
                .OrderByDescending(t => t.DateCall).FirstOrDefault();
        }
    }
}
