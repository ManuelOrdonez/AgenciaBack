namespace AgenciaDeEmpleoVirutal.Utils
{
    using System;
    using System.Linq;

    public static class RandomGenerator
    {
        /// <summary>
        /// The random
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Randoms the number.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
