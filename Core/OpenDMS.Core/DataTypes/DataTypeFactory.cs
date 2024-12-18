using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Services;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Core.DataTypes
{
    public class DataTypeFactory : IDataTypeFactory
    {
        private readonly IApplicationDbContextFactory contextFactory;
        private readonly IServiceProvider serviceProvider;
        protected Dictionary<string, IDataTypeManager> _managers = new Dictionary<string, IDataTypeManager>();
        protected Func<string, IDataTypeManager> _getManager;

        public DataTypeFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            foreach (var M in serviceProvider.GetServices<IDataTypeManager>())
                _managers.Add(M.DataTypeValue, M);
        }

        public IEnumerable<FieldType> GetAllTypes()
        {
            foreach (var type in _managers.Values)
            {
                foreach (var d in type.AvailableFields)
                {
                    yield return d;
                }
            }

        }

        public async Task<IDataTypeManager> Instance(string dataType)
        {
            if (dataType == "$$t" || dataType is null) dataType = "";
            var found = _managers.ContainsKey(dataType);
            if (!found)
                    dataType = "";
            return _managers[dataType];
        }

        public async Task Register(string dataType, IDataTypeManager manager)
        {
            var found = _managers.ContainsKey(dataType);
            if (!found) {
                _managers.Add(dataType, manager);
            }



        }
    }
}
