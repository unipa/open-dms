using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Models
{
    public class IDC
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string CodiceAzienda { get; set; } = "";
        public int AnnoCompetenza { get; set; }
        public string CodiceTipologia { get; set; }
        public string IDC_CS { get; set; } 
        public string VDC { get; set; }
        public string IDCFile { get; set; }
        public string VDCPath { get; set; }
        public string Tipologia { get; set; }
        public string Produttore { get; set; }
        public string Signer { get; set; }
        public string FunzioneHash { get; set; }
        public string TimeRef { get; set; }
        public string HashCode { get; set; }
        public string Metadata { get; set; }
        public int Signed { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public string DT_Da { get; set; }
        public string DT_A { get; set; }
        public string NomeProduttore { get; set; }
        public string RuoloProduttore { get; set; }
        public string CFProduttore { get; set; }
        public int Ultimo_Doc_PDA { get; set; }
        public int AnnoScadenza { get; set; }

    }

}
