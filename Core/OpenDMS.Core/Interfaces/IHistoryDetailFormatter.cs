using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.Interfaces
{
    public interface IHistoryDetailFormatter
    {
        List<HistoryDetail> Format(HistoryEntry entry);
    }
}
