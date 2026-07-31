using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DigitalPhotoPrintingSystem.Services
{
    public static class EncryptionHelper
    {
        // Secret Key for Encryption/Decryption (32 Characters)
        private static readonly string Key = "12345678901234567890123456789012";

        // Encryption Method (Credit Card Number Encrypt karne ke liye)
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = new byte[16]; // Default 16-byte zero IV for simple AES

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Decryption Method (Credit Card Read karne ke liye)
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}