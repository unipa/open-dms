using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class TextQueryField : QueryField
    {

        public TextQueryField(QueryModel modelId, string id, string title, string description, int size = 0, bool resizable = true, bool sortable = true)
            : base(modelId, id, ColumnDataType.Text, title, description, modelId.ToString(), size, resizable, sortable)
        {
        }


        public TextQueryField(QueryModel modelId, string id, string title, string description, int size = 0, bool sortable = true)
            : base(modelId, id, ColumnDataType.Text, title, description, modelId.ToString(), size, true, sortable)
        {
        }



    }
}
