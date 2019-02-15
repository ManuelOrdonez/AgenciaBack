namespace AgenciaDeEmpleoVirutal.UnitedTests.PdiBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Microsoft.Extensions.Options;
    using Moq;

    public class PdiBlTestBase : BusinessBase<PDI>
    {
        /// <summary>
        /// Interface to Convert PDF Mock
        /// </summary>
        protected Mock<IPdfConvertExternalService> PdfConvertServiceMock;

        /// <summary>
        /// The web page service Mock
        /// </summary>
        protected Mock<IWebPageService> WebPageServiceMock;

        /// <summary>
        /// PDI Repository Mock
        /// </summary>
        protected Mock<IGenericRep<PDI>> PdiRepMock;

        /// <summary>
        /// User Repository Mock
        /// </summary>
        protected Mock<IGenericRep<User>> UserRepMock;

        /// <summary>
        /// Interface to Send Mails Mock
        /// </summary>
        protected Mock<ISendGridExternalService> SendMailServiceMock;

        /// <summary>
        /// pdi Business Logic
        /// </summary>
        protected PdiBl pdiBusinessLogic;

        /// <summary>
        /// Pdi Request Mock
        /// </summary>
        protected PDIRequest PdiRequestMock;

        /// <summary>
        /// user Mock
        /// </summary>
        protected User UserMock;

        /// <summary>
        /// The agent Mock
        /// </summary>
        protected User AgentMock;

        /// <summary>
        /// Constructor's Pdi Business logic Test Base
        /// </summary>
        public PdiBlTestBase()
        {
            WebPageServiceMock = new Mock<IWebPageService>();
            PdfConvertServiceMock = new Mock<IPdfConvertExternalService>();
            PdiRepMock = new Mock<IGenericRep<PDI>>();
            UserRepMock = new Mock<IGenericRep<User>>();
            SendMailServiceMock = new Mock<ISendGridExternalService>();
            IOptions<UserSecretSettings> options = Options.Create<UserSecretSettings>(new UserSecretSettings() { URLFront = "urlTest" });
            pdiBusinessLogic = new PdiBl(PdfConvertServiceMock.Object, PdiRepMock.Object, UserRepMock.Object, SendMailServiceMock.Object, options, WebPageServiceMock.Object);
            SetEntitiesMocks();
        }

        /// <summary>
        /// Set Entities Mocks
        /// </summary>
        private void SetEntitiesMocks()
        {
            PdiRequestMock = new PDIRequest()
            {
                AgentUserName = "AgentUserName",
                CallerUserName = "CallerUserName",
                MustPotentiate = "MustPotentiate",
                MyStrengths = "MyStrengths ",
                MyWeaknesses = "MyWeaknesses ",
                Observations = "Observations ",
                OnlySave = false,
                WhatAbilities = "WhatAbilities ",
                WhatJob = "WhatJob ",
                WhenAbilities = "WhenAbilities ",
                WhenJob = "WhenJob "
            };

            UserMock = new User()
            {
                PartitionKey = "cesante",
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "",
                Role = "",
                State = "Enable",
                Email = "test@colsubsidio.com",
                UserType = "cesante",
                UserName = "12345678_2",
                OpenTokSessionId = "sessionot",
            };

            AgentMock = new User
            {
                PartitionKey = "funcionario",
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "test@colsubsidio.com",
                UserType = "funcionario",
                UserName = "12345678_2",
                OpenTokSessionId = "sessionot",
            };
        }
    }
}
