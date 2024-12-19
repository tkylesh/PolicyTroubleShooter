using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPICommon
{
  public static class Extensions
  {
        /// <summary>
        /// Decrypts a string of text using a Rijndael method with a pre-defined set of settings 
        /// </summary>
        /// <param name="encryptedString">String to be decrypted</param>
        /// <returns>string</returns>

        public static string Encrypt(this string decryptedString)
        {
            return RijndaelSimple.DoRijndael(decryptedString, EncrytionDirection.Encrypt);
        }

        public static string Decrypt(this string encryptedString)
    {
      return RijndaelSimple.DoRijndael(encryptedString, EncrytionDirection.Decrypt);
    }

        public static string Modulus10(this string s)
        {
            var mod10Array = s.ToCharArray();
            var y = 1;
            var value = 0;
            var sum = 0;
            for (int i = 0; i < mod10Array.Length; i++)
            {
                value = (int)Char.GetNumericValue(mod10Array[i]);
                switch (y)
                {
                    case 1:
                        y = 3;
                        break;
                    case 3:
                        y = 7;
                        break;
                    case 7:
                        y = 1;
                        break;
                }
                sum += value * y;
            }

            var sum2 = ((Math.Truncate((decimal)sum / 10)) + 1) * 10;
            var checkDigit = sum2 - sum;
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }
            return checkDigit.ToString();
        }

    public static string Base10(this string s)
    {
        var base10Array = s.ToCharArray();
        var y = 1;
        var value = 0;
        var sum = 0;
        for (int i = 0; i < base10Array.Length; i++)
        {
            value = (int)Char.GetNumericValue(base10Array[i]);
            sum += value;
        }

        var sum2 = ((Math.Truncate((decimal)sum / 10)) + 1) * 10;
        var checkDigit = sum2 - sum;
        if (checkDigit == 10)
        {
            checkDigit = 0;
        }
        return checkDigit.ToString();
    }
 }

}