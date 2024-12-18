namespace Core.TitulusIntegration.Models
{
    public class InboundDocument : BaseDocument
    {


        public CustomDataEnum CriterioRicercaPersoneInterne { get; set; } = CustomDataEnum.PersoneInterneDaCognome;
        public CustomDataEnum CriterioRicercaPersonaEsterna { get; set; } = CustomDataEnum.PersonaEsternaDaMatricola;
        public CustomDataEnum CriterioRicercaStrutturaEsterna { get; set; } = CustomDataEnum.StruttureEsterneDaCodice;


        public List<string> StruttureInterne { get; set; } = new();
        public List<string> PersoneInterne { get; set; } = new();
        //public List<string> StruttureInterne { get; set; } = new();
        public List<string> PersoneInterneCC { get; set; } = new();


        //public CustomDataEnum CriterioRicercaPersoneInterne { get; set; } = CustomDataEnum.PersoneInterneDaCognome;
        public string PersonaEsterna { get; set; } = "";
        public string StrutturaEsterna { get; set; } = "";

    }
}