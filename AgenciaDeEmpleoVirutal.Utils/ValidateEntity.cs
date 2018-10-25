namespace AgenciaDeEmpleoVirutal.Utils
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Validate Entity Class
    /// </summary>
    public static class ValidateEntity
    {
        /// <summary>
        /// Method to validate properties od any entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public static IList<string> Validate<T>(this T entidad) where T : class, new()
        {
            var contextoValidador = new ValidationContext(entidad);
            var errores = new List<ValidationResult>();
            var respuesta = new List<string>();
            Validator.TryValidateObject(entidad, contextoValidador, errores, true);
            errores.ForEach(e => respuesta.Add(e.ErrorMessage));
            return respuesta;
        }
    }
}
