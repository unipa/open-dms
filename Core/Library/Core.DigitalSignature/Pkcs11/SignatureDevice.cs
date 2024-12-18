using Core.DigitalSignature.Model;
using iText.Commons.Bouncycastle.Cert;
using iText.Forms;
using iText.Forms.Form.Element;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System.Security.Cryptography.X509Certificates;

namespace Core.DigitalSignature.Pkcs11
{
    public class SignatureDevice
    {
        private readonly string driver;

        public SignatureDevice(string Driver)
        {
            driver = Driver;
        }

        public List<SlotItem> GetSlots()
        {
            List<SlotItem> lista = new();
            List<X509Certificate2> Certs = new List<X509Certificate2>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                var slots = pkcs11Library.GetSlotList(SlotsType.WithOrWithoutTokenPresent);
                if (slots != null)
                {
                    foreach (var slot in slots)
                    {
                        try
                        {
                            var t = slot.GetSlotInfo();
                            var L = new SlotItem() { Id = (int)slot.SlotId, Name = t.SlotDescription, ManufacturerId = t.ManufacturerId };
                            lista.Add(L);
                        }
                        catch (Exception ex) { };
                    }
                }

            }
            return lista;
        }
        public List<TokenItem> GetTokens(int? slotId = null)
        {
            List<TokenItem> lista = new();
            List<X509Certificate2> Certs = new List<X509Certificate2>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                var slots = pkcs11Library.GetSlotList(SlotsType.WithOrWithoutTokenPresent); // GetUsableSlot(pkcs11Library);
                if (slotId != null) slots = slots.Where(s => s.SlotId == (ulong)slotId).ToList();
                if (slots != null)
                {
                    foreach (var slot in slots)
                    {
                        try
                        {
                            var t = slot.GetTokenInfo();
                            var L = new TokenItem() { Serial = t.SerialNumber, Label = t.Label, Model = t.Model, Slot = (int)t.SlotId, ManufacturerId = t.ManufacturerId };
                            lista.Add(L);
                        }
                        catch (Exception ex) { };
                    }
                }

            }
            return lista;
        }
        public TokenItem GetTokenByLabel(string tokenLabel)
        {
            List<TokenItem> lista = new();
            List<X509Certificate2> Certs = new List<X509Certificate2>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                var slots = GetUsableSlot(pkcs11Library);
                if (slots != null)
                {
                    foreach (var slot in slots)
                    {
                        try
                        {
                            var t = slot.GetTokenInfo();
                            if (t.Label.CompareTo(tokenLabel) == 0)
                                return new TokenItem() { Serial = t.SerialNumber, Label = t.Label, Model = t.Model, Slot = (int)t.SlotId, ManufacturerId = t.ManufacturerId };
                        }
                        catch (Exception ex) { };
                    }
                }

            }
            return null;
        }
        public TokenItem GetTokenBySerial(string tokenSerial)
        {
            List<TokenItem> lista = new();
            List<X509Certificate2> Certs = new List<X509Certificate2>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                var slots = GetUsableSlot(pkcs11Library);
                if (slots != null)
                {
                    foreach (var slot in slots)
                    {
                        try
                        {
                            var t = slot.GetTokenInfo();
                            if (t.SerialNumber.CompareTo(tokenSerial) == 0)
                                return new TokenItem() { Serial = t.SerialNumber, Label = t.Label, Model = t.Model, Slot = (int)t.SlotId, ManufacturerId = t.ManufacturerId };
                        }
                        catch (Exception ex) { };
                    }
                }

            }
            return null;
        }

        public List<CertificateItem> GetCertificates(int slotid, bool all=false)
        {
            List<CertificateItem> Certs = new List<CertificateItem>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                List<ISlot> slots = GetUsableSlot(pkcs11Library);
                //                Settings.NormalUserPin = "08022008";
                try
                {
                    foreach (var slot in slots)
                    {
                        if (slot.SlotId == (ulong)slotid)
                        {
                            // Get token info
                            ITokenInfo tokenInfo = slot.GetTokenInfo();
                            using (Net.Pkcs11Interop.HighLevelAPI.ISession session = slot.OpenSession(SessionType.ReadOnly))
                            {
                                ISessionInfo sessionInfo = session.GetSessionInfo();
                                //session.Login(CKU.CKU_USER, Settings.NormalUserPin);

                                IObjectHandle publicKey = null;
                                IObjectHandle privateKey = null;



                                List<IObjectAttribute> objectAttributes = new List<IObjectAttribute>();
                                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
                                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_PRIVATE, false));
                                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));
                                //objectAttributes.Add(new ObjectAttribute(CKA.CKA_LABEL, UTF8Encoding.UTF8.GetBytes("Certificat d'Authentification CPS")));
                                //CurrentSession.FindObjectsInit(objectAttributes);
                                var oObjCollection = session.FindAllObjects(objectAttributes);
                                //if (oObjCollection.Count > 0)
                                foreach (var item in oObjCollection)
                                {
                                    var el = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_VALUE, CKA.CKA_ID, CKA.CKA_LABEL });
                                    var id = el[1].GetValueAsString();
                                    var label = el[2].GetValueAsString();
                                    var c = new X509Certificate2(el[0].GetValueAsByteArray());
                                    if (!all && !c.Extensions.Any(a => a is X509KeyUsageExtension && ((X509KeyUsageExtension)a).KeyUsages == X509KeyUsageFlags.NonRepudiation)) continue;

                                    {
                                        var names = c.Subject.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                                        var n = names.FirstOrDefault(n => n.StartsWith("CN="))?.Substring(3)+ " ";
                                        n += names.FirstOrDefault(n => n.StartsWith("SERIALNUMBER="))?.Substring(13);
                                        n += " "+c.NotAfter.ToString("dd/MM/yyyy");
                                        var L = new CertificateItem() { Id = id, Name = n };
                                        Certs.Add(L);
                                    }
                                }
                                session.Logout();
                            }
                            //break;

                        }
                    }
                }
                catch (Exception ex)
                {
                }
                // Do something interesting in RO session

                // Alternatively session can be closed with CloseSession method of Slot class.

            }
            return Certs;
        }

        public List<PrivateKey> GetPrivateKeys(int slotid, string pin)
        {
            List<PrivateKey> PrivateKeys = new List<PrivateKey>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                List<ISlot> slots = GetUsableSlot(pkcs11Library);
                try
                {
                    foreach (var slot in slots)
                    {
                        if (slot.SlotId == (ulong)slotid)
                        {
                            
                            // Get token info
                            ITokenInfo tokenInfo = slot.GetTokenInfo();
                            using (Net.Pkcs11Interop.HighLevelAPI.ISession session = slot.OpenSession(SessionType.ReadOnly))
                            {
                                ISessionInfo sessionInfo = session.GetSessionInfo();
                                if (!string.IsNullOrEmpty(pin))
                                    session.Login(CKU.CKU_USER, pin);

                                //IObjectHandle publicKey = null;
                                //IObjectHandle privateKey = null;


                                List<CKA> keyAttributes = new List<CKA>();
                                keyAttributes.Add(CKA.CKA_ID);
                                keyAttributes.Add(CKA.CKA_LABEL);
                                keyAttributes.Add(CKA.CKA_KEY_TYPE);

                                // Define RSA private key attributes that should be read
                                List<CKA> rsaAttributes = new List<CKA>();
                                rsaAttributes.Add(CKA.CKA_MODULUS);
                                rsaAttributes.Add(CKA.CKA_PUBLIC_EXPONENT);

                                List<IObjectAttribute> keySearchTemplate = new List<IObjectAttribute>();
                                keySearchTemplate.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY));
                                keySearchTemplate.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));

                                // Find private keys
                                List<IObjectHandle> foundKeyObjects = session.FindAllObjects(keySearchTemplate);
                                foreach (IObjectHandle foundKeyObject in foundKeyObjects)
                                {
                                    List<IObjectAttribute> keyObjectAttributes = session.GetAttributeValue(foundKeyObject, keyAttributes);

                                    string ckaId = ConvertUtils.BytesToHexString(keyObjectAttributes[0].GetValueAsByteArray());
                                    string ckaLabel = keyObjectAttributes[1].GetValueAsString();
                                    AsymmetricKeyParameter publicKey = null;

                                    if (keyObjectAttributes[2].GetValueAsUlong() == Convert.ToUInt64(CKK.CKK_RSA))
                                    {
                                        List<IObjectAttribute> rsaObjectAttributes = session.GetAttributeValue(foundKeyObject, rsaAttributes);

                                        BigInteger modulus = new BigInteger(1, rsaObjectAttributes[0].GetValueAsByteArray());
                                        BigInteger exponent = new BigInteger(1, rsaObjectAttributes[1].GetValueAsByteArray());
                                        publicKey = new RsaKeyParameters(false, modulus, exponent);
                                    }

                                    PrivateKeys.Add(new PrivateKey(ckaId, ckaLabel, publicKey));
                                }
                                session.Logout();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                // Do something interesting in RO session

                // Alternatively session can be closed with CloseSession method of Slot class.

            }
            return PrivateKeys;
        }


        public List<CertificateItem> GetAll(string pin)
        {
            List<CertificateItem> Certs = new List<CertificateItem>();
            using (IPkcs11Library pkcs11Library = Settings.Factories.Pkcs11LibraryFactory.LoadPkcs11Library(Settings.Factories, driver, Settings.AppType))
            {
                // Find first slot with token present
                List<ISlot> slots = GetUsableSlot(pkcs11Library);
                try
                {
                    foreach (var slot in slots)
                    {
                        ITokenInfo tokenInfo = slot.GetTokenInfo();
                        using (Net.Pkcs11Interop.HighLevelAPI.ISession session = slot.OpenSession(SessionType.ReadOnly))
                        {
                            ISessionInfo sessionInfo = session.GetSessionInfo();
                            session.Login(CKU.CKU_USER, pin);

                            IObjectHandle publicKey = null;
                            IObjectHandle privateKey = null;



                            List<IObjectAttribute> objectAttributes = new List<IObjectAttribute>();
                            //objectAttributes.Add(new ObjectAttribute(CKA.CKA_LABEL, UTF8Encoding.UTF8.GetBytes("Certificat d'Authentification CPS")));
                            //CurrentSession.FindObjectsInit(objectAttributes);
                            var oObjCollection = session.FindAllObjects(objectAttributes);
                            //if (oObjCollection.Count > 0)
                            foreach (var item in oObjCollection)
                            {
                                var el = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_VALUE, CKA.CKA_ID, CKA.CKA_LABEL });
                                var id = el[1].GetValueAsString();
                                var label = el[2].GetValueAsString();
                                //                                var c = new X509Certificate2(el[0].GetValueAsByteArray());
                                {
                                    //var names = c.Subject.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                                    //var n = names.FirstOrDefault(n => n.StartsWith("CN="))?.Substring(3) + " ";
                                    //n += names.FirstOrDefault(n => n.StartsWith("SERIALNUMBER="))?.Substring(13);
                                    //n += " " + c.NotAfter.ToString("dd/MM/yyyy");
                                    var L = new CertificateItem() { Id = id, Name = label };
                                    Certs.Add(L);
                                }
                            }
                            session.Logout();
                        }
                    }
                }
                catch { };

                return Certs;
            }

        }


        public byte[] Sign(byte[] source, string tokenSerial, string certificateSerial, string pin)
        {
            HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256;
            SignatureScheme sigScheme = SignatureScheme.RSASSA_PKCS1_v1_5;
            using (Pkcs7SignatureGenerator pkcs7SignatureGenerator = new Pkcs7SignatureGenerator(driver, tokenSerial, pin, certificateSerial, hashAlgorithm, sigScheme))
            {
                byte[] signingCertificate = pkcs7SignatureGenerator.GetSigningCertificate();
                List<byte[]> otherCertificates = pkcs7SignatureGenerator.GetAllCertificates();
                ICollection<Org.BouncyCastle.X509.X509Certificate> certPath = CertUtils.BuildCertPath(CertUtils.ToDotNetObject(signingCertificate), otherCertificates, true);
                // Perform signing and signature encoding
                // Generate detached PKCS#7 signature
                byte[] signature = pkcs7SignatureGenerator.GenerateSignature(source, false, CertUtils.ToBouncyCastleObject(signingCertificate), certPath);
                return signature;

            }

        }



        public byte[] SignPDF(byte[] source, string reason, string tokenSerial, string certificateSerial, string pin, string? signField, int page=0, int x=50, int y=90)
        {
            try
            {
                var token = GetTokenBySerial(tokenSerial);



                using (var dest = new MemoryStream())
                {

                    using (var M = new MemoryStream(source))
                    {
                        using (Pkcs11Signature signature = new Pkcs11Signature(driver, (ulong)token.Slot)
                                    .Select(null, certificateSerial, pin).SetHashAlgorithm("SHA256"))

                        using (PdfReader pdfReader = new PdfReader(M))
                        {
                            if (!string.IsNullOrEmpty(signField))
                            {
                                //using (var pdfDoc = new PdfDocument(pdfReader)) 
                                {
                                    //var acroForm = PdfAcroForm.GetAcroForm(pdfDoc, false);
                                    //if (acroForm != null)
                                    //{
                                    //    var f = acroForm.GetField(signField);
                                    //    if (f != null)
                                    //    {
                                    //        var w = f.GetWidgets().First();
                                    //        var arect = w.GetRectangle();
                                    //        var Left = (float)arect.GetAsNumber(0).FloatValue();
                                    //        var Bottom = (float)arect.GetAsNumber(1).FloatValue();
                                    //        var Right = (float)arect.GetAsNumber(2).FloatValue();
                                    //        var Top = (float)arect.GetAsNumber(3).FloatValue();
                                    //        var rect = new Rectangle(Left, Top, Right - Left, Bottom - Top);
                                    //        page = pdfDoc.GetPageNumber(w.GetPage());
                                            PdfSigner pdfSigner = new PdfSigner(pdfReader, dest, new StampingProperties().UseAppendMode());
                                            pdfSigner.SetSignDate(DateTime.Now);
                                            pdfSigner.SetFieldName(signField);
                                            ITSAClient tsaClient = new TSAClientBouncyCastle("http://timestamp.entrust.net/TSS/RFC3161sha2TS");
                                            var t = tsaClient.GetMessageDigest();
                                            pdfSigner.SignDetached(signature, (IX509Certificate[])signature.GetChain(), null, null, tsaClient, 0, PdfSigner.CryptoStandard.CMS);
                                        //}
                                    //}
                                }
                            } else
                            {

                                PdfSigner pdfSigner = new PdfSigner(pdfReader, dest, new StampingProperties().UseAppendMode());
                                pdfSigner.SetSignDate(DateTime.Now);
                                if (page > 0)
                                {
                                    Rectangle pageSize = pdfSigner.GetDocument().GetPage(page).GetPageSizeWithRotation();
                                    var w = pageSize.GetWidth();
                                    var h = pageSize.GetHeight();
                                    Rectangle rect = new Rectangle(w * x / 100, (h * (93 - y) / 100), (w / 100) * 30, (h / 100) * 7);

                                    PdfSignatureAppearance appearance = pdfSigner.GetSignatureAppearance();
                                    appearance
                                            .SetReason(reason)
                                            .SetLocation("")

                                            // Specify if the appearance before field is signed will be used
                                            // as a background for the signed field. The "false" value is the default value.
                                            .SetReuseAppearance(false)
                                            .SetPageRect(rect)
                                            .SetPageNumber(page);
                                    if (string.IsNullOrEmpty(signField))
                                        signField = "signature_p" + page + "_x" + x + "_y" + y;
                                    pdfSigner.SetFieldName(signField);

                                }
                                ITSAClient tsaClient = new TSAClientBouncyCastle("http://timestamp.entrust.net/TSS/RFC3161sha2TS");
                                var t = tsaClient.GetMessageDigest();
                                pdfSigner.SignDetached(signature, (IX509Certificate[])signature.GetChain(), null, null, tsaClient, 0, PdfSigner.CryptoStandard.CMS);
                            }

                        }
                    }
                    return dest.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        private List<ISlot> GetUsableSlot(IPkcs11Library pkcs11Library)
        {
            List<ISlot> slots = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent);
            List<ISlot> matchingSlot = new();
            // ...unless there are matching criteria specified in Settings class
            if (Settings.TokenSerial != null || Settings.TokenLabel != null)
            {
                matchingSlot = null;

                foreach (ISlot slot in slots)
                {
                    ITokenInfo tokenInfo = null;

                    try
                    {
                        tokenInfo = slot.GetTokenInfo();
                    }
                    catch (Pkcs11Exception ex)
                    {
                        if (ex.RV != CKR.CKR_TOKEN_NOT_RECOGNIZED && ex.RV != CKR.CKR_TOKEN_NOT_PRESENT)
                            throw;
                    }

                    if (tokenInfo == null)
                        continue;

                    if (!string.IsNullOrEmpty(Settings.TokenSerial))
                        if (0 != string.Compare(Settings.TokenSerial, tokenInfo.SerialNumber, StringComparison.Ordinal))
                            continue;

                    if (!string.IsNullOrEmpty(Settings.TokenLabel))
                        if (0 != string.Compare(Settings.TokenLabel, tokenInfo.Label, StringComparison.Ordinal))
                            continue;

                    matchingSlot.Add(slot);
                }
            }
            else matchingSlot = slots;

            //                Assert.IsTrue(matchingSlot != null, "Token matching criteria specified in Settings class is not present");
            return matchingSlot;
        }


    }
}
