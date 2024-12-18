using System.Data.Common;
using System.Data;
using Elmi.Core.DataAccess.Interfaces.Dialects;
using Elmi.Core.DataAccess.Interfaces;

namespace Elmi.Core.DataAccess.Dialects;


internal class MySqlDialect : ISqlDialect
{
    private bool PrimaryKeyCreated = false;
    private HashSet<string> indexFields = new HashSet<string>();


    public MySqlDialect()
    {
    }

    public String GetCatalogFromConnectionString(string connectionString)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        CSB["database"] = "sys";
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
        return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME=" + IDataSource.AsString(DataBaseName);
    }

    public String CreateSchema(string DataBaseName)
    {
        string x = "";
        x += "CREATE DATABASE " +TableQuote( DataBaseName);
        return x;
    }
    public String DropSchema(string DataBaseName)
    {
        return "DROP DATABASE " + TableQuote(DataBaseName);
    }


    public String InsertWithOutput()
    {
        return "INSERT INTO {0} ({1}) VALUES({2}); SELECT LAST_INSERT_ID()";
    }
    public bool RetrieveOutputWithScalar { get; } = true;

    public String CheckTable(string DataBaseName, string TableName)
    {
        return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=" + IDataSource.AsString(DataBaseName) + " AND TABLE_NAME=" + IDataSource.AsString(TableName);
    }
    public String CheckView(string DataBaseName, string TableName)
    {
        return "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=" + IDataSource.AsString(DataBaseName) + " AND TABLE_NAME=" + IDataSource.AsString(TableName);
    }
    public String CreateTable(string TableName, string FieldName, string FieldType, bool Nullable, string DefaultValue)
    {
        return "CREATE TABLE " + TableQuote( TableName) + " (" +FieldQuote( FieldName) + " " + FieldType + " " + (Nullable ? "NULL" : "NOT NULL") + " " + DefaultValue + ")";
    }
    public String GetTableSchema(string DataBaseName, string TableName)
    {
        return "SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=" + IDataSource.AsString(DataBaseName) + " AND TABLE_NAME=" + IDataSource.AsString(TableName);
    }

    public String AddColumn()
    {
        return "ALTER TABLE {0} ADD COLUMN {1} {2} {3} {4}";
    }
    public String AlterColumn()
    {
        return "ALTER TABLE {0} CHANGE COLUMN {1} {2} {3} {4}";
    }
    public String RemoveColumn()
    {
        return "ALTER TABLE {0} DROP COLUMN {1} ";
    }

    public String FieldType(string SqlType)
    {
        String[] val = SplitFieldType(SqlType);
        if (val[0] == "identity")
        {
            PrimaryKeyCreated = true;
            return "int AUTO_INCREMENT PRIMARY KEY";
        }
        else
        if (val[0] == "bit" || val[0] == "integer")
            return "int";
        else
            if ((val[0] == "varchar" && (val[1] == "max" || Convert.ToInt32(val[1]) > 255)) || val[0] == "ntext")
            return "longtext";
        else
        if (val[0] == "nvarchar")
        {
            return "varchar(" + val[1] + ")";
        }
        else
            return SqlType;
    }
    public String FieldQuote(string SqlType)
    {
        return "`" + SqlType + "`";
    }
    public String TableQuote(string SqlType)
    {
        return "`" + SqlType + "`";
    }

    public String Normalize(string sql)
    {
        String s = sql
            .Replace(" len(", " length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("(len(", "(length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("-len(", "-length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("+len(", "+length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("*len(", "*length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace(",len(", ",length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("/len(", "/length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("=len(", "=length(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("substring(", "SUBSTR(", StringComparison.InvariantCultureIgnoreCase)
            .Replace("space(", "LPAD(' ',", StringComparison.InvariantCultureIgnoreCase)
            .Replace("GETDATE()", "CURDATE()", StringComparison.InvariantCultureIgnoreCase);
            //.Replace("INTEGER", "UNSIGNED", StringComparison.InvariantCultureIgnoreCase);
        return s;
    }
    public String NormalizeParameter(string sql)
    {
        return "@" + sql;
    }
    public String CheckIndex(string IndexName, string TableName)
    {
        if (IndexName == "") IndexName = "PK_" + TableName; // "PRIMARY";
        return "SELECT COUNT(1) IndexIsThere FROM INFORMATION_SCHEMA.STATISTICS WHERE table_schema = DATABASE() AND table_name = " + IDataSource.AsString(TableName) + " AND index_name = " + IDataSource.AsString(IndexName);
    }

    public String CreatePrimaryKey(string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);

        if (PrimaryKeyCreated) 
            return "";
        else
            return "ALTER TABLE " + TableQuote(TableName) + " ADD CONSTRAINT " + TableQuote("PK_"+TableName)+" PRIMARY KEY (" + f + ")";
    }
    public String CreateUniqueIndex(string IndexName, string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);
        return "CREATE UNIQUE INDEX " + TableQuote(IndexName) + " ON " + TableQuote(TableName) + " (" + f + ")";
    }
    public String CreateIndex(string IndexName, string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);
        return "CREATE INDEX " + TableQuote(IndexName) + " ON " + TableQuote(TableName) + " (" + f+ ")";
    }

    public string DropTable(string TableName)
    {
        return "DROP TABLE " + TableQuote(TableName);
    }



    public string DropIndex(string IndexName, string TableName)
    {
        return "DROP INDEX " + TableQuote(IndexName);// + " ON " + TableQuote(TableName);
    }

    public string CheckView(string ViewName)
    {
        throw new NotImplementedException();
    }

    public string CreateView(string ViewName, string Sql)
    {
        string s = Sql.Trim();
        int i = Sql.IndexOf(" ");
        int ifrom = Sql.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase);
        if (i > 0 && ifrom > 0)
        {
            string select = Sql.Substring(0, i + 1);
            string from = Sql.Substring(ifrom);
            string fields = Sql.Substring(i + 1, ifrom - i - 1);
            string[] campi = fields.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string[] oraclecampi = campi.Select(campi => FieldQuote(campi.Trim())).ToArray();
            Sql = select + string.Join(",", oraclecampi) + from;
        }
        return "CREATE VIEW " + TableQuote(ViewName) + " AS " + (Sql);
    }

    public void OpenConnection(IDbConnection connection)
    {
        using (IDbCommand cmd = connection.CreateCommand())
        {
            cmd.CommandText = "SET SESSION sql_mode = 'PIPES_AS_CONCAT,ANSI_QUOTES,IGNORE_SPACE,NO_BACKSLASH_ESCAPES'";
            cmd.ExecuteNonQuery();
        }
    }

    public string Skip(string Sql, int recordsToSkip)
    {
        if (recordsToSkip >= 0)
            return Sql + " LIMIT " + recordsToSkip;
        return Sql;
    }

    public string Take(string Sql, int recordsToTake)
    {
        if (recordsToTake >= 0)
            return Sql + ", " + recordsToTake;
        return Sql;
    }

    public string GetDate()
    {
        return "CURDATE()";
    }

    public string GetDateTime()
    {
        return "CURDATE()";
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







