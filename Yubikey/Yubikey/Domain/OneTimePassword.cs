using System;
using System.Security.Cryptography;
using Yubikey.Domain;
namespace Yubikey.Domain
{
	public class OneTimePassword
	{
		//
		// Original comment: 
		// Decrypt TOKEN using KEY and store output in OUT structure.  Note
		// that there is no error checking whether the output data is valid or
		// not, use pof_check_* for that. 
		//

		/**
		 * <p>
		 *   Decrypt and Parse YubiKey OTP.
		 * </p>
		 * @param block {@link Challenge} encoded representation of the YubiKey OTP.
		 * @param key   128 bit AES key used to encrypt {@code block}, now will be
		 *              used to decrypt.
		 * @throws GeneralSecurityException If decryption fails.               
		 */
		public static DecryptionToken Parse(String block, byte[] key)
		{
			byte[] decoded = Challenge.Decode(block);
			byte[] b = Decrypt(key, decoded);
			return new DecryptionToken(b);
		}

		private static byte[] Decrypt(byte[] key, byte[] decoded)
		{
			RijndaelManaged rijndaelCipher = new RijndaelManaged
			{
				Mode = CipherMode.ECB,
				Padding = PaddingMode.None,
				KeySize = 0x80,
				BlockSize = 0x80
			};

			byte[] encryptedData = decoded;
			byte[] pwdBytes = key;
			byte[] keyBytes = new byte[0x10];
			int len = pwdBytes.Length;
			if (len > keyBytes.Length)
			{
				len = keyBytes.Length;
			}
			Array.Copy(pwdBytes, keyBytes, len);
			rijndaelCipher.Key = keyBytes;
			rijndaelCipher.IV = keyBytes;
			return rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
		}
	}
}
