using System.Security.Cryptography;
using System.Text;

namespace DesktopGBT;

internal static class Crypt
{
    internal static string GenerateRandomString(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var res = new StringBuilder();
        using (var rng = new RNGCryptoServiceProvider())
        {
            var uintBuffer = new byte[4];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                var num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int) (num % (uint) valid.Length)]);
            }
        }

        return res.ToString();
    }

    internal static string Decrypt(byte[] cipherText, string encryptionKey, string iv)
    {
        var plaintext = string.Empty;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(cipherText))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    plaintext = sr.ReadToEnd();
                }
            }
        }

        return plaintext;
    }

    internal static byte[] Encrypt(string clearText, string encryptionKey, string iv)
    {
        byte[] encrypted;
        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(clearText);
                }

                encrypted = ms.ToArray();
            }
        }

        return encrypted;
    }

    public static bool DoesEnvironmentVariableExist(string variableName)
    {
        var variableValue = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);
        return variableValue != null;
    }
}