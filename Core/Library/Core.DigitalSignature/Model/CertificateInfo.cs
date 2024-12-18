using Org.BouncyCastle.Security.Certificates;

namespace Core.DigitalSignature.Pkcs11;

public class CertificateData
{
    public string SerialNumber { get; set; }
    public string Digest { get; set; }
    public string Algorithm { get; set; }
    public DateTime NotBefore { get; set; }
    public DateTime NotAfter { get; set; }
    public string Version { get; set; }

    public ObjectInfo Issuer { get; set; } = new ObjectInfo();
    public ObjectInfo Subject { get; set; } = new ObjectInfo();




    public virtual bool IsValid(DateTime time)
    {
        if (time.CompareTo(NotBefore) >= 0)
        {
            return time.CompareTo(NotAfter) <= 0;
        }
        return false;
    }

    public virtual void CheckValidity(DateTime time)
    {
        if (time.CompareTo(NotAfter) > 0)
        {
            throw new CertificateExpiredException("certificate expired on " + NotAfter.ToString());
        }

        if (time.CompareTo(NotBefore) < 0)
        {
            throw new CertificateNotYetValidException("certificate not valid until " + NotBefore.ToString());
        }
    }
}
