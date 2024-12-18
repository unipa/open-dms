
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.ViewModel.ColumnTypes
{
    public class GenericImageColumn : ViewColumn
    {
        private readonly Func<string[], SearchResultColumn> imageResolver;

        public GenericImageColumn(
            string id,
            string title,
            string description,
            string category,
            List<string> fields = null,
            Func<string[], SearchResultColumn> imageResolver = null,
            int size = 0) : base(id, ColumnDataType.Image, fields, title, description, category, size, false, false, textfunctions)
        {
            this.imageResolver = imageResolver;
        }



        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            if (imageResolver != null) return imageResolver(fields);
            else
                return await base.Render(fields);  
        }

    }
}
