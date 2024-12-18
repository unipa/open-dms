using Microsoft.Extensions.Primitives;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Reports.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.BusinessLogic
{
    public interface IQueryBuilder
    {
        IQueryBuilder CountInstance();
        IQueryBuilder AddIndicator(string indicatorId, AggregateType aggregateType = AggregateType.None, bool ascendingSortOrder = true);
        IQueryBuilder RemoveIndicator(string indicatorId);


        IQueryBuilder AddGroupBy(string fieldName, bool ascendingSortOrder = true);
        IQueryBuilder RemoveGroupBy(string fieldName);


        IQueryBuilder AddFilter(string fieldName, string value);
        IQueryBuilder AddFilter(string fieldName, params string[] values);
        IQueryBuilder RemoveFilter(string fieldName);


        IQueryBuilder Take(int numberOfRecords);



    }
}
