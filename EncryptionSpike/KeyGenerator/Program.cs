using System;
using System.IO;
using System.Security.Cryptography;

namespace KeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var rsa = new RSACryptoServiceProvider())
            {
                var publicKey = rsa.ExportCspBlob(includePrivateParameters: false);
                var privateKey = rsa.ExportCspBlob(includePrivateParameters: true);
                File.WriteAllBytes(@".\public-key.rsa", publicKey);
                File.WriteAllBytes(@".\private-key.rsa", privateKey);
            }
            Console.WriteLine("New keys generated. Press any key to exit.");
            Console.ReadKey();
        }
    }
}