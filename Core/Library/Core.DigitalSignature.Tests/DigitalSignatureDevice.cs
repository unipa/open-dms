
namespace Core.DigitalSignature.Tests
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Read_Slot_With_Right_Vendor()
        {
            // Prendo il primo vendor gestito
            var driver = global::DigitalSignature.Token.Vendors.List.First().Key;
            // inizializzo la libreria
            Core.DigitalSignature.Pkcs11.SignatureDevice SignatureDevice = new Pkcs11.SignatureDevice(driver);
            // recupero le porte
            var slots = SignatureDevice.GetSlots();
            if (slots.Count > 0) Assert.Pass($"Trovati {slots.Count} slot");
            else Assert.Fail("Nessuno Slot Trovato");
        }


        [Test]
        public void Get_Error_With_Empty_Vendor()
        {
            // Prendo il primo vendor gestito
            var driver = "";
            // inizializzo la libreria
            try
            {
                Core.DigitalSignature.Pkcs11.SignatureDevice SignatureDevice = new Pkcs11.SignatureDevice(driver);
                // recupero le porte
                var slots = SignatureDevice.GetSlots();
                Assert.Fail($"Non è stato generato un errore passando un Vendor vuoto");
            }
            catch (Exception ex)
            {
                Assert.Pass($"OK. Errore {ex.Message}");
            }
        }


        [Test]
        public void Get_Certificates_On_First_Slot()
        {
            // Prendo il primo vendor gestito
            var driver = global::DigitalSignature.Token.Vendors.List.First().Key;
            // inizializzo la libreria
            Core.DigitalSignature.Pkcs11.SignatureDevice SignatureDevice = new Pkcs11.SignatureDevice(driver);
            // recupero le porte
            var slot = SignatureDevice.GetSlots().First();
            var certificate = SignatureDevice.GetCertificates(slot.Id, true);
            if (certificate.Count > 0)
                Assert.Pass($"Trovati {certificate.Count} certificati");
            else Assert.Fail("Nessun Certificato Trovato");
        }
    }
}