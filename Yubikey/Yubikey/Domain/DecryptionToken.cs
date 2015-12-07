using System;
using System.Text;

namespace Yubikey.Domain
{
    public class DecryptionToken
    {
        static int BLOCK_SIZE = 16;
        static int UID_SIZE = 6;
        static long CRC_OK_RESIDUE = 0xf0b8;

        /* Unique (secret) ID. */
        byte[] uid;  //UID_SIZE

        /* Session counter (incremented by 1 at each startup + real use).
           High bit indicates whether caps-lock triggered the token. */
        byte[] sessionCounter;  //2

        /* Timestamp incremented by approx 8Hz (low part). */
        byte[] timestampLow; //2

        /* Timestamp (high part). */
        byte timestampHigh;

        /* Number of times used within session + activation flags. */
        byte timesUsed;

        /* Pseudo-random value. */
        byte[] random; //2

        /* CRC16 value of all fields. */
        byte[] crc; //2

        private static int calculateCrc(byte[] b)
        {
            //System.out.println("in calc crc, b[] = "+toString(b));
            int crc = 0xffff;

            for (int i = 0; i < b.Length; i += 1)
            {
                crc ^= b[i] & 0xFF;
                for (int j = 0; j < 8; j++)
                {
                    int n = crc & 1;
                    crc >>= 1;
                    if (n != 0)
                    {
                        crc ^= 0x8408;
                    }
                }
            }
            return crc;
        }

        /**
         * <p>
         *   Gets <i>reference</i> to the CRC16 checksum of the OTP.
         * </p>
         * <p>
         *   This property is of little interest to other then unit test code since
         *   the checksum was validated when constructing {@code this}. 
         * </p>
         * @return CRC16.
         */
        public byte[] getCrc() { return crc; }

        /**
         * <p>
         *   Gets <i>reference</i> to the random bytes of the OTP.
         * </p>
         * <p>
         *   This property is of little interest to other then unit test code. 
         * </p>
         * @return Random bytes.
         */
        public byte[] getRandom() { return random; }

        /**
         * <p>
         *   Gets <i>reference</i> to bytes making up secret id.
         * </p>
         * @return Secret id.
         */
        public byte[] getUid() { return uid; }

        /**
         * <p>
         *   Gets <i>reference</i> to byte sequence of session counter.
         * </p>
         * @return Session counter byte sequence.
         * @see #getCleanCounter()
         */
        public byte[] getSessionCounter() { return sessionCounter; }

        /**
         * <p>
         *   Gets high byte of time stamp.
         * </p>
         * @return High time stamp.
         */
        public byte getTimestampHigh() { return timestampHigh; }

        /**
         * <p>
         *   Gets <i>reference</i> to byte sequence of low part of time stamp.
         * </p>
         * @return Session counter byte sequence of length {@code 2}.
         */
        public byte[] getTimestampLow() { return timestampLow; }

        public void setUid(byte[] value) { uid = value; }

        /**
         * <p>
         *   Constructor.
         * </p>
         * @param b Decrypted OTP to be parsed.
         * @throws IllegalArgumentException If {@code b} not accepted as being a OTP.
         */
        public DecryptionToken(byte[] b)
        {
            if (b.Length != BLOCK_SIZE)
            {
                throw new Exception("Not " + BLOCK_SIZE + " length");
            }

            int calcCrc = DecryptionToken.calculateCrc(b);
            //System.out.println("calc crc  = "+calcCrc);
            //System.out.println("ok crc is = "+Token.CRC_OK_RESIDUE);
            if (calcCrc != DecryptionToken.CRC_OK_RESIDUE)
            {
                throw new Exception("CRC failure");
            }

            int start = 0;

            uid = new byte[UID_SIZE];
            Array.Copy(b, start, uid, 0, UID_SIZE);
            start += UID_SIZE;

            sessionCounter = new byte[2];
            Array.Copy(b, start, sessionCounter, 0, 2);
            start += 2;

            timestampLow = new byte[2];
            Array.Copy(b, start, timestampLow, 0, 2);
            start += 2;

            timestampHigh = b[start];
            start += 1;

            timesUsed = b[start];
            start += 1;

            random = new byte[2];
            Array.Copy(b, start, random, 0, 2);
            start += 2;

            crc = new byte[2];
            Array.Copy(b, start, crc, 0, 2);
        }

        private static String toString(byte b)
        {
            return toString(new byte[] { b });
        }

        static String toString(byte[] b)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < b.Length; i += 1)
            {
                if (i > 0) sb.Append(",");
                sb.Append((b[i] & 0xFF).ToString("X4"));
            }
            return sb.ToString();
        }

        /**
         * <p>
         *   Gets session counter bytes with cap-lock triggered bit cleared.
         * </p>
         * @return Session counter.
         */
        public byte[] getCleanCounter()
        {
            byte[] b = new byte[2];
            b[0] = (byte)(sessionCounter[0] & (byte)0xFF);
            b[1] = (byte)(sessionCounter[1] & (byte)0x7F);
            return b;
        }

        /**
         * <p>
         *   Gets byte value of counter that increases for each generated OTP during
         *   a session. 
         * </p>
         * @return Value.
         */
        public byte getTimesUsed() { return timesUsed; }

        /**
         * <p>
         *   Tells if triggered by caps lock.
         * </p>
         * @return {@code true} if, {@code false} if not.
         */
        public bool wasCapsLockOn()
        {
            return ((byte)(sessionCounter[1] & (byte)0x80)) != 0;
        }

        // Overrides.
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Token uid: " + DecryptionToken.toString(uid));
            sb.AppendLine(", counter: " + DecryptionToken.toString(sessionCounter));
            sb.AppendLine(", timestamp (low): " + DecryptionToken.toString(timestampLow));
            sb.AppendLine(", timestamp (high): " + DecryptionToken.toString(timestampHigh));
            sb.AppendLine(", session use: " + DecryptionToken.toString(timesUsed));
            sb.AppendLine(", random: " + DecryptionToken.toString(random));
            sb.AppendLine(", crc: " + DecryptionToken.toString(crc));
            sb.AppendLine(", clean counter: " + DecryptionToken.toString(getCleanCounter()));
            sb.AppendLine(", CAPS pressed: " + wasCapsLockOn() + "]");
            return sb.ToString();
        }

    }
}
