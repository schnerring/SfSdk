using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace SfSdk
{
    internal static class Helpers
    {
        internal static double ToUnixTimeStamp(this DateTime date)
        {
            TimeSpan t = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return t.TotalMilliseconds;
        }


        public static string ToMd5Hash(this string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }
        }


        public static T Convert<T>(this string input)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
            // TODO catch Invalid
            if (converter != null)
            {
                try
                {
                    //Cast ConvertFromString(string text) : object to (T)
                    return (T) converter.ConvertFromString(input);
                }
                catch (Exception e)
                {
                    throw new NotImplementedException();
                }
            }
            return default(T);
        }
    }
}