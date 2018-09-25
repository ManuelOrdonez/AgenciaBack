namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    public class GetAgentAvailableResponse
    {
        public string IDSession { get; set; }
        public string IDToken { get; set; }
        public string AgentName { get; set; }
        public string AgentLatName { get; set; }

        public string AgentUserName { get; set; }
    }
}
