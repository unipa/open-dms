using Elmi.Core.DataAccess.Interfaces.Dialects;
using System.Data;

namespace Elmi.Core.DataAccess.Interfaces;

public interface IDataSource
{
    public static string DefaultDateFormat = "";



    ISqlDialect SqlDialect { get; }
    int TimeOut { get; set; }
    int MaxFails { get; set; }
    bool IgnoreTransaction { get; set; }

    IDbConnection Connection { get; }
    string ProviderName { get; }
    string ConnectionString { get; }


    IDataReader Select(string sql, params IDataParameter[] parameters);
    IDataReader Select(string sql, int Top, params IDataParameter[] parameters);
    IDataReader Select(string sql, int Skip, int Take, params IDataParameter[] parameters);

    IEnumerable<T> Select<T>(string sql, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters);
    IEnumerable<T> Select<T>(string sql, int Top, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters);
    IEnumerable<T> Select<T>(string sql, int Skip, int Take, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters);

    T GetFirst<T>(string sql, params IDataParameter[] parameters);
    T GetFirst<T>(string sql, Func<IDataReader, T> RowMapper, params IDataParameter[] parameters);

    decimal ReadDecimal(string sql, params IDataParameter[] parameters);
    int ReadInteger(string sql, params IDataParameter[] parameters);
    string ReadString(string sql, params IDataParameter[] parameters);
    DateTime? ReadDate(string sql, params IDataParameter[] parameters);

    int Execute(string sql, params IDataParameter[] parameters);
    int Insert(string TableName, params IDataParameter[] parameters);
    int Update(string TableName, int FilterParameters, params IDataParameter[] parameters);
    int Delete(string TableName, params IDataParameter[] parameters);
    int InsertWithOutput(string TableName, string Id, params IDataParameter[] parameters);

    void SaveChanges();
    void RollBack();



    IAsyncEnumerable<T> SelectAsync<T>(string sql, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters);
    IAsyncEnumerable<T> SelectAsync<T>(string sql, int Top, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters);
    IAsyncEnumerable<T> SelectAsync<T>(string sql, int Skip, int Take, Func<IDataReader, int, T> RowMapper, params IDataParameter[] parameters);

    Task<T> GetFirstAsync<T>(string sql, params IDataParameter[] parameters);
    Task<T> GetFirstAsync<T>(string sql, Func<IDataReader, T> RowMapper, params IDataParameter[] parameters);

    Task<decimal> ReadDecimalAsync(string sql, params IDataParameter[] parameters);
    Task<int> ReadIntegerAsync(string sql, params IDataParameter[] parameters);
    Task<string> ReadStringAsync(string sql, params IDataParameter[] parameters);
    Task<DateTime?> ReadDateAsync(string sql, params IDataParameter[] parameters);

    Task<int> ExecuteAsync(string sql, params IDataParameter[] parameters);
    Task<int> InsertAsync(string TableName, params IDataParameter[] parameters);
    Task<int> UpdateAsync(string TableName, int FilterParameters, params IDataParameter[] parameters);
    Task<int> DeleteAsync(string TableName, params IDataParameter[] parameters);

    Task SaveChangesAsync();
    Task RollBackAsync();


    IDataParameter Parameter(string parameterName, DbType parameterType, int Size, object value);
    IDataParameter Parameter(string parameterName, DbType parameterType, object value);
    IDataParameter Parameter(string parameterName, object value);


    string ToDate(DateTime Date);
    string ToDateTime(DateTime Date);

    

    public static string AsString(String Text)
    {
        return !String.IsNullOrEmpty(Text) ? "'" + Text.Replace("'", "''") + "'" : "''";
    }
    public static string AsNumber(Decimal Num)
    {
        return Num.ToString().Replace(".", "").Replace(",", ".");
    }
    public static string AsNumber(Double Num)
    {
        return Num.ToString().Replace(".", "").Replace(",", ".");
    }
    public static string AsNumber(Single Num)
    {
        return Num.ToString().Replace(".", "").Replace(",", ".");
    }
    public static string AsNumber(DateTime Date)
    {
        string DateFormat = "yyyyMMdd";
        return Date.ToString(DateFormat);
    }
    public static string AsDate(DateTime Date)
    {
        string DateFormat = DefaultDateFormat; // Config["Date Format"];
        if (String.IsNullOrEmpty(DateFormat))
            //            format = "yyyy-dd-MM";
            DateFormat = "yyyy-dd-MM"; // System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        if (Date > DateTime.MinValue)
            //   string DateFormat = Config["DB.DateFormat"] ?? "yyyy-MM-dd";
            return "'" + Date.ToString(DateFormat) + "'";
        else
            return "null";
    }
    public static string AsDateTime(DateTime Date)
    {
        string DateFormat = DefaultDateFormat; // Config["Date Format"];
        if (String.IsNullOrEmpty(DateFormat))
            //            format = "yyyy-dd-MM";
            DateFormat = "yyyy-dd-MM"; // System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

        DateFormat += " HH:mm:ss";
        return "'" + Date.ToString(DateFormat).Replace('.', ':') + "'";
    }
    public static string AsBool(Int32 Num)
    {
        return (Num == 0 ? "false" : "true");
    }
    public static string AsBool(String Text)
    {
        return (Text == "" || Text == "N") ? "false" : "true";
    }


}