namespace Core.TitulusIntegration.Models
{
    public class InternalDocument : BaseDocument
    {
        public string PersonaInterna { get; set; } = "";
        //public string StrutturaInterna { get; set; } = "";

        public List<string> PersoneEsterne { get; set; } = new();
        public List<string> PersoneEsterneCC { get; set; } = new();

        public List<string> StruttureInterne { get; set; } = new();
        public List<string> StruttureInterneCC { get; set; } = new();

        public CustomDataEnum CriterioRicercaPersonaInterna { get; set; } = CustomDataEnum.PersoneInterneDaLogin;//.PersoneInterneDaCognome;
        public CustomDataEnum CriterioRicercaStrutturaInterna { get; set; } = CustomDataEnum.StruttureInterneDaNome;
    }
}