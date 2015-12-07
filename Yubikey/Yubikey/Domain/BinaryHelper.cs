using System;

namespace Yubikey.Domain
{
	public class BinaryHelper
	{
		/// <summary>
		/// Compare two arrays.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static bool AreEqual(byte[] x, byte[] y)
		{
			if (x.Length != y.Length)
			{
				return false;
			}

			for (int i = 0; i < x.Length; i++)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Convert a array of characters to bytes
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] GetBytes(char[] chars)
		{
			byte[] bytes = new byte[chars.Length * 2];
			for (int i = 0; i < chars.Length; i++)
			{
				// set the high order byte
				bytes[i * 2 + 1] = (byte)((int)chars[i] >> 8);

				// set the low order byte.  Cast devalues high order bytes.
				bytes[i * 2] = (byte)(chars[i]);
			}
			return bytes;
		}

		/// <summary>
		/// Convert a array of characters to bytes. Only preserve lower order bytes
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] GetLowOrderBytes(char[] chars)
		{
			byte[] bytes = new byte[chars.Length];
			for (int i = 0; i < chars.Length; i ++)
			{
				// Cast devalues high order bytes.
				bytes[i] = (byte)chars[i];
			}
			return bytes;
		}

		/// <summary>
		/// Get an array of character from bytes.  The high order bytes are 0.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static char[] GetCharsOneBytePer(byte[] bytes)
		{
			char[] chars = new char[bytes.Length];
			for (int i = 0; i < bytes.Length; i++)
			{
				chars[i] = (char) bytes[i];
			}
			return chars;
		}

		/// <summary>
		/// Get an array of character from bytes.  Every character is two bytes.
		/// The first character is the low order byte. the second is the high order byte.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static char[] GetCharsTwoBytesPer(byte[] bytes)
		{
			if (bytes.Length % 2 != 0)
			{
				throw new ArgumentException("the bytes size must be even or data loss will occur.");
			}

			int charLength = bytes.Length / 2;
			char[] chars = new char[charLength];
			for (int i = 0; i < charLength; i++)
			{
				chars[i] = (char)((bytes[i * 2 + 1] << 8) + (int)bytes[i * 2]);				
			}
			return chars;
		}

		/// <summary>
		/// Append to byte arrays together
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static byte[] Append(byte[] x, byte[] y)
		{
			byte[] appendedBytes = new byte[x.Length + y.Length];
			for (int i = 0; i < x.Length; i++)
			{
				appendedBytes[i] = x[i];
			}

			for (int i = 0; i < y.Length; i++)
			{
				appendedBytes[i + x.Length] = y[i];
			}

			return appendedBytes;
		}
	}
}
