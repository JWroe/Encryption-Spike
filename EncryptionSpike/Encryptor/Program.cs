using System;
using System.Diagnostics;
using System.IO;
using EncryptionHelpers;

namespace Encryptor
{
    class Program
    {
        private const string PublicKeyPath = @".\public-key.rsa";
        private const string InputFilePath = @".\FakeFile.csv";
        private const string OutputFilePath = @".\EncryptedFakeFile.csv.Encrypted";

        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            var toEncrypt = File.ReadAllText(InputFilePath);
            var encrypted = toEncrypt.AesEncrypt();

            Console.WriteLine($"encrypted data in {watch.Elapsed}, encrypted data is {encrypted.CipherText.Length} bytes");

            watch.Restart();

            var publicRsaKey = File.ReadAllBytes(PublicKeyPath);
            var encryptedAesKey = encrypted.Key.RsaEncrypt(publicRsaKey);
            var outputFile = new byte[encrypted.CipherText.Length + encrypted.IV.Length + encryptedAesKey.Length];

            Array.Copy(encrypted.IV, 0, outputFile, 0, encrypted.IV.Length);
            Array.Copy(encryptedAesKey, 0, outputFile, encrypted.IV.Length, encryptedAesKey.Length);
            Array.Copy(encrypted.CipherText, 0, outputFile, encrypted.IV.Length + encryptedAesKey.Length, encrypted.CipherText.Length);

            File.WriteAllBytes(OutputFilePath, encrypted.CipherText);

            Console.WriteLine($"written encrypted data to output file in {watch.Elapsed}");

            Console.ReadKey();
        }
    }
}