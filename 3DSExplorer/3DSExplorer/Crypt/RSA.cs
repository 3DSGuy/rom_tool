using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace _3DSExplorer.Crypt
{
    public static class RSA
    {

        static public bool CheckSignature(byte[] data, int offset, byte[] signedHash)
        {

            //Create a new instance of RSACryptoServiceProvider.
            var rsa = new RSACryptoServiceProvider(2048);

            //The hash to sign.
            HashAlgorithm ha = SHA256.Create();
            var hash = ha.ComputeHash(data, offset, 0x100);

            //Create an RSAPKCS1SignatureDeformatter object and pass it the 
            //RSACryptoServiceProvider to transfer the key information.
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);

            rsaDeformatter.SetHashAlgorithm("SHA256");

            //Verify the hash and display the results to the console.
            return rsaDeformatter.VerifySignature(hash, signedHash);
        }

        static public byte[] RSAEncrypt(int bits, byte[] dataToEncrypt, RSAParameters rsaKeyInfo, bool doOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (var rsa = new RSACryptoServiceProvider(bits))
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    rsa.ImportParameters(rsaKeyInfo);

                    var rsaExportParameters = rsa.ExportParameters(true);

                    var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                    rsaFormatter.SetHashAlgorithm("SHA256");

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = rsa.Encrypt(dataToEncrypt, doOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        static public byte[] RSADecrypt(int bits, byte[] dataToDecrypt, RSAParameters rsaKeyInfo, bool doOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (var rsa = new RSACryptoServiceProvider(bits))
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    rsa.ImportParameters(rsaKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = rsa.Decrypt(dataToDecrypt, doOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }
    }
}
