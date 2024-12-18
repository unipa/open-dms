using System;
using System.Security.Cryptography;
using System.Text;


public class CryptoUtils
{
    public static String PUBLICKEY = "";
    public static String PRIVATEKEY = "";
    public static string Encrypt(string vv)
    {
        // la password vuota rimante vuota
        if (String.IsNullOrEmpty(vv)) return vv;

        String cert = PUBLICKEY;

        byte[] data = Encoding.UTF8.GetBytes(vv);

        if (!String.IsNullOrEmpty(cert))
        {
            if (vv.StartsWith("rsa://"))
                return vv;
            // tolgo la vecchia codifica
            if (vv.StartsWith("decrypt://"))
                data = Convert.FromBase64String(vv.Substring("decrypt://".Length));

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(cert);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return "rsa://" + base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        // Utilizzo il vecchio metodo
        else
            if (vv.StartsWith("decrypt://"))
            return vv;
        else
            return "decrypt://" + Convert.ToBase64String(data);

    }

    public static string Decrypt(string data)
    {
        if (String.IsNullOrEmpty(data)) return data;

        String cert = PRIVATEKEY;
        if (data.StartsWith("decrypt://"))
        {
            data = Encoding.UTF8.GetString(Convert.FromBase64String(data.Substring("decrypt://".Length)));
            return data;
        }
        else
        {
            if (!String.IsNullOrEmpty(cert))
            {
                // se non è criptata ritorno la stringa originale
                if (!data.ToLower().StartsWith("rsa://"))
                    return data;

                data = data.Substring("rsa://".Length);
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    try
                    {
                        rsa.FromXmlString(cert);


                        var resultBytes = Convert.FromBase64String(data);
                        var decryptedBytes = rsa.Decrypt(resultBytes, false);
                        var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                        return decryptedData;
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }
                }
            }
            else
                // se nonc'è un certificato ritorno la stringa originale
                return data;
        }
    }



    public static string _Encrypt(byte[] src, byte[] key, byte[] iv, int keysize = 128, int blocksize = 128, CipherMode cipher = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
    {


        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.BlockSize = blocksize;
        aes.KeySize = keysize;
        aes.Mode = cipher;
        aes.Padding = padding;

        // byte[] src = Encoding.UTF8.GetBytes(text);
        using (ICryptoTransform encrypt = aes.CreateEncryptor(key, iv))
        {
            byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
            encrypt.Dispose();
            return Convert.ToBase64String(dest);
        }
    }

    public static string _Decrypt(string text, byte[] key, byte[] iv, int keysize = 128, int blocksize = 128, CipherMode cipher = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
    {
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.BlockSize = blocksize;
        aes.KeySize = keysize;
        aes.Mode = cipher;
        aes.Padding = padding;

        byte[] src = Convert.FromBase64String(text);
        using (ICryptoTransform decrypt = aes.CreateDecryptor(key, iv))
        {
            byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
            decrypt.Dispose();
            return Encoding.UTF8.GetString(dest); //Padding is invalid and cannot be removed. 
        }
    }





}

