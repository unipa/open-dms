
namespace Elmi.Core.DataAccess.Builders;


public class SqlExpressionBuilder
{
    private string Sql = "";
    public SqlExpressionBuilder() { }
    public SqlExpressionBuilder(SqlExpressionBuilder InnerExpression)
    {
        Sql += "(" + InnerExpression.Build() + ")";
    }

    public static SqlExpressionBuilder Create(SqlExpressionBuilder Expression)
    {
        return new SqlExpressionBuilder(Expression);
    }

    public SqlExpressionBuilder Field(string Field)
    {
        Sql += "(" + Field;
        return this;
    }
    public SqlExpressionBuilder Is(OperatorType OperatorType, params string[] Value)
    {
        Sql += OperatorToString(OperatorType);
        Sql += "'" + Value[0].Replace("'", "''") + "'";
        Sql += ")";
        return this;
    }
    public SqlExpressionBuilder Is(OperatorType OperatorType, params int[] Value)
    {
        Sql += OperatorToString(OperatorType);
        Sql += Value[0];
        Sql += ")";
        return this;
    }
    public SqlExpressionBuilder Is(OperatorType OperatorType, params decimal[] Value)
    {
        Sql += OperatorToString(OperatorType);
        Sql += Value[0];
        Sql += ")";
        return this;
    }
    public SqlExpressionBuilder Is(OperatorType OperatorType, params DateTime[] Value)
    {
        Sql += OperatorToString(OperatorType);
        Sql += Value[0];
        Sql += ")";
        return this;
    }
    public SqlExpressionBuilder Is(OperatorType OperatorType, params bool[] Value)
    {
        Sql += OperatorToString(OperatorType);
        Sql += Value[0];
        Sql += ")";
        return this;
    }

    public SqlExpressionBuilder And()
    {
        Sql += "AND";
        return this;
    }
    public SqlExpressionBuilder Or()
    {
        Sql += "OR";
        return this;
    }
    public SqlExpressionBuilder AndNot()
    {
        Sql += "AND NOT";
        return this;
    }
    public SqlExpressionBuilder OrNot()
    {
        Sql += "OR NOT";
        return this;
    }

    public string Build()
    {
        return Sql;
    }

    protected string OperatorToString(OperatorType Operator)
    {
        switch (Operator)
        {
            case OperatorType.GreaterThan:
                return " > {0}";
            case OperatorType.LessThan:
                return " < {0}";
            case OperatorType.GraterOrEqualTo:
                return " >= {0}";
            case OperatorType.LessOrEqualTo:
                return " <= {0}";
            case OperatorType.Between:
                return " BETWEEN {0} AND {1}";
            case OperatorType.In:
                return " IN {0}";
            case OperatorType.NotIn:
                return " NOT IN {0}";
            case OperatorType.StartsWidth:
                return " LIKE {0}";
            case OperatorType.EndsWidth:
                return " LIKE {0}";
            case OperatorType.Contains:
                return " LIKE {0}";
            default:
                return " = {0}";
        }
    }

    protected string ValueToString(string sql, OperatorType Operator, params object[] Values)
    {

        for (int i = 0; i < Values.Length; i++)
        {
            string V = Values[i].ToString();
            if (Operator == OperatorType.Contains)
                V = "%" + V + "%";
            else if (Operator == OperatorType.StartsWidth)
                V = V + "%";
            else if (Operator == OperatorType.EndsWidth)
                V = "%" + V;
            if (Values[i] is string)
                V = "'"+V.Replace("'","''")+"'";
            else
            if (Values[i] is DateTime)
                V = ((DateTime)Values[i]).ToString("yyyyMMdd");

            sql = sql.Replace($"{i}", V);
        }
        return sql;
    }

}



public enum OperatorType
{
    EqualTo,
    GreaterThan,
    LessThan,
    GraterOrEqualTo,
    LessOrEqualTo,
    Between,
    In,
    NotIn,
    StartsWidth,
    EndsWidth,
    Contains
}
