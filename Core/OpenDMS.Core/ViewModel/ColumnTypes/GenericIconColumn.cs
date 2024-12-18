using OpenDMS.Core.DTOs;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.ViewModel.ColumnTypes
{
    public class GenericIconColumn : ViewColumn
    {
        private readonly int defaultValue;

        public GenericIconColumn(
            string id, 
            string title, 
            string description, 
            string category,
            List<string> tooltips,
            List<string> values, int defaultValue=0) : base(id, ColumnDataType.Icon, title, description, category,-1, false, false, null, tooltips, values)
        {
            this.defaultValue = defaultValue;
        }

        public async override Task<SearchResultColumn> Render(string[] fields)
        {
            if (int.TryParse(fields[0], out int v))
            {
                var l = v;
                if (l >= LookupValues.Count) { l = LookupValues.Count; }
                return new SearchResultColumn() { Value = v.ToString(), Description = l.ToString() };
            }
            else
            {
                return new SearchResultColumn() { Value = defaultValue.ToString(), Description = defaultValue.ToString() };
            }
        }
    }
}
