using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenDMS.Infrastructure.Database.DbContext
{
    public static class DbSetExtension
    {

        public static IQueryable<object[]> Select<TEntity>(this IQueryable<TEntity> DataSet, string[] columns) where TEntity : class
        {
            HashSet<string> Includes = new HashSet<string>();
            ParameterExpression pe = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name);
            List<string> selectedColumns = new List<string>();  
            foreach (var c in columns)
            {
                Expression me = pe;
                var cols = c.Split('.');
                if (cols.Length > 1)
                {
                    string TableToInclude = c.Substring(0, c.LastIndexOf('.'));
                    if (!Includes.Contains(TableToInclude))
                    {
                        DataSet.Include(TableToInclude);
                    }
                    selectedColumns.Add(c);
                } else
                {
                    //if (c.StartsWith("#"))
                    //{
                    //    DataSet.Join().Include(TableToInclude);

                    //}
                    //else
                        selectedColumns.Add(c);
                }
            }
            return DataSet.Select(s => new object[] { selectedColumns.ToArray() });
        }

        public static  IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> DataSet, List<SearchFilter> SearchFilters) where TEntity : class
        {
            //the 'IN' parameter for expression ie T=> condition
            ParameterExpression pe = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name);

            //combine them with and 1=1 Like no expression
            Expression combined = null;

            if (SearchFilters != null)
            {
                foreach (var fieldItem in SearchFilters)
                {
                    //Expression for accessing Fields name property
                    Expression columnNameProperty = GetPropertyExpression(pe, fieldItem.ColumnName);
                    //the name constant to match 
                    Expression e1 = GetFilter(columnNameProperty, fieldItem);
                    if (combined == null)
                        combined = e1;
                    else
                        combined = Expression.And(combined, e1);
                }
            }
            //create and return the predicate
            return DataSet.Where(Expression.Lambda<Func<TEntity, Boolean>>(combined, new ParameterExpression[] { pe }));
        }



        private static Expression GetPropertyExpression(Expression pe, string propertyName)
        {
            var properties = propertyName.Split('.');
            foreach (var property in properties)
                pe = Expression.Property(pe, property);
            return pe;
        }

        private static Expression GetFilter(Expression columnName, SearchFilter f)
        {
            Expression e = null;
            Expression field = Expression.Constant(columnName.Type.Name == "Int32" ? Int32.Parse(f.Values[0]) : f.Values[0]);
            Type c = columnName.Type;
            switch (f.Operator)
            {
                case OperatorType.EqualTo:
                    e = Expression.Equal(columnName, field);
                    break;
                case OperatorType.NotEqualTo:
                    e = Expression.NotEqual(columnName, field);
                    break;
                case OperatorType.GreaterThan:
                    e = Expression.GreaterThan(columnName, field);
                    break;
                case OperatorType.LessThan:
                    e = Expression.LessThan(columnName, field);
                    break;
                case OperatorType.GreaterThanOrEqualTo:
                    e = Expression.GreaterThanOrEqual(columnName, field);
                    break;
                case OperatorType.LessThanOrEqualTo:
                    e = Expression.LessThanOrEqual(columnName, field);
                    break;
                case OperatorType.Contains:
                    MethodInfo mi1 = c.GetMethod("Contains", new Type[] { c });
                    e = Expression.Call(columnName, mi1, field);
                    break;
                case OperatorType.NotContains:
                    MethodInfo mi2 = c.GetMethod("Contains", new Type[] { c });
                    Expression ne = Expression.Call(columnName, mi2, field);
                    e = Expression.IsFalse(ne);
                    break;
                case OperatorType.StarstWith:
                    MethodInfo mi3 = c.GetMethod("StartsWith", new Type[] { c });
                    e = Expression.Call(columnName, mi3, field);

                    break;
                case OperatorType.EndsWith:
                    MethodInfo mi4 = c.GetMethod("EndsWith", new Type[] { c });
                    e = Expression.Call(columnName, mi4, field);
                    break;
                default: // IN
                    Expression fields = Expression.Constant(f.Values.AsEnumerable());
                    var mi5 = typeof(Enumerable).GetRuntimeMethods().Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);
                    e = Expression.Call(columnName, mi5, fields);
                    break;
            }
            return e;
            //                return Expression<Func<T, bool>> lambda =  Expression.Lambda<Func<T, bool>>(call, e);
            //                return                                     Expression.Lambda<Func<T, bool>>(combined, new ParameterExpression[] { pe });
        }
    }
}
