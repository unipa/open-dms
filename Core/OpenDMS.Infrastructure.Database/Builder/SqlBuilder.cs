using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Builder;

public class SqlBuilder : ISqlBuilder
{
    private readonly ApplicationDbContext repo;
    private readonly IApplicationDbContextFactory contextFactory;


    private Dictionary<string, string> Aliases = new Dictionary<string, string>();
    private HashSet<string> SortColumns = new();
    private HashSet<string> Tables = new();
    private int index = 0;
    private int skip = 0;
    private int take = 0;
    private string SQLSelect = "";
    private string SQLFrom = "";
    private string SQLWhere = "";
    private string SQLOrderBy = "";
    private string SQLGroupBy = "";
    private string SQLHaving = "";

    public SqlBuilder(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        repo = (ApplicationDbContext)contextFactory.GetDbContext();
    }

//    public ISqlBuilder AddTable(string prefix, string table)
//    {
////        xTables.Add(prefix, table);
//        return this;
//    }

//    public ISqlBuilder AddRelation(string fromPrefix, string toPrefix, string rules)
//    {
//        Relations.Add(fromPrefix + "-" + toPrefix, rules);
//        return this;
//    }



    public string GetField(string FieldName)
    {
        if (Aliases.ContainsKey(FieldName))
        {
            return Aliases[FieldName];
        }
        return "";
    }

    public ISqlBuilder AddField(string FieldName)
    {
        if (!Aliases.ContainsKey(FieldName))
        {
            index++;
//            ColumnIndex.Add(FieldName, "A" + index);
            Aliases.Add(FieldName, "A" + index);
            if (!string.IsNullOrEmpty(SQLSelect)) SQLSelect += ",";
            SQLSelect += FieldName + " A" + index;
        }
        return this;
    }
    public ISqlBuilder AddFields(string[] FieldNames)
    {
        if (FieldNames != null)
            foreach (var f in FieldNames)
                AddField(f);
        return this;
    }

    public string FormatDate(DateTime date)
    {
        return repo.FormatDate(date);
    }

    public string Alias(string FieldName)
    {
        return Aliases.ContainsKey(FieldName) ? Aliases[FieldName] : "";
    }
    public ISqlBuilder AddTable(string TableName)
    {
        if (!Tables.Contains(TableName))
        {
            Tables.Add(TableName);
            SQLFrom += " " + TableName;
        }
        return this;
    }
    public ISqlBuilder AddTables(string[] TableNames)
    {
        if (TableNames != null)
            foreach (var f in TableNames)
                AddTable(f);
        return this;
    }

    public ISqlBuilder AddOrderBy(string FieldName, bool Ascending = true)
    {
        if (!SortColumns.Contains(FieldName))
        {
            var alias = Alias(FieldName);
            if (string.IsNullOrEmpty(alias))
            {
                AddField(FieldName);
                alias = Alias(FieldName);
            }
            SortColumns.Add(alias);
            if (!string.IsNullOrEmpty(SQLOrderBy)) SQLOrderBy += ",";
            SQLOrderBy += alias + (Ascending ? "" : " DESC");
        }
        return this;
    }



    public ISqlBuilder AddGroupBy(string[] FieldNames)
    {
        if (FieldNames != null)
            foreach (var f in FieldNames)
                AddGroupBy(f);
        return this;
    }
    public ISqlBuilder AddGroupBy(string Filter)
    {
        if (!string.IsNullOrEmpty(SQLGroupBy))
            SQLGroupBy += ",";
        SQLGroupBy += Alias(Filter);
        return this;
    }
    public ISqlBuilder AddHaving(string field, OperatorType operatorType, string[] values)
    {
        var Filter = repo.GetNumberFilter(field, operatorType, values);
        if (!string.IsNullOrEmpty(Filter))
        {
            if (!string.IsNullOrEmpty(SQLHaving))
                SQLHaving += "AND";
            SQLHaving += "(" + Filter + ")";
        }

        return this;
    }

    public ISqlBuilder AddFilter(string Filter)
    {
        if (!string.IsNullOrEmpty(SQLWhere))
            SQLWhere += "AND";
        SQLWhere += "(" + Filter + ")";
        return this;
    }
    public string GetTextFilter(string field, OperatorType operatorType, string[] values)
    {
        var Filter = repo.GetTextFilter(field, operatorType, values);
        return Filter;
    }

    public ISqlBuilder AddTextFilter(string field, OperatorType operatorType, string[] values)
    {
        var Filter = repo.GetTextFilter(field, operatorType, values);
        if (!string.IsNullOrEmpty(Filter))
        {
            if (!string.IsNullOrEmpty(SQLWhere))
                SQLWhere += "AND";
            SQLWhere += "(" + Filter + ")";
        }
        return this;
    }
    public ISqlBuilder AddDateFilter(string field, OperatorType operatorType, string[] values)
    {
        var Filter = repo.GetDateFilter(field, operatorType, values);
        if (!string.IsNullOrEmpty(Filter))
        {
            if (!string.IsNullOrEmpty(SQLWhere))
                SQLWhere += "AND";
            SQLWhere += "(" + Filter + ")";
        }
        return this;
    }
    public ISqlBuilder AddNumericFilter(string field, OperatorType operatorType, string[] values)
    {
        var Filter = repo.GetNumberFilter(field, operatorType, values);
        if (!string.IsNullOrEmpty(Filter))
        {
            if (!string.IsNullOrEmpty(SQLWhere))
                SQLWhere += "AND";
            SQLWhere += "(" + Filter + ")";
        }
        return this;
    }



    public ISqlBuilder Clear()
    {
        Tables.Clear();
        SortColumns.Clear();
        Aliases.Clear();
        SQLSelect = "";
        SQLWhere = "";
        SQLOrderBy = "";
        SQLGroupBy = "";
        SQLHaving = "";
        SQLFrom = "";
        index = 0;
        take = 0;
        skip = 0;
        return this;
    }
    public ISqlBuilder AddOrderBy(string[] FieldName, bool Ascending = true)
    {
        foreach (var f in FieldName)
            AddOrderBy(f, Ascending);
        return this;
    }
    public ISqlBuilder Take(int Rows)
    {
        take = Rows;
        return this;
    }
    public ISqlBuilder Skip(int Rows)
    {
        skip = Rows;
        return this;
    }
    public IEnumerable<QueryRow> Build()
    {
        var sql = repo.GetSql(SQLSelect, SQLFrom, SQLWhere, SQLGroupBy, SQLHaving, SQLOrderBy, skip, take);
        return repo.Select(sql);
    }


    public async Task<List<T>> Select<T>(int Skip, int Take, Func<Dictionary<string, string>, int, Task<T>> map)
    {

        return await repo.Select(SQLSelect, SQLFrom, SQLWhere, SQLGroupBy, SQLHaving, SQLOrderBy, Skip, Take, map);
    }

    public int Count()
    {
        var sql = repo.GetSql("COUNT(*)", SQLFrom, SQLWhere, SQLGroupBy, SQLHaving, "", 0, 0);
        return repo.Count(sql);
    }
    public decimal Aggregate(AggregateType aggregate, string FieldName)
    {
        string select = "";
        int div = 1;
        switch (aggregate)
        {
            case AggregateType.None:
                return 0;
                break;
            case AggregateType.Count:
                select = "COUNT(" + FieldName + ")";
                break;
            case AggregateType.Sum:
                select = "SUM(" + FieldName + ")";
                break;
            case AggregateType.Min:
                select = "MIN(" + FieldName + ")";
                break;
            case AggregateType.Max:
                select = "MAX(" + FieldName + ")";
                break;
            case AggregateType.Average:
                select = "AVG(" + FieldName + ")*100";
                div = 100;
                break;
            default:
                break;
        }
        var sql = repo.GetSql(select, SQLFrom, SQLWhere, SQLGroupBy, SQLHaving, "", 0, 0);
        try
        {
            return repo.Count(sql) / div;
        }
        catch { return 0; }
    }


    public string ToString()
    {
        return repo.GetSql(SQLSelect, SQLFrom, SQLWhere, SQLGroupBy, SQLHaving, SQLOrderBy, 0, 0);
    }
}

