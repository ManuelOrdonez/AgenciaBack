namespace AgenciaDeEmpleoVirutal.UnitedTests.MenuBITest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.Extensions.Options;
    using Moq;

    public class MenuBITestBase : BusinessBase<ParametersResponse>
    {
        /// <summary>
        /// Menu Repository
        /// </summary>
        protected Mock<IGenericRep<Menu>> _menuRepMock;

        /// <summary>
        /// Parameter Repository
        /// </summary>
        protected Mock<IGenericRep<Parameters>> _paramentRepMock;

        /// <summary>
        /// Menu Business Logic
        /// </summary>
        protected MenuBl menuBusinessLogic;

        /// <summary>
        /// User name.
        /// </summary>
        protected string Rol;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="paramentRep"></param>
        public MenuBITestBase()
        {
            _paramentRepMock = new Mock<IGenericRep<Parameters>>();
            _menuRepMock = new Mock<IGenericRep<Menu>>();
            IOptions<UserSecretSettings> options = Options.Create<UserSecretSettings>(new UserSecretSettings());
            menuBusinessLogic = new MenuBl(_paramentRepMock.Object, _menuRepMock.Object, options);
            Rol = "Orientador_Laboral";
        }
    }
}
