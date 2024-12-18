using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Services;

public interface IDataTypeManager
{
    /// <summary>
    /// Tio di dato: es. Testo, Data, Tabella
    /// </summary>
    string DataTypeValue { get;  }

    /// <summary>
    /// Nome del tipo di dato: es. "Testo", "Testo Multiriga"
    /// </summary>
    string DataTypeName { get;  }

    /// <summary>
    /// Classe CSS dell'icona che rappresenta la tipolgia di campo
    /// </summary>
    string Icon { get;  }

    /// <summary>
    /// Indica se è ricercabile
    /// </summary>
    bool IsSearchable { get; }

    /// <summary>
    /// Indica se è un campo calcolato
    /// </summary>
    bool IsCalculated { get; }

    /// <summary>
    /// Indica se il tipo di dato è ad uso interno
    /// </summary>
    bool IsInternal { get; }

    /// <summary>
    /// Indica se il dato va memorizzato nella tabella dei Blobl
    /// </summary>
    bool IsBlob { get; }

    /// <summary>
    /// Indica se deve essere registrato tra i contatti
    /// </summary>
    bool IsPerson { get; }

    /// <summary>
    /// Nome del componente web che gestirà la configurazione delle proprietà
    /// </summary>
    string AdminWebComponent { get;  }

    /// <summary>
    /// Nome del componente web che gestirà l'input del campo
    /// </summary>
    string WebComponent { get;  }

    /// <summary>
    /// Tipo di componente da renderizzare
    /// </summary>
    string ControlType { get; }


    /// <summary>
    /// Elenco di configurazioni di campi standard (es. campo testo, data, ...)
    /// </summary>
    FieldType[] AvailableFields { get;  }


    Task<FieldTypeValue> Lookup(FieldType M, string Value);
    Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document);
    Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8);

    string Validate(FieldType M, string Value);


    string Serialize(string Value, bool Cifrato = false);
    string Deserialize( string Value, bool Cifrato = false);

}