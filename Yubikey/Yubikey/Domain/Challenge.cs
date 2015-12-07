using System;
using System.IO;
using System.Text;

namespace Yubikey.Domain
{
    public class Challenge
    {
        /**
         * <p>
         *   Utility methods to 
         *   {@link #Encode(byte[]) Encode} a byte array to Challenge 
         *   {@code String} and to
         *   {@link #Decode(String) Decode} a Challenge
         *   {@link String} to a the byte array it represents.   
         * </p>
         * <p>
         *   Modehex encoding uses the 
         *   {@link #ALPHABET alphabet} {@code cbdefghijklnrtuv} which has the property
         *   of being at the same position on all keyboards.
         * </p>
         * @author Simon
         */
        private Challenge() { } // Utility pattern dictates private constructor.

        /**
        * <p>
        *   The Challenge alphabet: the letters used to Decode bytes in.
        * </p>
        */
        public static String ALPHABET = "cbdefghijklnrtuv";

        private static char[] trans = ALPHABET.ToCharArray();

        /**
         * <p>
         *   Encodes.
         * </p>
         * @param data Data to Encode.
         * @return Challenge.
         */
        public static String Encode(byte[] data)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                result.Append(trans[(data[i] >> 4) & 0xf]);
                result.Append(trans[data[i] & 0xf]);
            }

            return result.ToString();
        }

        /**
         * <p>
         *   Decodes.
         * </p>
         * @param s Challenge encoded
         *          {@link String}. Decoding ignores case of {@code s}.
         * @return Bytes {@code s} represents.
         * @throws IllegalArgumentException If {@code s} not valid Challenge.
         */
        public static byte[] Decode(String s)
        {
            MemoryStream baos = new MemoryStream();
            int len = s.Length;

            bool toggle = false;
            int keep = 0;

            for (int i = 0; i < len; i++)
            {
                char ch = s[i];
                int n = ALPHABET.IndexOf(char.ToLower(ch));
                if (n == -1)
                {
                    throw new
                      Exception(s + " is not properly encoded");
                }

                toggle = !toggle;

                if (toggle)
                {
                    keep = n;
                }
                else
                {
                    baos.WriteByte((byte)((keep << 4) | n));
                }
            }
            return baos.ToArray();
        }
    }
}