using Elmi.Core.DataAccess.Interfaces;
using Elmi.Core.DataAccess.Interfaces.Dialects;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Elmi.Core.DataAccess;


public class DataSource : IDisposable, IDataSource
{

    private DbProviderFactory dbProviderFactory;
    private IDbConnection connection = null;
    private IDbTransaction transaction = null;

    private bool Disposing = false;
    private bool ignoreTransaction = false;
    private readonly string connectionString;
    private string provider;

    public ISqlDialect SqlDialect { get; internal set; }
    public IDbConnection Connection { get { PrepareConnection(); return connection; } }
    public string ProviderName { get { return provider; } internal set { provider = value; } }
    public string ConnectionString { get { return connectionString; } }

    public Int32 TimeOut { get; set; } = 60;
    public Int32 MaxFails { get; set; } = 5;
    public bool IgnoreTransaction
    {
        get { return ignoreTransaction; }
        set
        {
            ignoreTransaction = value;
            RollBack();
        }
    }

    //public bool IsSQLServer { get; internal set; } = false;
    //public bool IsOracle { get; internal set; } = false;
    //public bool IsMySql { get; internal set; } = false;
    //public bool IsAs400 { get; internal set; } = false;
    //public bool IsPostgre { get; internal set; } = false;
    //public bool IsHANA { get; internal set; } = false;




    public static void FindDateFormat(DataSource dataSource)
    {
        IDataSource.DefaultDateFormat = "yyyy-MM-dd";
        try
        {
            using (IDataReader dr = dataSource.Select("select Cast('2000-31-12' as datetime) as Data"))
            {
                if (dr.Read())
                    IDataSource.DefaultDateFormat = "yyyy-dd-MM";
            }
        }
        catch (Exception)
        {

        }

    }




    public DataSource()
    {
        dbProviderFactory = null;
    }
    public DataSource(string connectionString) : this(connectionString, "")
    {
    }
    public DataSource(string connectionString, string provider)
    {
        this.connectionString = connectionString;
        this.provider = provider;
        Initialize();
    }


    private void Initialize()
    {
        if (String.IsNullOrEmpty(provider))
            this.provider = "System.Data.SqlClient";
        dbProviderFactory = DbProviderFactories.GetFactory(provider);
        string lowerprovider = provider.ToLower();
        SqlDialect = SqlDialectProvider.Get(provider);
        if (SqlDialect == null)
            throw new KeyNotFoundException($"Provider {provider} non trovato");
    }
    private bool PrepareConnection()
    {
        bool ok = false;
        int Failed = 0;
        Exception Ex = null;
        if (connection == null)
        {
            connection = dbProviderFactory.CreateConnection();
            connection.ConnectionString = connectionString;
        }

        do
        {
            try
            {

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlDialect.OpenConnection(connection);
                //if (IsOracle)
                //{
                //    using (IDbCommand cmd = Con.CreateCommand())
                //    {
                //        cmd.CommandText = "alter session set NLS_COMP=LINGUISTIC";
                //        cmd.ExecuteNonQuery();

                //        cmd.CommandText = "alter session set NLS_SORT=BINARY_CI";
                //        cmd.ExecuteNonQuery();
                //        string format = DefaultDateFormat; // Config["Date Format"];
                //        if (String.IsNullOrEmpty(format)) format = "yyyy-dd-MM";
                //        cmd.CommandText = "alter session set NLS_DATE_FORMAT = '" + format + " HH24:MI:SS'";
                //        cmd.ExecuteNonQuery();

                //    }
                //};
                //if (IsMySql)
                //{
                //    using (IDbCommand cmd = connection.CreateCommand())
                //    {
                //        cmd.CommandText = "SET SESSION sql_mode = 'MSSQL'";
                //        cmd.ExecuteNonQuery();
                //    }
                //};
                if (transaction == null && !IgnoreTransaction)
                    transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                ok = true;
            }
            catch (Exception ex)
            {
                Thread.Sleep(1);
                Failed += 1;
                Ex = ex;
            }
        } while ((!ok) && (Failed < MaxFails));
        if (!ok)
        {
            throw Ex;
        }
        return ok;
    }

    private IDbCommand Command(string Sql, params IDataParameter[] parameters)
    {
        IDbCommand com = connection.CreateCommand();
        com.CommandTimeout = TimeOut;
        com.CommandType = CommandType.Text;
        if (!IgnoreTransaction)
            com.Transaction = transaction;
        com.CommandText = SqlDialect.Normalize(Sql);
        foreach (IDataParameter parameter in parameters)
            com.Parameters.Add(parameter);
        return com;
    }


    public string ToDateTime(DateTime Date)
    {
        return SqlDialect.ToDateTime(Date);
    }
    public string ToDate(DateTime Date)
    {
        return SqlDialect.ToDate(Date);
    }

    public IDataParameter Parameter(String parameterName, Object value)
    {
        if (parameterName.StartsWith("@")) parameterName = parameterName.Substring(1);
        IDataParameter parameter = dbProviderFactory.CreateParameter();
        parameter.DbType = DbType.String;
        parameter.ParameterName = SqlDialect.NormalizeParameter(parameterName);
        //if (IsMySql)
        //    value = ((string)value).Replace("\"", "\\\"");
        parameter.Value = value ?? DBNull.Value;
        return parameter;

    }
    public IDataParameter Parameter(String parameterName, DbType parameterType, Object value)
    {
        if (parameterName.StartsWith("@")) parameterName = parameterName.Substring(1);
        IDataParameter parameter = dbProviderFactory.CreateParameter();
        parameter.DbType = parameterType;

        parameter.ParameterName = SqlDialect.NormalizeParameter(parameterName);
        //if (IsMySql && parameterType == DbType.String)
        //    value = ((string)value).Replace("\"", "\\\"");
        parameter.Value = value ?? DBNull.Value;
        return parameter;

    }
    public IDataParameter Parameter(String parameterName, DbType parameterType, Int32 Size, Object value)
    {
        if (parameterName.StartsWith("@")) parameterName = parameterName.Substring(1);
        IDataParameter parameter = dbProviderFactory.CreateParameter();
        parameter.DbType = parameterType;
        parameter.ParameterName = SqlDialect.NormalizeParameter(parameterName);
        //if (IsMySql && parameterType == DbType.String)
        //    value = ((string)value).Replace("\"", "\\\"");
        ((DbParameter)parameter).Size = Size;
        parameter.Value = value;
        return parameter;

    }



    public IDataReader Select(string sql, int Skip, int Take, params IDataParameter[] parameters)
    {

        if (Skip > 0 || Take > 0)
            sql = SqlDialect.Skip(sql, Skip);
        if (Take > 0)
            sql = SqlDialect.Take(sql, Take);
        IDataReader dr = null;
        PrepareConnection();
        IDbCommand com = Command(sql, parameters);
        dr = com.ExecuteReader(CommandBehavior.CloseConnection);
        return dr;
    }
    public IDataReader Select(string sql, int Top, params IDataParameter[] parameters)
    {
        return Select(sql, 0, Top, parameters);
    }
    public IDataReader Select(string sql, params IDataParameter[] parameters)
    {
        return Select(sql, 0, 0, parameters);
    }


    public IEnumerable<T> Select<T>(string sql, int Skip, int Take, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters)
    {
        if (Skip > 0 || Take > 0)
            sql = SqlDialect.Skip(sql, Skip);
        if (Take > 0)
            sql = SqlDialect.Take(sql, Take);
        int rows = 0;
        PrepareConnection();
        using (IDbCommand com = Command(sql, parameters))
        {
            using (var dr = com.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr != null && dr.Read())
                {
                    rows++;
                    var row = RowMapper(dr, rows);
                    yield return row;
                }
            }
        }
    }
    public IEnumerable<T> Select<T>(string sql, int Top, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters)
    {
        return Select<T>(sql, 0, Top, RowMapper, parameters);
    }
    public IEnumerable<T> Select<T>(string sql, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters)
    {
        return Select<T>(sql, 0, 0, RowMapper, parameters);
    }


    public T GetFirst<T>(string sql, Func<IDataReader, T> RowMapper, params IDataParameter[] parameters)
    {
        PrepareConnection();
        using (IDbCommand com = Command(sql, parameters))
        {
            using (var dr = com.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr != null && dr.Read())
                {
                    var row = RowMapper(dr);
                    return row;
                }
            }
        }
        return default(T);
    }
    public T GetFirst<T>(string sql, params IDataParameter[] parameters)
    {
        T r = default(T);
        PrepareConnection();
        using (IDbCommand com = Command(SqlDialect.Normalize(sql), parameters))
        {
            //com.CommandTimeout = TimeOut;
            object o = com.ExecuteScalar();
            r = (T)Convert.ChangeType(o, typeof(T));
        }
        return r;
    }


    public string ReadString(string sql, params IDataParameter[] parameters)
    {
        return GetFirst<string>(sql, parameters);
    }
    public int ReadInteger(string sql, params IDataParameter[] parameters)
    {
        return GetFirst<int>(sql, parameters);
    }
    public Decimal ReadDecimal(string sql, params IDataParameter[] parameters)
    {
        return GetFirst<decimal>(sql, parameters);
    }
    public DateTime? ReadDate(string sql, params IDataParameter[] parameters)
    {
        return GetFirst<DateTime>(sql, parameters);
    }





    public Int32 Execute(string sql, params IDataParameter[] parameters)
    {

        Int32 r = 0;
        if (string.IsNullOrEmpty(sql)) return 0;
        PrepareConnection();
        using (IDbCommand com = Command(sql, parameters))
        {
            r = com.ExecuteNonQuery();
        }
        return r;
    }
    public Int32 Insert(string TableName, params IDataParameter[] parameters)
    {
        string sql = "INSERT INTO {0} ({1}) VALUES({2})";
        string fields = "";
        string values = "";
        for (int i = 0; i < parameters.Length; i++)
        {
            string pname = parameters[i].ParameterName;
            values += (string.IsNullOrEmpty(values) ? "" : ",") + pname;
            pname = pname.Substring(1);
            fields += (string.IsNullOrEmpty(fields) ? "" : ",") + SqlDialect.FieldQuote(pname);
        }
        sql = String.Format(sql, SqlDialect.TableQuote(TableName), fields, values);
        return Execute(sql, parameters);
    }

    public Int32 InsertWithOutput(string TableName, string Id, params IDataParameter[] parameters)
    {
        var useScalar = SqlDialect.RetrieveOutputWithScalar;
        string sql = SqlDialect.InsertWithOutput(); // "INSERT INTO {0} ({1}) VALUES({2}) RETURNING ID INTO :IDENTITY";
        string fields = "";
        string values = "";
        for (int i = 0; i < parameters.Length; i++)
        {
            string pname = parameters[i].ParameterName;
            values += (string.IsNullOrEmpty(values) ? "" : ",") + pname;
            pname = pname.Substring(1);
            fields += (string.IsNullOrEmpty(fields) ? "" : ",") + SqlDialect.FieldQuote(pname);
        }
        sql = String.Format(sql, SqlDialect.TableQuote(TableName), fields, values, Id);
        int r = 0;
        if (useScalar)
            r = ReadInteger(sql, parameters.ToArray());
        else
        {
            var p = Parameter(Id, DbType.Int32, null);
            p.Direction = ParameterDirection.Output;
            var plist = parameters.ToList();
            plist.Add(p);
            r = Execute(sql, plist.ToArray());
            r = r > 0 ? (int)p.Value : 0;
        }
        return r; ;
    }

    public Int32 Update(string TableName, int FilterParameters, params IDataParameter[] Parameters)
    {
        string sql = "UPDATE {0} SET {1} {2}";
        StringBuilder fields = new StringBuilder();
        StringBuilder wheresql = new StringBuilder();
        if (FilterParameters > 0) wheresql.Append("WHERE ");
        int ParamCount = Parameters.Length;
        bool primo = true;
        bool primoWhere = true;
        for (int i = 0; i < ParamCount; i++)
        {
            string pname = Parameters[i].ParameterName;
            string name = pname.Substring(1);
            if (i < ParamCount - FilterParameters)
            {
                if (primo)
                    primo = false;
                else
                    fields.Append(",");
                fields.Append(SqlDialect.FieldQuote(name) + "=" + pname);
            }
            else
            {
                if (primoWhere)
                    primoWhere = false;
                else
                    wheresql.Append(" AND ");
                wheresql.Append(SqlDialect.FieldQuote(name) + "=" + pname);
            }
        }

        sql = String.Format(sql, SqlDialect.TableQuote(TableName), fields.ToString(), wheresql.ToString());
        return Execute(sql, Parameters);
    }
    public Int32 Delete(string TableName, params IDataParameter[] Parameters)
    {
        string sql = "DELETE FROM {0} {1}";
        StringBuilder wheresql = new StringBuilder();
        if (Parameters.Length > 0) wheresql.Append("WHERE ");
        int ParamCount = Parameters.Length;
        bool primoWhere = true;
        for (int i = 0; i < ParamCount; i++)
        {
            string pname = Parameters[i].ParameterName;
            string name = pname.Substring(1);
            if (primoWhere)
                primoWhere = false;
            else
                wheresql.Append(" AND ");
            wheresql.Append(SqlDialect.FieldQuote(name) + "=" + pname);
        }

        sql = String.Format(sql, SqlDialect.TableQuote(TableName), wheresql.ToString());
        return Execute(sql, Parameters);
    }




    #region Static Methods


    #endregion




    public void Dispose()
    {
        if (!Disposing)
        {
            Disposing = true;
            SaveChanges();
        }
    }
    public void SaveChanges()
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            if (transaction != null)
            {
                if (transaction.Connection != null)
                {

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                    }
                    transaction.Dispose();
                    transaction = null;
                }
            };

            connection.Close();
            connection = null;
        }
    }
    public void RollBack()
    {
        if (transaction != null)
        {
            try
            {
                transaction.Rollback();
            }
            catch { }
            transaction = null;
        };
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
            connection = null;
        }
    }







    public async IAsyncEnumerable<T> SelectAsync<T>(string sql, int Skip, int Take, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters)
    {
        if (Skip > 0 || Take > 0)
            sql += " OFFSET " + Skip + " ROWS";
        if (Take > 0)
            sql += " FETCH NEXT " + Take + " ROWS ONLY";
        IDataReader dr = null;
        PrepareConnection();
        int rows = 0;
        using (DbCommand com = (DbCommand)Command(sql, parameters))
        {
            using (dr = await com.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (dr != null && dr.Read())
                {
                    rows++;
                    var row = RowMapper(dr, rows);
                    yield return row;
                }
            }
        }
    }
    public IAsyncEnumerable<T> SelectAsync<T>(string sql, int Top, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters)
    {
        return SelectAsync<T>(sql, 0, Top, RowMapper, parameters);
    }
    public IAsyncEnumerable<T> SelectAsync<T>(string sql, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters)
    {
        return SelectAsync(sql, 0, 0, RowMapper, parameters);
    }


    public async Task<T> GetFirstAsync<T>(string sql, Func<IDataReader, T> RowMapper, params IDataParameter[] parameters)
    {
        PrepareConnection();
        using (DbCommand com = (DbCommand)Command(sql, parameters))
        {
            using (var dr = await com.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (dr != null && dr.Read())
                {
                    var row = RowMapper(dr);
                    return row;
                }
            }
        }
        return default(T);
    }
    public async Task<T> GetFirstAsync<T>(string sql, params IDataParameter[] parameters)
    {
        T r = default(T);
        PrepareConnection();
        using (DbCommand com = (DbCommand)Command(sql, parameters))
        {
            //com.CommandTimeout = TimeOut;
            object o = await com.ExecuteScalarAsync();
            r = (T)o;
        }
        return r;
    }


    public async Task<string> ReadStringAsync(string sql, params IDataParameter[] parameters)
    {
        return await GetFirstAsync<string>(sql, parameters);
    }
    public async Task<Int32> ReadIntegerAsync(string sql, params IDataParameter[] parameters)
    {
        return await GetFirstAsync<int>(sql, parameters);
    }
    public async Task<Decimal> ReadDecimalAsync(string sql, params IDataParameter[] parameters)
    {
        return await GetFirstAsync<decimal>(sql, parameters);
    }
    public async Task<DateTime?> ReadDateAsync(string sql, params IDataParameter[] parameters)
    {
        return await GetFirstAsync<DateTime>(sql, parameters);
    }




    public async Task<Int32> ExecuteAsync(string sql, params IDataParameter[] parameters)
    {

        Int32 r = 0;

        PrepareConnection();
        using (DbCommand com = (DbCommand)Command(sql, parameters))
        {
            r = await com.ExecuteNonQueryAsync();
        }
        return r;
    }
    public async Task<Int32> InsertAsync(string TableName, params IDataParameter[] parameters)
    {
        string sql = "INSERT INTO {0} ({1}) VALUES({2})";
        string fields = "";
        string values = "";
        for (int i = 0; i < parameters.Length; i++)
        {
            string pname = parameters[i].ParameterName;
            values += (string.IsNullOrEmpty(values) ? "" : ",") + pname;
            if (pname.StartsWith("@")) pname = pname.Substring(1);
            fields += (string.IsNullOrEmpty(fields) ? "" : ",") + SqlDialect.FieldQuote(pname);
        }
        sql = String.Format(sql, SqlDialect.TableQuote(TableName), fields, values);
        return await ExecuteAsync(sql, parameters);
    }
    public async Task<Int32> UpdateAsync(string TableName, int FilterParameters, params IDataParameter[] Parameters)
    {
        string sql = "UPDATE {0} SET {1} {2}";
        StringBuilder fields = new StringBuilder();
        StringBuilder wheresql = new StringBuilder();
        if (FilterParameters > 0) wheresql.Append("WHERE ");
        int ParamCount = Parameters.Length;
        bool primo = true;
        bool primoWhere = true;
        for (int i = 0; i < ParamCount; i++)
        {
            string pname = Parameters[i].ParameterName;
            string name = pname.Substring(1);
            if (i < ParamCount - FilterParameters)
            {
                if (primo)
                    primo = false;
                else
                    fields.Append(",");
                fields.Append(SqlDialect.FieldQuote(name) + "=" + pname);
            }
            else
            {
                if (primoWhere)
                    primoWhere = false;
                else
                    wheresql.Append(" AND ");
                wheresql.Append(SqlDialect.FieldQuote(name) + "=" + pname);
            }
        }

        sql = String.Format(sql, SqlDialect.TableQuote(TableName), fields.ToString(), wheresql.ToString());
        return await ExecuteAsync(sql, Parameters);
    }
    public async Task<Int32> DeleteAsync(string TableName, params IDataParameter[] Parameters)
    {
        string sql = "DELETE FROM {0} {1}";
        StringBuilder wheresql = new StringBuilder();
        if (Parameters.Length > 0) wheresql.Append("WHERE ");
        int ParamCount = Parameters.Length;
        bool primoWhere = true;
        for (int i = 0; i < ParamCount; i++)
        {
            string pname = Parameters[i].ParameterName;
            string name = pname.Substring(1);
            if (primoWhere)
                primoWhere = false;
            else
                wheresql.Append(" AND ");
            wheresql.Append(SqlDialect.FieldQuote(name) + "=" + pname);
        }

        sql = String.Format(sql, SqlDialect.TableQuote(TableName), wheresql.ToString());
        return await ExecuteAsync(sql, Parameters);
    }

    public async Task SaveChangesAsync()
    {
        if (transaction != null)
        {
            transaction.Commit();
            transaction = null;
        };
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
            connection = null;
        }
    }
    public async Task RollBackAsync()
    {
        if (transaction != null)
        {
            transaction.Rollback();
            transaction = null;
        };
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
            connection = null;
        }
    }


}


