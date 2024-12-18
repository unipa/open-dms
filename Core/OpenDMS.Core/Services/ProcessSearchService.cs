
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Core.ViewModel.CustomColumnTypes.Documents;
using OpenDMS.Domain;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.BusinessLogic
{
    public class ProcessSearchService : ISearchService
    {

        private readonly IQueryBuilder queryBuilder;
        private readonly ILookupTableService lookupTableService;
        private readonly IDataTypeFactory dataTypeFactory;
        private int RankColumn = -1;

        private Dictionary<string, ViewColumn> selectColumns = new();

        private List<ViewColumn> _allColumns = new();
        //private Dictionary<int, string> _ranking = new();
        //private Dictionary<string, List<string>> _aliases = new();

        public ProcessSearchService(IQueryBuilder queryBuilder,
            ILookupTableService lookupTableService,
            IDataTypeFactory dataTypeFactory)
        {
            this.queryBuilder = queryBuilder;
            this.lookupTableService = lookupTableService;
            this.dataTypeFactory = dataTypeFactory;
        }

        public async Task<bool> ChangeRowState(RowId RowKeys, RowState NewState, UserProfile acl)
        {
            return true;
        }

        public async Task<SearchResult> Get(ViewProperties view, SearchRequest request, UserProfile acl)
        {
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
            await PrepareSql(fields, request.Filters, acl);
            return await InternalGetPage(fields, request);
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
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " -> START-PROCESS-FIND");
            await PrepareSql(null, filters, acl);
            if (OrderBy != null)
            {
                foreach (var O in OrderBy)
                {
                    queryBuilder.Sort(O.ColumnId, !O.Descending);
                }
            }
            queryBuilder.Take(maxResults);
            var results = queryBuilder.Build().Select(r => r.AsInteger(queryBuilder.GetFields(ProcessColumn.Id)[0])).ToList();
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " -> END-PROCESS-FIND");
            return results;
        }

        public async Task<ViewProperties> GetDefaultViewProperties(string viewId, UserProfile acl)
        {
            ViewProperties V = new ViewProperties();
            V.ViewStyle = ViewStyle.List;
            V.KeyFields.Index.Add("Id");
            V.DoubleClickAction = "#VIEW";
            V.ViewId = viewId;
            V.Columns = await GetColumns(viewId, acl);
            return V;
        }
        public bool ResolveView(string viewId)
        {
            return viewId.ToLower().StartsWith("process.");
        }


        private async Task PrepareSql(IEnumerable<ViewColumn> columns, List<SearchFilter> Filters, UserProfile acl)
        {
            selectColumns.Clear();
            queryBuilder.Map(ProcessColumn.Id);
            queryBuilder.Map(ProcessColumn.TaskType);
            queryBuilder.Map(ProcessColumn.Status);
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

            var uid = acl.userId.Quoted();
            var roles = string.Join(',', acl.Roles.ConvertAll(c => c.Id.Quoted()));
            //if (string.IsNullOrEmpty(roles)) roles = "''";
            var groups = string.Join(',', acl.Groups.ConvertAll(c => c.Id.Quoted()));
            //if (string.IsNullOrEmpty(groups)) groups = "''";
            //var GlobalACL = "$GLOBAL$".Quoted();
            var permission = PermissionType.CanView;//.Quoted();
            var date = DateTime.UtcNow.ToString("yyyyMMdd");

            if (acl.userId != SpecialUser.SystemUser)
                queryBuilder.Filter(ProcessColumn.Permissions, OperatorType.In, new[] { uid, roles, groups, date, permission });
        }
        private async Task<SearchResultPage> InternalGetPage(IEnumerable<ViewColumn> fields, SearchRequest searchRequest)
        {
            SearchResultPage results = new SearchResultPage(searchRequest);
            queryBuilder.Skip(searchRequest.PageIndex * searchRequest.PageSize);
            queryBuilder.Take(searchRequest.PageSize);

            foreach (var r in queryBuilder.Build())
            {
                SearchResultRow srr = new SearchResultRow();

                srr.Keys.Add(r[queryBuilder.GetFields(ProcessColumn.Id)[0]]);
                srr.Keys.Add(r[queryBuilder.GetFields(ProcessColumn.TaskType)[0]]);
                var status = (DocumentStatus)(r.AsInteger(queryBuilder.GetFields(ProcessColumn.Status)[0]));
                srr.RowState = status == DocumentStatus.Deleted ? RowState.Deleted : RowState.Active;
                srr.Selectable = true;
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
            }


            if (RankColumn >= 0)
            {
                results.Rows = results.Rows.OrderByDescending(r => r.Columns[RankColumn].Description).ToList();
            }
            return results;
        }


        private async Task<List<ViewColumn>> GetColumns(string viewId, UserProfile acl)
        {

            string processId= "";
            if (viewId.StartsWith("process.schema."))
                processId = viewId.Substring("process.schema.".Length);

            _allColumns = new();
            // #0
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.Id, "#", "Id Processo", "Processo"));
            _allColumns.Add(new GenericTextColumn(ProcessColumn.SchemaId, "#", "Id Diagramma", "Processo"));
            _allColumns.Add(new GenericTextColumn(ProcessColumn.Name, "Nome", "Nome del Processo", "Processo"));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.TaskId, "ID Task", "Identificativo Task", "Attività"));
            _allColumns.Add(new GenericAvatarColumn(ProcessColumn.FromUser, "Richiedente", "Utente Richiedente", "Attività", await dataTypeFactory.Instance("$us")));
            _allColumns[0].Settings.Visible = false;
            _allColumns[1].Settings.Visible = false;
            _allColumns[3].Settings.Visible = false;
            // #5
            _allColumns.Add(new GenericTextColumn(ProcessColumn.Title, "Attività", "Titolo Attività", "Attività"));
            _allColumns.Add(new GenericAvatarColumn(ProcessColumn.Group, "Gruppo", "Gruppo Destinatario", "Attività", await dataTypeFactory.Instance("$gr")));
            _allColumns.Add(new GenericAvatarColumn(ProcessColumn.Role, "Ruolo", "Ruolo Destinatario", "Attività", await dataTypeFactory.Instance("$ro")));
            _allColumns.Add(new GenericAvatarColumn(ProcessColumn.User, "Utente", "Utente Destinatario", "Attività", await dataTypeFactory.Instance("$us")));
            _allColumns.Add(new GenericDateColumn  (ProcessColumn.ClaimDate, "Data PIC", "Data Presa in Carico", "Attività", true, DateTime.MaxValue));
            _allColumns[6].Settings.Visible = false;
            _allColumns[7].Settings.Visible = false;

            // #10
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.ClaimTime, "T.PIC", "Tempo Presa In Carico", "Attività"));
            _allColumns.Add(new GenericDateColumn(ProcessColumn.CreationDate, "Data Creazione", "Data Creazione Attività", "Attività", true, DateTime.MaxValue));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.DelayTime, "Ritardo", "Ritardo Attività", "Attività"));
            _allColumns.Add(new GenericTextColumn(ProcessColumn.EventId, "Evento", "Evento", "Attività"));
            _allColumns.Add(new GenericDateColumn(ProcessColumn.ExecutionDate, "Data Esecuzione", "Data Esecuzione", "Attività", true, DateTime.MaxValue));
            _allColumns[13].Settings.Visible = false;

            // #15
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.ExecutionTime, "T.Esecuzione", "Tempo di esecuzione", "Attività"));
            _allColumns.Add(new GenericDateColumn(ProcessColumn.ExpirationDate, "Scadenza", "Data Scadenza", "Attività", false, DateTime.MaxValue));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.ExpirationTime, "T.Scadenza", "Tempo di scadenza", "Attività"));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.JobTime, "T.Lavorazione", "Tempo di lavorazione", "Attività"));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.Percentage, "%", "Percentuale di completamento", "Attività"));

            // #20
            _allColumns.Add(new GenericTextColumn(ProcessColumn.Assignment, "Assegnazione", "Assegnazione", "Attività"));
            _allColumns.Add(new GenericLookupColumn(ProcessColumn.Category, "Categoria", "Categoria", "Attività"));
            _allColumns.Add(new GenericLookupColumn(ProcessColumn.Company, "S.I.", "Sistema Informativo", "Attività"));
            _allColumns.Add(new GenericLookupColumn(ProcessColumn.Priority, "!", "Priorità", "Attività"));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.Status, "?", "Stato Attività", "Attività"));
            _allColumns.Add(new GenericNumberColumn(ProcessColumn.TaskType, "?", "Tipo di Task", "Attività"));
            _allColumns[20].Settings.Visible = false;
            _allColumns[21].Settings.Visible = false;
            _allColumns[22].Settings.Visible = false;
            _allColumns[23].Settings.Visible = false;
            _allColumns[24].Settings.Visible = false;
            _allColumns[25].Settings.Visible = false;

            foreach (var F in await lookupTableService.GetAll("$PROCESS.DIMENSIONS$"))
            {
                var id = F.Id;
                _allColumns.Add(new GenericLookupColumn(ProcessColumn.Dimensions + id.ToLowerInvariant(), id, F.Description, "Dimensioni"));
            }


            if (!String.IsNullOrEmpty(processId))
            {
                //TODO: Recuperare le variabili specifiche di processo
                //foreach (var F in await lookupTableService.GetAll("$PROCESS.DIMENSIONS$"))
                //{
                //    var id = F.Id;
                //    _allColumns.Add(new GenericLookupColumn(ProcessColumn.Dimensions + id.ToLowerInvariant(), id, F.Description, "Dimensioni"));
                //}
            }
            return _allColumns;
        }
    

    }
}
