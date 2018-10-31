namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using System.Collections.Generic;

    /// <summary>
    /// Interface of Parameters Business logic
    /// </summary>
    public interface IMenuBl
    {
        Response<List<Menu>> GetMenu(GetMenuRequest request);
    }
}
