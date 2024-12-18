using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.BusinessLogic;


public class CustomFieldService : ICustomFieldService
{

    private readonly ILogger<ICustomFieldService> _logger;
    private readonly ICustomFieldTypeRepository _repository;
    private readonly IDataTypeFactory _dataTypeFactory;


    public CustomFieldService(ILogger<ICustomFieldService> logger,
        IDataTypeFactory dataTypeFactory,
        ICustomFieldTypeRepository repository)
    {
        this._logger = logger;
        this._dataTypeFactory = dataTypeFactory;
        this._repository = repository;

    }


    /// <summary>
    /// Metodo per ottenere un oggetto CustomFieldType(che identifica una metadato) tramite id. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    public async Task<FieldType> GetById(string id)
    {
        try
        {
            var result = await _repository.GetById(id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetById");
            throw; ;
        }
    }

    /// <summary>
    /// Metodo per ottenere la lista dei metadati 
    /// </summary>
    /// <returns></returns>

    public async Task<IEnumerable<FieldType>> GetAll()
    {
        try
        {
            return await _repository.GetAll();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAll");
            throw; ;
        }
    }

    /// <summary> 
    /// Metodo per salvare un metadato passando un oggetto CustomFieldType (metadato) tramite body 
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>

    public async Task<int> Insert(FieldType bd)
    {
        try
        {
            int res = await _repository.Insert(bd);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Insert", bd);
            throw;
        }
    }

    /// <summary>
    /// Metodo per aggiornare un metadato passando l'oggetto CustomFieldType (metadato) con il relativo id tramite body 
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>

    public async Task<int> Update(FieldType bd)
    {
        try
        {
            int res = await _repository.Update(bd);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update", bd);
            throw;
        }
    }


    /// <summary>
    /// Metodo per cancellare un CustomFieldType(metadato)
    /// </summary>
    /// <param name="Id">Id del CustomFieldType da cancellare</param>
    /// <returns></returns>

    public async Task<int> Delete(string Id)
    {
        try
        {
            var field = await _repository.GetById(Id);
            var result = await _repository.Delete(field);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete", Id);
            throw; ;
        }
    }


    public async Task<IEnumerable<FieldType>> GetAllTypes()
    {
        try
        {
            return (await _repository.GetAll()).Where(t => !t.Customized).ToList(); // _dataTypeFactory.GetAllTypes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllTypes");
            throw; ;
        }
    }

}