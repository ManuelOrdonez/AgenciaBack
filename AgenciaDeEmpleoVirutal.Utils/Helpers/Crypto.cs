namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class Crypto : IDisposable
    {
        
        private RijndaelManaged myRijndael = new RijndaelManaged();

        /// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        /// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "colsubsidiovecto";
        /// This constant is used to determine the keysize of the encryption algorithm
        private const int Keysize = 256;

        public Crypto()
        {
        }

        public static string DecryptPhone(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static string DecryptWeb(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                /// extract salt (first 16 bytes)
                var salt = cipherBytes.Take(16).ToArray();
                /// extract iv (next 16 bytes)
                var iv = cipherBytes.Skip(16).Take(16).ToArray();
                /// the rest is encrypted data
                var encrypted = cipherBytes.Skip(32).ToArray();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt, 100);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.Padding = PaddingMode.PKCS7;
                encryptor.Mode = CipherMode.CBC;
                encryptor.IV = iv;
                /// you need to decrypt this way, not the way in your question
                using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            myRijndael.Dispose();
        }
    }
}
