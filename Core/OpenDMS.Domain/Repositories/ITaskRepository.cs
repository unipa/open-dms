using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Models;
using System.Data;

namespace OpenDMS.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<int> Insert(TaskItem task);
        Task<int> Update(TaskItem task);
        Task<TaskItem> GetById(int taskId);
        Task<List<UserTask>> GetByParentId(int taskId);
        Task<List<TaskProgress>> GetProgress(int taskId);
        Task<List<UserTask>> GetUserTasks(int taskId);

        /* USER TASKS */
        Task<int> UpdateUserTask(UserTask userTask);
        Task<UserTask> GetUserTask(int userTaskId);
        Task<int> CountActiveUserTasks(int TaskItemId);
        
        Task<List<UserTask>> Find(TaskListFilter filter, UserProfile userInfo);
        Task<int> Count(TaskListFilter filter, UserProfile userInfo);

        Task<int> Delete (int userTaskId);
        Task<int> DeleteExecuted(int GracePeriodInDays);
        Task<int> DeleteByDocument(int documentId);


        Task<int> AddProgress(TaskProgress TP);
        Task<int> RemoveProgress(int TaskProgressId);

        Task<int> GetByJobInstanceId(string ProcessId);
        Task<int> CompleteByTaskId(int TaskItemId, string Message = "");
        Task<List<UserTask>> GetByEvent(int documentId, string EventId, UserProfile userInfo);
        Task<List<UserTask>> GetByDocument(int documentId, UserProfile userInfo);
        Task<List<UserTask>> GetAllDocumentTasks(int documentId, UserProfile userInfo);

        Task<int> RemoveOrphanTasks(DateTime LastUpdate);
        Task<int> Refresh(int TaskItemId);
        Task<List<ProcessSummary>> GetProcesses(ProcessFilter filters);
        Task<List<ProcessTaskUser>> GetProcessDetails(ProcessFilter filters);
        Task<List<UserTask>> GetByProcessId(string processId, UserProfile userInfo);

        Task Reassign(User user);
        Task Reassign(OrganizationNode node);

    }

}
