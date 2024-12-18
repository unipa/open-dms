using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder;

public interface IQueryElement
{
    /// <summary>
    /// Identificativo Univoco della colonna
    /// </summary>
    string Id { get;  }

    /// <summary>
    /// Tipo di metadato della colonna
    /// </summary>
    ColumnDataType DataType { get;  }

    QueryModel ModelId { get;  }
}
