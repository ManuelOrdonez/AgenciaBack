
namespace AgenciaDeEmpleoVirutal.UnitedTests.ParametersTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Moq;

    public class ParameterBITestBase : BusinessBase<ParametersResponse>
    {
        protected Mock<IGenericRep<Parameters>> _parameterRep;

        protected ParameterBI DepBussines;

        public ParameterBITestBase()
        {
            _parameterRep = new Mock<IGenericRep<Parameters>>();
            DepBussines = new ParameterBI(_parameterRep.Object);
        }
    }
}

