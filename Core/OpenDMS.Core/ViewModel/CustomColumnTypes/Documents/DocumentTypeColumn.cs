using OpenDMS.Core.DTOs;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.ViewModel.CustomColumnTypes.Documents
{
    public class DocumentTypeColumn : GenericTextColumn
    {
        public DocumentTypeColumn() : base(
            DocumentColumn.DocumentType,
            "Tipologia",
            "Tipologia",
            "Documento",
            0)
        {
            Fields = new List<string>() { DocumentColumn.DocumentType, DocumentColumn.ContentType };
        }

        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            var a = fields[0];
            var b = fields[1];
            if (string.IsNullOrEmpty(a))
                b = b == "1" ? "Documento Generico" : "Fascicolo Generico";
            return new SearchResultColumn() { Value = a, Description = b };
        }
    }
}
