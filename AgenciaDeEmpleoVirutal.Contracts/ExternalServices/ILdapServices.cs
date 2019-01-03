namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;

    /// <summary>
    /// Interface of Ldap services
    /// </summary>
    public interface ILdapServices
    {
        /// <summary>
        /// Methos to Authenticate in LDAP
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        LdapServicesResult<AuthenticateLdapResult> Authenticate(string userName, string pass);

        /// <summary>
        /// Method to Register in LDAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LdapServicesResult<AuthenticateLdapResult> Register(RegisterLdapRequest request);

        /// <summary>
        /// Method to Password Change Request in LDAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LdapServicesResult<AuthenticateLdapResult> PasswordChangeRequest(PasswordChangeRequest request);

        /// <summary>
        /// Method to Password Change Confirm in LDAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LdapServicesResult<AuthenticateLdapResult> PasswordChangeConfirm(PasswordChangeConfirmRequests request);

        /// <summary>
        /// Method to Request Status.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RequestStatusResult RequestStatus(FosfecRequest request);

        /// <summary>
        /// Method to Benefits Payable.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BenefitsPayableResult BenefitsPayable(FosfecRequest request);
    }
}
