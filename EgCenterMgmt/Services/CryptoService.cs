using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace EgCenterMgmt.Services
{
    public static class CryptoService
    {
        private readonly static string _encryptionKey = "12345678901234567890123456789012"; // مفتاح 256 بت

        public static string DecryptData(string encryptedData)
        {
            byte[] ivAndEncrypted = Convert.FromBase64String(encryptedData);

            byte[] iv = ivAndEncrypted.Take(16).ToArray(); // استخراج IV
            byte[] encrypted = ivAndEncrypted.Skip(16).ToArray(); // استخراج البيانات المشفرة

            byte[] key = Encoding.UTF8.GetBytes(_encryptionKey);

            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            cipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));

            byte[] outputData = new byte[cipher.GetOutputSize(encrypted.Length)];
            int length = cipher.ProcessBytes(encrypted, 0, encrypted.Length, outputData, 0);
            length += cipher.DoFinal(outputData, length);

            return Encoding.UTF8.GetString(outputData, 0, length).TrimEnd('\0');
        }
    }
}
