namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PDIRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CallUser_Required")]
        public string CallerUserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "AgentUser_Required")]
        public string AgentUserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string MyStrengths     { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string MyWeaknesses    { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string MustPotentiate  { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhatAbilities   { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhenAbilities   { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhatJob { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "AgentUser_Required")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhenJob { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "AgentUser_Required")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string Observations { get; set; }

        public DateTime DateOfOrientation { get; set; }
    }
}