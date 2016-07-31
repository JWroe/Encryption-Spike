using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EncryptionHelpers;

namespace Decryptor
{
    class Program
    {
        private const string PrivateKeyPath = @"..\..\..\output\private-key.rsa";
        private const string OutputFilePath = @"..\..\..\output\FakeFile-decrypted.csv";
        private const string InputFilePath = @"..\..\..\output\FakeFile.Encrypted";

        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            var toDecrypt = File.ReadAllBytes(InputFilePath);
            var privateKey = File.ReadAllBytes(PrivateKeyPath);
            var iv = toDecrypt.Take(16).ToArray();
            var encryptedKey = toDecrypt.Skip(iv.Length).Take(128).ToArray();
            var cipherText = toDecrypt.Skip(iv.Length + encryptedKey.Length).ToArray();
            var encryptionDetails = new AesEncryptionDetails
                                    {
                                        IV = iv,
                                        Key = encryptedKey.RsaDecrypt(privateKey),
                                        CipherText = cipherText
                                    };
            var decrypted = encryptionDetails.AesDecrypt();
            Console.WriteLine($"decrypted data in {watch.Elapsed}");

            File.WriteAllText(OutputFilePath, decrypted);

            Console.WriteLine($"written decrypted data to output file in {watch.Elapsed}");
        }
    }
}