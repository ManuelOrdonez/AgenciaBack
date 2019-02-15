namespace AgenciaDeEmpleoVirutal.UnitedTests.ResetBITest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Moq;

    public class ResetBITestBase : BusinessBase<ResetResponse>
    {
        /// <summary>
        /// Interface to dens mails mock.
        /// </summary>
        protected Mock<ISendGridExternalService> _sendMailServiceMock;

        /// <summary>
        /// Interface of Ldap Services mock.
        /// </summary>
        protected Mock<ILdapServices> _ldapServicesMock;

        /// <summary>
        /// User repository mock.
        /// </summary>
        protected Mock<IGenericRep<User>> _userRepMock;

        /// <summary>
        /// Reset Password repository mock.
        /// </summary>
        protected Mock<IGenericRep<ResetPassword>> _passwordRepMock;

        /// <summary>
        /// Parameters repository mock.
        /// </summary>
        protected Mock<IGenericRep<Parameters>> _parametersRepMock;

        /// <summary>
        /// Reset Business Logic
        /// </summary>
        protected ResetBI resetBusinessLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetBITestBase"/> class.
        /// </summary>
        public ResetBITestBase()
        {
            _sendMailServiceMock = new Mock<ISendGridExternalService>();
            _ldapServicesMock = new Mock<ILdapServices>();
            _userRepMock = new Mock<IGenericRep<User>>();
            _passwordRepMock = new Mock<IGenericRep<ResetPassword>>();
            _parametersRepMock = new Mock<IGenericRep<Parameters>>();
            resetBusinessLogic = new ResetBI(_userRepMock.Object, _passwordRepMock.Object, _parametersRepMock.Object, _sendMailServiceMock.Object, _ldapServicesMock.Object);
        }
    }
}
