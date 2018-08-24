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

        public AdminBlTestBase()
        {
            FuncionaryRepMock = new Mock<IGenericRep<User>>();
            AdminBusinessLogic = new AdminBl(FuncionaryRepMock.Object);
            LoadMoqs();
        }

        private void LoadMoqs()
        {
            FuncionatyGodrequest = new CreateFuncionaryRequest()
            {
                InternalMail = "pepe12",
                State = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador",
                //Role = UsersTypes.Orientador.ToString()
            };

            FuncionatyBadrequest = new CreateFuncionaryRequest()
            {
                InternalMail = "",
                State = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador",
                //Role = UsersTypes.Orientador.ToString()
            };
        }
    }
}
