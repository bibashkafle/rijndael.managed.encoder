using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rijindael
{
    /// <summary>
    /// Utility class for encoding and decoding Yubico's MODHex
    /// format.
    /// </summary>
    /// <remarks>
    /// <para>MODHex is the format used to encode Yubikey authentication
    /// strings.  Rather than the typical encoding formats, the
    /// alphabet used by MODHex (<c>cbdefghijklnrtuv</c>) has the advantage
    /// of being in the same location on all known keyboard layouts (or so
    /// YubiCo syas).  This makes it ideal for encoding, since the YubiKey
    /// generates fake keyboard events to simulate typing by the user.</para>
    /// <para>The general idea here are pretty much taken from the
    /// YubiCo-J project.  However, the lookup-table-based implementation is
    /// new to this C# port.</para>
    /// </remarks>
    /// <seealso cref="http://code.google.com/p/yubico-j/"/>

    public static partial class ModHex
    {
        public const string ALPHABET = "cbdefghijklnrtuv";
        public const string DVORAK_ALPHABET = "jxe.uidchtnbpygk";
        private static readonly char[] charTable = ALPHABET.ToCharArray();
        private const short INVALID = -1;
        private const int TABLE_OFFSET = (int)'A';

        /// <summary>
        /// Encodes an octet stream in MODHex.
        /// </summary>
        /// <param name="bytes">The bytes to be encoded.  Must not be null.</param>
        /// <returns>The encoded bytes.  An empty array will return an empty string.</returns>
        public static String Encode(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            StringBuilder sb = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(charTable[(bytes[i] >> 4) & 0x0F]);
                sb.Append(charTable[bytes[i] & 0x0F]);
            }

            return sb.ToString();
        }
        
        /// <summary>
        /// Decodes a MODHex-encoded string back into an octet array.
        /// </summary>
        /// <param name="encodedBytes">The bytes to be decoded.  May not be null,
        /// but an empty string is acceptable.</param>
        /// <returns>The decoded data.  If an empty string is passed in, then an empty
        /// array will be returned.</returns>
        /// <exception cref="ModHexDecodeException">Thrown if there is a problem with the encoding.</exception>
        
        /*
        public static byte[] Decode(string encodedBytes)
        {
            if (encodedBytes == null)
            {
                throw new ArgumentNullException("encodedBytes");
            }

            if (encodedBytes.Length % 2 != 0)
            {
                throw new ModHexEncodingException("Bad encoding.  Length is not even.");
            }

            byte[] bytes = new byte[encodedBytes.Length / 2];
            int ptr = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                int c1 = (int)(encodedBytes[ptr++]) - TABLE_OFFSET;

                if (c1 < 0 || c1 > LOOKUP_WIDTH)
                {
                    throw new ModHexEncodingException(string.Format("Bad encoding: {0}", encodedBytes[ptr - 1]));
                }

                int c2 = (int)(encodedBytes[ptr++]) - TABLE_OFFSET;

                if (c2 < 0 || c2 > LOOKUP_HEIGHT)
                {
                    throw new ModHexEncodingException(string.Format("Bad encoding: {0}", encodedBytes[ptr - 1]));
                }

                short v = LOOKUP[c1, c2];

                if (v == INVALID)
                {
                    throw new ModHexEncodingException("Bad encoding.");
                }

                bytes[i] = (byte)v;
            }

            return bytes;
        }
         * */

        public static string ConvertToDvorak(string qwerty)
        {
            StringBuilder sb = new StringBuilder(qwerty.Length);
            char[] cs = qwerty.ToLower().ToCharArray();

            for (int i = 0; i < cs.Length; i++)
            {
                int index = ALPHABET.IndexOf(cs[i]);

                if (index < 0)
                {
                    throw new ModHexEncodingException("Bad character: " + cs[i]);
                }

                sb.Append(DVORAK_ALPHABET[index]);
            }

            return sb.ToString();
        }

        public static string ConvertFromDvorak(string dvorak)
        {
            StringBuilder sb = new StringBuilder(dvorak.Length);
            char[] cs = dvorak.ToLower().ToCharArray();

            for (int i = 0; i < cs.Length; i++)
            {
                int index = DVORAK_ALPHABET.IndexOf(cs[i]);

                if (index < 0)
                {
                    throw new DvorakEncodingException("Bad character: " + cs[i]);
                }

                sb.Append(ALPHABET[index]);
            }

            return sb.ToString();
        }
    }

    public class ModHexEncodingException : Exception
    {
        public ModHexEncodingException(string message) : base(message) {}
    }

    public class DvorakEncodingException : ModHexEncodingException
    {
        public DvorakEncodingException(string message) : base(message) {}
    }
}
