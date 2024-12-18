using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Domain.QueryBuilder;

public interface IQueryField : IQueryElement
{
    /// <summary>
    /// Titolo della colonna da inserire come intestazione di tabella
    /// </summary>
    string Title { get;  }

    /// <summary>
    /// Titolo/descrizione estesa della colonna
    /// </summary>
    string Description { get;  }

    /// <summary>
    /// Raggruppamento della colonna
    /// </summary>
    string Category { get;  }

    /// <summary>
    /// Dimensione Minima
    /// </summary>
    int Size { get;  }

    /// <summary>
    /// Indica se il campo è ridimensionabile
    /// </summary>
    bool Resizable { get;  }

    /// <summary>
    /// Elenco posizionale di valori di decodifica per le colonne numeriche o binarie
    /// </summary>
    string[] LookupValues { get;  }

    /// <summary>
    /// Elenco posizionale di valori di tooltip per le colonne numeriche o binarie
    /// </summary>
    string[] ToolTips { get;  }


    /// <summary>
    /// Ritorna una coppia codice/decodifica da inserire in una colonna di risultati in griglia
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    //Task<SearchResultColumn> Render(IQueryResultRow row);

}
