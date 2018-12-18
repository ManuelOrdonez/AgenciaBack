namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Uper case String Class
    /// </summary>
    public static class UString
    {
        /// <summary>
        /// Method to Uppercase Words
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UppercaseWords(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1 && char.IsLower(array[0]))
            {
                array[0] = char.ToUpper(array[0]);
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
                else
                {
                    if (char.IsUpper(array[i]))
                    {
                        array[i] = char.ToLower(array[i]);
                    }
                }
            }
            return new string(array);
        }

        /// <summary>
        /// Method to Capitalize First Letter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(string value)
        {
            string auxValue = string.Empty;
            auxValue = value;
            auxValue = auxValue.ToLower(new CultureInfo("es-CO"));
            char[] array = auxValue.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1 && char.IsLower(array[0]))
            {
                array[0] = char.ToUpper(array[0]);
            }
            return new string(array);
        }

    }
}
