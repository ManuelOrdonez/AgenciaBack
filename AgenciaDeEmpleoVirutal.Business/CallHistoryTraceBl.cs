using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;

namespace AgenciaDeEmpleoVirutal.Business
{
    using Entities;
    using Entities.Requests;
    using Contracts.Business;
    using Contracts.Referentials;
    using Entities.Referentials;
    using Referentials;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;

    public class CallHistoryTraceBl : BusinessBase<CallHistoryTrace>, ICallHistoryTrace
    {
        /// <summary>
        /// The Call Repository
        /// </summary>
        private readonly IGenericRep<CallHistoryTrace> _callHistoryRepository;
        private readonly IGenericRep<Agent> _AgentRepository;

        public CallHistoryTraceBl(IGenericRep<CallHistoryTrace> callHistoryRepository,
            IGenericRep<Agent> agentRepository)
        {
            _callHistoryRepository = callHistoryRepository;
            _AgentRepository = agentRepository;
        }

        public Response<CallHistoryTrace> GetCallInfo(string OpenTokSessionId, string State)
        {
             if (string.IsNullOrEmpty(OpenTokSessionId))
             {
                 return ResponseFail<CallHistoryTrace>(ServiceResponseCode.BadRequest);
             }
             else
             {
                 var parameters = new List<ConditionParameter> {
                     new ConditionParameter{ColumnName="PartitionKey", Condition=Conditions.Equal ,Value=OpenTokSessionId.ToLower() },
                     new ConditionParameter{ColumnName="State", Condition=Conditions.Equal ,Value=State }
                 };
                 var call = _callHistoryRepository.GetSomeAsync(parameters).Result?
                     .OrderByDescending(t => t.DateCall).FirstOrDefault();
                 return ResponseSuccess(new List<CallHistoryTrace> {
                 call == null || string.IsNullOrWhiteSpace(call.OpenTokSessionId) ? null : call });
             }            
        }

        public Response<List<CallHistoryTrace>> GetAllCallsNotManaged(string OpenTokSessionId, string State)
        {
            if (string.IsNullOrEmpty(OpenTokSessionId))
            {
                return ResponseFail<List<CallHistoryTrace>>(ServiceResponseCode.BadRequest);
            }
            else
            {
                var parameters = new List<ConditionParameter> {
                     new ConditionParameter{ColumnName="PartitionKey", Condition=Conditions.Equal ,Value=OpenTokSessionId.ToLower() },
                     new ConditionParameter{ColumnName="State", Condition=Conditions.NotEqual, Value=State }
                 };
                var call = _callHistoryRepository.GetSomeAsync(parameters).Result;
                return ResponseSuccess(new List<List<CallHistoryTrace>> { call });
            }

        }


        /// <summary>
        /// Register call date and  trace 
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        public Response<CallHistoryTrace> SetCallTrace(DateCallRequest callRequest)
        {
            var messagesValidationEntity = callRequest.Validate().ToList();
            var stateInput = (CallStateCode)callRequest.State;

            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<CallHistoryTrace>(messagesValidationEntity);
            }

            //var existsCall = GetDefaultCallHistoryTrace(callRequest);
            var existsCall = GetCallInfo(callRequest.OpenTokSessionId, CallStateCode.Begun.ToString()).Data.FirstOrDefault();
            var callInfo = existsCall == null || string.IsNullOrWhiteSpace(existsCall.UserCall) ? GetDefaultCallHistoryTrace(callRequest) : existsCall;
            var agent = _AgentRepository.GetAsync(callRequest.EmailUserAddress).Result;

            switch (stateInput)
            {
                case CallStateCode.Begun:
                    callInfo.DateCall = DateTime.Now;
                    callInfo.UserCall = callRequest.EmailUserAddress;
                    callInfo.State = stateInput.ToString();
                    break;
                case CallStateCode.Answered:
                    callInfo.DateAnswerCall = DateTime.Now;
                    callInfo.UserAnswerCall = callRequest.EmailUserAddress;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();
                    if (agent != null)
                    {
                        agent.Available = false;
                        agent.CountCallAttended = agent.CountCallAttended ++;
                        _AgentRepository.AddOrUpdate(agent);
                    }
                    break;
                case CallStateCode.EndByWeb:
                    if (callInfo.State != CallStateCode.EndByWeb.ToString())
                    {
                        callInfo.DateFinishCall = DateTime.Now;
                        callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                        callInfo.State = stateInput.ToString();
                    }
                    if (agent != null)
                    {
                        agent.Available = false;
                        if (!_AgentRepository.AddOrUpdate(agent).Result) return ResponseFail();
                    }
                    break;
                case CallStateCode.EndByMobile:
                    if (callInfo.State != CallStateCode.EndByMobile.ToString())
                    {
                        callInfo.DateFinishCall = DateTime.Now;
                        callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                        callInfo.State = callInfo.State == CallStateCode.Begun.ToString() ?
                            CallStateCode.Lost.ToString() : stateInput.ToString();
                    }
                    break;
                case CallStateCode.Managed:
                    callInfo.DateFinishCall = DateTime.Now;
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    callInfo.State = stateInput.ToString();                    
                    if (agent != null)
                    {
                        agent.Available = true;
                        if (!_AgentRepository.AddOrUpdate(agent).Result) return ResponseFail();
                    }
                    break;
                default:
                    callInfo.State = CallStateCode.Unknown.ToString();
                    callInfo.Trace = callInfo.Trace + " - " + callRequest.Trace;
                    break;
            }
            if(callInfo.Trace != "Logout" )
                if (!_callHistoryRepository.AddOrUpdate(callInfo).Result) return ResponseFail();
            return ResponseSuccess();
        }

        private CallHistoryTrace GetDefaultCallHistoryTrace(DateCallRequest callRequest)
        {
           
            var call = GetCallForAnyManage(callRequest.OpenTokSessionId,
                    callRequest.OpenTokAccessToken);

            if (call != null)
            {
                return call;
            }
            else
            {
                var stateInput = (CallStateCode)callRequest.State;
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
        }

        private CallHistoryTrace GetCallForAnyManage(string OpenTokSessionId, string OpenTokAccessToken)
        {   
            return _callHistoryRepository.GetByPartitionKeyAndRowKeyAsync(OpenTokSessionId, OpenTokAccessToken).Result
                .OrderByDescending(t =>t.DateCall).FirstOrDefault();
        }

    }
}
