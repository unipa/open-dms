using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.ViewModel.ColumnTypes
{
    public class GenericAvatarColumn : ViewColumn
    {
        private readonly IDataTypeManager dataManager;

        public GenericAvatarColumn(string id, 
            string title, 
            string description, 
            string category,
            IDataTypeManager dataManager) 
            : base(id, ColumnDataType.Avatar, title, description, category, 150, true, true)
        {
            this.dataManager = dataManager;
        }

        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            FieldType fieldType = dataManager.AvailableFields[0];
            var a = fields[0];
            var data = await dataManager.Lookup(fieldType, a);
            return new SearchResultColumn() { Value = a, Description = data.LookupValue };
        }

    }
}
