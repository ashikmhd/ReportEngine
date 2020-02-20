using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.General
{
    public class MD5Encoder
    {
        /// <summary>
        /// Encodes the inputstring
        /// </summary>
        /// <param name="strText">string to encode</param>
        /// <returns></returns>
        public static string Encode(string strText)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = null;

            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(strText));
            String ba = Convert.ToBase64String(hashedDataBytes);
            return (ba.ToString());
        }


        /// <summary>
        ///  Verifies a hash against a string.
        /// </summary>
        /// <param name="input">Actual string to compare</param>
        /// <param name="hash">Encoded string</param>
        /// <returns></returns>
        public static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = Encode(input);

            // Create a StringComparer an comare the hashes.
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
    }
}
