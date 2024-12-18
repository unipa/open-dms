using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.QueryBuilder;


namespace OpenDMS.Infrastructure.Database.Builder;


public interface ISqlBuilder
{


    string Alias(string FieldName);

    ISqlBuilder AddField(string FieldName);
    ISqlBuilder AddTable(string TableName);
    ISqlBuilder AddFilter(string Filter);
    ISqlBuilder AddOrderBy(string FieldName, bool Ascending = true);
    ISqlBuilder AddOrderBy(string[] FieldName, bool Ascending = true);
    ISqlBuilder AddFields(string[] TableNames);
    ISqlBuilder AddTables(string[] TableNames);
    ISqlBuilder Clear();
    string GetField(string FieldName);

    string FormatDate(DateTime Date);
    Task<List<T>> Select<T>(int Skip, int Take, Func<Dictionary<string, string>, int, Task<T>> map);
    int Count();
    decimal Aggregate(AggregateType aggregate, string FieldName);

    string ToString();
    ISqlBuilder AddGroupBy(string[] FieldNames);
    ISqlBuilder AddHaving(string field, OperatorType operatorType, string[] values);
    ISqlBuilder AddGroupBy(string Filter);
    ISqlBuilder AddNumericFilter(string field, OperatorType operatorType, string[] values);
    ISqlBuilder AddDateFilter(string field, OperatorType operatorType, string[] values);
    ISqlBuilder AddTextFilter(string field, OperatorType operatorType, string[] values);
    ISqlBuilder Take(int Rows);
    ISqlBuilder Skip(int Rows);
    IEnumerable<QueryRow> Build();
    string GetTextFilter(string field, OperatorType operatorType, string[] values);
}

