
using Core.TitulusIntegration.Models;

namespace Core.TitulusIntegration.Test
{
    public class Tests
    {
        private Titulus4 TitulusService;
        [SetUp]
        public void Setup()
        {
            // Ambiente di Test di UNIPA
            TitulusService = new Titulus4("https://", "user", "pwd");
        }

        [Test]
        public void Get_Protocol_Should_Pass()
        {
            var response = TitulusService.GetProtocolData(2845368);
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno+" - physdoc: " + response.physdoc+ " - " + response.Oggetto);
            else
                Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void Get_Protocol_by_Number_Should_Pass()
        {
            var response = TitulusService.GetProtocolData("2024-UNPACLE-0000106");
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno + " - physdoc: " + response.physdoc + " - " + response.Oggetto);
            else
                Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void Create_Draft_Inbound_Protocol_Should_Pass()
        {
            InboundDocument doc = new InboundDocument() { 
                Note="Annotazioni",
                Oggetto="Questo oggetto è lungo almeno 30 caratteri",
                PersonaEsterna = "costantino", 
                //StrutturaEsterna = "SE000086",
                VoceIndice = "UWEB - Autorizzazione Missione",
                StruttureInterne = new List<string>() { "SI000001" },
                PersoneInterne = new List<string>() { "Tommaso" },
                PersoneInterneCC = new List<string>() { "annalisa" },
                CriterioRicercaPersoneInterne = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaPersonaEsterna = CustomDataEnum.PersoneInterneDaLogin
            };
            var response = TitulusService.SaveInboundProtocol(doc, true);
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno + " - physdoc: " + response.physdoc + " - " + response.Oggetto);
            else
                Assert.Fail();
        }


        [Test]
        public void Create_Inbound_Protocol_Should_Pass()
        {
            InboundDocument doc = new InboundDocument()
            {
                Note = "Annotazioni",
                Oggetto = "Questo oggetto è lungo almeno 30 caratteri",
                PersonaEsterna = "costantino",
                //StrutturaEsterna = "SE000086",
                VoceIndice = "UWEB - Autorizzazione Missione",
                //StruttureInterne = new List<string>() { "SI000001" },
                PersoneInterne = new List<string>() { "Tommaso" },
                PersoneInterneCC = new List<string>() { "annalisa" },
                CriterioRicercaPersoneInterne = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaPersonaEsterna = CustomDataEnum.PersoneInterneDaLogin
            };
            var response = TitulusService.SaveInboundProtocol(doc, false);
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno + " - physdoc: " + response.physdoc + " -  Prot." + response.Protocollo+" - " + response.Oggetto);
            else
                Assert.Fail();
        }

        [Test]
        public void Create_Outbound_Protocol_Should_Pass()
        {
            OutboundDocument doc = new OutboundDocument()
            {
                Note = "Annotazioni",
                Oggetto = "Questo oggetto è lungo almeno 30 caratteri",
                Tipologia="Dichiarazione",
                FileContent = new byte[] { 1, 2, 3 },
                FileName = "test con tre byte",
                VoceIndice = "UGOV - Ordini",
                PersoneEsterne = new List<string>() { "Tommaso" },
                StruttureEsterne = new List<string>() { "SE000086" },

                PersoneEsterneCC = new List<string>() { "annalisa" },
                StruttureEsterneCC = new List<string>() { "SE000086" },

                CriterioRicercaPersonaInterna = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaPersonaEsterna = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaStrutturaEsterna = CustomDataEnum.StruttureEsterneDaCodice
            };
            var response = TitulusService.SaveOutboundProtocol(doc, false);
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno);
            else
                Assert.Fail();
        }



        [Test]
        public void Create_Internal_Protocol_Should_Pass()
        {
            InternalDocument doc = new InternalDocument()
            {
                Note = "Annotazioni",
                Oggetto = "Questo oggetto è lungo almeno 30 caratteri",
                PersoneEsterne = new List<string>() { "Tommaso" },
                PersoneEsterneCC = new List<string>() { "annalisa" },
                Tipologia = "Dispositivo Missione",
                VoceIndice = "UGOV - Ordini",
                PersonaInterna = "costantino",
                CriterioRicercaPersonaInterna = CustomDataEnum.PersoneInterneDaLogin,
            };
            var response = TitulusService.SaveInternalProtocol(doc, false);
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno);
            else
                Assert.Fail();
        }

        [Test]
        public void Create_Outbound_Protocol_Policlinico_Should_Pass()
        {
            OutboundDocument doc = new OutboundDocument()
            {
                Note = "Annotazioni",
                Oggetto = "Questo oggetto è lungo almeno 30 caratteri",
                PersoneEsterne = new List<string>() { "Taramella Giuseppe" },
                StruttureEsterne = new List<string>() { "SE000095" },
                FileContent = new byte[] { 1, 2, 3 },
                FileName = "test con tre byte",
                VoceIndice = "Flusso per delibere",
                Tipologia = "Delibera_OLD",
                PersonaInterna = "Giovanni",
                CriterioRicercaPersonaInterna = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaPersonaEsterna = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaStrutturaEsterna = CustomDataEnum.StruttureEsterneDaCodice
            };
            var response = TitulusService.SaveOutboundProtocol(doc, false);
            if (response.IdInterno != "")
                Assert.Pass("ID: " + response.IdInterno);
            else
                Assert.Fail();
        }


    }
}