using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace SfSdk
{
    /// <summary>
    ///     Contains various extension methods used in the SfSdk.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        ///     Gets the unix time stamp from a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> object to be converted.</param>
        /// <returns>A unix time stamp as <see cref="double"/>.</returns>
        public static double ToUnixTimeStamp(this DateTime date)
        {
            TimeSpan t = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return t.TotalMilliseconds;
        }

        /// <summary>
        ///     Gets the MD5 hash from a <see cref="string"/>.
        /// </summary>
        /// <param name="input">The string to be hashed.</param>
        /// <returns>The MD5 hash as <see cref="string"/>.</returns>
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

        /// <summary>
        ///     Tries to convert a string to type T.
        /// </summary>
        /// <typeparam name="T">The expected type of the input string.</typeparam>
        /// <param name="input">The input string.</param>
        /// <returns>If the conversion succeeds the value of the converted input, if not the default value of type T.</returns>
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