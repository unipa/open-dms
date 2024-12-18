using System.Data.Common;
using System.Data;
using Elmi.Core.DataAccess.Interfaces.Dialects;
using Elmi.Core.DataAccess.Interfaces;

namespace Elmi.Core.DataAccess.Dialects;


public  class Oracle12cDialect : ISqlDialect
{

    public static string ORACLE_SYSTEM_USER = "system";
    public static string ORACLE_SYSTEM_PASSWORD = "oracle";

    private HashSet<string> indexFields = new HashSet<string>();

    private string password = "";
    public Oracle12cDialect()
    {
    }


    public String GetCatalogFromConnectionString(string connectionString)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        String x = CSB["user id"].ToString();
        password = CSB["password"].ToString();
        CSB["user id"] = ORACLE_SYSTEM_USER; // "system";
        CSB["password"] = ORACLE_SYSTEM_PASSWORD;// "oracle";
        return CSB.ConnectionString;
    }

    public String GetDatabaseFromConnectionString(string connectionString)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        return CSB["user id"].ToString();
    }
    public string ChangeDatabaseInConnectionString(string connectionString, string newDatabase)
    {
        DbConnectionStringBuilder CSB = new DbConnectionStringBuilder();
        CSB.ConnectionString = connectionString;
        CSB["user id"] = newDatabase;
        return CSB.ConnectionString;
    }


    public String CheckSchema(string DataBaseName)
    {
       return "SELECT COUNT(*) FROM ALL_USERS WHERE USERNAME=" + IDataSource.AsString(DataBaseName);
    }

    public String CreateSchema(string DataBaseName)
    {
        return "CREATE USER " + (DataBaseName) + " IDENTIFIED BY \"" + (password) + "\" TEMPORARY TABLESPACE TEMP QUOTA UNLIMITED ON USERS;GRANT ALL PRIVILEGES TO " + (DataBaseName);
    }
    public String DropSchema(string DataBaseName)
    {
        return "DROP USER " + DataBaseName+ " CASCADE";
    }


    public String CheckTable(string DataBaseName, string TableName)
    {
        return "SELECT COUNT(*) FROM ALL_TABLES WHERE TABLE_NAME=" + IDataSource.AsString(TableName) + " AND OWNER=" + IDataSource.AsString(DataBaseName);
    }
    public String CheckView(string DataBaseName, string TableName)
    {
        return "SELECT COUNT(*) FROM ALL_VIEWS WHERE VIEW_NAME=" + IDataSource.AsString(TableName) + " AND OWNER=" + IDataSource.AsString(DataBaseName);
    }

    public String CreateTable(string TableName, string FieldName, string FieldType, bool Nullable, string DefaultValue)
    {
        indexFields = new HashSet<string>();
        return "CREATE TABLE " + TableQuote(TableName) + " (" + FieldQuote(FieldName) + " " + FieldType + " " + DefaultValue + " " + (Nullable ? "NULL" : "NOT NULL") + ")";
    }
    public String GetTableSchema(string DataBaseName, string TableName)
    {
        return "SELECT COLUMN_NAME, DATA_TYPE || (CASE WHEN DATA_TYPE='NUMBER' THEN CASE WHEN DATA_PRECISION IS NULL THEN '' ELSE '(' || CAST(DATA_PRECISION AS VARCHAR2(3)) || ',' || CAST(DATA_SCALE AS VARCHAR2(5)) ||')' END ELSE CASE WHEN DATA_TYPE='VARCHAR2' THEN '(' || CAST(DATA_LENGTH AS varchar2(5)) || ')' ELSE '' END END)  AS COLUMN_TYPE, NULLABLE, DATA_DEFAULT FROM ALL_TAB_COLUMNS WHERE OWNER=" + IDataSource.AsString(DataBaseName) + " AND TABLE_NAME=" + IDataSource.AsString(TableName);
    }

    public String AddColumn()
    {
        return "ALTER TABLE {0} ADD {1} {2} {4} {3}";
    }
    public String AlterColumn()
    {
        return "ALTER TABLE {0} MODIFY {1} {2} {4} {3}";
    }
    public String RemoveColumn()
    {
        return "ALTER TABLE {0} DROP COLUMN {1}";
    }

    public String InsertWithOutput()
    {
        return "INSERT INTO {0} ({1}) VALUES({2}) RETURNING {3} INTO :OUTPUT";
    }
    public bool RetrieveOutputWithScalar { get; } = false;

    public String FieldType(string SqlType)
    {
        String[] val = SplitFieldType(SqlType);
        if (val[0] == "identity")
            return "NUMBER GENERATED ALWAYS AS IDENTITY";
        else
          if (val[0] == "bit" || val[0] == "integer")
            return "NUMBER";
        else
          if (val[0] == "decimal")
            return "NUMBER("+val[1]+","+val[2]+")";
        else
            if (((val[0] == "varchar" || val[0] == "nvarchar")&& (val[1] == "max" || Convert.ToInt32(val[1]) > 255)) || val[0]=="ntext")
            return "CLOB";
        else
                if (val[0] == "varchar" || val[0] == "nvarchar")
            return "varchar2(" + val[1] + ")";
        else
                    if (val[0] == "datetime")
            return "date";
        else

            return SqlType;
    }
    public String FieldQuote(string SqlType)
    {
        return "" + SqlType + "";
    }
    public String TableQuote(string SqlType)
    {
        return "" + SqlType + "";
    }

    public String Normalize(string sql)
    {
        String s = sql;
        if (s.TrimStart().ToLower().StartsWith("select ")) s = s.Replace("=''", " IS NULL").Replace("= ''", " IS NULL").Replace("<>''", " IS NOT NULL").Replace("<> ''", " IS NOT NULL");

        s = s
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
            .Replace("GETDATE()", "SYSDATE", StringComparison.InvariantCultureIgnoreCase);

          return s;
    }
    public String NormalizeParameter(string sql)
    {
        return ":" + sql;
    }
    public String CheckIndex(string IndexName, string TableName)
    {
        return "SELECT COUNT(*) FROM user_indexes WHERE index_name = "+ IDataSource.AsString(IndexName)+" AND Table_Name="+ IDataSource.AsString(TableName);
    }

    public String CreatePrimaryKey(string TableName, string[] KeyField)
    {
        var k = KeyField.Select(x => FieldQuote(x)).ToArray();
        var f = string.Join(",", k);
        if (indexFields.Contains(f)) return "";
        indexFields.Add(f);
        return "ALTER TABLE " + TableQuote(TableName) + " ADD CONSTRAINT PK_"+TableName+" PRIMARY KEY (" + f + ")";
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
        return "CREATE INDEX " + TableQuote(IndexName) + " ON " + TableQuote(TableName) + " (" + f + ")";
    }

    public string DropTable(string TableName)
    {
        indexFields = new HashSet<string>();
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
        int ifrom = Sql.IndexOf(" FROM",StringComparison.CurrentCultureIgnoreCase);
        if (i>0 && ifrom > 0)
        {
            string select = Sql.Substring(0, i+1);  
            string from = Sql.Substring(ifrom);   
            string fields = Sql.Substring(i + 1, ifrom - i - 1);
            string[] campi = fields.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string[]oraclecampi = campi.Select(campi => FieldQuote( campi.Trim())).ToArray();
            Sql = select + string.Join(",", oraclecampi) + from;
        }
        return "CREATE VIEW " + TableQuote(ViewName) + " AS " + (Sql);
    }

    public void OpenConnection(IDbConnection connection)
    {
        using (IDbCommand cmd = connection.CreateCommand())
        {
            cmd.CommandText = "alter session set NLS_COMP=LINGUISTIC";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "alter session set NLS_SORT=BINARY_CI";
            cmd.ExecuteNonQuery();
            string format = "yyyy-dd-MM";
            cmd.CommandText = "alter session set NLS_DATE_FORMAT = '" + format + " HH24:MI:SS'";
            cmd.ExecuteNonQuery();
        }
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
        return "SYSDATE";
    }

    public string GetDateTime()
    {
        return "SYSDATE";
    }

    public string ToDate(DateTime Date)
    {
        String format = "yyyy-dd-MM";
        String Text = Date.ToString(format);
        //return "TO_DATE('" + Text + "','" + format + " HH24:MI:SS')";
        return "TO_DATE('" + Text + "','" + format + "')";
    }

    public string ToDateTime(DateTime Date)
    {
        String format = "yyyy-dd-MM";
        String Text = Date.ToString(format + " " + " HH24:MI:SS");// System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern).Replace('.', ':');
        return "TO_DATE('" + Text + "','" + format + "')";
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






