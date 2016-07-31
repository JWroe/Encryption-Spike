using System;
using System.Diagnostics;
using System.IO;
using EncryptionHelpers;

namespace Encryptor
{
    class Program
    {
        private const string PublicKeyPath = @"..\..\..\output\public-key.rsa";
        private const string InputFilePath = @"..\..\..\output\FakeFile.csv";
        private const string OutputFilePath = @"..\..\..\output\FakeFile.Encrypted";

        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            var toEncrypt = File.ReadAllText(InputFilePath);
            var aesEncryptionDetails = toEncrypt.AesEncrypt();

            Console.WriteLine($"encrypted data in {watch.Elapsed}, encrypted data is {aesEncryptionDetails.CipherText.Length} bytes");

            watch.Restart();

            var publicRsaKey = File.ReadAllBytes(PublicKeyPath);
            var encryptedAesKey = aesEncryptionDetails.Key.RsaEncrypt(publicRsaKey);
            var outFileBytes = new byte[aesEncryptionDetails.CipherText.Length + aesEncryptionDetails.IV.Length + encryptedAesKey.Length];

            Array.Copy(aesEncryptionDetails.IV, outFileBytes, aesEncryptionDetails.IV.Length);
            Array.Copy(encryptedAesKey, sourceIndex: 0, destinationArray: outFileBytes, destinationIndex: aesEncryptionDetails.IV.Length, length: encryptedAesKey.Length);
            Array.Copy(aesEncryptionDetails.CipherText, sourceIndex: 0, destinationArray: outFileBytes, destinationIndex: encryptedAesKey.Length + aesEncryptionDetails.IV.Length, length: aesEncryptionDetails.CipherText.Length);

            File.WriteAllBytes(OutputFilePath, outFileBytes);

            Console.WriteLine($"written encrypted data to output file in {watch.Elapsed}");
        }
    }
}