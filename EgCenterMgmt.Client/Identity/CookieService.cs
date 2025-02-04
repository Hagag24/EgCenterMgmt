using Blazored.LocalStorage;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace EgCenterMgmt.Client.Identity
{
    public class CookieService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly string _encryptionKey = "12345678901234567890123456789012"; // مفتاح 256 بت

        public CookieService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SetAsync(string Key, string Data)
        {
            try
            {
                string encryptedData = EncryptData(Data);
                await _localStorage.SetItemAsync(Key, encryptedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetAsync(string Key)
        {
            try
            {
                var encryptedData = await _localStorage.GetItemAsync<string>(Key);

                if (string.IsNullOrEmpty(encryptedData))
                {
                    return string.Empty;
                }

                return encryptedData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting data: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task RemoveAsync(string Key)
        {
            try
            {
                await _localStorage.RemoveItemAsync(Key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing data: {ex.Message}");
            }
        }
        private string EncryptData(string data)
        {
            byte[] key = Encoding.UTF8.GetBytes(_encryptionKey);
            byte[] iv = new byte[16];
            new Random().NextBytes(iv); // توليد IV عشوائي

            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            cipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));

            byte[] inputData = Encoding.UTF8.GetBytes(data);
            byte[] outputData = new byte[cipher.GetOutputSize(inputData.Length)];
            int length = cipher.ProcessBytes(inputData, 0, inputData.Length, outputData, 0);
            length += cipher.DoFinal(outputData, length);

            byte[] ivAndEncrypted = iv.Concat(outputData).ToArray();
            return Convert.ToBase64String(ivAndEncrypted); // تخزين IV مع البيانات المشفرة
        }
    }
}
