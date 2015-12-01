using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CourceToWebUC.Model.Helpers
{
    /// <summary>
    /// класс для шифрования правильных ответов тестов
    /// </summary>
    public class Cripto
    {
        public static string GetMd5(string source)
        {
            
            string hash = "";
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, source);

                
            }
            return hash;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static Random rnd;
        /// <summary>
        /// получить случайную последовательность символов
        /// </summary>
        /// <param name="_length">длина последовательности</param>
        /// <returns>последовательность</returns>
        public static string GetRandomSeq(int _length)
        {
            if (_length < 1)
                throw new Exception("Для генерации случайных чисел необходимо указать положительное число.");
            string pass = "";

            if (rnd == null) rnd = new Random();
           
            while (pass.Length < _length)
            {
                Char c = (char)rnd.Next(33, 126);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }
            return pass;
        }

    }
}
