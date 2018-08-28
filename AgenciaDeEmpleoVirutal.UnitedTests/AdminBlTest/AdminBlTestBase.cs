namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using System;
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Moq;

    public class AdminBlTestBase : BusinessBase<User>
    {
        protected Mock<IGenericRep<User>> FuncionaryRepMock;

        protected AdminBl AdminBusinessLogic;

        protected CreateFuncionaryRequest FuncionatyGodrequest;

        protected CreateFuncionaryRequest FuncionatyBadrequest;

        protected UpdateFuncionaryRequest FuncionatyUpdateRequest;

        protected User MockInfoUser;

        public AdminBlTestBase()
        {
            FuncionaryRepMock = new Mock<IGenericRep<User>>();
            AdminBusinessLogic = new AdminBl(FuncionaryRepMock.Object);
            LoadMoqs();
        }

        private void LoadMoqs()
        {
            MockInfoUser = new User()
            {
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "jgilg@colsubsidio.com",                
            };

            FuncionatyUpdateRequest = new UpdateFuncionaryRequest()
            {
                InternalMail = "juang",
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = true
            };

            FuncionatyGodrequest = new CreateFuncionaryRequest()
            {
                InternalMail = "pepe12",
                State = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador",
            };

            FuncionatyBadrequest = new CreateFuncionaryRequest()
            {
                InternalMail = "",
                State = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador",
            };
        }
    }
}
