namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService;
    using AgenciaDeEmpleoVirutal.Entities.Requests;

    public interface ILdapServices
    {
        LdapServicesResult Authenticate(string userName, string pass);
        LdapServicesResult Register(RegisterInLdapRequest userReq);
    }
}
