using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    // Risultato di una misurazione (valore reale e valore formattato)
    public class Misura
    {
        public int Valore { get; set; }
        public int Etichetta { get; set; }
    }

    // Risultato relativo ad una tempistica (valore minimo, medio e massivo)
    public class MisuraTemporale
    {
        public Misura Minimo { get; set; }
        public Misura Medio { get; set; }
        public Misura Massimo { get; set; }
    }

    // 
    public class Misure
    {
        public Misura Istanze { get; set; }
        public Misura TaskNonGestiti { get; set; }
        public Misura TaskPresiInCarico { get; set; }
        public Misura TaskLavorati { get; set; }
        public Misura TaskInRitardo { get; set; }

        public MisuraTemporale TempoPresaInCarico { get; set; }
        public MisuraTemporale TempoLavorazione { get; set; }
        // Tempo medio di lavorazione rispetto alla scadenza
        public MisuraTemporale Performance { get; set; }
    }

    public class MisureProcesso  {
        public int BusinessProcessId { get; set; }
        public string BusinessProcessKey { get; set; }
        public string BusinessProcessName { get; set; }
        public Misura Risorse { get; set; }


        public Misure Misure { get; set; }
    }

    public class MisureRisorsa
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }

        public Misura Processi { get; set; }

        public Misure Misure { get; set; }
    }

    public class MisureMensili
    {
        public Misura Tempo { get; set; }
        public Misure Misure { get; set; }
    }


    public class DettaglioProcessi
    {
        public List<MisureProcesso> Processi { get; set; }
    }
    public class DettaglioRisorse
    {
        public List<MisureRisorsa> Risorse { get; set; }
    }
    public class DettaglioMensile
    {
        public List<MisureRisorsa> Mesi { get; set; }
    }


    public class Filtri
    {
        public int ReportType { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string BusinessProcessKey { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string GroupId { get; set; }

    }


    public class Dashboard
    {

        public List<Misure> Totali { get; set; }
        public DettaglioMensile DatiMensili { get; set; }
        public DettaglioProcessi  Processi { get; set; }
        public DettaglioRisorse Risorse { get; set; }


    }




}
