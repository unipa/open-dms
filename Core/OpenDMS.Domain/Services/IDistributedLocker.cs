using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Services
{
    public interface IDistributedLocker
    {
        bool Acquire(string objectId, string recordId, string serviceId, DateTime expirationTime);
        bool Update(string objectId, string recordId, string serviceId, DateTime expirationTime);
        bool Release(string objectId, string recordId, string serviceId);

    }
}
