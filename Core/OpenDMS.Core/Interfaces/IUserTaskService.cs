using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IUserTaskService
    {
        Task<TaskItemInfo> AddMessage(int UserTaskId, UserProfile userInfo, string Message);
        Task<TaskProgressInfo> AddProgress(int TaskItemId, string userId, string Message, decimal Percentage = -1, string variables = null);
        Task<UserTaskInfo> Claim(int UserTaskId, UserProfile userId);
        Task<int> Count(List<SearchFilter> filters, UserProfile userInfo);
        Task<int> Count(TaskListFilter filters, UserProfile userInfo);
        Task<TaskItemInfo> CreateMessage(CreateNewTaskMessage newTask, UserProfile userInfo); //, string processJobId = "", string processId = "");
        Task<TaskItemInfo> CreateTask(CreateNewTask newTask, UserProfile userInfo);//, string processJobId = "", string processId="");
                                                                                   //Task<UserTaskInfo> Execute(int UserTaskId, UserProfile userInfo, string Justification = "");
        Task<int> GetUsersToNotify(CreateNewTaskMessage newTask, UserProfile userInfo);
        Task<UserTaskInfo> Execute(int UserTaskId, UserProfile userInfo, string variables);
        Task<List<SearchFilters>> Filters(UserProfile userInfo);
        Task<List<UserTaskListItem>> Find(TaskListFilter filters, UserProfile userInfo);
        Task<UserTaskInfo> GetById(int UserTaskId, UserProfile userInfo);
        Task<TaskItemInfo> GetByTaskId(int TaskItemId, UserProfile userInfo);
        Task<UserTaskInfo> Reassign(int UserTaskId, UserProfile userInfo, string ProfileId, ProfileType ProfileType, string Justification);
        Task<UserTaskInfo> Reject(int UserTaskId, UserProfile userInfo, string Justification);
        Task<UserTaskInfo> Release(int UserTaskId, UserProfile userInfo,  string Justification);
        Task<int> RemoveFilter(int filterId, UserProfile userInfo);
        Task<int> RemoveProgress(int TaskProgressId);
        Task<SearchFilters> RenameFilter(int filterId, string NewName, UserProfile userInfo);
        Task<SearchFilters> SaveFilter(SearchFilters filters);
        Task<UserTaskInfo> Validate(int UserTaskId, UserProfile userInfo, string Justification = "");

        Task<int> GetByJobKey(string ProcessId);
        Task<int> CompleteByTaskId(int TaskItemId, string Message = "");
        Task<int> Refresh(int TaskItemId);
        Task RemoveOrphanTasks(DateTime LastUpdate);

        Task<List<UserTask>> GetByEvent(int documentId, string EventId, UserProfile userInfo);
        Task<List<UserTask>> GetByDocument(int documentId, UserProfile userInfo);
        Task<List<UserTask>> GetByProcessId(string processId, UserProfile userInfo);
        Task<List<UserTask>> GetAllDocumentTasks(int documentId, UserProfile userInfo);

        Task<int> CaptureEvent(UserTask UT, IEvent Event);
        Task<int> UpdateTaskVariables(UserTask UT, string EventVariables);


        Task<List<SearchFiltersGroup>> TaskListFilters(UserProfile u);
        Task<TaskItemInfo> CreateWarning(CreateNewTaskMessage newTask, UserProfile userInfo);
        Task<TaskItemInfo> CreateError(CreateNewTaskMessage newTask, UserProfile userInfo);
        Task<List<int>> GetDocuments(List<int> TaskIdList);
        Task<int> Delete(int userTaskId, UserProfile userInfo);
        Task<int> DeleteByDocument(int documentId);

        Task Rebase();

    }
}