﻿namespace AgenciaDeEmpleoVirutal.Business
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
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Agent Business Logic
    /// </summary>
    public class AgentBl : BusinessBase<Agent>, IAgentBl
    {
        /// <summary>
        /// Users repository
        /// </summary>
        private readonly IGenericRep<User> _userRepository;

        /// <summary>
        /// Busy Agents repository
        /// </summary>
        private readonly IGenericRep<BusyAgent> _busyAgentRepository;

        /// <summary>
        /// Agents Repository
        /// </summary>
        private readonly IGenericRep<User> _agentRepository;

        /// <summary>
        /// Agents Repository
        /// </summary>
        private readonly IGenericRep<Parameters> _parametersRepository;

        /// <summary>
        /// Interface of OpenTok External Service
        /// </summary>
        private readonly IOpenTokExternalService _openTokExternalService;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="AgentRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="openTokService"></param>
        /// <param name="busyAgentRepository"></param>
        public AgentBl(IGenericRep<User> AgentRepository, IGenericRep<User> userRepository, IOpenTokExternalService openTokService,
            IGenericRep<BusyAgent> busyAgentRepository, IGenericRep<Parameters> parametersRepository)
        {
            _userRepository = userRepository;
            _agentRepository = AgentRepository;
            _openTokExternalService = openTokService;
            _busyAgentRepository = busyAgentRepository;
            _parametersRepository = parametersRepository;
        }

        private static readonly Object obj = new Object();

        /// <summary>
        /// Method to validate shedule
        /// </summary>
        /// <returns></returns>
        private Response<GetAgentAvailableResponse> ValidateShedule()
        {
            var parameters = _parametersRepository.GetByPatitionKeyAsync("horario").Result;
            string[] days = {  "lunes", "martes", "miercoles", "jueves", "viernes", "sabado", "domingo" };
            string dayIni = parameters.FirstOrDefault(x => x.RowKey == "diainicio").Value.Replace("á","a").Replace("é", "e");
            string dayEnd = parameters.FirstOrDefault(x => x.RowKey == "diafin").Value.Replace("á", "a").Replace("é", "e");
            string hourIni = parameters.FirstOrDefault(x => x.RowKey == "horainicio").Value;
            string hourEnd = parameters.FirstOrDefault(x => x.RowKey == "horafin").Value;
            string MessageShedule = parameters.FirstOrDefault(x => x.RowKey == "message").Value;

            int diaIniPos = Array.IndexOf(days, dayIni.ToLower(new CultureInfo("es-CO")));
            int diaEndPos = Array.IndexOf(days, dayEnd.ToLower(new CultureInfo("es-CO")));
            string dayNow = DateTime.UtcNow.AddHours(-5).ToString("dddd", new CultureInfo("es-CO")).Replace("á", "a").Replace("é", "e");
            int dayNowPos = Array.IndexOf(days, dayNow);

            var result = new Response<GetAgentAvailableResponse>
            {
                CodeResponse = (int)ServiceResponseCode.OutOfService,
                Message = new List<string> { MessageShedule },
                TransactionMade = false
            };

            if (dayNowPos >= diaIniPos && dayNowPos <= diaEndPos)
            {
                DateTime timeInit = Convert.ToDateTime(DateTime.UtcNow.AddHours(-5).ToShortDateString() + " " + hourIni + ":00 am");
                DateTime timeEnd = Convert.ToDateTime(DateTime.UtcNow.AddHours(-5).ToShortDateString() + " " + hourEnd + ":00 pm");
                DateTime timeNow = DateTime.UtcNow.AddHours(-5);

                if (timeNow < timeInit || timeNow > timeEnd)
                {
                    return result;
                }
                else
                {
                    result.TransactionMade = true;
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Method to Get any Agent Available
        /// </summary>
        /// <param name="agentAvailableRequest"></param>
        /// <returns></returns>
        public Response<GetAgentAvailableResponse> GetAgentAvailable(GetAgentAvailableRequest agentAvailableRequest)
        {
            var validateShedule = ValidateShedule();
            if (!validateShedule.TransactionMade)
            {
                return validateShedule;
            }
            var errorMessages = agentAvailableRequest.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<GetAgentAvailableResponse>(errorMessages);
            }
            var userInfo = _userRepository.GetAsync(agentAvailableRequest.UserName).Result;
            if (userInfo == null)
            {
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserNotFound);
            }
            var userCalling = _busyAgentRepository.GetSomeAsync("UserNameCaller", userInfo.UserName).Result;
            if (!(userCalling.Count == 0 || userCalling is null))
            {
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserCalling);
            }

            lock (obj)
            {
                var query = new List<ConditionParameter>
                {
                    new ConditionParameter
                    {
                        ColumnName = "PartitionKey",
                        Condition = QueryComparisons.Equal,
                        Value = UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO"))
                    },
                    new ConditionParameter
                    {
                        ColumnName = "Available",
                        Condition = QueryComparisons.Equal,
                        ValueBool = true,
                    },

                     new ConditionParameter
                    {
                        ColumnName = "Authenticated",
                        Condition = QueryComparisons.Equal,
                        ValueBool = true
                    }
                };
                var advisors = _agentRepository.GetSomeAsync(query).Result;
                if (advisors.Count.Equals(0))
                {
                    return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);
                }
                var Agent = advisors.OrderBy(x => x.CountCallAttended).FirstOrDefault();
                try
                {
                    if(!_busyAgentRepository.Add(new BusyAgent {
                        PartitionKey = Agent.OpenTokSessionId.ToLower(new CultureInfo("es-CO")),
                        RowKey = Agent.UserName,
                        UserNameAgent = Agent.UserName,
                        UserNameCaller = agentAvailableRequest.UserName}).Result)
                    {
                        return ResponseFail<GetAgentAvailableResponse>();
                    }
                }
                catch (Exception ex)
                {
                    return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);
                }
                /// Disabled Agent
                Agent.Available = false;
                if (!_agentRepository.AddOrUpdate(Agent).Result)
                {
                    return ResponseFail<GetAgentAvailableResponse>();
                }
                var response = new GetAgentAvailableResponse();
                response.IDToken = _openTokExternalService.CreateToken(Agent.OpenTokSessionId, agentAvailableRequest.UserName);
                if (string.IsNullOrEmpty(response.IDToken))
                {
                    return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
                }
                response.IDSession = Agent.OpenTokSessionId;
                response.AgentName = Agent.Name;
                response.AgentLatName = Agent.LastName;
                response.AgentUserName = Agent.UserName;
                return ResponseSuccess(new List<GetAgentAvailableResponse> { response });
            }
        }

        /// <summary>
        /// Method to identify if an agent is aviable
        /// </summary>
        /// <param name="RequestAviable"></param>
        /// <returns></returns>
        public Response<User> ImAviable(AviableUserRequest RequestAviable)
        {
            if (RequestAviable == null)
            {
                throw new ArgumentNullException("RequestAviable");
            }
            if (string.IsNullOrEmpty(RequestAviable.UserName))
            {
                return ResponseFail<User>(ServiceResponseCode.BadRequest);
            }
            var user = _agentRepository.GetAsync(RequestAviable.UserName).Result;
            if(user is null)
            {
                return ResponseFail<User>();
            }
            return ResponseSuccess(new List<User> { user });
        }

        /// <summary>
        /// Get all agent fosfec.
        /// </summary>
        /// <returns></returns>
        public Response<GetAllAgentByRoleResponse> GetAllAgentByRole(int role)
        {
            if (default(int) == role)
            {
                return ResponseFail<GetAllAgentByRoleResponse>(ServiceResponseCode.ErrorRequest);
            }

            var queryRequestsActive = new List<ConditionParameter>
            {
                new ConditionParameter
                {
                    ColumnName = "Role",
                    Condition = QueryComparisons.Equal,
                    Value = EnumValues.GetDescriptionFromValue((Roles)role)
                }
            };

            var agents = _agentRepository.GetListQuery(queryRequestsActive).Result;
            if (agents is null)
            {
                return ResponseFail<GetAllAgentByRoleResponse>();
            }

            var agentFosfec = new List<GetAllAgentByRoleResponse>();

            foreach (var agent in agents)
            {
                agentFosfec.Add(new GetAllAgentByRoleResponse
                {
                    Name = agent.Name + " " + agent.LastName,
                    NoDocument = agent.NoDocument,
                    Role = agent.Role,
                    UserName = agent.RowKey
                });
            }
            return ResponseSuccess(agentFosfec);
        }
    }
}
