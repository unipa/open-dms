using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;
using System.Linq.Dynamic.Core;

namespace OpenDMS.Infrastructure.Database.Repositories
{
    public class TaskRepository : ITaskRepository
    {

        private readonly ApplicationDbContext DS;
        private readonly IApplicationDbContextFactory contextFactory;

        public TaskRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
        }


   

        public async Task<int> Count(TaskListFilter filter, UserProfile userInfo)
        {
            var query = GetFilteredTasks(filter, userInfo);
            return query.Count();
        }
        public async Task<List<UserTask>> Find(TaskListFilter filter , UserProfile userInfo)
        {
            var query = GetFilteredTasks(filter, userInfo);
            return query.ToList();
        }

        public async Task<List<UserTask>> GetByEvent(int documentId, string eventId, UserProfile userInfo)
        {
            var query = GetFilteredTasks(new TaskListFilter() { EventId=eventId, Status= new List<ExecutionStatus>() { ExecutionStatus.Unassigned, ExecutionStatus.Assigned, ExecutionStatus.Running, ExecutionStatus.Suspended }, DocumentId=documentId, Received=true }, userInfo);
            return query.ToList();
        }
        public async Task<List<UserTask>> GetByDocument(int documentId, UserProfile userInfo)
        {
            var query = GetFilteredTasks(new TaskListFilter() { Status = new List<ExecutionStatus>() { ExecutionStatus.Unassigned, ExecutionStatus.Assigned, ExecutionStatus.Running, ExecutionStatus.Suspended }, DocumentId = documentId, Received = true }, userInfo);
            return query.OrderByDescending(T => T.CreationDate).ToList();
        }
        public async Task<List<UserTask>> GetByProcessId(string processId, UserProfile userInfo)
        {
            var query = GetFilteredTasks(new TaskListFilter() {  ProcessInstanceId = processId, Received = true }, userInfo);
            return query.OrderByDescending(T => T.CreationDate).ToList();
        }

        public async Task<List<UserTask>> GetAllDocumentTasks (int documentId, UserProfile userInfo)
        {
            var query = GetFilteredTasks(new TaskListFilter() { DocumentId = documentId, TaskType = new[] { TaskType.Event, TaskType.Activity, TaskType.Warning, TaskType.Error} }, userInfo);
            return query.OrderByDescending(T => T.CreationDate).ToList();
        }



        public async Task<int> DeleteExecuted(int GracePeriodInDays)
        {
            await DS.TaskItems.Where(t => (t.Status == Domain.Enumerators.ExecutionStatus.Executed
            || t.Status == Domain.Enumerators.ExecutionStatus.Aborted
            || t.Status == Domain.Enumerators.ExecutionStatus.Deleted)
            && (t.LastUpdate.AddDays(-1 * GracePeriodInDays) < DateTime.UtcNow)
            ).ExecuteDeleteAsync();
            return await DS.SaveChangesAsync();

        }
        public async Task<int> DeleteByDocument(int documentId)
        {
            //var lista = (DS.TaskItems.Where(t => (t.Status != Domain.Enumerators.ExecutionStatus.Executed
            //&& t.Status != Domain.Enumerators.ExecutionStatus.Validated
            //&& t.Status != Domain.Enumerators.ExecutionStatus.Aborted
            //&& t.Status != Domain.Enumerators.ExecutionStatus.Deleted)
            //&& (t.Attachments.Any(a => a.DocumentId == documentId))
            //)).ToList();

            await DS.TaskItems.Where(t => (t.Status != Domain.Enumerators.ExecutionStatus.Executed
            && t.Status != Domain.Enumerators.ExecutionStatus.Validated
            && t.Status != Domain.Enumerators.ExecutionStatus.Aborted
            && t.Status != Domain.Enumerators.ExecutionStatus.Deleted)
            && (t.Attachments.Any(a=>a.DocumentId == documentId))
            ).ExecuteUpdateAsync (p=>p.SetProperty(f=>f.Status, v=> ExecutionStatus.Aborted));

            await DS.UserTasks.Where(t => (t.Status != Domain.Enumerators.ExecutionStatus.Executed
            && t.Status != Domain.Enumerators.ExecutionStatus.Validated
            && t.Status != Domain.Enumerators.ExecutionStatus.Aborted
            && t.Status != Domain.Enumerators.ExecutionStatus.Deleted)
            && (t.TaskItem.Attachments.Any(a => a.DocumentId == documentId))
            ).ExecuteUpdateAsync(p => p.SetProperty(f => f.Status, v => ExecutionStatus.Aborted));

            return await DS.SaveChangesAsync();

        }

        public async Task<int> Delete(int userTaskId)
        {
            var userTask = await DS.UserTasks.FirstOrDefaultAsync(t => t.Id == userTaskId);
            if (userTask == null) return 0;

            var r = 0;
            TaskItem taskItem = null;
            try
            {
                DS.UserTasks.Remove(userTask);

                var usertasks = await DS.TaskItems.AsNoTracking().Where(t => t.Id == userTask.TaskItemId).CountAsync();
                if (usertasks == 1)
                {
                    taskItem = await DS.TaskItems.FirstOrDefaultAsync(t => t.Id == userTask.TaskItemId);
                    DS.TaskItems.Remove (taskItem);
                }
                r = await DS.SaveChangesAsync();
            }
            finally
            {
                if (taskItem != null)
                    DS.Entry<TaskItem>(taskItem).State = EntityState.Detached;
                DS.Entry<UserTask>(userTask).State = EntityState.Detached;
            }
            return r;
        }


        public async Task<TaskItem> GetById(int taskId)
        {
            return await DS.TaskItems.Include(t=>t.Recipients).Include(t => t.Attachments).AsNoTracking().FirstOrDefaultAsync(t=>t.Id == taskId);
        }

        public async Task<UserTask> GetUserTask(int userTaskId)
        {
            return await DS.UserTasks.AsNoTracking()/*.Include(t=>t.TaskItem)*/.FirstOrDefaultAsync(t => t.Id == userTaskId);
        }

        public async Task<int> Insert(TaskItem task)
        {
            int r = 0;
            try
            {
                if (task.Title.Length > 255) task.Title = task.Title.Substring(0,250)+"...";
                DS.TaskItems.Add(task);
                r = await DS.SaveChangesAsync();
            }
            finally
            {
                DS.Entry<TaskItem>(task).State = EntityState.Detached;
            }
            return r;
        }

        public async Task<int> Update(TaskItem task)
        {
            int r = 0;
            try
            {
                DS.Entry<TaskItem>(task).State = EntityState.Modified;
                r = await DS.SaveChangesAsync();
            }
            finally
            {
                DS.Entry<TaskItem>(task).State = EntityState.Detached;
            }
            return r;
        }
        public async Task<int> CountActiveUserTasks(int TaskItemId)
        {
            return (await DS.UserTasks.AsNoTracking().CountAsync(t => (t.Status == ExecutionStatus.Unassigned || t.Status == ExecutionStatus.Assigned || t.Status == ExecutionStatus.Running || t.Status == ExecutionStatus.Suspended) && !t.CC && t.TaskItemId == TaskItemId)); //Select(t => t.Id)).ToListAsync()).Count();
        }


    public async Task<int> UpdateUserTask(UserTask userTask)
        {
            int r = 0;
            try
            {
                DS.Entry<UserTask>(userTask).State = EntityState.Modified;
                r = await DS.SaveChangesAsync();
            }
            finally
            {
                DS.Entry<UserTask>(userTask).State = EntityState.Detached;
            }
            return r;
        }

        public async Task<List<TaskProgress>> GetProgress(int taskId)
        {
            return await DS.TaskProgress.AsNoTracking().Where(t => t.TaskItemId == taskId).OrderByDescending(o => o.CreationDate).ToListAsync();
        }

        public async Task<List<UserTask>> GetByParentId(int userTaskId)
        {
            return await DS.UserTasks.AsNoTracking().Where(t => t.TaskItem.ParentId == userTaskId).OrderByDescending(o => o.CreationDate).ToListAsync();
        }

       


        public async Task<int> AddProgress(TaskProgress TP)
        {
            int r = 0;
            try
            {

                if (TP.Percentage == -1)
                    TP.Percentage = DS.TaskProgress.Where(p => p.UserTaskId == TP.UserTaskId).Select(p => p.Percentage).AsEnumerable().DefaultIfEmpty(0).Max(); //.AsEnumerable().DefaultIfEmpty(0).Max(();
                await DS.TaskProgress.AddAsync(TP);
                r = await DS.SaveChangesAsync();
                await SynchTaskItemProgress(TP);
            }
            finally
            {
                DS.Entry<TaskProgress>(TP).State = EntityState.Detached;
            }
            return r;
        }

        private async Task<int> SynchTaskItemProgress(TaskProgress TP)
        {
            var UT = await DS.UserTasks.FirstOrDefaultAsync(t => t.Id == TP.UserTaskId && t.UserId == TP.UserId);
            if (UT != null)
            {
                UT.Percentage = TP.Percentage; // Math.Round(DS.TaskProgress.Where(p => p.TaskItemId == TP.TaskItemId && p.UserId == TP.UserId).Select(p => p.Percentage).DefaultIfEmpty().Max());
                UT.LastExecutionDate = UT.Percentage == 100 ? DateTime.UtcNow : null;
                try
                {
                    await DS.SaveChangesAsync();
                }
                catch {
                }
                finally
                {
                    DS.Entry<UserTask>(UT).State = EntityState.Detached;
                }
            }
            var TI = await DS.TaskItems.FirstOrDefaultAsync(t => t.Id == TP.TaskItemId);
            TI.ExecutionPercentage = Math.Round(DS.UserTasks.Where(p => p.TaskItemId == TP.TaskItemId).Select(p=>p.Percentage).AsEnumerable().DefaultIfEmpty().Average());
            if (TI.ExecutionDate == null) TI.ExecutionDate = DateTime.UtcNow;
            if (TI.ExecutionPercentage > 0 && (TI.Status == ExecutionStatus.Unassigned || TI.Status == ExecutionStatus.Assigned)) TI.Status = ExecutionStatus.Running;
            //TI.Status = Math.Round(TI.ExecutionPercentage) == 100 ? ExecutionStatus.Executed : TI.ExecutionPercentage > 0 ? ExecutionStatus.Running : ExecutionStatus.Assigned;
            int r = 0;
            try
            {
                r = await DS.SaveChangesAsync();
            }
            catch { }
            finally
            {
                DS.Entry<TaskItem>(TI).State = EntityState.Detached;
            }
            return r;
        }


        public async Task<int> RemoveProgress(int TaskProgressId)
        {
            var TP = DS.TaskProgress.Find(TaskProgressId);
            DS.TaskProgress.Remove(TP);
            try
            {
                int r = await DS.SaveChangesAsync();
                await SynchTaskItemProgress(TP);
                return r;
            } finally
            {
                DS.Entry<TaskProgress>(TP).State = EntityState.Detached;
            }
        }
    

        public async Task<List<UserTask>> GetUserTasks(int taskId)
        {
            return await DS.UserTasks.AsNoTracking().Where (t => t.TaskItemId == taskId).ToListAsync();
        }




        public async Task<int> GetByJobInstanceId(string JobInstanceId)
        {
            var p = (await DS.TaskItems.AsNoTracking().FirstOrDefaultAsync(t => t.JobInstanceId == JobInstanceId));
            return (p != null) ? p.Id : 0;
        }


        public async Task<int> CompleteByTaskId(int TaskItemId, string Message = "")
        {
            var TI = await DS.TaskItems.FirstOrDefaultAsync(t => t.Id == TaskItemId);
            try
            { 
                if (TI.Status != ExecutionStatus.Executed) return 0;

                TI.LastUpdate = DateTime.UtcNow;
                TI.Status = ExecutionStatus.Executed;

                var items = await DS.UserTasks
                    .Where(t => t.TaskItemId == TaskItemId
                                && (t.Status == ExecutionStatus.Unassigned || t.Status == ExecutionStatus.Assigned || t.Status == ExecutionStatus.Running)
                          ).ToListAsync();
                foreach (var item in items)
                {
                    item.LastExecutionDate = DateTime.UtcNow;
                    item.Status = ExecutionStatus.Aborted;
                }
                if (!String.IsNullOrEmpty(Message))
                {
                    TaskProgress TP = new TaskProgress();
                    TP.Message = Message;
                    TP.UserId = SpecialUser.SystemUser;
                    TP.Percentage = 100;
                    TP.CreationDate = DateTime.UtcNow;
                    TP.TaskItemId = TI.Id;
                    DS.TaskProgress.Add(TP);
                    await SynchTaskItemProgress(TP);
                }
                return await DS.SaveChangesAsync();
            } finally
            {
                DS.Entry<TaskItem>(TI).State = EntityState.Detached;
            }
        }



        public async Task<int> RemoveOrphanTasks(DateTime LastUpdate)
        {
            if (LastUpdate < DateTime.UtcNow && LastUpdate > DateTime.MinValue)
            {
                var tasks = DS.TaskItems.Include(t => t.UserTasks).Where(t => t.LastUpdate < LastUpdate && !String.IsNullOrEmpty(t.ProcessInstanceId) && t.Status != ExecutionStatus.Executed && t.Status != ExecutionStatus.Validated && t.TaskType != TaskType.Message && t.TaskType != TaskType.Warning);
                try
                {
                    foreach (var TI in tasks)
                    {
                        TI.Status = ExecutionStatus.Deleted;
                        foreach (var UT in TI.UserTasks)
                            UT.Status = ExecutionStatus.Deleted;
                    };
                    return await DS.SaveChangesAsync();
                } finally
                {
                    foreach (var TI in tasks)
                    {
                        DS.Entry<TaskItem>(TI).State = EntityState.Detached;
                        foreach (var UT in TI.UserTasks)
                            DS.Entry<UserTask>(UT).State = EntityState.Detached;
                    };

                }
            }
            return 0;
        }


        public async Task<int> Refresh(int TaskItemId)
        {
            var TI = await DS.TaskItems.FirstOrDefaultAsync(t => t.Id == TaskItemId);
            if (TI != null)
            {
                try
                {
                    TI.LastUpdate = DateTime.UtcNow;
                    return await DS.SaveChangesAsync();
                }
                finally
                {
                    DS.Entry<TaskItem>(TI).State = EntityState.Detached;
                }
            }
            return 0;
        }




        private IEnumerable<UserTask> GetFilteredTasks (TaskListFilter filter, UserProfile userInfo)
        {
            var query = DS.UserTasks
                .Include(t=>t.TaskItem)
                .ThenInclude(t=>t.Recipients)
                .AsNoTracking()
                .AsQueryable();
        

            if (filter.TaskType != null)
                query = query.Where(t => filter.TaskType.Contains(t.TaskItem.TaskType));
            if (!String.IsNullOrEmpty(filter.FreeText))
                query = query.Where(t => t.TaskItem.Description.Contains(filter.FreeText) || t.TaskItem.Title.Contains(filter.FreeText));
            if (filter.ProcessDataId > 0)
            {
                query = query.Where(t => t.TaskItem.ProcessDataId == filter.ProcessDataId);
            }
            if (filter.Status.Count > 0)
            {
                query = query.Where(t => filter.Status.Contains(t.Status));
            }
            if (!String.IsNullOrEmpty(filter.Sender))
                query = query.Where(t => t.TaskItem.FromUserId == (filter.Sender.Substring(1)));
            if (!String.IsNullOrEmpty(filter.CategoryId))
                query = query.Where(t => t.TaskItem.CategoryId == (filter.CategoryId));
            if (!String.IsNullOrEmpty(filter.FormKey))
                query = query.Where(t => t.TaskItem.FormKey == (filter.FormKey));
            if (filter.DocumentId > 0)
                query = query.Where(t => t.TaskItem.Attachments.Any(a=>a.DocumentId == filter.DocumentId && !a.IsLinked));
            if (!String.IsNullOrEmpty(filter.EventId))
                query = query.Where(t => t.TaskItem.EventId == (filter.EventId));

            if (!String.IsNullOrEmpty(filter.ProcessInstanceId))
                query = query.Where(t => t.TaskItem.ProcessInstanceId == (filter.ProcessInstanceId));
            if (!String.IsNullOrEmpty(filter.ProcessDefinitionKey))
                query = query.Where(t => t.TaskItem.ProcessDefinitionKey == (filter.ProcessDefinitionKey));
            if (filter.ProcessDefinitionId > 0)
                query = query.Where(t => t.TaskItem.ProcessDefinitionId == filter.ProcessDefinitionId);

            var roles = userInfo.Roles.Where(r=>r.Id.IndexOf("\\") >= 0).Select(r => new { roleId = r.Id.Split("\\")[0], groupId = r.Id.Split("\\")[1]  }).ToList();

            if (filter.ParentId > 0)
                query = query.Where(t => t.TaskItem.ParentId == filter.ParentId);
            else
                if (filter.TaskItemId > 0)
                    query = query.Where(t => t.TaskItem.Id == filter.TaskItemId);
 
            if (!String.IsNullOrEmpty(filter.Recipient) && filter.Recipient.StartsWith(((int)ProfileType.User).ToString()))
                query = query.Where(t => t.UserId == (filter.Recipient.Substring(1)));
            if (!String.IsNullOrEmpty(filter.Recipient) && filter.Recipient.StartsWith(((int)ProfileType.Group).ToString()))
                query = query.Where(t => t.GroupId == (filter.Recipient.Substring(1)));
            if (!String.IsNullOrEmpty(filter.Recipient) && filter.Recipient.StartsWith(((int)ProfileType.Role).ToString()))
                query = query.Where(t => t.RoleId == (filter.Recipient.Substring(1)));
            if (filter.ExpirationDate.HasValue)
                query = query.Where(t => t.ExpirationDate < filter.ExpirationDate);
            if (filter.FromCreationDate.HasValue)
                query = query.Where(t => t.CreationDate >= filter.FromCreationDate);
            if (filter.ProjectId > 0)
                query = query.Where(t => t.TaskItem.ProjectId == filter.ProjectId || t.TaskItem.ProcessDataId == filter.ProjectId || t.TaskItem.Attachments.Any(a => a.DocumentId == filter.ProjectId && !a.IsLinked));
            List<UserTask> queryList;

            if (!(userInfo.userId.Equals(SpecialUser.SystemUser)))
            {
                //queryList = query.Where(t =>
                //    ((t.TaskItem.FromUserId == userInfo.userId) && !filter.Received) ||
                //    (t.UserId == (userInfo.userId)) ||
                //    (userInfo.GlobalRoles.Select(s=>s.Id).Contains(t.RoleId+"\\"+t.GroupId)) ||
                //    (userInfo.Groups.Select(s => s.Id).Contains(t.GroupId))
                //).ToList();

                query = query.Where(t =>
                    ((t.TaskItem.FromUserId == userInfo.userId) && !filter.Received) ||
                    (t.UserId == (userInfo.userId)) ||
                    ((userInfo.Roles.Select(s => s.Id).Contains(t.RoleId + "\\" + t.GroupId)) && !string.IsNullOrEmpty(t.RoleId) && !string.IsNullOrEmpty(t.GroupId)) ||
                    (userInfo.GlobalRoles.Select(s => s.Id).Contains(t.RoleId) && !string.IsNullOrEmpty(t.RoleId) && string.IsNullOrEmpty(t.GroupId)) ||
                    (userInfo.Groups.Select(s => s.Id).Contains(t.GroupId) && !string.IsNullOrEmpty(t.GroupId) && string.IsNullOrEmpty(t.RoleId))
                );
            }
//            else
//                queryList = query.ToList();
//            IEnumerable<UserTask> queryEnumerable = queryList.AsEnumerable();
            if (filter.PageIndex > 0)
                query = query.Skip(filter.PageIndex * filter.PageSize);
            if (filter.PageSize > 0)
                query = query.Take(filter.PageSize);
            //  Console.WriteLine(query.ToQueryString());
            return query.AsEnumerable();
        }


        public async Task<List<ProcessSummary>> GetProcesses(ProcessFilter filters)
        {
            List<ProcessSummary> Procs = new List<ProcessSummary>();

            string Where = "";
            if (filters.FromDate > DateTime.MinValue) { Where += "AND(FromDate>='" + filters.FromDate.ToString("s") + "')"; }
            if (filters.ToDate > DateTime.MinValue) { Where += "AND(ToDate<='" + filters.ToDate.ToString("s") + "')"; }
            if (!string.IsNullOrEmpty(filters.BusinessProcessId)) { Where += "AND(BusinessProcessId=" + filters.BusinessProcessId.Quoted() + ")"; }
            if (!String.IsNullOrEmpty(Where)) Where = "WHERE " + Where.Substring(3);

            IDbConnection connection = DS.Database.GetDbConnection();
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            connection.Open();
            command.CommandText = "SELECT * FROM v" + (filters.Expired ? "Expired" : "") + (filters.Closed ? "Closed" : "Active") + "Processes " + Where + " ORDER BY AvgHours DESC";
            using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader != null && reader.Read())
                {
                    string bid = reader["BusinessProcessId"].ToString();
                    var P = new ProcessSummary();
                    //if (reader.GetOrdinal("BusinessProcessName") >= 0)
                    //    P.BusinessProcessName = reader["BusinessProcessName"].ToString();
                    //else
                    //    P.BusinessProcessName = "* Processo Esterno *";

                    P.BusinessProcessId = bid;
                    P.MinHours = (int)Math.Round(float.Parse(reader["MinHours"].ToString()));
                    P.MaxHours = (int)Math.Round(float.Parse(reader["MaxHours"].ToString()));
                    P.AvgHours = (int)Math.Round(float.Parse(reader["AvgHours"].ToString()));
                    P.Count = int.Parse(reader["Instances"].ToString());
                    P.Users = int.Parse(reader["Users"].ToString());

                    P.FromDate = DateTime.Parse(reader["FromDate"].ToString());
                    P.ToDate = DateTime.Parse(reader["ToDate"].ToString());
                    Procs.Add(P);
                }
            }
            return Procs;
        }


        public async Task<List<ProcessTaskUser>> GetProcessDetails(ProcessFilter filters)
        {
            List<ProcessTaskUser> Procs = new List<ProcessTaskUser>();

            string Where = "";
            if (filters.FromDate > DateTime.MinValue) { Where += "AND(FromDate>='" + filters.FromDate.ToString("s") + "')"; }
            if (filters.ToDate > DateTime.MinValue) { Where += "AND(ToDate<='" + filters.ToDate.ToString("s") + "')"; }
            if (!string.IsNullOrEmpty(filters.BusinessProcessId)) { Where += "AND(BusinessProcessId=" + filters.BusinessProcessId.Quoted() + ")"; }
            if (!String.IsNullOrEmpty(Where)) Where = "WHERE " + Where.Substring(3);

            IDbConnection connection = DS.Database.GetDbConnection();
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            connection.Open();
            command.CommandText = "SELECT * FROM v" + (filters.Expired ? "Expired" : "") + (filters.Closed ? "Closed" : "Active") + "TaskUsers " + Where + " ORDER BY AvgHours"+(filters.Expired ? " DESC" : "");
            using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader != null && reader.Read())
                {
                    string bid = reader["BusinessProcessId"].ToString();
                    var P = new ProcessTaskUser();

                    P.BusinessProcessId = bid;
                    P.MinHours = (int)Math.Round(float.Parse(reader["MinHours"].ToString()));
                    P.MaxHours = (int)Math.Round(float.Parse(reader["MaxHours"].ToString()));
                    P.AvgHours = (int)Math.Round(float.Parse(reader["AvgHours"].ToString()));
                    P.Tasks = int.Parse(reader["Instances"].ToString());
                    P.UserId= (reader["UserId"].ToString());
                    P.UserName = (reader["UserName"].ToString());

                    P.FromDate = DateTime.Parse(reader["FromDate"].ToString());
                    P.ToDate = DateTime.Parse(reader["ToDate"].ToString());
                    Procs.Add(P);
                }
            }
            return Procs;
        }

        // Rilascio le attività non personali in carico all'utente
        // Riassegno al mittente le attività personali dell'utente
        public async Task Reassign(User user)
        {
            IDbConnection connection = DS.Database.GetDbConnection();
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            connection.Open();
            command.CommandText = $"UPDATE UserTasks SET UserId='',Status=0 WHERE UserId={user.Id.Quoted()} AND (RoleId<>'' OR GroupId<>'') AND Status < 250";
            command.ExecuteNonQuery();
            command.CommandText = $"UPDATE UserTasks SET Status=0, UserId=(SELECT COALESCE (FromUserId,'') FROM TaskItems ti WHERE ti.Id = TaskItemId) WHERE UserId={user.Id.Quoted()} AND RoleId='' AND GroupId='' AND Status < 250";
            command.ExecuteNonQuery();
            connection.Close();
        }

        // Riassegno le attività in base allo StrategyReallocation
        public async Task Reassign(OrganizationNode node)
        {
            int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            var strategy = node.TaskReallocationStrategy;
            var groupId = node.UserGroupId;
            var group = await DS.OrganizationNodes.FirstOrDefaultAsync(g => g.UserGroupId == groupId && g.StartISODate >= node.EndISODate && (g.EndISODate == 0 || g.EndISODate > data));
            // Non faccio nulla se esiste un altro nodo attivo intestato allo stesso ufficio.
            if (group != null)
                return;

            IDbConnection connection = DS.Database.GetDbConnection();
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            connection.Open();

            switch (strategy)
            {
                case 1: // Sposto i task attivi al Nodo Padre
                    command.CommandText = $"UPDATE UserTasks SET GroupId={node.ParentUserGroupId.Quoted()} WHERE GroupId={node.UserGroupId.Quoted()} AND Status < 250";
                    command.ExecuteNonQuery();
                    break;
                case 2: // Sposto i task attivi su un Nodo Specifico
                    command.CommandText = $"UPDATE UserTasks SET GroupId={node.TaskReallocationProfile.Quoted()} WHERE GroupId={node.UserGroupId.Quoted()} AND Status < 250";
                    command.ExecuteNonQuery();
                    break;
                case 3: // Sposto i task attivi ad un Profilo 
                    command.CommandText = $"UPDATE UserTasks SET GroupId='', RoleId={node.TaskReallocationProfile.Quoted()} WHERE GroupId={node.UserGroupId.Quoted()} AND Status < 250";
                    command.ExecuteNonQuery();
                    break;
                default: // Chiudo le attività
                    command.CommandText = $"UPDATE UserTasks SET Status = 250 WHERE GroupId={node.UserGroupId.Quoted()} AND Status < 250";
                    command.ExecuteNonQuery();
                    command.CommandText = $"UPDATE TaskItems SET Status = 250 WHERE Id IN (SELECT TaskItemId FROM UserTasks GROUP BY TaskItemId HAVING MIN(Status) = 250";
                    command.ExecuteNonQuery();
                    break;
            }
            connection.Close();
        }

    }
}
