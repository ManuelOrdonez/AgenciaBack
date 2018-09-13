

namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ResetResponse
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
