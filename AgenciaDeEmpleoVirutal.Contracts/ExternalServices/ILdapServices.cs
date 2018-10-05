namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;

    public interface ILdapServices
    {
        LdapServicesResult<AuthenticateLdapResult> Authenticate(string userName, string pass);

        LdapServicesResult<AuthenticateLdapResult> Register(RegisterLdapRequest request);

        LdapServicesResult<AuthenticateLdapResult> PasswordChangeRequest(PasswordChangeRequest request);

        LdapServicesResult<AuthenticateLdapResult> PasswordChangeConfirm(PasswordChangeConfirmRequests request);
    }
}
