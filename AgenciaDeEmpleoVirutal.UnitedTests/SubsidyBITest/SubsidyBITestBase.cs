namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.Extensions.Options;
    using Moq;

    public class SubsidyBITestBase : BusinessBase<Subsidy>
    {
        /// <summary>
        /// Subsidy reppository mock
        /// </summary>
        protected Mock<IGenericRep<Subsidy>> SubsidyRepMock;

        /// <summary>
        /// User Repository mock
        /// </summary>
        protected Mock<IGenericRep<User>> UserRepMock;

        /// <summary>
        /// Interface to Send Mails
        /// </summary>
        protected Mock<ISendGridExternalService> _sendMailServiceMock;

        /// <summary>
        /// Interface of ldap services
        /// </summary>
        protected Mock<ILdapServices> _LdapServices;

        /// <summary>
        /// Subsidy Request Mock
        /// </summary>
        protected SubsidyRequest SubsidyRequestMock;

        /// <summary>
        /// Subsidy Request Mock
        /// </summary>
        protected ChangeSubsidyStateRequest ChangeSubsidyRequestMock;

        /// <summary>
        /// Subsidy Business Logic
        /// </summary>
        protected SubsidyBl subsidyBusinessLogic;

        public SubsidyBITestBase()
        {
            _sendMailServiceMock = new Mock<ISendGridExternalService>();
            SubsidyRepMock = new Mock<IGenericRep<Subsidy>>();
            UserRepMock = new Mock<IGenericRep<User>>();
            _LdapServices = new Mock<ILdapServices>();
            IOptions<UserSecretSettings> options = Options.Create<UserSecretSettings>(new UserSecretSettings());
            subsidyBusinessLogic = new SubsidyBl(SubsidyRepMock.Object, UserRepMock.Object, options, _sendMailServiceMock.Object, _LdapServices.Object);
            SetEntitiesMocks();
        }

        private void SetEntitiesMocks()
        {
            SubsidyRequestMock = new SubsidyRequest
            {
                NoSubsidyRequest = "2018120612547",
                UserName = "1069444555_2"
            };

            ChangeSubsidyRequestMock = new ChangeSubsidyStateRequest
            {
                NoSubsidyRequest = "2018120612547",
                UserName = "1069444555_2",
                Observations = "Observations",
                Reviewer = "Revisor Fosfec",
                NumberSap = "5846123",
                State = SubsidyStates.Approved.GetHashCode()
            };
        }
    }
}
