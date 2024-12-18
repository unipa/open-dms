
namespace Elmi.Core.DataAccess.Builders;


public static class Expression
{
    public static SqlExpressionBuilder Create(SqlExpressionBuilder Expression)
    {
        return new SqlExpressionBuilder(Expression);
    }

}
