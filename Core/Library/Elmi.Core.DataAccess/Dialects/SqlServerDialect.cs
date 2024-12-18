using Elmi.Core.DataAccess.Interfaces;
using Elmi.Core.DataAccess.Interfaces.Dialects;
using System.Data;
using System.Data.Common;


namespace Elmi.Core.DataAccess.Dialects;


internal class SqlServerDialect : ISqlDialect
{
    private HashSet<string> indexFields = new HashSet<string>();


    public SqlServerDialect()
    {
    }


    public String GetCatalogFromConnectionString(string connectionString)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        CSB.Remove("database");
        return CSB.ConnectionString;
    }
    public String GetDatabaseFromConnectionString(string connectionString)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        return CSB["database"].ToString();
    }
    public string ChangeDatabaseInConnectionString(string connectionString, string newDatabase)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        CSB["database"] = newDatabase;
        return CSB.ConnectionString;
    }


    public String CheckSchema(string DataBaseName)
    {
        return "SELECT COUNT(*) FROM SYS.DATABASES WHERE NAME=" + IDataSource.AsString(DataBaseName);
    }

    public String CreateSchema(string DataBaseName)
    {
        return "CREATE DATABASE [" + DataBaseName + "]";
    }
    public String DropSchema(string DataBaseName)
    {
        return "DROP DATABASE [" + DataBaseName + "]";
    }

    public String InsertWithOutput()
    {
        return "INSERT INTO {0} ({1}) VALUES({2}); SELECT SCOPE_IDENTITY()";
    }
    public bool RetrieveOutputWithScalar { get; } = true;

    public String CheckTable(string DataBaseName, string TableName)
    {
        return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG=" + IDataSource.AsString(DataBaseName) + " AND TABLE_NAME=" + IDataSource.AsString(TableName);
    }
    public String CheckView(string DataBaseName, string TableName)
    {
        return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG=" + IDataSource.AsString(DataBaseName) + " AND TABLE_NAME=" + IDataSource.AsString(TableName);
    }
    public String CreateTable(string TableName, string FieldName, string FieldType, bool Nullable, string DefaultValue)
    {
        return "CREATE TABLE " + TableQuote(TableName) + " (" + FieldQuote(FieldName) + " " + FieldType + " " + (Nullable ? "NULL" : "NOT NULL") + " " +DefaultValue + ")";
    }
    public String GetTableSchema(string DataBaseName, string TableName)
    {
        return "SELECT COLUMN_NAME, (CASE WHEN CHARACTER_MAXIMUM_LENGTH IS NULL OR DATA_TYPE ='text' THEN CASE WHEN DATA_TYPE='decimal' OR NUMERIC_SCALE > 0 THEN DATA_TYPE+'('+CAST(NUMERIC_PRECISION AS VARCHAR(3))+','+CAST(NUMERIC_SCALE AS VARCHAR(5))+')'  ELSE DATA_TYPE END ELSE  DATA_TYPE+'('+CAST(CHARACTER_MAXIMUM_LENGTH AS varchar(5))+')' END) AS COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG='" + DataBaseName + "' AND TABLE_NAME='" + TableName + "'"; //SELECT COLUMN_NAME, DATA_TYPE + (CASE WHEN CHARACTER_MAXIMUM_LENGTH IS NULL THEN CASE WHEN DATA_TYPE='int' THEN '' ELSE '('+CAST(NUMERIC_PRECISION AS VARCHAR(3))+','+CAST(NUMERIC_SCALE AS VARCHAR(5))+')' END ELSE '('+CAST(CHARACTER_MAXIMUM_LENGTH AS varchar(5))+')' END) AS COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG='" + DataBaseName + "' AND TABLE_NAME='" + TableName + "'";
    }

    public String AddColumn()
    {
        return "ALTER TABLE {0} ADD {1} {2} {3} {4}";
    }
    public String AlterColumn()
    {
        return "ALTER TABLE {0} ALTER COLUMN {1} {2} {3} {4}";
    }
    public String RemoveColumn()
    {
        return "ALTER TABLE {0} DROP COLUMN {1}";
    }

    public String FieldType(string SqlType)
    {
        string[] val = SplitFieldType(SqlType);

        if (val[0].ToLower() == "identity" || SqlType.ToLower() == "identity")
            return "INT IDENTITY(1,1)";
        else
        if (val[0].ToLower() == "integer" )
            return "INT";
        else
            return SqlType.Replace(" ", "").Replace("IDENTITY", " IDENTITY");
    }
    public String FieldQuote(string SqlType)
    {
        return "\"" + SqlType+"\"";
    }
    public String TableQuote(string SqlType)
    {
        return "[" + SqlType + "]";
    }

    public String Normalize(string sql)
    {
        return sql;
    }
    public String NormalizeParameter(string sql)
    {
        return "@" + sql;
    }
    public String CheckIndex(string IndexName,  string TableName)
    {
        if (IndexName == "") IndexName = "PK_"+TableName;
        return "SELECT COUNT(*) FROM sys.indexes WHERE name=" + IDataSource.AsString(IndexName);
    }

    public String CreatePrimaryKey(string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);
        return "ALTER TABLE " + TableQuote(TableName) + " ADD  CONSTRAINT "+ TableQuote("PK_"+TableName)+ " PRIMARY KEY (" + f + ")";
    }
    public String CreateUniqueIndex(string IndexName, string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);
        return "CREATE UNIQUE INDEX " + TableQuote(IndexName) + " ON " + TableQuote(TableName) + " (" + f  + ")"; 
    }
    public String CreateIndex(string IndexName, string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);
        return "CREATE INDEX " + TableQuote(IndexName) + " ON " + TableQuote( TableName) + " (" + f + ")";
    }

    public string DropTable( string TableName)
    {
        return "DROP TABLE " + TableQuote(TableName);
    }

 

    public string DropIndex(string IndexName, string TableName)
    {
        return "DROP INDEX " + TableQuote(IndexName) + " ON " + TableQuote(TableName);
    }

    public string CheckView(string ViewName)
    {
        throw new NotImplementedException();
    }

    public string CreateView(string ViewName, string Sql)
    {
        return "CREATE VIEW " + TableQuote(ViewName) + " AS " + (Sql);
    }

    public void OpenConnection(IDbConnection connection)
    {
        
    }

    public string Skip(string Sql, int recordsToSkip)
    {
        if (recordsToSkip >= 0)
            return Sql + " OFFSET " + recordsToSkip + " ROWS ";
        return Sql;
    }

    public string Take(string Sql, int recordsToTake)
    {
        if (recordsToTake >= 0)
            return Sql + " FETCH NEXT " + recordsToTake + " ROWS ONLY";
        return Sql;
    }

    public string GetDate()
    {
        return "GETDATE()";
    }

    public string GetDateTime()
    {
        return "GETDATE()";
    }

    public string ToDate(DateTime Date)
    {
        String format = "yyyy-dd-MM";
        String Text = Date.ToString(format);
        return ("'" + Text + "'").Replace('.', ':');
    }

    public string ToDateTime(DateTime Date)
    {
        String format = "yyyy-dd-MM";
        String Text = Date.ToString(format + " " + System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern).Replace('.', ':');
        return ("'" + Text + "'").Replace('.', ':');
    }

    protected string[] SplitFieldType(string FieldType)
    {
        List<String> s = new List<string>();
        int i1 = FieldType.IndexOf("(");
        //  if (i1 < 0) i1 = FieldType.IndexOf(",");
        if (i1 < 0) i1 = FieldType.Length;
        s.Add(FieldType.Substring(0, i1).Trim().ToLower());
        if (i1 >= FieldType.Length) FieldType = "";
        else
        {
            FieldType = FieldType.Substring(i1 + 1);
            int i2 = FieldType.IndexOf(")");
            foreach (string p in FieldType.Substring(0, i2).Split(','))
            {
                s.Add(p.Trim().ToLower());
            }
        }
        return s.ToArray();
    }

}
