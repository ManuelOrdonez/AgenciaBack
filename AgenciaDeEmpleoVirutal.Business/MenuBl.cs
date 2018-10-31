
namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Menu Business Iogic
    /// </summary>
    public class MenuBl : BusinessBase<ParametersResponse>, IMenuBl
    {
        private readonly IGenericRep<Menu> _menuRep;
        private readonly IGenericRep<Parameters> _paramentRep;


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="paramentRep"></param>
        public MenuBl(IGenericRep<Parameters> paramentRep, IGenericRep<Menu> menuRep)
        {
            _paramentRep = paramentRep;
            _menuRep = menuRep;
        }

        public Response<List<Menu>> GetMenu(string request)
        {
            var parameter = string.Empty;
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            

            var role = request.Replace(" ", "_");

            if (role == Roles.Administrador.ToString()) { parameter = "menu_administrador"; }
            if (role == Roles.Analista_Revisor_FOSFEC.ToString()) { parameter = "menu_analista"; }
            if (role == Roles.Orientador_Laboral.ToString()) { parameter = "menu_orientador"; }
            if (role == Roles.Supervisor_de_Agencia.ToString()) { parameter = "menu_supervisor"; }
            if (role == Roles.Oferente.ToString()) { parameter = "menu_oferente"; }

            ParameterBI parameterBl = new ParameterBI(_paramentRep);
            var menuRol = parameterBl.GetParametersByType(parameter).Data;

            List<string> menuIds = new List<string>();
            List<Menu> optionsMenu = new List<Menu>();

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

            if (optionsMenu == null || optionsMenu.Count == 0)
            {
                return ResponseFail<List<Menu>>();
            }

            var listMenu = new List<List<Menu>>();

            listMenu.Add(optionsMenu);

            return ResponseSuccess(listMenu);
        }

    }
}
