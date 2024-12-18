namespace OpenDMS.Domain.Enumerators;
//{

public enum ArchivingStrategy
{
    /// <summary>
    /// Abilita il drag & drop di files dall'estensione prevista nella tipologia
    /// </summary>
    DragAndDrop = 0,
    /// <summary>
    /// Abilita l'acquisizione da barcode, scanner e drag & drop
    /// </summary>
    BarcodeRecognition = 1,
    /// <summary>
    /// Utilizza un altro documento come contenuto predefinito
    /// </summary>
    Template = 4,

    Form = 8,
    //CreateWF = 9,
    //CreateQuery = 10
}
//}