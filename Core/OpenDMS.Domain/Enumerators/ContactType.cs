namespace OpenDMS.Domain.Enumerators
{

    // G=Impresa,
    // P=PA(deve avere codiceipa),
    // G=Professionista (Piva e sesso),
    // C=Cittadino (cf e sesso),
    // =Generico(nulla),
    // @=Contatto Aziendale(email/pec/tel),
    // I=Indirizzo

    public enum  ContactType
    {
        Undefined = 0,
        // Indirizzo Email o Telefonico.
        // non hanno obbligo di Partita IVA o C.F.
        Contact = 1,
        // Cittadino
        // Deve avere un C.F. Univoco
        Citizen = 2,
        // Professionista a Partita IVA
        // Deve avere una Partita IVA Univoca
        FreeLancer = 3,
        // Impresa
        // Deve avere una Partita IVA Univoca
        Company = 4,
        // PA Centrale
        // Deve avere un C.F. Univoco e un Codice IPA 
        CentralAdministration = 5,
        // PA Locale
        // non ha obbligo di avere un C.F. Univoco ma deve avere un Codice IPA e il riferimento alla PA Centrale
        LocalAdministration = 6,
        // Altre Organizzazioni
        // Nessun obbligo di Partita IVA o C.F. Univoci
        NonProfit = 7

    }
}