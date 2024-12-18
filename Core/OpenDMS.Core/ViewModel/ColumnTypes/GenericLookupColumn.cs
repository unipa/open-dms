using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.ViewModel.ColumnTypes
{
    public class GenericLookupColumn : ViewColumn
    {
        private const int defaultSize = 150;

        public GenericLookupColumn(
            string id, 
            string title, 
            string description,
            string category
            
            ) : base(id, ColumnDataType.Text, new() { id }, title, description, category, defaultSize, false, true, new() { AggregateType.Count})
        {
        }

        public GenericLookupColumn(
            string id,
            string fieldid,
            string title,
            string description,
            string category,
            ColumnDataType columnDataType = ColumnDataType.Text

            ) : base(id, columnDataType, new() { fieldid }, title, description, category, defaultSize, false, true, columnDataType == ColumnDataType.Number ? ViewColumn.allfunctions : columnDataType == ColumnDataType.Date ? ViewColumn.datefunctions : ViewColumn.textfunctions)
        {
        }
        //public override async Task<SearchResultColumn> Render(string[] fields)
        //{
        //    FieldType FieldType = dataManager.AvailableFields[0];
        //    var a = fields[0];
        //    var data = await dataManager.Lookup(FieldType, a);
        //    return new SearchResultColumn() { Value = a, Description = data.LookupValue };
        //}


    }
}
