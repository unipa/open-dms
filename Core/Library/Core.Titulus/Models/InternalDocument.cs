namespace Core.TitulusIntegration.Models
{
    public class InternalDocument : BaseDocument
    {
        public string VoceIndice { get; set; } = "";
        public string PersonaInterna { get; set; } = "";
        //public string StrutturaInterna { get; set; } = "";

        public List<string> PersoneEsterne { get; set; } = new();

        public List<string> PersoneEsterneCC { get; set; } = new();
        public CustomDataEnum CriterioRicercaPersonaInterna { get; set; } = CustomDataEnum.PersoneInterneDaCognome;
    }
}