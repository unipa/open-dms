using OpenDMS.Domain.Entities.Schemas;

namespace OpenDMS.Domain.Services
{
    public interface IDataTypeFactory
    {
        Task<IDataTypeManager> Instance(string dataType);

        IEnumerable<FieldType> GetAllTypes();

        Task Register(string dataType, IDataTypeManager manager);

    }
}

