using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Sarek.MessagePassing.Encryption;
using System.Security.Cryptography;

/*
 * To learn more about cryptography in .NET see: 
 * https://msdn.microsoft.com/en-us/library/system.security.cryptography.aes(v=vs.110).aspx
 * 
 * Running Unit tests in Visual Studio see:
 * https://msdn.microsoft.com/en-us/library/hh270865.aspx
*/
namespace Sarek_UnitTesting
{
    [TestClass]
    public class Testing_Encryption
    {
        Crypt crypt;



        // Test the encryption and decryption process to ensure the server is getting the same 
        // string that we send.
        [TestMethod]
        public void Fidelity()
        {
            // ----------- Arrange -------------- 
            crypt = new Crypt();
            string original = "bobbytables123";
            Aes myAes = Aes.Create();

            // ----------- Act ------------------            
            byte[] encrypted = crypt.EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);
            string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);
            

            // ----------- Assert ---------------
            Assert.AreEqual(original, roundtrip);            
        }




        //  ==================== Emulate server side decryption ====================
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }



    }// end Testing_Encryption
}// end namespace Sarek_unittesting
