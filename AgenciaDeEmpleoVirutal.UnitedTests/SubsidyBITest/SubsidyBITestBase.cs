﻿namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
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

    public class SubsidyBITestBase : BusinessBase<Subsidy>
    {
        /// <summary>
        /// Subsidy reppository mock
        /// </summary>
        protected Mock<IGenericRep<Subsidy>> _subsidyRepMock;

        /// <summary>
        /// User Repository mock
        /// </summary>
        protected Mock<IGenericRep<User>> _userRepMock;

        /// <summary>
        /// Interface to Send Mails
        /// </summary>
        protected Mock<ISendGridExternalService> _sendMailServiceMock;

        /// <summary>
        /// Pdi Request Mock
        /// </summary>
        protected SubsidyRequest SubsidyRequestMock;

        /// <summary>
        /// pdi Business Logic
        /// </summary>
        protected SubsidyBl subsidyBusinessLogic;

        public SubsidyBITestBase()
        {
            _sendMailServiceMock = new Mock<ISendGridExternalService>();
            _subsidyRepMock = new Mock<IGenericRep<Subsidy>>();
            _userRepMock = new Mock<IGenericRep<User>>();
            IOptions<UserSecretSettings> options = Options.Create<UserSecretSettings>(new UserSecretSettings());
            subsidyBusinessLogic = new SubsidyBl(_subsidyRepMock.Object, _userRepMock.Object, options, _sendMailServiceMock.Object);
            SetEntitiesMocks();
        }

        private void SetEntitiesMocks()
        {
            SubsidyRequestMock = new SubsidyRequest
            {
                NoSubsidyRequest = "2018120612547",
                UserName = "1069444555_2"
            };
        }
    }
}