namespace EncryptionHelpers
{
    public class AesEncryptionDetails
    {
        public byte[] CipherText { get; set; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
    }
}