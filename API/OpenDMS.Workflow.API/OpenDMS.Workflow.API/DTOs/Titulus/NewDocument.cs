using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;

namespace OpenDMS.Workflow.API.DTOs.Titulus
{
    public class NewDocument
    {
        public string Oggetto { set; get; }
        public string Tipo { set; get; }
        public string data_prot { set; get; }
        public string? data_arrivo { set; get; }
        public string commento_prot_differito { set; get; }
        public string tipologia_cod { set; get; }
        public string? mezzo_trasmissione_costo { set; get; }
        public string? mezzo_trasmissione_valuta { set; get; }
        public string? mezzo_trasmissione_cod { set; get; }
        public string? note { set; get; }
        public string riferimenti_innertext { set; get; }
        public string voce_indice_innertext { set; get; }
        public string rif_esterno_dataprot { set; get; }
        public string rif_esterno_numprot { set; get; }
        public string rif_esterno_nome_cod { set; get; }
        public string rif_esterno_nome_innertext { set; get; }
        public string rif_esterno_referente_cod { set; get; }
        public string rif_esterno_referente_nominativo { set; get; }
        public string? rif_esterno_indirizzo_email { set; get; }
        public string? rif_esterno_indirizzo_email_certificata { set; get; }
        public string? rif_esterno_fax { set; get; }
        public string? rif_esterno_tel { set; get; }
        public string? rif_esterno_indirizzo_innertext { set; get; }
        public string rif_interno_diritto_rpa = "RPA";
        public string rif_interno_codice_persona { set; get; }
        public string rif_interno_codice_ufficio { set; get; }
        public string rif_interno_nome_persona { set; get; }
        public string rif_interno_nome_ufficio { set; get; }
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public string Content { get; set; }
        public string Description { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FileName { get; set; }
        public string MimeType = "application/pdf";
    }
}
