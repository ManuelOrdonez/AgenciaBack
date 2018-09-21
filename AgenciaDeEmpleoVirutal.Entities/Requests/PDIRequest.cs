namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class PDIRequest
    {

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CallUser_Required")]
        public string CallerUserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "AgentUser_Required")]
        public string AgentUserName { get; set; }

        public string MyStrengths     { get; set; }
        public string MyWeaknesses    { get; set; }
        public string MustPotentiate  { get; set; }
        public string WhatAbilities   { get; set; }
        public string WhenAbilities   { get; set; }
        public string WhatJob         { get; set; }
        public string WhenJob         { get; set; }
        public string Observations { get; set; }   
    }
}