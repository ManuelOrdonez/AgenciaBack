namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    /// <summary>
    /// Register user response.
    /// </summary>
    public class RegisterUserResponse
    {
        /// <summary>
        /// Gets or sets the IsRegister.
        /// </summary>
        public bool IsRegister { get; set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Gets or sets the UserType.
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public User User { get; set; }
    }
}
