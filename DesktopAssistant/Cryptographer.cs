using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DesktopAssistant
{
    // Кастомный шифровальщик. Специально написан под шифровку/расшифровку набора строк, а не одной строки
    public static class Cryptographer
    {
        public static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string[] Crypt(params string[] stringsArr)
        {
            List<string> strList = new List<string>();

            foreach (var str in stringsArr)
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
                byte[] inputBuffer = Encoding.Unicode.GetBytes(str);
                byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                var encryptedString = Convert.ToBase64String(outputBuffer);
                strList.Add(encryptedString);
            }
            return strList.ToArray();
        }

        public static string[] Decrypt(params string[] stringsArr)
        {
            List<string> strList = new List<string>();

            foreach (var str in stringsArr)
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
                byte[] inputbuffer = Convert.FromBase64String(str);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                var decryptedString = Encoding.Unicode.GetString(outputBuffer);
                strList.Add(decryptedString);
            }
            return strList.ToArray();
        }
    }
}
