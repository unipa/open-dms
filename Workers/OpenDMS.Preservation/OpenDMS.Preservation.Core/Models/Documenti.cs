using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Models
{
    public class Documenti
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string CodiceAzienda { get; set; } = "";
        public int AnnoCompetenza { get; set; }
        public string CodiceTipologia { get; set; }
        public string IDC { get; set; }
        public string VDC { get; set; }
        public int IDDocumento { get; set; }
        public string FileType { get; set; }
        public int IDImmagine { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public string Produttore { get; set; }
        public string Descrizione { get; set; }
        public string NomeFile { get; set; }
        public decimal DimensioneFile { get; set; }
        public int Signed { get; set; }
        public string FunzioneHash { get; set; }
        public string RiferimentoTemporale { get; set; }
        public string HashCode { get; set; }

    }
}


