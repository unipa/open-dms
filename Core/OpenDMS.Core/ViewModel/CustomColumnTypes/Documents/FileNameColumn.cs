using OpenDMS.Core.DTOs;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;
using OpenDMS.Core.ViewModel.ColumnTypes;

namespace OpenDMS.Core.ViewModel.CustomColumnTypes.Documents
{
    public class Document_FileName : GenericTextColumn
    {
        public Document_FileName() : base(
            DocumentColumn.FileName, "Nome File", "Nome File", "Contenuto",  0)
        {
        }

        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            var v = fields[0];
            if (!string.IsNullOrEmpty(v)) v = Path.GetFileName(v);
            return new SearchResultColumn() { Value = v, Description = v };
        }
    }
}
