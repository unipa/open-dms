using System.Data;

namespace Elmi.Core.DataAccess.Interfaces.Dialects;

public interface ISqlDialect
{
    //      string ParameterPrefix { get; }

    string GetCatalogFromConnectionString(string connectionString);
    string GetDatabaseFromConnectionString(string connectionString);
    string ChangeDatabaseInConnectionString(string connectionString, string newDatabase);
    string CheckSchema(string DataBaseName);
    string CreateSchema(string DataBaseName);
    string DropSchema(string DataBaseName);


    string CheckTable(string DataBaseName, string TableName);
    string CheckView(string DataBaseName, string ViewName);
    string CreateTable(string TableName, string FieldName, string FieldType, bool Nullable, string DefaultValue);
    string GetTableSchema(string DataBaseName, string TableName);
    string DropTable(string TableName);
    string AddColumn();
    string AlterColumn();
    string RemoveColumn();
    string FieldType(string SqlType);
    string FieldQuote(string TableName);
    string TableQuote(string TableName);

    string CheckIndex(string IndexName, string TableName);
    string CreatePrimaryKey(string TableName, params string[] KeyField);
    string CreateUniqueIndex(string IndexName, string TableName, params string[] KeyField);
    string CreateIndex(string IndexName, string TableName, params string[] KeyField);
    string DropIndex(string IndexName, string TableName);

    string CheckView(string ViewName);
    string CreateView(string ViewName, string Sql);

    String InsertWithOutput();
    bool RetrieveOutputWithScalar { get; }

    void OpenConnection(IDbConnection connection);
    string Normalize(string sql);
    string NormalizeParameter(string parameterName);
    string Skip(string Sql, int recordsToSkip);
    string Take(string Sql, int recordsToTake);

    // Function
    string GetDate();
    string GetDateTime();
    string ToDate(DateTime Date);
    string ToDateTime(DateTime Date);

}