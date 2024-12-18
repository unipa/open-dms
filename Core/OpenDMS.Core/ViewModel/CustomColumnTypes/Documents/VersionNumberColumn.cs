using OpenDMS.Core.DTOs;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.ViewModel.CustomColumnTypes.Documents
{
    public class VersionNumberColumn : GenericNumberColumn
    {
        public VersionNumberColumn() : base(
            DocumentColumn.VersionNumber, "Ver.", "Versione", "Contenuto", 60)
        {
            Fields = new() { DocumentColumn.Version, DocumentColumn.Revision };
        }

        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            var c = fields[0];
            var d = fields[1];
            var e = string.IsNullOrEmpty(c) ? "" : c + "." + d.PadLeft(2, '0');
            return new SearchResultColumn() { Value = e, Description = e };
        }
    }
}
