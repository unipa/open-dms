using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenDMS.Reports.DTOs
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
        public Misura Minimo { get; set; } = new();
        public Misura Medio { get; set; } = new();
        public Misura Massimo { get; set; } = new();
    }

    // 
    public class Misure
    {
        public Misura Istanze { get; set; } = new();
        public Misura TaskNonGestiti { get; set; } = new();
        public Misura TaskPresiInCarico { get; set; } = new();
        public Misura TaskLavorati { get; set; } = new();
        public Misura TaskInRitardo { get; set; } = new();

        public MisuraTemporale TempoPresaInCarico { get; set; } = new();
        public MisuraTemporale TempoLavorazione { get; set; } = new();
        // Tempo medio di lavorazione rispetto alla scadenza
        public MisuraTemporale Performance { get; set; } = new();
    }

    public class MisureProcesso  {
        public int BusinessProcessId { get; set; }
        public string BusinessProcessKey { get; set; }
        public string BusinessProcessName { get; set; }
        public Misura Risorse { get; set; } = new();


        public Misure Misure { get; set; } = new();
    }

    public class MisureRisorsa
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }

        public Misura Processi { get; set; } = new();

        public Misure Misure { get; set; } = new();
    }

    public class MisureMensili
    {
        public Misura Tempo { get; set; } = new();
        public Misure Misure { get; set; } = new();
    }


    public class DettaglioProcessi
    {
        public List<MisureProcesso> Processi { get; set; } = new();
    }

    public class DettaglioTasks
    {
        public List<MisureProcesso> Tasks { get; set; } = new();
    }

    public class DettaglioRisorse
    {
        public List<MisureRisorsa> Risorse { get; set; } = new();
    }

    public class DettaglioMensile
    {
        public List<MisureRisorsa> Mesi { get; set; } = new();
    }

    public class KPI
    {
        // Identificatore dello schema di processo
        public string BusinessProcessId { get; set; }
        
        // Identificatore dell'ìstanza di processo
        public string BusinessProcessKey { get; set; }

        // Identificatore del tipo di Chiave (es. "Tempo da RDA o ODA", oppure "Ordini > 50K", oppure "Importo Medio Ordine")
        public string IndicatorId { get; set; }

        public string UserId { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime EvaluationDate { get; set; }

    }

    public class ReportFilters
    {
        public int ReportType { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProcessId { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string GroupId { get; set; }
    }


    public class Dashboard
    {
        public ReportFilters Filters { get; set; } = new();

        public Misure Summary { get; set; } = new();
        public DettaglioMensile Chart { get; set; } = new();
        public DettaglioProcessi  Processes { get; set; } = new();
        public DettaglioTasks Tasks { get; set; } = new();
        public DettaglioRisorse Resources { get; set; } = new();

    }



}
