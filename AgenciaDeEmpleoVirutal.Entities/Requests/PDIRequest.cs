namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Pdi request.
    /// </summary>
    public class PDIRequest
    {
        /// <summary>
        /// Gets or sets for CallerUserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CallUser_Required")]
        public string CallerUserName { get; set; }

        /// <summary>
        /// Gets or sets for AgentUserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "AgentUser_Required")]
        public string AgentUserName { get; set; }

        /// <summary>
        /// Gets or sets for MyStrengths.
        /// </summary>
        ///  [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string MyStrengths { get; set; }

        /// <summary>
        /// Gets or sets for MyWeaknesses.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string MyWeaknesses { get; set; }

        /// <summary>
        /// Gets or sets for MustPotentiate.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string MustPotentiate { get; set; }

        /// <summary>
        /// Gets or sets for WhatAbilities.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhatAbilities { get; set; }

        /// <summary>
        /// Gets or sets for WhenAbilities.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhenAbilities { get; set; }

        /// <summary>
        /// Gets or sets for WhatJob.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhatJob { get; set; }

        /// <summary>
        /// Gets or sets for WhenJob.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Required")]
        /// [StringLength(400, MinimumLength = 1, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "PDIField_Length")]
        public string WhenJob { get; set; }

        /// <summary>
        /// Gets or sets for Observations.
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets for OnlySave.
        /// </summary>
        public bool OnlySave { get; set; }
    }
}