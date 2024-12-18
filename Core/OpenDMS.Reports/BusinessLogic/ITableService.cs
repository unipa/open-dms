using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.BusinessLogic
{
    public interface ITableService
    {
        /// <summary>
        /// Identificativo Univoco del gestore.
        /// Questo ID sarà utilizzato come prefisso per le query di questo gestore
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Nome visualizzato del gestore
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Nome della tabella sottostante
        /// </summary>
        string TableName { get; }


    }
}
