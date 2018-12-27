namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Enum Values.
    /// </summary>
    public static class EnumValues
    {
        /// <summary>
        /// Get value from description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException("Not found.", "description");
        }

        /// <summary>
        /// Get description from value.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string GetDescriptionFromValue(this Enum valor)
        {
            DescriptionAttribute atributoDescripcion = new DescriptionAttribute();
            if (valor != null)
            {
                FieldInfo enumInfo = valor.GetType().GetField(valor.ToString());
                atributoDescripcion = Attribute.GetCustomAttribute(enumInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return atributoDescripcion == null ? valor.ToString() : atributoDescripcion.Description;
            }
            return string.Empty;
        }
    }
}
