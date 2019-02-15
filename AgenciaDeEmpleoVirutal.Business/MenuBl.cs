namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Menu Business Iogic
    /// </summary>
    public class MenuBl : BusinessBase<ParametersResponse>, IMenuBl
    {
        /// <summary>
        /// The menu rep
        /// </summary>
        private readonly IGenericRep<Menu> _menuRep;

        /// <summary>
        /// The parament rep
        /// </summary>
        private readonly IGenericRep<Parameters> _paramentRep;

        /// <summary>
        /// The user secret settings
        /// </summary>
        private readonly IOptions<UserSecretSettings> _UserSecretSettings;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="paramentRep"></param>
        public MenuBl(IGenericRep<Parameters> paramentRep, IGenericRep<Menu> menuRep, IOptions<UserSecretSettings> options)
        {
            _paramentRep = paramentRep;
            _menuRep = menuRep;
            _UserSecretSettings = options;
        }

        /// <summary>
        /// Get menu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<List<Menu>> GetMenu(string request)
        {
            var parameter = string.Empty;
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            
            var role = request.Replace(" ", "_");

            if (role == Roles.Administrador.ToString())
            {
                parameter = "menu_administrador";
            }
            if (role == Roles.Analista_Revisor_FOSFEC.ToString())
            {
                parameter = "menu_analista";
            }
            if (role == Roles.Orientador_Laboral.ToString())
            {
                parameter = "menu_orientador";
            }
            if (role == Roles.Supervisor_de_Agencia.ToString())
            {
                parameter = "menu_supervisor";
            }
            if (role == Roles.Oferente.ToString())
            {
                parameter = "menu_oferente";
            }
            if (role == "empresa")
            {
                parameter = "menu_oferente_empresa";
            }

            ParameterBI parameterBl = new ParameterBI(_paramentRep, _UserSecretSettings);
            var menuRol = parameterBl.GetParametersByType(parameter).Data;

            List<Menu> optionsMenu = new List<Menu>();

            optionsMenu = ListsMenus(menuRol);

            if (optionsMenu == null || optionsMenu.Count == 0)
            {
                return ResponseFail<List<Menu>>();
            }

            var listMenu = new List<List<Menu>>();
            listMenu.Add(optionsMenu);

            return ResponseSuccess(listMenu);
        }

        /// <summary>
        /// Lists menus.
        /// </summary>
        /// <param name="menuRol"></param>
        /// <returns></returns>
        private List<Menu> ListsMenus(IList<ParametersResponse> menuRol)
        {
            List<string> menuIds = new List<string>();
            List<Menu> optionsMenu = new List<Menu>();
            if (menuRol is null)
            {
                return optionsMenu;
            }

            foreach (var param in menuRol)
            {
                if (param.State)
                {
                    menuIds.Add(param.Value);
                }
            }

            foreach (var idMenu in menuIds)
            {
                optionsMenu.Add(_menuRep.GetByPatitionKeyAsync(idMenu).Result.FirstOrDefault());
            }

            return optionsMenu;
        }
    }
}
