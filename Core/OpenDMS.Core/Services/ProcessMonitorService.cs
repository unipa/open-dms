using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.Infrastructure.Database.Repositories
{
    public class ProcessMonitorService : IProcessMonitorService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IDocumentService documentService;

        public ProcessMonitorService(ITaskRepository taskRepository, IDocumentService documentService)
        {
            this.taskRepository = taskRepository;
            this.documentService = documentService;
        }






        public async Task<List<ProcessSummary_DTO>> GetActiveProcesses(UserProfile u, string businessProcessId = "")
        {
            Dictionary<string, ProcessSummary_DTO> PList = new();

            ProcessFilter filters = new ProcessFilter() { Closed = false, Expired = false, BusinessProcessId = businessProcessId };
            foreach (var p in await taskRepository.GetProcesses(filters))
            {
                try
                {
                    if (!PList.ContainsKey(p.BusinessProcessId)) PList.Add(p.BusinessProcessId, new ProcessSummary_DTO());
                    ProcessSummary_DTO Pi = PList[p.BusinessProcessId];
                    Pi.Description = p.BusinessProcessName;
                    Pi.BusinessProcessId = p.BusinessProcessId;
                    Pi.Users = p.Users;
                    Pi.NotExpired = p.Count;
                    Pi.FromDate = p.FromDate.ToString("dd/MM/yyyy");
                    Pi.ToDate = p.ToDate.ToString("dd/MM/yyyy");

                    var bid = await documentService.GetByDocumentTypeAndNumber ("$DIAGRAM$,$DOCUMENT-DIAGRAM$",p.BusinessProcessId);
                    //TODO: Cercare i documenti tramite Tipo e numero
                    var doc = await documentService.Load(bid, u);
                    if (doc != null)
                    {
                        Pi.ProcessKey = doc.Id;
                        Pi.Version = doc.Image.VersionNumber + "." + doc.Image.RevisionNumber.ToString("00");
                        Pi.Description = doc.Description;
                        Pi.Category = doc.DocumentTypeCategory;
                    }
                }
                catch (Exception)
                {
                }
            }
            filters.Expired = true;
            foreach (var p in await taskRepository.GetProcesses(filters))
            {
                try
                {
                    if (!PList.ContainsKey(p.BusinessProcessId)) PList.Add(p.BusinessProcessId, new ProcessSummary_DTO()
                    {
                        BusinessProcessId = p.BusinessProcessId,
                        Description = p.BusinessProcessName,
                        Users = p.Users,
                        FromDate = p.FromDate.ToString("dd/MM/yyyy"),
                        ToDate = p.ToDate.ToString("dd/MM/yyyy")

                    });
                    ProcessSummary_DTO Pi = PList[p.BusinessProcessId];

                    var bid = await documentService.GetByDocumentTypeAndNumber("$DIAGRAM$,$DOCUMENT-DIAGRAM$", p.BusinessProcessId);
                    //TODO: Cercare i documenti tramite Tipo e numero
                    var doc = await documentService.Load(bid, u);
                    if (doc != null)
                    {
                        Pi.ProcessKey = doc.Id;
                        Pi.Version = doc.Image.VersionNumber + "." + doc.Image.RevisionNumber.ToString("00");
                        Pi.Description = doc.Description;
                        Pi.Category = doc.DocumentTypeCategory;
                    }
                    Pi.Expired = p.Count;
                }
                catch (Exception ex)
                {
                }
            }
            return PList.Values.ToList();
        }

        public async Task<List<ProcessSummary_DTO>> GetClosedProcesses(UserProfile u, DateTime FromDate, DateTime ToDate, string businessProcessId = "")
        {
            Dictionary<string, ProcessSummary_DTO> PList = new();

            ProcessFilter filters = new ProcessFilter() { Closed = true, FromDate = FromDate, ToDate = ToDate, Expired = false, BusinessProcessId = businessProcessId };
            foreach (var p in await taskRepository.GetProcesses(filters))
            {
                try
                {
                    if (!PList.ContainsKey(p.BusinessProcessId)) PList.Add(p.BusinessProcessId, new ProcessSummary_DTO());
                    ProcessSummary_DTO Pi = PList[p.BusinessProcessId];
                    Pi.BusinessProcessId = p.BusinessProcessId;
                    Pi.Users = p.Users;
                    Pi.Description = p.BusinessProcessName;
                    Pi.NotExpired = p.Count;
                    Pi.FromDate = p.FromDate.ToString("dd/MM/yyyy");
                    Pi.ToDate = p.ToDate.ToString("dd/MM/yyyy");

                    var bid = await documentService.GetByDocumentTypeAndNumber("$DIAGRAM$,$DOCUMENT-DIAGRAM$", p.BusinessProcessId);
                    //TODO: Cercare i documenti tramite Tipo e numero
                    var doc = await documentService.Load(bid, u);
                    if (doc != null)
                    {
                        Pi.ProcessKey = doc.Id;
                        Pi.Version = doc.Image.VersionNumber + "." + doc.Image.RevisionNumber.ToString("00");
                        Pi.Description = doc.Description;
                        Pi.Category = doc.DocumentTypeCategory;
                    }
                }
                catch (Exception)
                {
                }
            }
            filters.Expired = true;
            foreach (var p in await taskRepository.GetProcesses(filters))
            {
                try
                {
                    if (!PList.ContainsKey(p.BusinessProcessId)) PList.Add(p.BusinessProcessId, new ProcessSummary_DTO());
                    ProcessSummary_DTO Pi = PList[p.BusinessProcessId];
                    Pi.BusinessProcessId = p.BusinessProcessId;
                    Pi.Users += p.Users;
                    Pi.Description = p.BusinessProcessName;
                    //Pi.Expired = p.Count;
                    Pi.FromDate = p.FromDate.ToString("dd/MM/yyyy");
                    Pi.ToDate = p.ToDate.ToString("dd/MM/yyyy");


                    var bid = await documentService.GetByDocumentTypeAndNumber("$DIAGRAM$,$DOCUMENT-DIAGRAM$", p.BusinessProcessId);
                    //TODO: Cercare i documenti tramite Tipo e numero
                    var doc = await documentService.Load(bid, u);
                    if (doc != null)
                    {
                        Pi.ProcessKey = doc.Id;
                        Pi.Version = doc.Image.VersionNumber + "." + doc.Image.RevisionNumber.ToString("00");
                        Pi.Description = doc.Description;
                        Pi.Category = doc.DocumentTypeCategory;
                    }
                    Pi.Expired = p.Count;
                }
                catch (Exception)
                {
                }
            }
            return PList.Values.ToList();
        }



        public async Task<List<ProcessTaskUser>> GetActiveProcessDetails(UserProfile u, string businessProcessId, bool Expired=false )
        {
            ProcessFilter filters = new ProcessFilter() { Closed = false, Expired = Expired, BusinessProcessId = businessProcessId };
            return await taskRepository.GetProcessDetails(filters);
        }

        public async Task<List<ProcessTaskUser>> GetClosedProcessDetails(UserProfile u, DateTime FromDate, DateTime ToDate, string businessProcessId, bool Expired=false)
        {
            ProcessFilter filters = new ProcessFilter() { Closed = true, FromDate = FromDate, ToDate = ToDate, Expired = Expired, BusinessProcessId = businessProcessId };
            return await taskRepository.GetProcessDetails(filters);
        }


    }
}
