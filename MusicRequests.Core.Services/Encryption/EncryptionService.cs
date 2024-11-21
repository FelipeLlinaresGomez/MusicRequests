using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MusicRequests.Core.Services
{
    public class EncryptionService : IEncryptionService
    {
        internal const string DEEPLINKING_CALL_SECRET = "8732CFFF-75BA-4E80-BE3A-84D3EF2E1119";

        public bool VerifyDeepLinkHash(string hash, string timestamp)
        {
            var myHash = CalculateMd5Hash(timestamp);
            return hash.Equals(myHash);
        }

        public string CalculateMd5Hash(string value)
        {
            byte[] dataTB = Encoding.UTF8.GetBytes(value);
            var hash = MD5.HashData(dataTB);

            var hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        private const int Keysize = 256;

        public string Encrypt(string value, string password)
        { 
            byte[] salt = new byte[16]; // A random salt should be generated and stored securely
            RandomNumberGenerator.Create().GetBytes(salt);

            var encryptionKey = CreateKey(password, salt);
            var data = Encoding.UTF8.GetBytes(value);

            byte[] cyphertextBytes;
            using var aes = Aes.Create();

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = encryptionKey;
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length);
                memoryStream.Write(salt, 0, salt.Length);

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                }

                cyphertextBytes = memoryStream.ToArray();
                return Convert.ToBase64String(cyphertextBytes);
            }
        }

        public string Decrypt(string value, string password)
        {
            var cipherText = Convert.FromBase64String(value);

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extract the IV from the beginning of the cipherTextBytes
                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(cipherText, 0, iv, 0, iv.Length);
                aes.IV = iv;

                // Extract the salt from the beginning of the cipherTextBytes
                var salt = new byte[16];
                Array.Copy(cipherText, iv.Length, salt, 0, salt.Length);

                var encryptionKey = CreateKey(password, salt);
                aes.Key = encryptionKey;

                var actualCipherText = new byte[cipherText.Length - iv.Length - salt.Length];
                Array.Copy(cipherText, iv.Length + salt.Length, actualCipherText, 0, actualCipherText.Length);

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(actualCipherText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }

        }

        #region CreateKey

        /// <summary>
        /// Creates the encryption key based on the password
        /// </summary>
        /// <returns>The key.</returns>
        /// <param name="password">Password.</param>
        private byte[] CreateKey(string password, byte[] salt)
        {
            int iterations = 5000; // higher makes brute force attacks more expensive
            int keyLengthInBytes = 32;

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA1))
            {
                return rfc2898DeriveBytes.GetBytes(keyLengthInBytes);
            }
        }

        #endregion

    }
}

