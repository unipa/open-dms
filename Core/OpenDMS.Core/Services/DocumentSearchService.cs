using Microsoft.Extensions.Logging;
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
    public class DocumentSearchService : ISearchService
    {
        private readonly IQueryBuilder queryBuilder;
        private readonly ILogger<ISearchService> logger;
        private readonly IDocumentTypeService documentTypeService;
        private readonly ICustomFieldService customfieldService;
        private readonly ISearchEngine searchEngine;
        private readonly IDataTypeFactory dataTypeFactory;
        private Dictionary<int, float> _ranking = new Dictionary<int, float>();
        private int RankColumn = -1;

        private Dictionary<string, ViewColumn> selectColumns = new();

        private List<ViewColumn> _allColumns = new();
        //private Dictionary<int, string> _ranking = new();
        //private Dictionary<string, List<string>> _aliases = new();

        public DocumentSearchService(IQueryBuilder queryBuilder,
            ILogger<ISearchService> logger,
            IDocumentTypeService documentTypeService,
            ICustomFieldService customfieldService,
            ISearchEngine searchEngine,
            IDataTypeFactory dataTypeFactory)
        {
            this.queryBuilder = queryBuilder;
            this.logger = logger;
            this.documentTypeService = documentTypeService;
            this.customfieldService = customfieldService;
            this.searchEngine = searchEngine;
            this.dataTypeFactory = dataTypeFactory;
        }

        public async Task<bool> ChangeRowState(RowId RowKeys, RowState NewState, UserProfile acl)
        {
            return true;
        }

        public async Task<SearchResult> Get(ViewProperties view, SearchRequest request, UserProfile acl)
        {
            queryBuilder.Clear();
            var cols = await GetColumns(request, acl);
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
            sr.Pages = request.PageSize > 0 ? ((sr.Count + request.PageSize - 1) / request.PageSize) : 0;

            foreach (var o in fields)
            {
                if (o.Settings.AggregateType != AggregateType.None)
                {
                    var tot = queryBuilder.Aggregate(o.Settings.AggregateType, o.Id);
                    var c = cols.FirstOrDefault(c => c.Id == o.Id);
                    try
                    {
                        sr.Totals.Add(await c.Render(new[] { tot.ToString(), tot.ToString() }));
                    }
                    catch {
                        sr.Totals.Add(new SearchResultColumn() { Value = "0", Description = "" });
                    };
                } 
                else
                {
                    sr.Totals.Add(new SearchResultColumn() { Value = "0", Description = "" });
                }
            }
            return sr;
        }

        public async Task<SearchResultPage> GetPage(ViewProperties view, SearchRequest request, UserProfile acl)
        {
            queryBuilder.Clear();
            await GetColumns(request, acl);
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
            await GetColumns(new SearchRequest() { ViewId = "", Filters = filters }, acl);
//            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " -> START-DOCUMENT-FIND");
            await PrepareSql(null, filters, acl);
            if (OrderBy != null)
            {
                foreach (var O in OrderBy)
                {
                    //var field = _allColumns.FirstOrDefault(c=>c.Id == O.ColumnId);
                    //if (field != null)
                    //{
                        queryBuilder.Sort(O.ColumnId, !O.Descending);
//                        foreach (var f in field.Fields)
//                        {
//                            var alias = queryBuilder.Map(f);
//                        }
                    //}
                }
            }
            queryBuilder.Take(maxResults);
            List<int> results = null;
            try
            {
                results = queryBuilder.Build().Select(r => r.AsInteger(queryBuilder.GetFields(DocumentColumn.Id)[0])).ToList();
//                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " -> END-DOCUMENT-FIND");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Document.Find");
            }
            return results;
        }

        public async Task<ViewProperties> GetDefaultViewProperties(string viewId, UserProfile acl)
        {
            ViewProperties V = new ViewProperties();
            V.ViewStyle = ViewStyle.List;
            V.KeyFields.Index.Add(DocumentColumn.Id);
            V.KeyFields.Index.Add(DocumentColumn.ContentType);
            V.DoubleClickAction = "#VIEW";
            V.ViewId = viewId;
            V.Columns = await GetColumns(new SearchRequest() { ViewId = viewId }, acl);
            return V;
        }
        public bool ResolveView(string viewId)
        {
            return viewId.ToLower().StartsWith("doc.");
        }


        private async Task PrepareSql(IEnumerable<ViewColumn> columns, List<SearchFilter> Filters, UserProfile acl)
        {
            var uid = acl.userId.Quoted();
            var roles = string.Join(',', acl.Roles.Select(c => c.Id.Quoted()));
            var groles = string.Join(',', acl.GlobalRoles.Select(c => c.Id.Quoted()));
            var groups = string.Join(',', acl.Groups.Select(c => c.Id.Quoted()));
            var permission = PermissionType.CanView;
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var companies = string.Join(',', acl.Companies.Select(c => c.Id.ToString()).Append("0"));

            selectColumns.Clear();
            queryBuilder.Add("uid", uid);
            queryBuilder.Add("roles", roles);
            queryBuilder.Add("globalroles", groles);
            queryBuilder.Add("groups", groups);
            queryBuilder.Add("date", date);
            queryBuilder.Add("companies", companies);
            queryBuilder.Add("permission", permission);
            queryBuilder.Map(DocumentColumn.Id);
            queryBuilder.Map(DocumentColumn.ContentType);
            queryBuilder.Map(DocumentColumn.Status);
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
                if (columnId == DocumentColumn.FreeText && RankColumn >= 0)
                {
                    // ricerca per contenuto
                    _ranking = await searchEngine.Search(filter.Values[0], 100, acl);
                    if (_ranking.Count > 0)
                    {
                        queryBuilder.Filter(DocumentColumn.Id, OperatorType.In, _ranking.Select(r => r.Key.ToString()).ToArray());
                    }
                    else
                    {
                        queryBuilder.Filter(columnId, filter.Operator, filter.Values.ToArray());
                    }
                }
                else
                {
                    queryBuilder.Filter(columnId, filter.Operator, filter.Values.ToArray());
                }
            }

            if (acl.userId != SpecialUser.SystemUser)
                queryBuilder.Filter(DocumentColumn.Permissions, OperatorType.In, new[] { uid, roles, groups, date, permission, companies, groles });
        }
        private async Task<SearchResultPage> InternalGetPage(IEnumerable<ViewColumn> fields, SearchRequest searchRequest)
        {
            SearchResultPage results = new SearchResultPage(searchRequest);
            queryBuilder.Skip(searchRequest.PageIndex * searchRequest.PageSize);
            queryBuilder.Take(searchRequest.PageSize);

            foreach (var r in queryBuilder.Build())
            {
                SearchResultRow srr = new SearchResultRow();

                srr.Keys.Add(r[queryBuilder.GetFields(DocumentColumn.Id)[0]]);
                srr.Keys.Add(r[queryBuilder.GetFields(DocumentColumn.ContentType)[0]]);
                var status = (DocumentStatus)(r.AsInteger(queryBuilder.GetFields(DocumentColumn.Status)[0]));
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
                        if (columnId == DocumentColumn.Rank && _ranking != null)
                        {
                            fieldValues[0] = (_ranking[r.AsInteger(queryBuilder.GetFields(DocumentColumn.Id)[0])]).ToString();
                            RankColumn = srr.Columns.Count;
                        }
                        if (column != null)
                        {
                            var values = await column.Render(fieldValues);
                            srr.Columns.Add(values);
                        }
                        else srr.Columns.Add(new SearchResultColumn() { Value="", Description="" });
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


        private async Task<List<ViewColumn>> GetColumns(SearchRequest request, UserProfile acl)
        {
            string viewId = request.ViewId;
            List<string> doctypes = new List<string>();
            var f = request.Filters.FirstOrDefault(f => f.ColumnName == DocumentColumn.DocumentType);
            if (f != null)
                doctypes = f.Values;

            var doctype = "";
            if (viewId.StartsWith("doc.type."))
                doctype = viewId.Substring("doc.type.".Length);

            //var doctype = doctypes.Count > 0 ? doctypes.First() : "";
            var tp = await documentTypeService.GetById(doctype);

            _allColumns = new();

            var c1 =new GenericNumberColumn(DocumentColumn.Id, "#", "Id Documento", "Documento");
            _allColumns.Add(c1);

            if (viewId.StartsWith("doc.content."))
                _allColumns.Add(new GenericIconColumn(DocumentColumn.Rank, "Rank", "Rank", "Documento", null, DocumentColumn.RankIcons));

            _allColumns.Add(new ContentTypeColumn());

            if (!string.IsNullOrEmpty(tp.DescriptionLabel))
                _allColumns.Add(new GenericTextColumn(DocumentColumn.Description, tp.DescriptionLabel, tp.DescriptionLabel, "Documento"));
            if (!string.IsNullOrEmpty(tp.DocumentNumberLabel))
                _allColumns.Add(new GenericLookupColumn(DocumentColumn.DocumentNumber, tp.DocumentNumberLabel, tp.DocumentNumberLabel, "Documento"));
            if (!string.IsNullOrEmpty(tp.DocumentDateLabel))
                _allColumns.Add(new GenericDateColumn(DocumentColumn.DocumentDate, tp.DocumentDateLabel, tp.DocumentDateLabel, "Documento", false));

            //fields.Add(new GenericTextColumn(DocumentColumn.Company, "S.I.", "Sistema Informativo", "Documento"));

            if (doctypes.Count != 1)
            //if (!String.is)
                _allColumns.Add(new DocumentTypeColumn());

            HashSet<string> metas = new();
            if (tp.Fields != null)
            {
                foreach (var F in tp.Fields.Where(t => !t.Deleted && !t.Tag).OrderBy(t => t.FieldIndex))
                {
                    var meta = await customfieldService.GetById(F.FieldTypeId);
                    var id = F.FieldName; // meta.Id.ToLowerInvariant();
                    if (id != null)
                    {
                        var cdt = ColumnDataType.Text;
                        if (F.FieldTypeId == "$us") cdt = ColumnDataType.Avatar;
                        if (F.FieldTypeId == "$$i" || F.FieldTypeId == "$$f")
                            cdt = ColumnDataType.Number;
                        if (F.FieldTypeId == "$$d") cdt = ColumnDataType.Date;

                        _allColumns.Add(new GenericLookupColumn(DocumentColumn.Field + id.ToLowerInvariant(), id, string.IsNullOrEmpty(F.Title) ? meta.Title : F.Title, meta.Description, "Metadati", cdt));
                        metas.Add(id);
                    }
                }
            }

            ViewColumn c7 = null;
            if (viewId.StartsWith("doc.last"))
            {

            }
            else
            {
                _allColumns.Add(new GenericAvatarColumn(DocumentColumn.Owner, "Proprietario", "Utente Proprietario", "Documento", await dataTypeFactory.Instance("$us")));
                _allColumns.Add(new GenericTextColumn(DocumentColumn.ProtocolFormattedNumber, "Protocollo", "Protocollo", "Protocollo"));
                _allColumns.Add(new GenericDateColumn(DocumentColumn.ProtocolDate, "Data Prot.", "Data Protocollo", "Protocollo",false));
                if (tp.ContentType == ContentType.Document || (int)tp.ContentType == 0)
                {
                    c7 = (new Document_FileName());
                    _allColumns.Add(c7);
                    _allColumns.Add(new GenericSizeColumn(DocumentColumn.FileSize, "Dim.", "Dimensione File", "Contenuto"));

                    //_allColumns.Add(new GenericNumberColumn(DocumentColumn.Revision, "Rev.", "Nr.Revisione", "Contenuto"));
                    //_allColumns.Add(new GenericNumberColumn(DocumentColumn.Version, "Ver.", "Nr.Versione", "Contenuto"));
                    _allColumns.Add(new VersionNumberColumn());
                    _allColumns.Add(new GenericIconColumn(DocumentColumn.Preview, "<i class='fa fa-eye'></i>", "Stato Generazione Preview", "Stati", DocumentColumn.JobStatusTooltips, DocumentColumn.JobStatusIcons, 6));
                    _allColumns.Add(new GenericIconColumn(DocumentColumn.Indexing, "<i class='fa fa-search'></i>", "Stato Indicizzazione", "Stati", DocumentColumn.JobStatusTooltips, DocumentColumn.JobStatusIcons, 6));
                    _allColumns.Add(new GenericIconColumn(DocumentColumn.Sending, "<i class='fa fa-globe'></i>", "Stato Pubblicazione", "Stati", DocumentColumn.JobStatusTooltips, DocumentColumn.JobStatusIcons, 6));
//                    _allColumns.Add(new GenericIconColumn(DocumentColumn.Preservation, "<i class='fa fa-lock'></i>", "Stato Conservazione", "Stati", DocumentColumn.JobStatusTooltips, DocumentColumn.JobStatusIcons, 6));
                    _allColumns.Add(new GenericIconColumn(DocumentColumn.Signature, "<i class='fa fa-pencil'></i>", "Stato Sottoscrizione", "Stati", DocumentColumn.JobStatusTooltips, DocumentColumn.JobStatusIcons, 6));
                }
            }

            _allColumns.Add(new GenericDateColumn(DocumentColumn.ExpirationDate, "Scadenza", "Data Scadenza", "Documento", false, DateTime.MaxValue));
            var c2 = new GenericIconColumn(DocumentColumn.Status, "", "Stato", "Documento", DocumentColumn.StatusTooltips, DocumentColumn.StatusIcons);
            _allColumns.Add(c2);
            var c3 = (new GenericTextColumn(DocumentColumn.ProtocolNumber, "Nr.Prot.", "Nr.Protocollo", "Protocollo"));
            _allColumns.Add(c3);
            var c4 = (new GenericDateColumn(DocumentColumn.ProtocolDate, "Data Prot.", "Data Protocollo", "Protocollo"));
            _allColumns.Add(c4);

            var c6 = new GenericTextColumn(DocumentColumn.ExternalId, "ID Univoco", "Id Univoco Globale", "Documento");
            _allColumns.Add(c6);

            var c8 = (new GenericDateColumn(DocumentColumn.CreationDate, "Creato Il", "Data Creazione", "Documento"));
            c8.Settings.SortType = SortingType.Descending;
            _allColumns.Add(c8);

            var c5 = (new GenericDateColumn(DocumentColumn.ViewDate+acl.userId,  "Visto il", "Data Visualizzazione", "Documento"));
            c5.Settings.SortType = SortingType.Descending;
            _allColumns.Add(c5);


            //foreach (var meta in (customfieldService.GetAll().GetAwaiter().GetResult()).Where(m => m.Customized))
            //{
            //    var id = meta.Id.ToLowerInvariant();
            //    if (!metas.Contains(id))
            //        _allColumns.Add(new GenericLookupColumn(DocumentColumn.Meta + meta.Id.ToLowerInvariant(), meta.Id, meta.Title, meta.Description, "Metadati"));
            //}

            if (viewId.StartsWith("doc.content"))
            {
                RankColumn = _allColumns.FindIndex(f => f.Id == DocumentColumn.Rank);
                _allColumns[RankColumn].Settings.Visible = true;
            } else RankColumn = -1;
            //_allColumns.ForEach(f=> f.Settings.Visible = true);
            c1.Settings.Visible = false;
            c2.Settings.Visible = false;
            c3.Settings.Visible = false;
            c4.Settings.Visible = false;
            if (!viewId.StartsWith("doc.lastviewed"))
                c5.Settings.Visible = false;
            else
                c8.Settings.Visible = false;
            c6.Settings.Visible = false;
            if (c7 != null)
                c7.Settings.Visible = false;
            return _allColumns;
        }
    }
}

