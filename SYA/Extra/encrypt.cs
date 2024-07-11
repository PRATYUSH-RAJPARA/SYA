using System.Security.Cryptography;
using System.Text;
namespace SYA
{
    public class encrypt
    {
        // Encrypt data using passphrase
        public static string EncryptData(string data, string passphrase)
        {
            using (Aes aes = Aes.Create())
            {
                // Generate random initialization vector (IV)
                aes.GenerateIV();
                byte[] iv = aes.IV;
                // Derive encryption key from passphrase
                byte[] key = DeriveKeyFromPassphrase(passphrase, aes.KeySize / 8);
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                // Encrypt the data
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
                {
                    byte[] encryptedData = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(data), 0, data.Length);
                    byte[] combinedData = new byte[iv.Length + encryptedData.Length];
                    Array.Copy(iv, 0, combinedData, 0, iv.Length);
                    Array.Copy(encryptedData, 0, combinedData, iv.Length, encryptedData.Length);
                    return Convert.ToBase64String(combinedData);
                }
            }
        }
        // Decrypt data using passphrase
        public static string DecryptData(string encryptedData, string passphrase)
        {
            byte[] combinedData = Convert.FromBase64String(encryptedData);
            byte[] iv = new byte[16]; // Initialization vector length
            byte[] encryptedBytes = new byte[combinedData.Length - iv.Length];
            // Extract IV and encrypted bytes from combined data
            Array.Copy(combinedData, iv, iv.Length);
            Array.Copy(combinedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);
            // Derive encryption key from passphrase
            byte[] key = DeriveKeyFromPassphrase(passphrase, 256 / 8); // Assuming 256-bit key
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                // Decrypt the data
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        // Derive encryption key from passphrase
        private static byte[] DeriveKeyFromPassphrase(string passphrase, int keySize)
        {
            byte[] salt = Encoding.UTF8.GetBytes("YourSaltValue"); // Salt value for key derivation
            using (Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(passphrase, salt, 10000))
            {
                return deriveBytes.GetBytes(keySize);
            }
        }
    }
}