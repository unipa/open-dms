namespace Core.TitulusIntegration.Models
{
    public class OutboundDocument : BaseDocument
    {


        public CustomDataEnum CriterioRicercaPersonaEsterna  { get; set; } = CustomDataEnum.PersonaEsternaDaMatricola;
        public CustomDataEnum CriterioRicercaPersonaInterna { get; set; } = CustomDataEnum.PersoneInterneDaCognome;
        public CustomDataEnum CriterioRicercaStrutturaEsterna { get; set; } = CustomDataEnum.StruttureEsterneDaNome;



        public string PersonaInterna { get; set; } = "";

        public List<string> PersoneEsterne { get; set; } = new();
        public List<string> StruttureEsterne { get; set; } = new();

        public List<string> PersoneEsterneCC { get; set; } = new();
        public List<string> StruttureEsterneCC { get; set; } = new();



    }
}