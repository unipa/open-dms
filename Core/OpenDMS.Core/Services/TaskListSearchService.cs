using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Core.ViewModel.CustomColumnTypes.Tasks;
using OpenDMS.Domain;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.BusinessLogic
{
    public class TaskListSearchService : ISearchService
    {
        private readonly ITaskRepository repository;
        private readonly IQueryBuilder queryBuilder;
        private readonly IDataTypeFactory dataTypeFactory;

        private List<ViewColumn> _allColumns = new();
        private Dictionary<string, ViewColumn> selectColumns = new();


        public TaskListSearchService(ITaskRepository repository, IQueryBuilder SqlBuilder, IDataTypeFactory dataTypeFactory)
        {
            this.repository = repository;
            queryBuilder = SqlBuilder;
            this.dataTypeFactory = dataTypeFactory;
        }

        public async Task<bool> ChangeRowState(RowId RowKeys, RowState NewState, UserProfile acl)
        {
            var id = int.Parse( RowKeys.Index[0]);
            var UT = await repository.GetUserTask(id);
            UT.Read = NewState != RowState.New ? true : false;
            return await repository.UpdateUserTask(UT) > 0;
        }
        public async Task<SearchResult> Get(ViewProperties view, SearchRequest request, UserProfile acl)
        {
            Console.WriteLine(DateTime.UtcNow.ToString("s") + " TASK_GET-START");
            queryBuilder.Clear();
            await GetColumns(view.ViewId, acl);
            var fields = view.Columns.Where(c => c.Settings.Visible);

            foreach (var o in request.OrderBy)
            {
                var f = fields.FirstOrDefault(f => f.Id == o.ColumnId);
                if (f != null) f.Settings.SortType = (o.Descending ? SortingType.Descending : SortingType.Ascending);
            }

            await PrepareSql(fields, request.Filters, acl);

            SearchResult sr = new SearchResult();
            sr.View = view;
            sr.Page = await InternalGetPage(fields, request);
            sr.Count = queryBuilder.Count();
            sr.Pages = ((sr.Count + request.PageSize - 1) / request.PageSize);
            Console.WriteLine(DateTime.UtcNow.ToString("s") + " TASK_GET-STOP");
            return sr;
        }

        public async Task<SearchResultPage> GetPage(ViewProperties view, SearchRequest request, UserProfile acl)
        {
            queryBuilder.Clear();
            await GetColumns(view.ViewId, acl);
            var fields = view.Columns.Where(c => c.Settings.Visible);
            foreach (var o in request.OrderBy)
            {
                var f = fields.FirstOrDefault(f => f.Id == o.ColumnId);
                if (f != null) f.Settings.SortType = (o.Descending ? SortingType.Descending : SortingType.Ascending);
            }
            await PrepareSql(fields,  request.Filters, acl);
            return await InternalGetPage(fields,  request);
        }
        public async Task<int> Count(List<SearchFilter> filters, UserProfile acl)
        {
            await PrepareSql(null, filters, acl);
            return queryBuilder.Count();
        }

        public async Task<List<int>> Find(List<SearchFilter> filters, UserProfile acl, int maxResults, List<SortingColumn> OrderBy = null)
        {
            queryBuilder.Clear();
            await GetColumns("", acl);
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " -> START-DOCUMENT-FIND");
            await PrepareSql(null, filters, acl);
            if (OrderBy != null)
            {
                foreach (var O in OrderBy)
                {
                    var field = _allColumns.FirstOrDefault(c => c.Id == O.ColumnId);
                    if (field != null)
                    {
                        foreach (var f in field.Fields)
                        {
                            var alias = queryBuilder.Map(f);
                            queryBuilder.Sort(f, !O.Descending);
                        }
                    }
                }
            }
            queryBuilder.Take(maxResults);
            var results = queryBuilder.Build().Select(r => r.AsInteger(queryBuilder.GetFields(DocumentColumn.Id)[0])).ToList();
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " -> END-DOCUMENT-FIND");
            return results;
        }
        
        public async Task<ViewProperties> GetDefaultViewProperties(string viewId, UserProfile acl)
        {
            ViewProperties V = new ViewProperties();
            V.ViewStyle = ViewStyle.List;
            V.KeyFields.Index.Add("Id");
            //V.KeyFields.Index.Add("Direction");
            V.DoubleClickAction = "#VIEW";
            V.ViewId = viewId;
            V.Columns = await GetColumns(viewId, acl);
            return V;
        }
        public bool ResolveView(string viewId)
        {
            return viewId.ToLower().StartsWith("task");
        }


        private async Task PrepareSql(IEnumerable<ViewColumn> columns, List<SearchFilter> Filters, UserProfile acl)
        {
            //sqlBuilder.Clear();
            var uid = acl.userId.Quoted();
            var roles = string.Join(',', acl.Roles.Select(c => c.Id.Quoted()));
            var groles = string.Join(',', acl.GlobalRoles.Select(c => c.Id.Quoted()));
            var groups = string.Join(',', acl.Groups.Select(c => c.Id.Quoted()));
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var companies = string.Join(',', acl.Companies.Select(c => c.Id.ToString()).Append("0"));

            selectColumns.Clear();
            queryBuilder.Add("uid", uid);
            queryBuilder.Add("roles", roles);
            queryBuilder.Add("globalroles", groles);
            queryBuilder.Add("groups", groups);
            queryBuilder.Add("date", date);
            queryBuilder.Add("companies", companies);
            queryBuilder.Map(TaskColumn.Id);
            queryBuilder.Map(TaskColumn.Status);
            queryBuilder.Map(TaskColumn.User);
            queryBuilder.Map(TaskColumn.Destinatario);
            if (columns != null)
            {
                foreach (var field in columns)
                {
                    var sort = field.Settings.SortType;
                    var columnId = field.Id;
                    selectColumns.Add(columnId, _allColumns.Find(x => x.Id == columnId));
                    queryBuilder.Map(columnId);
                    if (sort != SortingType.None)
                        queryBuilder.Sort(columnId, sort == SortingType.Ascending);
                }
            }

            foreach (var filter in Filters)
            {
                string columnId = filter.ColumnName;
                queryBuilder.Filter(columnId, filter.Operator, filter.Values.ToArray());
            }


            if (acl.userId != SpecialUser.SystemUser)
                queryBuilder.Filter(TaskColumn.Permissions, OperatorType.In, new[] { uid, roles, groups, date, companies });

        }

        private async Task<SearchResultPage> InternalGetPage(IEnumerable<ViewColumn> fields, SearchRequest searchRequest)
        {
            SearchResultPage results = new SearchResultPage(searchRequest);
            queryBuilder.Skip(searchRequest.PageIndex * searchRequest.PageSize);
            queryBuilder.Take(searchRequest.PageSize);

            foreach (var r in queryBuilder.Build())
            {
                SearchResultRow srr = new SearchResultRow();

                var status = (ExecutionStatus)(r.AsInteger(queryBuilder.GetFields(TaskColumn.Status)[0]));
                string Destinatario = r[queryBuilder.GetFields(TaskColumn.Destinatario)[0]];
                // Compongo la chiave come Id, Destinatario(1/0) e Stato 
                srr.Keys.Add(r[queryBuilder.GetFields(TaskColumn.Id)[0]]);
                srr.Keys.Add(Destinatario);
                srr.Keys.Add(((int)status).ToString());
                srr.RowState = status == ExecutionStatus.Deleted || status == ExecutionStatus.Aborted
                                ? RowState.Deleted
                                : status == ExecutionStatus.Executed || status == ExecutionStatus.Validated
                                    ? RowState.Archived
                                    : status == ExecutionStatus.Unassigned
                                        ? RowState.New
                                        : RowState.Active;
                // Riciclo i campi per recuperare i valori delle colonne
                foreach (var f in fields)
                {
                    string columnId = f.Id;
                    var names = queryBuilder.GetFields(columnId);
                    if (names != null)
                    {
                        string[] fieldValues = new string[names.Length];
                        for (int i = 0; i < names.Length; i++)
                        {
                            fieldValues[i] = r[names[i]];
                        }
                        var column = selectColumns[columnId];
                        var values = await column.Render(fieldValues);
                        srr.Columns.Add(values);
                    }
                    else
                    {
                        var values = new SearchResultColumn()
                        {
                            Value = "",
                            Description = ""
                        };
                        srr.Columns.Add(values);
                    }
                }
                results.Rows.Add(srr);
            };
            return results;
        }


        private async Task<List<ViewColumn>> GetColumns(string viewId, UserProfile acl)
        {
       
            _allColumns = new();

            //fields.Add(new GenericNumberColumn(TaskColumn.Id, "#", "Id", "Attività"));
            _allColumns.Add(new GenericAvatarColumn(TaskColumn.FromUser, "Richiedente", "Richiedente", "Attività", await dataTypeFactory.Instance("$us")));
            //fields.Add(new GenericNumberColumn(TaskColumn.Group, "Gruppo", "Gruppo Destinatario", "Attività"));
            //fields.Add(new GenericNumberColumn(TaskColumn.Role, "Ruolo", "Ruolo Destinatario", "Attività"));
            //fields.Add(new GenericNumberColumn(TaskColumn.User, "Utente", "Utente Destinatario", "Attività"));
            _allColumns.Add(new GenericTextColumn(TaskColumn.Title, "Titolo", "Titolo", "Attività"));
            _allColumns.Add(new GenericTextColumn(TaskColumn.Description, "Messaggio", "Messaggio", "Attività"));
            _allColumns.Add(new GenericTextColumn(TaskColumn.Category, "Categoria", "Categoria", "Attività"));
            _allColumns.Add(new GenericNumberColumn(TaskColumn.Percentage, "%", "Percentuale Completamento", "Attività", 50, NumberFormat.Percentage,true));

            _allColumns.Add(new TaskAssignmentColumn(TaskColumn.Assignment, "#", "Tipo Assegnazione", "Attività",""));
            _allColumns.Add(new GenericDateColumn(TaskColumn.ExpirationDate, "Scadenza", "Data Scadenza", "Attività",false));
            _allColumns.Add(new GenericDateColumn(TaskColumn.CreationDate, "Data", "Data Notifica", "Attività"));
            _allColumns.Add(new GenericNumberColumn(TaskColumn.TaskType, "?", "Tipo di Attività", "Attività"));
            //            fields.Add(new GenericNumberColumn(TaskColumn.FreeText, "", "", "Attività"));
            //            fields.Add(new GenericNumberColumn(TaskColumn.Direction, "#", "Id Documento", "Attività"));
            //            fields.Add(new GenericNumberColumn(TaskColumn.Parent, "#", "Id Documento", "Attività"));
            _allColumns.Add(new GenericNumberColumn(TaskColumn.Attachment, "", "Allegati", "Attività"));
            _allColumns.Add(new GenericNumberColumn(TaskColumn.Status, "?", "Stato", "Attività"));
            _allColumns.Add(new GenericAvatarColumn(TaskColumn.User, "Utente", "Destinatario", "Attività", await dataTypeFactory.Instance("$us")));

            _allColumns.Add(new GenericTextColumn(TaskColumn.EventId, "Azione", "Azione Richiesta", "Attività"));

            //_allColumns.Add(new GenericNumberColumn(TaskColumn.Priority, "!", "Priorità", "Attività"));
            //_allColumns.Add(new GenericNumberColumn(TaskColumn.Company, "S.I.", "Sistema Informativo", "Attività"));

            //Columns.Add(CreateColumn("fromuser"));
            ////Columns.Add(CreateColumn("Recipients"));
            //Columns.Add(CreateColumn("title"));
            //Columns.Add(CreateColumn("description"));
            //Columns.Add(CreateColumn("category"));
            //Columns.Add(CreateColumn("percentage"));
            //Columns.Add(CreateColumn("assignment"));
            //Columns.Add(CreateColumn("expirationdate"));
            //Columns.Add(CreateColumn("creationdate"));
            //Columns.Add(CreateColumn("tasktype"));
            //Columns.Add(CreateColumn("attachment"));
            //Columns.Add(CreateColumn("user"));


            //_Columns.Add("id", new Task_Id());
            //_Columns.Add("fromuser", new Task_FromUser());
            //_Columns.Add("group", new Task_Group());
            //_Columns.Add("role", new Task_Role());
            //_Columns.Add("user", new Task_User());
            //_Columns.Add("title", new Task_Title());
            //_Columns.Add("description", new Task_Description());
            //_Columns.Add("category", new Task_Category());
            //_Columns.Add("company", new Task_Company());
            //_Columns.Add("companyid", new Task_Company());
            //_Columns.Add("percentage", new Task_Percentage());
            //_Columns.Add("global_percentage", new Task_Global_Percentage());
            //_Columns.Add("priority", new Task_Priority());
            //_Columns.Add("status", new Task_Status());
            //_Columns.Add("tasktype", new Task_TaskType());
            //_Columns.Add("creationdate", new Task_CreationDate());
            //_Columns.Add("expirationdate", new Task_ExpirationDate());

            //_Columns.Add("freetext", new Task_FreeText());
            //_Columns.Add("direction", new Task_Direction());
            //_Columns.Add("parentid", new Task_ParentId());
            //_Columns.Add("attachment", new Task_AttachmentId());
            //_Columns.Add("assignment", new Task_Assignment());

            return _allColumns;
        }

  
    }
}
