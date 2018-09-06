
namespace AgenciaDeEmpleoVirutal.UnitedTests.DepartamentBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Moq;

    public class DepartamentBlTestBase : BusinessBase<DepartamenCityResponse>
    {
        protected Mock<IGenericRep<DepartamenCity>> _depCityRep;

        protected DepartamentBl DepBussines;

        public DepartamentBlTestBase()
        {
            _depCityRep = new Mock<IGenericRep<DepartamenCity>>();
            DepBussines = new DepartamentBl(_depCityRep.Object);
        }
    }
}
