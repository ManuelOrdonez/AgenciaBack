namespace AgenciaDeEmpleoVirutal.UnitedTests.MenuBITest
{
    using System.Collections.Generic;
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.Extensions.Options;
    using Moq;

    /// <summary>
    /// Menu BI Test Base
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.Business.Referentials.BusinessBase{AgenciaDeEmpleoVirutal.Entities.Responses.ParametersResponse}" />
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
        /// The parameter rep result
        /// </summary>
        protected List<Parameters> ParameterRepResult;

        /// <summary>
        /// The menu rep result
        /// </summary>
        protected List<Menu> MenuRepResult;

        /// <summary>
        /// The response operation
        /// </summary>
        protected List<List<Menu>> ResponseOperation;

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
            LoadDataMocks();
        }

        /// <summary>
        /// Loads the data mocks.
        /// </summary>
        private void LoadDataMocks()
        {
            ParameterRepResult = new List<Parameters>
            {
                new Parameters
                {
                    State = true,
                    Description = "Description",
                    ImageFile = "ImageFile",
                    Required = true,
                    Value ="Value",
                    Id ="Id"
                }
            };
            MenuRepResult = new List<Menu>
            {
                new Menu
                {
                    Click = "Click",
                    Html = "Html",
                    Value = "Value",
                    Id = "Id"
                }
            };
            ResponseOperation = new List<List<Menu>>
            {
                MenuRepResult
            };
        }
    }
}
