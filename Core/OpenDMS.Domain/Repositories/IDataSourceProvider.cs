using OpenDMS.Domain.Entities.Schemas;

namespace OpenDMS.Domain.Repositories;

public interface IDataSourceProvider
{
    public ExternalDatasource this[string Name] { get; set; }
    public ExternalDatasource this[string Company, string Name] { get; set; }

    Task<ExternalDatasource> Get(string Id);

    Task<int> Set(string Id, ExternalDatasource Value);

    Task<Dictionary<string, ExternalDatasource>> GetAll();

    Task<int> Delete(string Id);

    Task TestConnection(ExternalDatasource Source);
    Task<List<string[]>> Query(string Id, string Query, int pageSize = 50, int pageIndex = 0);


}
