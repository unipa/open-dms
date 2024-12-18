/*
namespace OpenDMS.Core.Managers
{
    public class TemplateManager
    {

        public static string GetNewFileName(DataSource DS, BancaDati b, Documento d, TipoDocumento tp, string filename, int NrRevisione)
        {
            // Estraggo l'estensione completa del file (es. .pdf.p7m) e cerco di escludere le estensioni più lunghe di 4
            string fn = filename.ToLower();
            string x = "";
            string ext = "";
            do
            {
                x = Path.GetExtension(fn);
                if (!string.IsNullOrEmpty(x))
                {
                    if (x != ".")
                    {
                        ext = x + ext;
                        fn = fn.Substring(0, fn.Length - x.Length);
                    }
                    else x = "";
                }
                else x = "";
            } while (!string.IsNullOrEmpty(x));

            string NewFileName;
            if (tp.Codice.ToLower().StartsWith("td") && tp.Codice.Length == 4 ||
                tp.Direzione == "W"  || tp.Direzione == "S" )
            {
                string piva = TemplateManager.GetPIVA(b.Codice.PadLeft(3, '0'));
                if (!Path.GetFileName(filename).StartsWith(piva) && piva.Length > 3)
                    NewFileName = TemplateManager.GetFEFileName(DS, piva, b.Codice.PadLeft(3, '0'), ".xml" + (ext.EndsWith(".p7m") ? ".p7m" : ""));
                else
                {
                    string f = Path.GetFileName(filename);
                    if (f.ToUpper().StartsWith("TEMP_"))
                    {
                        f = f.Substring(5);
                        int i = f.IndexOf("_");
                        if (i >= 0)
                            f = f.Substring(i + 1);
                    }
                    NewFileName = f;
                }
            }
            else
                NewFileName = "GDD" + b.Codice.PadLeft(3, '0') + "-" + d.id.ToString().PadLeft(11, '0') + "-" + NrRevisione.ToString().PadLeft(3, '0') + ext;
            return NewFileName;
        }

        public static string GetStorePath(BancaDati b, Documento d, TipoDocumento tp, string filename)
        {
            string percorso = b.Percorso;
            if (string.IsNullOrEmpty(percorso))
                percorso = ConfigurationManager.AppSettings["DocPath"];
            string percorsofinale = ConfigurationManager.AppSettings["DocSubPath"] + "";
            if (string.IsNullOrEmpty(percorsofinale))
                percorsofinale = "[ANNO]\\[MESE]";
            //Modifica Giovanni
            string Settimana = (d.DataProtocollo.DayOfYear / 7 + (d.DataProtocollo.DayOfYear % 7 > 0 ? 1 : 0)).ToString();
            percorsofinale = percorsofinale.Replace("[ANNO]", d.DataProtocollo.Year.ToString()).Replace("[MESE]", d.DataProtocollo.Month.ToString()).Replace("[TIPO]", tp.Descrizione.Replace("\\", "-")).Replace("[TIPOLOGIA]", tp.Descrizione.Replace("\\", "-")).Replace("[CODICETIPO]", tp.Codice).Replace("[SETTIMANA]", Settimana);

            percorsofinale = percorsofinale.Replace("/", "-").Replace("*", "").Replace(":", "").Replace("<", "-").Replace(">", "-").Replace("|", "");
            percorso = Path.Combine(percorso, percorsofinale);

            string NewFileName = percorso;
            return NewFileName;
        }



 


    }
}
*/