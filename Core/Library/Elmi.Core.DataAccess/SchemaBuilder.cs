using Elmi.Core.DataAccess.Interfaces;
using Elmi.Core.DataAccess.Interfaces.Dialects;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Text;


namespace Elmi.Core.DataAccess;

public class SchemaBuilder : ISchemaBuilder
{
    private StringBuilder migration = new StringBuilder();
    private readonly IDataSource dataSource;
    private readonly IDataSource database;
    private readonly ISqlDialect _catalog;
    private NameValueCollection TableSchema = null;
    private Boolean _TableExist = false;
    private string _DataBaseName = "";
    private string _connectionString = "";
    private string _provider = "";
    private string TableName = "";
    private Boolean _exist = false;
    private Boolean JustCreated = false;
    private int IndexCount = 0;



    public SchemaBuilder(string connectionString, string provider)
    {
        this._catalog = SqlDialectProvider.Get(provider);
        this._connectionString = connectionString;
        this._provider = provider;
        this._DataBaseName = _catalog.GetDatabaseFromConnectionString(connectionString);
        this.dataSource = new DataSource(_catalog.GetCatalogFromConnectionString(connectionString), provider);
        this.dataSource.IgnoreTransaction = true;

        this.database = new DataSource(connectionString, provider);
    }


    public Boolean Initialized { get; internal set; }

    public virtual void OnOpen(IDataSource datasource)
    {
    }
    public bool Exist()
    {
        if (!_exist)
        {
            try
            {
                Int64 r = dataSource.ReadInteger(_catalog.CheckSchema(_DataBaseName));
                _exist = r > 0;
                if (_exist)
                {
                    OnOpen(dataSource);
                }
            }
            catch
            {
                _exist = false;
            }
        }
        return _exist;
    }
    public ISchemaBuilder Create()
    {
        if (!Exist())
        {
            String Sql = _catalog.CreateSchema(_DataBaseName);
            if (!String.IsNullOrEmpty(Sql))
            {
                foreach (string s in Sql.Split(';'))
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        dataSource.Execute(s);
                    }
                }
            }
            _exist = true;
            Initialized = true;// Exist();
        }
        return this;
    }

    public ISchemaBuilder Drop()
    {
        if (Exist())
        {
            String Sql = _catalog.DropSchema(_DataBaseName);
            if (!String.IsNullOrEmpty(Sql))
            {
                foreach (string s in Sql.Split(';'))
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        dataSource.Execute(s);
                    }
                }
            }
            _exist = false;
        }
        return this;
    }


    public ISchemaBuilder View(string Name, String Description)
    {
        Int64 r = database.ReadInteger(_catalog.CheckView(_DataBaseName, Name));
        if (r > 0) return this;
        migration.AppendLine(_catalog.CreateView(Name, Description));
        return this;
    }


    public bool TableExist
    {
        get { return _TableExist; }
    }
    public ISchemaBuilder CreateTable(string Name, String Description)
    {
        JustCreated = false;
        TableName = Name.ToLower();
        Int64 r = database.ReadInteger(_catalog.CheckTable(_DataBaseName, TableName));
        _TableExist = r > 0;
        TableSchema = null;
        return this;
    }

    public ISchemaBuilder OnCreate(string SqlScript)
    {
        if (JustCreated)
        {
            foreach (var sql in SqlScript.Split(";", StringSplitOptions.RemoveEmptyEntries))
            {
                migration.AppendLine(_catalog.Normalize( sql));
            }
        }
        return this;
    }

    public ISchemaBuilder DropTable(string Name)
    {
        {

            TableName = Name.ToLower();
            Int64 r = database.ReadInteger(_catalog.CheckTable(_DataBaseName, TableName));
            if (r > 0)
            {
                migration.AppendLine(_catalog.DropTable(TableName));
                if (Name == TableName)
                {
                    _TableExist = false;
                    TableSchema = null;
                }
            }
        }
        return this;
    }

    public ISchemaBuilder WithColumn(string FieldName, String Description, string Type, bool Nullable = true, string DefaultValue = "")
    {
        int r = 0;
        String AFtype = FieldType(Type);
        String guid = (AFtype + "-" + (Type == "IDENTITY" ? "" : Nullable ? "null" : "not null")); // + "-" + DefaultValue.ToLower());
        try
        {
            if (!_TableExist)
            {
                TableSchema = new NameValueCollection();
                string d = "";
                if (!string.IsNullOrEmpty(DefaultValue) && Type != "IDENTITY" && !AFtype.ToLower().Contains("text"))
                    if (AFtype.ToLower().Contains("char") || AFtype.ToLower().Contains("text"))
                        d =/* " CONSTRAINT CTX_" + FieldName + */" DEFAULT '" + DefaultValue + "'";
                    else
                        d =/* " CONSTRAINT CTX_" + FieldName + */" DEFAULT " + DefaultValue.Replace(",", ".");

                migration.AppendLine(_catalog.CreateTable(TableName, FieldName, AFtype, Nullable, d));
                TableSchema[FieldName.ToLower()] = guid;
                _TableExist = true;
                //LoadSchema();
                JustCreated = true;
            }
            else
            {
                LoadSchema();
                String FType = GetSchemaFieldType(FieldName);

                if (!string.IsNullOrEmpty(FType)) // Il Campo Esiste
                {
                    bool ok = (Type != "IDENTITY");
                    if ((FType != guid) && ok)
                    {
                        string d = "";
                        if (!string.IsNullOrEmpty(DefaultValue) && !AFtype.ToLower().Contains("text"))
                            if (AFtype.ToLower().Contains("char") || AFtype.ToLower().Contains("text"))
                                d =/* " CONSTRAINT CTX_" + FieldName + */" DEFAULT '" + DefaultValue + "'";
                            else
                                d =/* " CONSTRAINT CTX_" + FieldName + */" DEFAULT " + DefaultValue.Replace(",",".");
                        migration.AppendLine(string.Format(_catalog.AlterColumn(), _catalog.TableQuote(TableName), _catalog.FieldQuote(FieldName), AFtype, (Nullable ? "" : "NOT NULL"), ""));
                        TableSchema[FieldName.ToLower()] = guid;
                    }
                }
                else
                {
                    string d = "";
                    if (!string.IsNullOrEmpty(DefaultValue) && (Type != "IDENTITY") && !AFtype.ToLower().Contains("text"))
                        if (AFtype.ToLower().Contains("char") || AFtype.ToLower().Contains("text"))
                            d =/* " CONSTRAINT CTX_" + FieldName + */" DEFAULT '" + DefaultValue + "'";
                        else
                            d =/* " CONSTRAINT CTX_" + FieldName + */" DEFAULT " + DefaultValue.Replace(",", ".");
                    migration.AppendLine(string.Format(_catalog.AddColumn(), _catalog.TableQuote(TableName), _catalog.FieldQuote(FieldName), AFtype, (Nullable ? "" : "NOT NULL"), d));
                    TableSchema[FieldName.ToLower()] = guid;
                }
            }
        }
        catch (Exception ex)
        {
            //    Log4NetHelper.GetLogger("APP").Error("ALTER-COLUMN: " + TableName+" ->" + FieldName+" - "+ guid+" ("+DBConnection.Database+")", ex);
        }
        return this;
    }
    public ISchemaBuilder AddString(string FieldName, String Description, int Length, bool Nullable = true, string DefaultValue = "")
    {
        return WithColumn(FieldName, Description, "NVARCHAR(" + (Length <= 0 ? "MAX" : Length.ToString()) + ")", Nullable, DefaultValue);
    }
    public ISchemaBuilder AddInteger(string FieldName, String Description, bool Nullable = true, int DefaultValue = int.MinValue)
    {
        return WithColumn(FieldName, Description, "INTEGER", Nullable, DefaultValue == int.MinValue ? "" : DefaultValue.ToString());
    }
    public ISchemaBuilder AddIdentity(string FieldName, String Description)
    {
        return WithColumn(FieldName, Description, "IDENTITY", false, "");
    }
    public ISchemaBuilder AddDecimal(string FieldName, String Description, int Length, int Mantissa, bool Nullable = false, decimal DefaultValue = 0)
    {
        return WithColumn(FieldName, Description, "DECIMAL(" + Length.ToString() + "," + Mantissa.ToString() + ")", Nullable, ((DefaultValue == 0) && (Nullable)) ? "" : DefaultValue.ToString());
    }
    public ISchemaBuilder AddBoolean(string FieldName, String Description, bool DefaultValue = false)
    {
        return WithColumn(FieldName, Description, "INTEGER", false, DefaultValue ? "1" : "0");
    }
    public ISchemaBuilder AddDate(string FieldName, String Description, bool Nullable = true)
    {
        return WithColumn(FieldName, Description, "DATETIME", Nullable, "");
    }
    public ISchemaBuilder AddText(string FieldName, String Description, bool Nullable = true)
    {
        return WithColumn(FieldName, Description, "NTEXT", Nullable, "");
    }
    public virtual ISchemaBuilder AddPrimaryKey(params string[] FieldName)
    {
        if (!_TableExist) return this;
        Int64 r = database.ReadInteger(_catalog.CheckIndex("", TableName));
        if (r > 0) return this;
        migration.AppendLine(_catalog.CreatePrimaryKey(TableName, FieldName));
        return this;
    }
    public ISchemaBuilder AddIndex(string IndexName, params string[] FieldName)
    {
        if (!_TableExist) return this;
        Int64 r = database.ReadInteger(_catalog.CheckIndex(IndexName, TableName));
        if (r > 0) return this;

        migration.AppendLine(_catalog.CreateIndex(IndexName, TableName, FieldName));
        return this;
    }
    public ISchemaBuilder AddUniqueIndex(string IndexName, params string[] FieldName)
    {
        if (!_TableExist) return this;
        Int64 r = database.ReadInteger(_catalog.CheckIndex(IndexName, TableName));
        if (r > 0) return this; migration.AppendLine(_catalog.CreateUniqueIndex(IndexName, TableName, FieldName));
        return this;
    }
    public virtual ISchemaBuilder RemoveIndex(string IndexName)
    {
        Int64 r = database.ReadInteger(_catalog.CheckIndex(IndexName, TableName));
        if (r > 0) migration.AppendLine(_catalog.DropIndex(IndexName, TableName));
        return this;
    }


    public string[] Columns()
    {
        List<String> campi = new List<string>();

        using (var reader = database.Select(_catalog.GetTableSchema(_DataBaseName, TableName)))
        {
            while (reader.Read())
            {
                String f1 = reader[0].ToString().ToLower();
                campi.Add(f1);
            }
            reader.Close();
        }
        return campi.ToArray();
    }


    protected string GetSchemaFieldType(string FieldName)
    {
        return TableSchema[FieldName.ToLower()] ?? "";
    }
    protected virtual void LoadSchema()
    {
        if (TableSchema == null)
        {
            TableSchema = new NameValueCollection();
            using (var reader = database.Select(_catalog.GetTableSchema(_DataBaseName, TableName)))
            {
                while (reader.Read())
                {
                    String f1 = reader.GetString(0).ToLower();
                    String f2 = reader.GetString(1).ToLower();
                    //if (f2 == "int")
                    //    f2 = "integer";
                    //else
                    if (f2 == "ntext(*)")
                        f2 = "ntext";
                    else
                    if (f2 == "nvarchar(-1)")
                        f2 = "nvarchar(max)";
                    String f3 = reader.GetString(2).ToLower();
                    if (f3 == "false" || f3 == "0" || f3 == "no" || f3 == "n") f3 = "not null"; else f3 = "null";
                    //String f4 = reader[3].ToString().Replace("(","").Replace(")","").Replace("'","");
                    //if (string.IsNullOrEmpty(f4) && f3 == "not null")
                    //    if ((f2 == "integer" || f2 == "decimal" || f2 == "double" || f2 == "float"))
                    //        f4 = "0";
                    //if (f3 == "not null") f3 = "0";

                    String f = f2 + "-" + f3; // + "-" + f4;

                    TableSchema[f1] = f;
                }
            }
        }
    }



    protected virtual String FieldType(string SqlType)
    {
        return _catalog.FieldType(SqlType).ToLower();
    }



    public void Build()
    {
        var sqlcommands = migration.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        //migration.Clear();
        if (Exist())
        //{
        //    Create();
        //    foreach (var row in migration.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        //        dataSource.Execute(row);
        //}
        {
            foreach (var row in sqlcommands)
                database.Execute(row);
            database.SaveChanges();
        }
    }

    public async Task BuildAsync()
    {
        var sqlcommands = migration.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        migration.Clear();
        if (Exist())
        //{
        //    Create();
        //    foreach (var row in migration.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        //        await dataSource.ExecuteAsync(row);
        //}
        {
            foreach (var row in sqlcommands)
                await database.ExecuteAsync(row);
            await database.SaveChangesAsync();
        }

    }


    public override string ToString()
    {
        return migration.ToString();
    }

}
