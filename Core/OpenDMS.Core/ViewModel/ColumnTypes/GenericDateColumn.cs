using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.ViewModel.ColumnTypes
{
    public class GenericDateColumn : ViewColumn
    {
        private readonly bool includeTime;
        private readonly DateTime? defaultValue;

        public GenericDateColumn(
            string id, 
            string title, 
            string description, 
            string category, 
            bool IncludeTime = true,
            DateTime? defaultValue = null) 
            : base(id, ColumnDataType.Date, new() { id }, title, description, category, IncludeTime ? 165 : 95, false, true, ViewColumn.datefunctions)
        {
            includeTime = IncludeTime;
            this.defaultValue = defaultValue;
        }


        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            var c = fields[0];
            if (string.IsNullOrEmpty(c))
            {
                if (defaultValue is null || !defaultValue.HasValue)
                    c = "";
                else
                    c = defaultValue.Value.ToString("s"); ;
            }
//            if (!c.StartsWith("9999-12-31"))
//                    if (c.StartsWith(DateTime.UtcNow.Date.ToString("yyyy-MM-dd")))
//                        d = "Oggi " + c.Substring(11);
//                    else
            var d = (string.IsNullOrEmpty(c) || c.Length< 8) ? "" : (c.Substring(8, 2) + "/" + c.Substring(5, 2) + "/" + c.Substring(0, 4) + " " + (includeTime ?  c.Substring(11) : "")).Trim();
            return new SearchResultColumn() { Value = c, Description = d };
        }


  
    }
}
