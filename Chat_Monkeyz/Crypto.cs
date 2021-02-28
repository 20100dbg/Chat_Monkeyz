using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Chat_Monkeyz
{
    public class RSA
    {
        public String publickey;
        public String privatekey;
        public int keyLength;

        RSACryptoServiceProvider rsa;
        CspParameters cspParams;

        public RSA(int keyLength)
        {
            this.keyLength = keyLength;
            cspParams = new CspParameters(1, "SpiderContainer");
            cspParams.Flags = CspProviderFlags.CreateEphemeralKey;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";   

            AssignNewKey();
        }


        public void AssignNewKey()
        {
            rsa = new RSACryptoServiceProvider(keyLength, cspParams);
            privatekey = rsa.ToXmlString(true);
            publickey = rsa.ToXmlString(false);
        }


        public String Encrypt(String str)
        {
            rsa.FromXmlString(publickey);

            byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] cipherbytes = rsa.Encrypt(plainbytes, false);
            return Convert.ToBase64String(cipherbytes);
        }

        public String Encrypt(Byte[] str)
        {
            rsa.FromXmlString(publickey);

            byte[] cipherbytes = rsa.Encrypt(str, false);
            return Convert.ToBase64String(cipherbytes);
        }


        


        public String DecryptToString(String str)
        {
            rsa.FromXmlString(privatekey);

            byte[] cypherBytes = Convert.FromBase64String(str);
            byte[] plainBytes = rsa.Decrypt(cypherBytes, false);
            return Encoding.UTF8.GetString(plainBytes);
        }


        public Byte[] DecryptToBytes(String str)
        {
            rsa.FromXmlString(privatekey);
            byte[] cypherBytes = Convert.FromBase64String(str);
            return rsa.Decrypt(cypherBytes, false);
        }
    }





    public class AES
    {
        AesCryptoServiceProvider aes;
        public Byte[] sendKey;
        public Byte[] receiveKey;

        public Byte[] sendIV;
        public Byte[] receiveIV;


        public AES()
        {
            //by default, the key size is 256. Longest size at this moment.
            aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128; //defaut
            aes.KeySize = 256; //defaut

            RegenerateKey();
        }


        public void RegenerateKey()
        {
            aes.GenerateIV();
            aes.GenerateKey();
        }

        public Byte[] Key
        {
            get { return aes.Key; }
        }

        public Byte[] IV
        {
            get { return aes.IV; }
        }



        public String EncryptToString(String txt, Byte[] key = null, Byte[] IV = null)
        {
            if (key == null) key = aes.Key;
            if (IV == null) IV = aes.IV;

            byte[] plainText = Encoding.UTF8.GetBytes(txt);
            Byte[] cipherBytes = Encrypt(plainText, key, IV);
            return Convert.ToBase64String(cipherBytes);
        }



        public Byte[] EncryptToBytes(Byte[] plainText, Byte[] key = null, Byte[] IV = null)
        {
            if (key == null) key = aes.Key;
            if (IV == null) IV = aes.IV;

            return Encrypt(plainText, key, IV);
        }



        public Byte[] Encrypt(Byte[] plainText, Byte[] key, Byte[] IV)
        {
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Mode = CipherMode.CBC;

            ICryptoTransform aesEncryptor = rijndael.CreateEncryptor(key, IV);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, aesEncryptor, CryptoStreamMode.Write);
            cs.Write(plainText, 0, plainText.Length);
            cs.FlushFinalBlock();

            byte[] CipherBytes = ms.ToArray();
            ms.Close();
            cs.Close();

            return CipherBytes;
        }



        public String DecryptToString(String txt, Byte[] key = null, Byte[] IV = null)
        {
            if (key == null) key = aes.Key;
            if (IV == null) IV = aes.IV;

            byte[] cipheredData = Convert.FromBase64String(txt);
            byte[] plainTextData = Decrypt(cipheredData, key, IV);
            return Encoding.UTF8.GetString(plainTextData);
        }


        public Byte[] DecryptToBytes(Byte[] cipherBytes, Byte[] key = null, Byte[] IV = null)
        {
            if (key == null) key = aes.Key;
            if (IV == null) IV = aes.IV;

            return Decrypt(cipherBytes, key, IV);
        }


        public Byte[] Decrypt(Byte[] cipheredData, Byte[] key, Byte[] IV)
        {
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = rijndael.CreateDecryptor(key, IV);
            MemoryStream ms = new MemoryStream(cipheredData);
            CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

            byte[] plainTextData = new byte[cipheredData.Length];
            int decryptedByteCount = cs.Read(plainTextData, 0, plainTextData.Length);

            ms.Close();
            cs.Close();

            return plainTextData;
        }

    }

}