﻿using AgenciaDeEmpleoVirutal.Entities.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class UpdateFuncionaryRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_Required")]
        public string InternalMail { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "LastName_Required")]
        public string LastName { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Role_Required")]
        public string Role { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "State_Required")]
        public bool State { get; set; }
    }
}
