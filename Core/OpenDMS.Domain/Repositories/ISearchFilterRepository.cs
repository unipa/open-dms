using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;


public interface ISearchFilterRepository
{
    Task<Entities.Tasks.TaskListCustomFilter> GetById(int filterId);
    Task<int> Insert(Entities.Tasks.TaskListCustomFilter bd);
    Task<int> Update(Entities.Tasks.TaskListCustomFilter bd);
    Task<int> Delete(int filterId);
    Task<IList<Entities.Tasks.TaskListCustomFilter>> GetAll(UserProfile uerInfo);

}


