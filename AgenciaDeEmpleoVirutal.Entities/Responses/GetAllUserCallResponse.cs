using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    public class GetAllUserCallResponse
    {
        public string AgentName { get; set; }

        public string DateCall { get; set; }

        public CallHistoryTrace CallInfo { get; set; }

    }
}
