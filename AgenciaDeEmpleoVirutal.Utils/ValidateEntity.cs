namespace AgenciaDeEmpleoVirutal.Utils
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public static class ValidateEntity
    {
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
