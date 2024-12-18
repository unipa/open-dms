using Org.BouncyCastle.Utilities.Zlib;

namespace OpenDMS.PdfManager.test
{
    public class Tests
    {
        public const string JSONVariables = "{ 'title':'Questo è il titolo', 'richiesto' : '1', 'rows' : [{ 'index':-2, 'name':'a' },{ 'index':1, 'name':'b' },{ 'index':2, 'name':'c' }   ] }";
        public const string HtmlTemplate = "<h1>{{title}}</h1><div style='margin-top:20px'><ul><for rows><li>{{rows[{index}].index}}/{count} - {{rows[{index}].name}}.</li></for rows></ul>"
            + "<div style='width:720px;height:10px;background-color:#888'></div>"
            + "<div style='border:1px solid red;height:160px;width:33%;text-align:center'>Firma<br/><span style='font-size:2em'>[FIRMA:DIRETTORE]</span></div>"
            + "<div style='border:1px solid red;position:relative;top:-200px;left:66%;height:160px;width:33%;text-align:center'>Firma<br/>[FIRMA:RICHIEDENTE-1]</div>"
            + "<br/><br/><br/>"
            + "<div style='border:1px solid red;height:160px;width:33%;text-align:center'>Firma<br/><span style='font-size:2em'>[FIRMA:DIRETTORE]</span></div>"
            + "<br/><br/><br/>"
            + "<div style='border:1px solid red;height:160px;width:33%;text-align:center'>Firma<br/><span style='font-size:2em'>[FIRMA:DIRETTORE]</span></div>"
            + "<br/><br/><br/>"
            + "<div style='border:1px solid red;height:160px;width:33%;text-align:center'>Firma<br/><span style='font-size:2em'>[FIRMA:DIRETTORE]</span></div>"
            + "<br/><show-if richiesto>RICHIESTO SHOW</show-if richiesto><br/><br/>"
            + "<br/><show-if richiesto==1>RICHIESTO 1 SHOW</show-if richiesto==1><br/><br/>"
            + "<br/><hide-if richiesto>RICHIESTO HIDE</hide-if richiesto><br/><br/>"
            + "<br/><hide-if richiesto!=0>RICHIESTO 1 HIDE</hide-if richiesto!=0><br/><br/>"
            + "<br/><hide-if richiesto==0>RICHIESTO 0 HIDE</hide-if richiesto==0><br/><br/>"
            + "<br/><br/><br/>"
            + "<div style='border:1px solid red;height:160px;width:33%;text-align:center'>Firma<br/><span style='font-size:2em'>[FIRMA:DIRETTORE]</span></div>"
            + "</div>";

        public const string OutputPdf = "z:\\test.pdf";
        [Test]
        public async Task CreatePDFWithSignatures()
        {
            var template = HtmlTemplateParser.Parse(HtmlTemplate, JSONVariables);
            using (MemoryStream input = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(template)))
            {
                var output = (MemoryStream)await HTMLConverter.ConvertToPDF(input);
                File.WriteAllBytes(OutputPdf, output.ToArray());    
            }
            Assert.Pass(File.Exists(OutputPdf).ToString()+" - "+ new FileInfo(OutputPdf).Length.ToString());
        }

        [Test]
        public void Parserows()
        {
            var template = HtmlTemplateParser.Parse(HtmlTemplate, JSONVariables);
            Assert.Pass(template);
        }

        [Test]
        public void ParseShowIfWithNoOperator()
        {
            var template = HtmlTemplateParser.Parse(HtmlTemplate, JSONVariables);
            Assert.IsTrue (template.IndexOf("RICHIESTO HIDE") <= 0 && template.IndexOf("RICHIESTO SHOW") >= 0);
        }
        [Test]
        public void ParseShow1IfWithOperator()
        {
            var template = HtmlTemplateParser.Parse(HtmlTemplate, JSONVariables);
            Assert.IsTrue(template.IndexOf("RICHIESTO HIDE") <= 0 
                && template.IndexOf("RICHIESTO SHOW") >= 0 
                && template.IndexOf("RICHIESTO 1 HIDE") <= 0 
                && template.IndexOf("RICHIESTO 1 SHOW") >= 0 
                && template.IndexOf("RICHIESTO 0 HIDE") >= 0);
        }
    }
}