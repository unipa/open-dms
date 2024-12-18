namespace Core.DigitalSignature.Pkcs11;
public class SignatureInfo
{
    public CertificateData[] Certificates { get; set; }
    public CertificateData SigningCertificate { get; set; }

    public DateTime SignDate { get; set; }
    public string Location { get; set; }
    public string Reason { get; set; }
    public string SignName { get; set; }
    public string Digest { get; set; }
    public string Algorithm { get; set; }
    public string SerialNumber { get; set; }
    public string TimeStamp { get; set; }
    public int Signatures { get; set; }
    public int ValidSignatures { get; set; }
    public int InvalidSignatures { get; set; }
    public string SignatureType { get; set; } // es. PADES, CASES, XADES, TSA
    public string FileExtension { get; set; } = "";




}
