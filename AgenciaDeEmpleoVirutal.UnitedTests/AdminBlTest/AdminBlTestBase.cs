namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using Moq;

    public class AdminBlTestBase : BusinessBase<User>
    {
        public Mock<IGenericRep<User>> FuncionaryRepMock;

        public AdminBl AdminBusinessLogic;

        public AdminBlTestBase()
        {
            FuncionaryRepMock = new Mock<IGenericRep<User>>();
            AdminBusinessLogic = new AdminBl(FuncionaryRepMock.Object);
        }
    }
}
