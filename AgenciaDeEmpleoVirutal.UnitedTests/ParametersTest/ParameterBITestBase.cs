
namespace AgenciaDeEmpleoVirutal.UnitedTests.ParametersTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.Extensions.Options;
    using Moq;

    public class ParameterBITestBase : BusinessBase<ParametersResponse>
    {
        protected Mock<IGenericRep<Parameters>> _parameterRep;

        protected ParameterBI ParameterBussines;

        public ParameterBITestBase()
        {
            _parameterRep = new Mock<IGenericRep<Parameters>>();
            IOptions<UserSecretSettings> options = Options.Create<UserSecretSettings>(new UserSecretSettings());
            ParameterBussines = new ParameterBI(_parameterRep.Object, options);
        }
    }
}

