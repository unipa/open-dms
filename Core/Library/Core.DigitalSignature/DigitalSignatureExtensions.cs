using Core.DigitalSignature.Pkcs11;
using iText.Bouncycastle.X509;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Core.DigitalSignature;

public static class DigitalSignatureExtensions
{
    public enum SignatureType
    {
        None,
        PAdES,
        CAdES,
        XAdES,
        TimeStamp,
        M7M
    }

    public static SignatureInfo[] GetSignatureInfo(this Stream SignedFileStream)
    {
        List<SignatureInfo> Signatures = new List<SignatureInfo>();
        //TODO
        //        Signatures.AddRange(GetM7MInfo(SignedFileStream));
        //        Signatures.AddRange(GetTSDInfo(SignedFileStream));
        var SType = SignedFileStream.GetSignatureType();
        if (SType == SignatureType.CAdES || SType == SignatureType.TimeStamp)
            Signatures.AddRange(GetP7MInfo(SignedFileStream));
        else
        if (SType == SignatureType.PAdES)
            Signatures.AddRange(GetPDFInfo(SignedFileStream));
        else
        if (SType == SignatureType.XAdES)
            Signatures.AddRange(GetXMLInfo(SignedFileStream));
        else
        if (SType == SignatureType.M7M)
            Signatures.AddRange(GetM7MInfo(SignedFileStream));
        return Signatures.ToArray();
    }

    public static SignatureType GetSignatureType(this Stream fs)
    {
        SignatureType res = SignatureType.None;
        try
        {
            byte[] b = new byte[6];
            fs.Seek(0, SeekOrigin.Begin);
            int ByteRead = fs.Read(b, 0, b.Length);
            fs.Seek(0, SeekOrigin.Begin);
            if (ByteRead >= 4)
            {
                byte[] header = { b[0], b[1], b[2], b[3] };
                if (header.SequenceEqual(pdfMagicNumber))
                    return SignatureType.PAdES;

                if (ByteRead >= 6)
                {
                    header = new byte[] { b[0], b[1], b[2], b[3], b[4], b[5] };
                    if (header.SequenceEqual(xmlMagicNumber))
                        return SignatureType.XAdES;
                }
                try
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    CmsSignedData sd = new CmsSignedData(fs);
                    if (sd != null)
                        return  SignatureType.CAdES;
                }
                catch (Exception)
                {
                }
                try
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    
                    OpenPop.Mime.Message M = OpenPop.Mime.Message.Load(fs);
                    //List<OpenPop.Mime.MessagePart> parts = M.FindAllAttachments();
                    var parts = M.FindAllMessagePartsWithMediaType("application/timestamp-reply");
                    if (parts.Count > 0)
                    {
                        return SignatureType.M7M;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        catch { }
        return SignatureType.None;
    }

    public static string FindFileType (this Stream Content )
    {

        Content.Seek(0, SeekOrigin.Begin);
        int len = 6;
        byte[] Bytes = new byte[len];
        int count = Content.Read(Bytes, 0, len);
        string start = System.Text.Encoding.UTF8.GetString(Bytes, 0, count).ToLower();
        if (start.StartsWith("<?xml ")) return ".xml";
        if (start.StartsWith("<!doct")) return ".html";
        if (start.StartsWith("‰png")) return ".png";
        if (start.StartsWith("%pdf-")) return ".pdf";
        if (start.StartsWith("pk")) return ".zip";
        return "";

    }


    public static Stream VerifyAndExtract(this Stream SignedFile)
    {
        var SignatureType = GetSignatureType(SignedFile);
        switch (SignatureType)
        {
          //  case DigitalSignatureExtensions.SignatureType.TimeStamp:
            case DigitalSignatureExtensions.SignatureType.CAdES:
                using (Stream SignedFileArray = GetAllBytes(SignedFile))
                    {
                        CmsSignedData sd = new CmsSignedData(SignedFileArray);
                        CmsProcessable pro = sd.SignedContent;
                        MemoryStream str = new MemoryStream();
                        pro.Write(str);
                        return VerifyAndExtract(str);
                }
                break;
            case DigitalSignatureExtensions.SignatureType.XAdES:
                return SignedFile;
            case DigitalSignatureExtensions.SignatureType.TimeStamp:
            case DigitalSignatureExtensions.SignatureType.M7M:
                try
                {
                    OpenPop.Mime.Message M = OpenPop.Mime.Message.Load(SignedFile);
                    List<OpenPop.Mime.MessagePart> parts = M.FindAllAttachments();
                    using (MemoryStream m = new MemoryStream())
                    {
                        if (parts.Count >= 2)
                        {
                            parts[1].Save(m);
                            return VerifyAndExtract(m);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            default:
                break;
        }
        return SignedFile;

    }





    private static SignatureInfo[] GetP7MInfo(Stream SignedFileStream)
    {
        List<SignatureInfo> SIList = new List<SignatureInfo>();
        try
        {
            SignedFileStream.Seek(0, SeekOrigin.Begin);
            CmsSignedData sd = new CmsSignedData(SignedFileStream);

            CmsProcessable pro = sd.SignedContent;
            var x = sd.GetCertificates("collection");
            SignerInformationStore sis = sd.GetSignerInfos();

            foreach (SignerInformation si in sis.GetSigners())
            {
                X509CertStoreSelector sel = new X509CertStoreSelector();
                sel.Issuer = si.SignerID.Issuer;
                sel.SerialNumber = si.SignerID.SerialNumber;
                // Try find a match
                var certificatesFound = x.GetMatches(sel);
                int sin = 0;

                //SHA256Managed S = new SHA256Managed();
                
                SignatureInfo SI = new SignatureInfo();
                if (si.SignedAttributes[new DerObjectIdentifier("1.2.840.113549.1.9.5")] != null)
                {
                    try
                    {
                        var attr = si.SignedAttributes[new DerObjectIdentifier("1.2.840.113549.1.9.5")].AttrValues[0];
                        if (attr is DerUtcTime)
                            SI.SignDate = DerUtcTime.GetInstance(attr).ToDateTime();
                        else if (attr is DerGeneralizedTime)
                            SI.SignDate = DerGeneralizedTime.GetInstance(attr).ToDateTime();
                        //else if (attr is  Asn1GeneralizedTime) 
                        //    SI.SignDate = Asn1GeneralizedTime.GetInstance(attr).ToDateTime();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (si.UnsignedAttributes != null)
                {
                    var ts = si.UnsignedAttributes[new DerObjectIdentifier("1.2.840.113549.1.9.16.2.14")];
                    if (ts != null)
                    {
                        byte[] m7m = ts.AttrValues[0].GetDerEncoded();
                        SIList.AddRange(GetSignatureInfo(new MemoryStream(m7m)));
                    }
                }
                if (si.SignedAttributes[new DerObjectIdentifier("1.2.840.113549.1.9.4")] != null)
                    SI.Digest = si.SignedAttributes[new DerObjectIdentifier("1.2.840.113549.1.9.4")].AttrValues[0].ToString().ToUpper().Replace("#", "").Replace("-", "");
                SI.SerialNumber = si.SignerID.SerialNumber.ToString();
                SI.Algorithm = si.DigestAlgorithmID.ToAsn1Object().ToString();
                SI.SignatureType = sd.SignedContentTypeOid == "1.2.840.113549.1.7.1" ? "Marca Temporale" : "Firma CADES";
                //SI.SignatureType = "Firma CADES";
                SI.Location = "";
                SI.Reason = "";
                SI.SignName = "";
                SIList.Add(SI);

                MemoryStream str = new MemoryStream();
                pro.Write(str);
                SI.FileExtension = str.FindFileType();
                int Ok = 0;
                int NoOk = 0;
                List<CertificateData> CertList = new List<CertificateData>();

                foreach (var C in certificatesFound)
                {
                    //Org.BouncyCastle.X509.X509Certificate C = (Org.BouncyCastle.X509.X509Certificate)certificatesFound[i];

                    CertificateData CI = GetCertificateInfo((X509Certificate)C);
                    CertList.Add(CI);
                    if (CI.IsValid(DateTime.UtcNow)) Ok++; else NoOk++;
                }
                SI.Certificates = CertList.ToArray();
                SI.SigningCertificate = CertList.FirstOrDefault();
                SI.Signatures = Ok + NoOk;
                SI.ValidSignatures = Ok;
                SI.InvalidSignatures = NoOk;

            }
        }
        catch { };
        return SIList.ToArray();
    }
    private static SignatureInfo[] GetPDFInfo(Stream SignedFileStream)
    {
        SignedFileStream.Seek(0, SeekOrigin.Begin);
        List<SignatureInfo> SIList = new List<SignatureInfo>();
        PdfReader reader = new PdfReader(SignedFileStream);
        try
        {
            PdfDocument pdfDoc = new PdfDocument(reader);
            SignatureUtil signUtil = new SignatureUtil(pdfDoc);
            IList<string> names = signUtil.GetSignatureNames();
            foreach (string name in names)
            {
                PdfPKCS7 pk = signUtil.ReadSignatureData(name);
                bool wholeDocument = signUtil.SignatureCoversWholeDocument(name);
                bool signatureIntegrityAndAuthenticity = pk.VerifySignatureIntegrityAndAuthenticity();
                SignatureInfo SI = new SignatureInfo();
                SI.SignatureType = "Firma PADES";
                SI.SignDate = pk.GetSignDate();
                SI.Location = pk.GetLocation();
                SI.Reason = pk.GetReason();
                SI.SignName = pk.GetSignName();
                SI.FileExtension = ".pdf";
                SIList.Add(SI);
                int Ok = 0;
                int NoOk = 0;
                SI.Algorithm = pk.GetDigestAlgorithmName();
                SI.TimeStamp = pk.GetTimeStampDate().ToString();
                List<CertificateData> CertList = new List<CertificateData>();
                
                foreach (var CC in pk.GetSignCertificateChain())
                {
                    var BC = (X509CertificateBC)CC;
                    var C = BC.GetCertificate();
                    CertificateData CI = GetCertificateInfo(C);
                    CertList.Add(CI);
                    if (CI.IsValid(DateTime.UtcNow)) Ok++; else NoOk++;
                }
                SI.Certificates = CertList.ToArray();
                SI.SigningCertificate = CertList.FirstOrDefault();
                SI.Signatures = Ok + NoOk;
                SI.ValidSignatures = Ok;
                SI.InvalidSignatures = NoOk;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            reader.Close();
        }
        return SIList.ToArray();
    }
    private static SignatureInfo[] GetXMLInfo(Stream SignedFileStream)
    {
        List<SignatureInfo> SIList = new List<SignatureInfo>();
        try
        {

            SignedFileStream.Seek(0, SeekOrigin.Begin);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(SignedFileStream);
            XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);
            mgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            mgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            XmlNode nodeSignature = xmlDoc.SelectSingleNode("//ds:Signature", mgr);

            bool isSigned = nodeSignature != null;
            if (isSigned)
            {
//                SI.SignatureType = "Firma PADES";
                //TODO
                //X509Certificate2 cert = new X509Certificate2(Encoding.ASCII.GetBytes(xmlDoc.SelectSingleNode("//ds:X509Certificate", mgr).InnerText));
                // Load the signature for verification

                //System.Security.Cryptography.Xml.KeyInfoX509Data KeyInfo = new KeyInfoX509Data(cert);

                //SignatureInfo SI = new SignatureInfo();
                //SI.SignDate = cert.. pk.GetSignDate();
                //SI.Location = pk.GetLocation();
                //SI.Reason = pk.GetReason();
                //SI.SignName = pk.GetSignName();
                //SIList.Add(SI);
                //Int32 Ok = 0;
                //Int32 NoOk = 0;
                //List<Core.DigitalSignatureVerifier.Model.CertificateInfo> CertList = new List<Core.DigitalSignatureVerifier.Model.CertificateInfo>();


                //SignerInfo["P7M Signers"] = KeyInfo.Certificates.Count > 0 ? KeyInfo.Certificates.Count.ToString() : "";
                //for (int i = 0; i < KeyInfo.Certificates.Count; i++)
                //{
                //    cert = (System.Security.Cryptography.X509Certificates.X509Certificate2)(KeyInfo.Certificates[i]);



                //    foreach (var C in pk.GetSignCertificateChain())
                //    {
                //        Core.DigitalSignatureVerifier.Model.CertificateInfo CI = new Core.DigitalSignatureVerifier.Model.CertificateInfo();
                //        CI.Issuer = new ObjectInfo(C.IssuerDN);
                //        CI.SerialNumber = C.SerialNumber.ToString();
                //        CI.DataInizioValidita = C.NotBefore;
                //        CI.DatafineValidita = C.NotAfter;
                //        CI.Subject = new ObjectInfo(C.SubjectDN);
                //        CertList.Add(CI);
                //        if (CI.IsValid(DateTime.UtcNow)) Ok++; else NoOk++;
                //    }
                //    SI.Certificates = CertList.ToArray();
                //    SI.Signatures = Ok + NoOk;
                //    SI.ValidSignatures = Ok;
                //    SI.InvalidSignatures = NoOk;


                //    SignerInfo["P7M" + (i + counter).ToString() + ".CA.SN"] = cert.SerialNumber.ToString();
                //    SignerInfo["P7M" + (i + counter).ToString() + ".CA.Validità Al"] = cert.NotAfter.ToString("yyyy/MM/dd");
                //    SignerInfo["P7M" + (i + counter).ToString() + ".CA.Validità Dal"] = cert.NotBefore.ToString("yyyy/MM/dd");
                //    SignerInfo["P7M" + (i + counter).ToString() + ".CA.PublicKeyInfo"] = cert.GetPublicKey().ToString();
                //    SignerInfo["P7M" + (i + counter).ToString() + ".CA.Algoritmo"] = cert.SignatureAlgorithm.FriendlyName;
                //    SignerInfo["P7M" + (i + counter).ToString() + ".IsValid"] = "OK";
                //}

            }
        }
        catch
        {
        };
        return SIList.ToArray();
    }
    private static SignatureInfo[] GetM7MInfo(Stream SignedFileStream)
    {
        List<SignatureInfo> SIList = new List<SignatureInfo>();

        SignedFileStream.Seek(0, SeekOrigin.Begin);
        OpenPop.Mime.Message M = OpenPop.Mime.Message.Load(SignedFileStream);
        List<OpenPop.Mime.MessagePart> parts = M.FindAllAttachments();
        if (parts.Count > 0)
        {
            MemoryStream M2 = new MemoryStream();
            parts[0].Save(M2);
            MemoryStream M3 = new MemoryStream();
            parts[1].Save(M3);

            Org.BouncyCastle.Tsp.TimeStampResponse T = new Org.BouncyCastle.Tsp.TimeStampResponse(M3.ToArray());
            Org.BouncyCastle.Tsp.TimeStampToken token = T.TimeStampToken;
            var x = token.GetCertificates();
            Org.BouncyCastle.X509.X509Certificate cert = null;

            X509CertStoreSelector sel = new X509CertStoreSelector();
            sel.Issuer = token.SignerID.Issuer;
            sel.SerialNumber = token.SignerID.SerialNumber;

            SignatureInfo SI = new SignatureInfo();
            SI.SignatureType = "Marca Temporale";
            SI.Location = "";
            SI.Reason = "";
            SI.SignName = "";
            SI.SerialNumber = token.TimeStampInfo.SerialNumber.ToString();
            SI.Algorithm = token.TimeStampInfo.HashAlgorithm.ToString();
            SI.SignDate = token.TimeStampInfo.GenTime;
            SI.TimeStamp = token.TimeStampInfo.GenTime.ToString();
            SI.Digest = BitConverter.ToString(token.TimeStampInfo.GetMessageImprintDigest()).Replace("-", "");
            SI.SignName = token.TimeStampInfo.Tsa != null ? token.TimeStampInfo.Tsa.Name.ToString() : "";

            SI.FileExtension = M3.FindFileType();

            Int32 Ok = 0;
            Int32 NoOk = 0;
            List<CertificateData> CertList = new List<CertificateData>();

            // Try find a match
            IList certificatesFound = new ArrayList(x.GetMatches(null));
            if (certificatesFound.Count > 0)
            {
                foreach (var c in certificatesFound)
                {
                    var C =DotNetUtilities.FromX509Certificate(  DotNetUtilities.ToX509Certificate((X509CertificateStructure)c));
                    CertificateData CI = GetCertificateInfo(C);
                    CertList.Add(CI);
                    if (CI.IsValid(DateTime.UtcNow)) Ok++; else NoOk++;
                }

            }

            SI.Certificates = CertList.ToArray();
            SI.SigningCertificate = CertList.FirstOrDefault();
            SI.Signatures = Ok + NoOk;
            SI.ValidSignatures = Ok;
            SI.InvalidSignatures = NoOk;
            SIList.Add(SI);

            M2.Position = 0;
            SIList.AddRange(GetSignatureInfo(M2));

            //CmsSignedData sd = token.ToCmsSignedData();
            //CmsProcessable pro = sd.SignedContent;
            //var xx = sd.GetCertificates("collection");
            //SignerInformationStore sis = sd.GetSignerInfos();

            //foreach (SignerInformation si in sis.GetSigners())
            //{
            //    Org.BouncyCastle.X509.X509Certificate certx = null;

            //    X509CertStoreSelector selx = new X509CertStoreSelector();
            //    selx.Issuer = si.SignerID.Issuer;
            //    selx.SerialNumber = si.SignerID.SerialNumber;

            //    // Try find a match
            //    IList certificatesFoundx = new ArrayList(xx.GetMatches(selx));

            //    if (certificatesFoundx.Count > 0) // Match found
            //    {
            //        //non lo uso
            //        var C = (Org.BouncyCastle.X509.X509Certificate)certificatesFoundx[0];

            //        CertificateData CI = GetCertificateInfo(C);
            //        CertList.Add(CI);
            //        if (CI.IsValid(DateTime.UtcNow)) Ok++; else NoOk++;

            //        //                    SignerInfo["M7M.SI.SN"] = si.SignerID.SerialNumber.ToString();
            //        //                    GetInfo(si.SignerID.Issuer.GetOidList(), si.SignerID.Issuer.GetValueList(), "M7M.SI", SignerInfo);
            //    }
            //}
        }
        return SIList.ToArray();
    }


 
    private static byte[] pdfMagicNumber = { 0x25, 0x50, 0x44, 0x46 };
    private static byte[] xmlMagicNumber = { 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20 };


    private static CertificateData GetCertificateInfo(X509Certificate C)
    {
        CertificateData CI = new CertificateData();
        CI.Issuer = GetInfo(C.IssuerDN.GetOidList(), C.IssuerDN.GetValueList());
        CI.SerialNumber = C.SerialNumber.ToString();
        CI.NotBefore = C.NotBefore;
        CI.NotAfter = C.NotAfter;
        CI.Subject = GetInfo(C.SubjectDN.GetOidList(), C.SubjectDN.GetValueList());
        CI.Subject = GetInfo(C.SubjectDN.GetOidList(), C.SubjectDN.GetValueList());
        //TODO
        //        GetInfo(cert.IssuerDN.GetOidList(), cert.IssuerDN.GetValueList(), "P7M" + (counter).ToString() + ".CA", SignerInfo);
        //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), "P7M" + (counter).ToString() + ".Soggetto", SignerInfo);
        //        GetInfo(si.SignerID.Issuer.GetOidList(), si.SignerID.Issuer.GetValueList(), p + ".SI", SignerInfo);
        //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), p + ".Soggetto", SignerInfo);
        //        GetInfo(cert.IssuerDN.GetOidList(), cert.IssuerDN.GetValueList(), "M7M.CA", SignerInfo);
        //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), "M7M.Soggetto", SignerInfo);
        //        GetInfo(si.SignerID.Issuer.GetOidList(), si.SignerID.Issuer.GetValueList(), p + ".SI", SignerInfo);
        //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), p + ".Soggetto", SignerInfo);

        return CI;
    }
    //private static CertificateData GetPDFCertificateInfo(IX509Certificate C)
    //{
    //    CertificateData CI = new CertificateData();
    //    //TODO
    //    //        GetInfo(cert.IssuerDN.GetOidList(), cert.IssuerDN.GetValueList(), "P7M" + (counter).ToString() + ".CA", SignerInfo);
    //    //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), "P7M" + (counter).ToString() + ".Soggetto", SignerInfo);
    //    //        GetInfo(si.SignerID.Issuer.GetOidList(), si.SignerID.Issuer.GetValueList(), p + ".SI", SignerInfo);
    //    //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), p + ".Soggetto", SignerInfo);
    //    //        GetInfo(cert.IssuerDN.GetOidList(), cert.IssuerDN.GetValueList(), "M7M.CA", SignerInfo);
    //    //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), "M7M.Soggetto", SignerInfo);
    //    //        GetInfo(si.SignerID.Issuer.GetOidList(), si.SignerID.Issuer.GetValueList(), p + ".SI", SignerInfo);
    //    //        GetInfo(cert.SubjectDN.GetOidList(), cert.SubjectDN.GetValueList(), p + ".Soggetto", SignerInfo);

    //    return CI;
    //}

    private static ObjectInfo GetInfo(IList OID, IList Value)
    {
        ObjectInfo O = new ObjectInfo();
        for (int k = 0; k < Value.Count; k++)
        {
            string Oid = OID[k].ToString();
            string value = Value[k].ToString();
            switch (Oid)
            {
                case "2.5.4.3":
                    O.NomeComune = value;
                    break;
                case "2.5.4.4":
                    O.Cognome = value;
                    break;
                case "2.5.4.5":
                    O.CodiceFiscale = value;
                    break;
                case "2.5.4.6":
                    O.Stato = value;
                    break;
                case "2.5.4.10":
                    O.Organizzazione = value;
                    break;
                case "2.5.4.11":
                    O.NomeUnitaOrganizzativa = value;
                    break;
                case "2.5.4.42":
                    O.Nome = value;
                    break;
                case "2.5.4.46":
                    O.CodiceIdentita = value;
                    break;

                case "2.5.4.15":
                    O.CategoriaBusiness = value;
                    break;
                case "2.5.4.65":
                    O.Pseudonimo = value;
                    break;
                case "2.5.4.17":
                    O.CodicePostale = value;
                    break;
                case "2.5.4.9":
                    O.Strada = value;
                    break;
                case "1.3.6.1.5.5.7.9.3": //1.3.6.1.5.5.7.9.1
                    O.DatadiNascita = value;
                    break;
                case "1.2.840.113549.1.9.1":
                    O.Email = value;
                    break;
                case "2.5.4.43":
                    O.Iniziali = value;
                    break;
                case "2.5.4.7":
                    O.Localita = value;
                    break;
                case "2.5.4.8":
                    O.CodicePaese = value;
                    break;
                case "1.3.6.1.5.5.7.9.2":
                    O.PaeseDiNascita = value;
                    break;
                case "2.5.4.44":
                    O.Generazione = value;
                    break;
                case "2.5.4.45":
                    O.IdentificativoUnivoco = value;
                    break;
                case "2.5.4.20":
                    O.Telefono = value;
                    break;
                case "2.5.4.12":
                    O.Titolo = value;
                    break;
                default:
                    break;
            }
        }
        return O;
    }

    private static bool IsBase64String(byte[] data)
    {
        var s = Encoding.UTF8.GetString(data);
        s = s.Replace("\r", "").Replace("\n", "");
        s = s.Trim();
        return s.Length % 4 == 0 && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }
    private static Stream GetAllBytes(Stream fls)
    {

        byte[] PKCS7_BEGIN = Encoding.UTF8.GetBytes("-----BEGIN PKCS7-----");
        byte[] PKCS7_END = Encoding.UTF8.GetBytes("-----END PKCS7-----");

        byte[] fl = null;
        bool pkcs7 = false;
        try
        {
            var buff1 = new byte[PKCS7_BEGIN.Length];
            fls.Read(buff1, 0, buff1.Length);
            if (buff1.SequenceEqual(PKCS7_BEGIN))
            {
                buff1 = new byte[PKCS7_END.Length];
                fls.Position = fls.Length - buff1.Length;
                fls.Read(buff1, 0, buff1.Length);
                if (buff1.SequenceEqual(PKCS7_END))
                    pkcs7 = true;
            }
            if (pkcs7)
            {
                fls.Position = PKCS7_BEGIN.Length;
                fl = new byte[fls.Length - PKCS7_BEGIN.Length - PKCS7_END.Length];
            }
            else
            {
                fls.Position = 0;
                fl = new byte[fls.Length];
            }
            fls.Read(fl, 0, fl.Length);
            if (IsBase64String(fl))
                fl = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(fl);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new MemoryStream(fl);
    }
    private static Stream DecodeFromFile(Stream FileNameStream)
    {
        MemoryStream M = new MemoryStream();
        try
        {
            using (FromBase64Transform myTransform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces))
            {
                byte[] myOutputBytes = new byte[myTransform.OutputBlockSize];

                //Open the input and output files.

                //Retrieve the file contents into a byte array. 
                byte[] myInputBytes = new byte[FileNameStream.Length];
                FileNameStream.Read(myInputBytes, 0, myInputBytes.Length);

                //Transform the data in chunks the size of InputBlockSize. 
                int i = 0;
                while (myInputBytes.Length - i > 4/*myTransform.InputBlockSize*/)
                {
                    int bytesWritten = myTransform.TransformBlock(myInputBytes, i, 4/*myTransform.InputBlockSize*/, myOutputBytes, 0);
                    i += 4/*myTransform.InputBlockSize*/;
                    M.Write(myOutputBytes, 0, bytesWritten);
                }

                //Transform the final block of data.
                myOutputBytes = myTransform.TransformFinalBlock(myInputBytes, i, myInputBytes.Length - i);
                M.Write(myOutputBytes, 0, myOutputBytes.Length);

                //Free up any used resources.
                myTransform.Clear();
            }
        }
        catch
        {
        }

        return M;
    }




}
