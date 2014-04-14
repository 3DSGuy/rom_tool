//
// Aes128Ctr.cs
//
// Copyright 2011 Eli Sherer
//

using System;
using System.Linq;
using System.Security.Cryptography;

namespace _3DSExplorer
{
	public class Aes128Ctr
    {
        private const int KeySize = 128;
        private const int BlockSize = KeySize / 8;

		private readonly byte[] _key;
	    private readonly byte[] _iv;
	    private readonly AesManaged _am; //Aes for the counterBlock

        /// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">Key byte array (should be 16 bytes long)</param>
        /// <param name="iv">Initialization Vector byte array (should be 16 bytes long)</param>
		///        
        public Aes128Ctr(byte[] key, byte[] iv)
        {
            _key = new byte[BlockSize];
            _iv = new byte[BlockSize];
            Buffer.BlockCopy(key, 0, _key, 0, BlockSize);
            Buffer.BlockCopy(iv, 0, _iv, 0, BlockSize);
            _am = new AesManaged
                      {
                          KeySize = KeySize,
                          Key = _key,
                          IV = new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                          Mode = CipherMode.ECB,
                          Padding = PaddingMode.None
                      };
        }

		/// <summary>
		/// Decrypt or Encrypt a block in AES-128 CTR Mode (changes the input array)
		/// </summary>
        /// <returns>The same array that was input</returns>
        public void TransformBlock(byte[] input)
        {
            var ict = _am.CreateEncryptor(); //reflective
            var encryptedIV = new byte[BlockSize];
            var counter = BitConverter.ToUInt64(_iv.Reverse().ToArray(), 0); //get the nonce
            
            for (int offset = 0; offset < input.Length; offset += BlockSize, counter++)
            {
                for (int i = 0; i < 8; i++) //Push the new counter to the end of iv
                    _iv[i + BlockSize - 8] = (byte)((counter >> ((7 - i) * 8)) & 0xff);
                ict.TransformBlock(_iv, 0, BlockSize, encryptedIV, 0); // ECB on counter
                // Xor it with the data
                for (int i = 0; i < BlockSize && i + offset < input.Length; i++)
                    input[i + offset] ^= encryptedIV[i];
            }
		}
	}
}