using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.DataAccess.Builders;

public class SqlBuilder
{

    private string Sql = "";
    private int TableNumber = 0;

    private bool WhereAssigned = false;
    private bool OrderByAssigned = false;
    private bool HavingAssigned = false;

    public SqlBuilder() { }

    public static SqlBuilder Create()
    {
        return new SqlBuilder();
    }

    public SqlBuilder Select(string Fields)
    {
        Sql += "SELECT " + Fields;
        return this;
    }

    public SqlBuilder From(string TableName)
    {
        TableNumber++;
        Sql += " FROM " + TableName + " T" + TableNumber;
        WhereAssigned = false;
        OrderByAssigned = false;
        return this;
    }
    public SqlBuilder LeftJoin(string TableName)
    {
        TableNumber++;
        Sql += " LEFT JOIN " + TableName + " T" + TableNumber;
        WhereAssigned = false;
        OrderByAssigned = false;
        return this;
    }
    public SqlBuilder RightJoin(string TableName)
    {
        TableNumber++;
        Sql += " RIGHT JOIN " + TableName + " T" + TableNumber;
        WhereAssigned = false;
        OrderByAssigned = false;
        return this;
    }
    public SqlBuilder Join(string TableName)
    {
        TableNumber++;
        Sql += " JOIN " + TableName + " T" + TableNumber;
        WhereAssigned = false;
        OrderByAssigned = false;
        return this;
    }
    public SqlBuilder On(SqlExpressionBuilder Expression)
    {
        Sql += " ON " + Expression.Build();
        return this;
    }
    public SqlBuilder On(string Expression)
    {
        Sql += " ON " + Expression;
        return this;
    }
    public SqlBuilder Where(SqlExpressionBuilder Expression)
    {
        Sql += (WhereAssigned ? " WHERE " : " AND ") + Expression.Build();
        WhereAssigned = true;
        return this;
    }
    public SqlBuilder Where(string Expression)
    {
        Sql += (WhereAssigned ? " WHERE " : " AND ") + Expression;
        WhereAssigned = true;
        return this;
    }
    public SqlBuilder GroupBy(params string[] Fields)
    {
        Sql += " GROUP BY " + string.Join(",", Fields);
        return this;
    }
    public SqlBuilder Having(SqlExpressionBuilder Expression)
    {
        Sql += (HavingAssigned ? " AND " : " HAVING ") + Expression.Build();
        HavingAssigned = true;
        return this;
    }
    public SqlBuilder Having(string Expression)
    {
        Sql += (HavingAssigned ? " AND " : " HAVING ") + Expression;
        HavingAssigned = true;
        return this;
    }
    public SqlBuilder OrderBy(string Field, bool Ascending = true)
    {
        Sql += (OrderByAssigned ? "," : " ORDER BY ") + Field + (Ascending ? "" : " DESC");
        OrderByAssigned = true;
        return this;
    }
    public SqlBuilder Skip(int Records)
    {
        return this;
    }
    public SqlBuilder Take(int Records)
    {
        return this;
    }
    public string Build()
    {
        return Sql;
    }
}
