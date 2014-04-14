using System;

namespace _3DSExplorer
{
    public class CRC16
    {
        private const ushort InitalValue = 0xFFFF;
        private const ushort Polynomial = 0x8005; // (1000 0000 0000 0101)
        private const ushort RevPolynomial = 0xA001; //reversed (1010 0000 0000 0001)

        public static byte[] GetCRC(byte[] message)
        {
            return GetCRC(message, 0, message.Length);
        }

        public static byte[] GetCRC(byte[] message, long offset, long length)
        {
            var crcFull = InitalValue;

            for (var i = offset; i < offset + length; i++)
            {
                crcFull = (ushort)(crcFull ^ message[i]);
                for (var j = 0; j < 8; j++)
                {
                    var crclsb = (char)(crcFull & 0x0001);
                    crcFull = (ushort)((crcFull >> 1) & 0x7FFF);
                    if (crclsb == 1)
                        crcFull = (ushort)(crcFull ^ RevPolynomial);
                }
            }
            return BitConverter.GetBytes(crcFull);
        }

        public static byte Xor2(byte[] crcBytes)
        {
            return (byte)(crcBytes[0] ^ crcBytes[1]);
        }
    }

}