using OpenDMS.Core.DTOs;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.ViewModel.ColumnTypes
{

    public enum NumberFormat
    {
        Unformatted = 0,
        Money = 1,
        Percentage = 2
    }

    public class GenericNumberColumn : ViewColumn
    {
        protected bool percentage = false;

        private readonly NumberFormat format;

        public GenericNumberColumn(
            string id,
            string title,
            string description,
            string category,
            int size = 90,
            NumberFormat format = NumberFormat.Unformatted,
            bool resizable = false,
            bool sortable = false)
            : base (id, ColumnDataType.Number, title, description, category, size, resizable, sortable, allfunctions, null)
        {
            this.format = format;
        }

        public async override Task<SearchResultColumn> Render(string[] fields)
        {
            var v = fields[0];
            if (string.IsNullOrEmpty(v)) v = "0";
            var l = v;
            if (format == NumberFormat.Percentage)
            {
                var d = decimal.Parse(v.Replace(".", "").Replace(",", "."));
                if (d > 1) d = d / 100;
                l = d.ToString("0.00") + "%";
                if (l == "0,00%") l = "";
            } else
            if (format == NumberFormat.Money)
            {
                var valuta = "";
                if (fields.Length > 1) { valuta = " " + fields[1]; }
                decimal d = 0;
                if (decimal.TryParse(v.Replace(".", "").Replace(",", "."), out d))
                {
                    l = d.ToString("#,###") + valuta;
                }
            }
            else
            {
                var prefix = "";
                if (fields.Length > 1) { prefix = " " + fields[1]; }
                decimal d = 0;
                if (decimal.TryParse(v.Replace(".", "").Replace(",", "."), out d))
                {
                    l = d.ToString("#,###").Replace(".", "").Replace(",",".") + prefix;
                }

            }
            return new SearchResultColumn() { Value = v, Description = l };
        }

    }
}
