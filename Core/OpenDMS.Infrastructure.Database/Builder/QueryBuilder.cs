using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.QueryBuilder;


namespace OpenDMS.Infrastructure.Database.Builder
{

    public partial class QueryBuilder  :IQueryBuilder
    {


        protected readonly ISqlBuilder sqlBuilder;
        private List<SelectField> selectFields = new();
        private List<FilterRule> filters = new();
        private List<FilterRule> reducers = new();
        private List<SortField> sorts = new();
        private bool prepared = false;

        private Dictionary<string,string> Globals = new();
        private int take = 0;
        private int skip = 0;

        public QueryBuilder(ISqlBuilder sqlBuilder)
        {
            this.sqlBuilder = sqlBuilder;
        }

        public IEnumerable<QueryRow> Build()
        {
            prepareBuilder();
            return sqlBuilder.Build();
        }
        public void Clear()
        {
            sqlBuilder.Clear();
            selectFields = new();
            filters = new();
            reducers = new();
            sorts = new();
            prepared = false;
            Globals = new();
            take = 0;
            skip = 0;
        }
        public int Count()
        {
            prepareBuilder();
            return sqlBuilder.Count();
        }
        public decimal Aggregate(AggregateType aggregate, string FieldName)
        {
            prepareBuilder();
            var f = getSelectFields(FieldName)[0];
            return sqlBuilder.Aggregate(aggregate, f);
        }


        public void Filter(string element, OperatorType operatorType, string[] values)
        {
            filters.Add(new FilterRule(element, operatorType, values));
        }
        public void Add(string key, string value)
        {
            Globals.Add(key, value);
        }
        public string Var(string key)
        {
            return Globals[key];
        }
        public string[] GetFields(string field)
        {
            List<string> indices = new();
            var _fields = getSelectFields(field);
            if (_fields != null)
            foreach (var f in _fields)
            {
                var i = sqlBuilder.GetField(f);
                if (!String.IsNullOrEmpty(i))
                    indices.Add(i);
            }
            return indices.ToArray();
        }

        public string Map(string field)
        {
//            var fields = getSelectFields(field);
            if (!String.IsNullOrEmpty(field))
            {
                var alias = getIdentifier();
                selectFields.Add(new SelectField(alias, field, AggregateType.None));
                return alias;
            }
            return "";
        }

        public string Map(string field, AggregateType aggregateType)
        {
            if (!String.IsNullOrEmpty(field))
            {
                var alias = getIdentifier();
                selectFields.Add(new SelectField(alias, field, aggregateType));
                return alias;
            }
            return "";
        }

        public void Reduce(string field, OperatorType operatorType, string[] values)
        {
            if (!String.IsNullOrEmpty(field))
            {
                reducers.Add(new FilterRule(field, operatorType, values));
            }
        }

        public void Skip(int rows)
        {
            this.skip = rows;
        }

        public void Sort(string field, bool Ascending = true)
        {
            if (!String.IsNullOrEmpty(field))
            {
                sorts.Add(new SortField(field, Ascending));
            }
        }

        public void Take(int rows)
        {
            this.take = rows;
        }


        private void prepareBuilder()
        {
            if (prepared) return;
            prepared = true;
            sqlBuilder.Clear();
            var IsGrouped = selectFields.Any(e => e.AggregateType != AggregateType.None);
            Dictionary<string, string> _alias = new();
            foreach (SelectField field in selectFields)
            {
                var prefix = getTablePrefix(field.Field);
                if (prefix != null)
                {
                    foreach (var rel in prefix)
                    {
                        var table = GetTableRelations(rel,  field.Field);
                        if (!String.IsNullOrEmpty(table))
                        {
                            sqlBuilder.AddTable(table);
                        }
                    }
                }
                var _fields = getSelectFields(field.Field);
                if (_fields != null)
                {
                    foreach (var f in _fields)
                    {
                        sqlBuilder.AddField(addAggregationType(f, field.AggregateType));
                    }
                    if (IsGrouped && field.AggregateType == AggregateType.None)
                    {
                        sqlBuilder.AddGroupBy(_fields);
                    }
                }
            }

            foreach (var f in filters)
            {
                getFilter(f);
            }

            foreach (var r in reducers)
            {

                GetReducer(r);
            }

            foreach (var s in sorts)
            {

                var _fields = getSelectFields(s.Field);
                if (_fields != null)
                {
                    sqlBuilder.AddOrderBy(_fields, s.Ascending);
                }
            }

            sqlBuilder.Skip(skip);
            sqlBuilder.Take(take);

        }


        private void GetReducer(FilterRule r)
        {
            var queryField = r.Element;
            var dbField = getSelectFields(queryField)[0];
            sqlBuilder.AddHaving(dbField, r.OperatorType, r.Values);
        }





        private string getIdentifier()
        {
            return Guid.NewGuid().ToString(); // aliasPrefix + (selectFields.Count).ToString();
        }

        private string addAggregationType(string field, AggregateType aggregateType)
        {
            string f = field;
            switch (aggregateType)
            {
                case AggregateType.None:
                    break;
                case AggregateType.Count:
                    f = $"COUNT({f})";
                    break;
                case AggregateType.Sum:
                    f = $"SUM({f})";
                    break;
                case AggregateType.Min:
                    f = $"MIN({f})";
                    break;
                case AggregateType.Max:
                    f = $"MAX({f})";
                    break;
                case AggregateType.Average:
                    f = $"AVG({f})";
                    break;
                default:
                    break;
            }
            return f;
        }


    }
}
