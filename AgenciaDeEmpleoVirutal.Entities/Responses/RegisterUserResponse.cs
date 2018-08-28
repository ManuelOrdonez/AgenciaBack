using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    public class RegisterUserResponse
    {
        public bool IsRegister { get; set; }
        public bool State { get; set; }
        public string UserType { get; set; }
        public User User { get; set; }
    }
}
