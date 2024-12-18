using System.Security.Cryptography;
using System.Text;

namespace OpenDMS.Domain.DigitalSignature;

public class MessageDigest
{

    public static Exception LastException = null;
    public enum HashType { SHA1, SHA256, SHA384, SHA512 };

    public static HashType Algorithm(string AlgorithmName)
    {
        string s = AlgorithmName.ToLower();
        switch (s)
        {
            case "sha1": return HashType.SHA1;
            case "sha256": return HashType.SHA256;
            case "sha384": return HashType.SHA384;
            case "sha512": return HashType.SHA512;
            case "sha-1": return HashType.SHA1;
            case "sha-256": return HashType.SHA256;
            case "sha-384": return HashType.SHA384;
            case "sha-512": return HashType.SHA512;
            default:
                return HashType.SHA1;
        }
    }

    public static byte[] Hash(HashType HashType, byte[] FileContent)
    {
        byte[] arrbytHashValue = null;
        HashAlgorithm SHA;
        switch (HashType)
        {
            case HashType.SHA1:
                SHA = new SHA1CryptoServiceProvider();
                break;
            case HashType.SHA256:
                SHA = new SHA256Managed();
                break;
            case HashType.SHA384:
                SHA = new SHA384Managed();
                break;
            case HashType.SHA512:
                SHA = new SHA512Managed();
                break;
            default:
                SHA = new SHA1CryptoServiceProvider();
                break;
        }
        try
        {
            arrbytHashValue = SHA.ComputeHash(FileContent);
            LastException = null;
        }
        catch (Exception ex)
        {
            LastException = ex;
        }
        return arrbytHashValue;
    }
    public static byte[] Hash(HashType HashType, string FileName)
    {
        byte[] arrbytHashValue = null;
        HashAlgorithm SHA;
        switch (HashType)
        {
            case HashType.SHA1:
                SHA = new SHA1CryptoServiceProvider();
                break;
            case HashType.SHA256:
                SHA = new SHA256Managed();
                break;
            case HashType.SHA384:
                SHA = new SHA384Managed();
                break;
            case HashType.SHA512:
                SHA = new SHA512Managed();
                break;
            default:
                SHA = new SHA1CryptoServiceProvider();
                break;
        }
        try
        {
            using (FileStream F = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                arrbytHashValue = SHA.ComputeHash(F);
            }
            LastException = null;
        }
        catch (Exception ex)
        {
            LastException = ex;
        }
        return arrbytHashValue;
        //byte[] FileContent = File.ReadAllBytes(FileName);
        //byte[] hash = Hash(HashType, FileContent);
        //return hash;
    }

    public static string HashText(HashType HashType, string Content)
    {
        byte[] arrbytHashValue = null;
        HashAlgorithm SHA;
        switch (HashType)
        {
            case HashType.SHA1:
                SHA = new SHA1CryptoServiceProvider();
                break;
            case HashType.SHA256:
                SHA = new SHA256Managed();
                break;
            case HashType.SHA384:
                SHA = new SHA384Managed();
                break;
            case HashType.SHA512:
                SHA = new SHA512Managed();
                break;
            default:
                SHA = new SHA1CryptoServiceProvider();
                break;
        }
        try
        {
            arrbytHashValue = SHA.ComputeHash(Encoding.UTF8.GetBytes(Content));
            LastException = null;
        }
        catch (Exception ex)
        {
            LastException = ex;
        }
        return arrbytHashValue == null ? "" : BitConverter.ToString(arrbytHashValue).Replace("-", "");
    }

    public static string HashString(HashType HashType, byte[] FileContent, bool Trim)
    {
        byte[] hash = Hash(HashType, FileContent);
        string strHashData = BitConverter.ToString(hash);
        if (Trim) strHashData = strHashData.Replace("-", "");
        return strHashData;
    }
    public static string HashString(HashType HashType, byte[] FileContent)
    {
        return HashString(HashType, FileContent, true);
    }
    public static string HashString(HashType HashType, string FileName, bool Trim)
    {
        byte[] FileContent = File.ReadAllBytes(FileName);
        string hash = HashString(HashType, FileContent, Trim);
        return hash;
    }
    public static string HashString(HashType HashType, string FileName)
    {
        return HashString(HashType, FileName, true);
    }


}