namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class SendMailWelcomeRequest
    {
        public bool IsCesante { get; set; }

        public string IsMale { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string DocType { get; set; }

        public string DocNum { get; set; }

        public string Pass { get; set; }

        public string Mail { get; set; }
    }
}
