using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenDMS.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.QueryBuilder
{


    public interface IQueryBuilder
    {
        void Clear();

        void Add(string key, string value);


        string Var(string key);

        string Map(string field);
        string Map(string field, AggregateType aggregateType);
        void Filter(string element, OperatorType operatorType, string[] values);
        void Reduce(string Alias, OperatorType OperatorType, string[] Values);
        void Sort(string Alias, bool Ascending = true);
        string[] GetFields(string field);

        IEnumerable<QueryRow> Build();
        int Count();
        decimal Aggregate(AggregateType aggregate, string FieldName);
        void Skip(int Rows);
        void Take(int Rows);
    }
}
