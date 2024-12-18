using OpenDMS.Domain.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Repositories
{
    public interface IProcessInstanceRepository
    {
        Task Delete(int documentId);
        Task<List<ProcessInstance>> GetByDocumentId(int documentId);
        Task<List<ProcessInstance>> GetByProcessDefinitionId(int processDefinitionId);
        Task<ProcessInstance> GetByProcessInstanceId(string processInstanceId);
        Task<List<ProcessInstance>> GetByProcessKey(string processKey);
        Task Start(ProcessInstance processInstance);
        Task Stop(ProcessInstance processInstance);
    }
}
