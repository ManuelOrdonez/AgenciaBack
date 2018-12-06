namespace AgenciaDeEmpleoVirutal.UnitedTests.PdiBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Moq;

    public class PdiBlTestBase : BusinessBase<PDI>
    {
        /// <summary>
        /// Interface to Convert PDF Mock
        /// </summary>
        protected Mock<IPDFConvertExternalService> _pdfConvertServiceMock;

        /// <summary>
        /// PDI Repository Mock
        /// </summary>
        protected Mock<IGenericRep<PDI>> _pdiRepMock;

        /// <summary>
        /// User Repository Mock
        /// </summary>
        protected Mock<IGenericRep<User>> _userRepMock;

        /// <summary>
        /// Interface to Send Mails Mock
        /// </summary>
        protected Mock<ISendGridExternalService> _sendMailServiceMock;

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
        protected User userMock;

        /// <summary>
        /// Constructor's Pdi Business logic Test Base
        /// </summary>
        public PdiBlTestBase()
        {
            _pdfConvertServiceMock = new Mock<IPDFConvertExternalService>();
            _pdiRepMock = new Mock<IGenericRep<PDI>>();
            _userRepMock = new Mock<IGenericRep<User>>();
            _sendMailServiceMock = new Mock<ISendGridExternalService>();
            pdiBusinessLogic = new PdiBl(_pdfConvertServiceMock.Object, _pdiRepMock.Object, _userRepMock.Object, _sendMailServiceMock.Object);
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

            userMock = new User()
            {
                PartitionKey = "cesante",
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
