namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Set parameter value request.
    /// </summary>
    public class SetParameterValueRequest
    {
        /// <summary>
        /// Gets or sets for Category.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Category_Required")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets for ParameterId.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "IdParameter__Required")]
        public string ParameterId { get; set; }

        /// <summary>
        /// Gets or sets for ParameterValue.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "ValueParameter_Required")]
        public string ParameterValue { get; set; }

        /// <summary>
        /// Gets or sets for ParameterDesc.
        /// </summary>
        public string ParameterDesc { get; set; }

        /// <summary>
        /// Gets or sets for ParameterImg.
        /// </summary>
        public string ParameterImg { get; set; }

        /// <summary>
        /// Gets or sets for ParameterState.
        /// </summary>
        public bool ParameterState { get; set; }
    }
}
