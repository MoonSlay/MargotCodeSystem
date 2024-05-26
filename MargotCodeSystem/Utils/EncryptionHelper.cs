using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MargotCodeSystem.Utils
{
    public static class EncryptionHelper
    {
        private const string EncryptionKey = "s3cureK3y!123456789012345678901234";
        private const int IterationsCount = 100000;  // Increased iterations for stronger security
        private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        public static string EncryptString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(text);
                using (Aes encryptor = Aes.Create())
                {
                    // Use the constructor with HashAlgorithmName and IterationsCount
                    using (var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt, IterationsCount, HashAlgorithmName.SHA256))
                    {
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(clearBytes, 0, clearBytes.Length);
                            }
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (logging code would go here)
                throw new Exception("An error occurred during encryption.", ex);
            }
        }

        public static string DecryptString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            try
            {
                text = text.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(text);
                using (Aes encryptor = Aes.Create())
                {
                    // Use the constructor with HashAlgorithmName and IterationsCount
                    using (var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt, IterationsCount, HashAlgorithmName.SHA256))
                    {
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                            }
                            return Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch (FormatException)
            {
                // Log specific FormatException for Base-64 errors
                throw new FormatException("The input is not a valid Base-64 string. Ensure the string is correctly encoded and has not been altered.");
            }
            catch (Exception ex)
            {
                // Log other exceptions
                throw new Exception("An error occurred during decryption.", ex);
            }
        }
    }
}
