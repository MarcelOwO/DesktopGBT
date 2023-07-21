using System.Security.Cryptography;
using System.Text;

namespace DesktopGBT;

internal static class Crypt
{
    internal static string GenerateRandomString(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[4];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return res.ToString();
    }

    internal static string Decrypt(byte[] cipherText, string encryptionKey, string iv)
    {
        string plaintext = null;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
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
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
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
        string variableValue = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);
        return variableValue != null;
    }

}