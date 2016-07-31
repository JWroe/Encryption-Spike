using System.IO;
using System.Security.Cryptography;

namespace EncryptionHelpers
{
    public static class EncryptorMethods
    {
        public static byte[] RsaEncrypt(this byte[] dataToEncrypt, byte[] publicKey)
        {
            byte[] encryptedData;
            using (var rsaServiceProvider = new RSACryptoServiceProvider())
            {
                rsaServiceProvider.ImportCspBlob(publicKey);
                encryptedData = rsaServiceProvider.Encrypt(dataToEncrypt, true);
            }
            return encryptedData;
        }

        public static byte[] RsaDecrypt(this byte[] cipherText, byte[] privateKey)
        {
            byte[] decryptedData;
            using (var rsaServiceProvider = new RSACryptoServiceProvider())
            {
                rsaServiceProvider.ImportCspBlob(privateKey);
                decryptedData = rsaServiceProvider.Decrypt(cipherText, true);
            }
            return decryptedData;
        }

        public static AesEncryptionDetails AesEncrypt(this string dataToEncrypt)
        {
            using (var aesAlg = Aes.Create())
            {
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(dataToEncrypt);
                        }
                        return new AesEncryptionDetails
                               {
                                   CipherText = msEncrypt.ToArray(),
                                   IV = aesAlg.IV,
                                   Key = aesAlg.Key
                               };
                    }
                }
            }
        }

        public static string AesDecrypt(this AesEncryptionDetails encryptionDetails)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionDetails.Key;
                aesAlg.IV = encryptionDetails.IV;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(encryptionDetails.CipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}