using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Text.RegularExpressions;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.BL
{
    public class DocProcessesBL : IDocProcessesBL
    {
        private readonly IConfiguration _config;
        private readonly ISearchService _docSearchService;
        private readonly IDocumentService _docService;
        private readonly IDocumentTypeService _docTypeService;


        public DocProcessesBL(IConfiguration config, ISearchService docSearchService, IDocumentService docService, IDocumentTypeService docTypeService)
        {
            _config = config;
            _docSearchService = docSearchService;
            _docService = docService;
            _docTypeService = docTypeService;
        }

        public async Task<IEnumerable<DocumentType>> GetAllDocTypes(UserProfile u)
        {
            var docs =  await _docTypeService.GetByPermission(u, PermissionType.CanView);
            docs.Add(await GetDocType("*"));
            return docs;
        }
        public async Task<DocumentType> GetDocType(string Id)
        {
            if (Id == "*") Id = "";
            var doc = await _docTypeService.GetById(Id);
            if (Id == "") { doc.Id = "*"; doc.Name = "Documento o Fascicolo Generico"; }
            return doc;
        }
        public async Task<List<int>?> GetProcesses(UserProfile u)
        {
            var Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new List<string> { ((int)(ContentType.Workflow)).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new List<string> { ((int)DocumentStatus.Active).ToString() } });
            return await _docSearchService.Find(Filters, u, 100);
        }

        public async Task<DocumentInfo> GetProcess(int Id, UserProfile u)
        {
            return await _docService.Load(Id, u);
        }
        public async Task<List<DocumentTypeWorkflow_DTO>> GetAllDocumentTypeWorkflow(string TypeId, List<DocumentType> types, List<SelectListItem> ProcessList)
        {
            if (TypeId == "*") { TypeId = "";  };
            var dtw = new List<DocumentTypeWorkflow>();
            dtw = TypeId.Equals("ALL") ? (await _docTypeService.GetAllTypesWorkflows()).ToList() : (await _docTypeService.GetAllWorkflows(TypeId)).ToList();
            return DocumentTypeWorkflowToDTO(dtw, types, ProcessList);
        }

        public async Task<DocumentTypeWorkflow> GetDocumentTypeWorkflow(string TypeId, string EventName)
        {
            if (TypeId == "*") { TypeId = ""; };
            return await _docTypeService.GetWorkflow(TypeId, EventName);
        }
        public async Task<DocumentTypeWorkflow> AggiungiDocumentTypeWorkflow(DocumentTypeWorkflow bd)
        {
            try
            {
                if (bd.DocumentTypeId == "*") bd.DocumentTypeId = "";
                //controllo se esiste già un dtw con gli stessi dtId - eventName
                if (await _docTypeService.GetWorkflow(bd.DocumentTypeId, bd.EventName) != null)
                    throw new Exception($"Esiste già una regola Evento-Processo per questa tipologia documentale associata all'evento {bd.EventName.Replace("Document.", "")}. Modifica o cancella la regola già esistente.");
                if (await _docTypeService.AddWorkflow(bd) > 0)
                    return await _docTypeService.GetWorkflow(bd.DocumentTypeId, bd.EventName);
                else
                    throw new Exception("Salvataggio fallito. Errore sul server.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 
        public async Task<int> ModificaDocumentTypeWorkflow(string TypeId, string EventName, string ProcessId)
        {
            if (TypeId == "*") TypeId = "";

            var bd = await _docTypeService.GetWorkflow(TypeId, EventName);
            if (bd != null)
            {
                bd.ProcessKey = ProcessId;
                var res = await _docTypeService.UpdateWorkflow(bd);
                if (res > 0) return res;
                else throw new Exception("La modifica non è andata a buon fine. Errore sul server.");
            }
            else throw new Exception("La regola Evento-Processo non è stata trovata.");
            //try
            //{
            //    await GetClientAsync();
            //    var bd = await client.ApiAdminDocumentTypeWorkflowGetAsync(TypeId, EventName);
            //    if (bd != null)
            //    {
            //        bd.ProcessId = ProcessId;
            //        var res = await client.ApiAdminDocumentTypeWorkflowPutAsync(bd);
            //        if (res > 0) return res;
            //        else throw new Exception("La modifica non è andata a buon fine. Errore sul server.");
            //    }
            //    else throw new Exception("La regola Evento-Processo non è stata trovata.");
            //}
            //catch (CS.ApiException ex)
            //{
            //    if (ex.StatusCode == 404)
            //        throw new Exception("La regola Evento-Processo non è stata trovata.");
            //    else
            //        throw ex;
            //}
        }

        public async Task<int> EliminaDocumentTypeWorkflow(string TypeId, string EventName)
        {

            if (TypeId == "*") TypeId = "";
            var bd = await _docTypeService.GetWorkflow(TypeId, EventName);
            if (bd != null)
            {
                var res = await _docTypeService.RemoveWorkflow(bd);
                if (res > 0) return res;
                else throw new Exception("L'eliminazione non è andata a buon fine. Errore sul server.");
            }
            else throw new Exception("La regola Evento-Processo non è stata trovata.");

            //try
            //{
            //    await GetClientAsync();
            //    return await client.ApiAdminDocumentTypeWorkflowDeleteAsync(TypeId, EventName);
            //}
            //catch (CS.ApiException ex)
            //{
            //    if (ex.StatusCode == 200)
            //        return "Workflow eliminato con successo.";
            //    if (ex.StatusCode == 404)
            //        return "La regola Evento-Processo non è stata trovata.";
            //    else
            //        throw ex;
            //}
        }

        private List<DocumentTypeWorkflow_DTO> DocumentTypeWorkflowToDTO(List<DocumentTypeWorkflow> dtw, List<DocumentType> types, List<SelectListItem> processList)
        {
            var Dtos = new List<DocumentTypeWorkflow_DTO>();
            foreach (var d in dtw)
            {
                var id = d.DocumentTypeId;
                if (string.IsNullOrEmpty(id)) id = "*";
                var name =   types.FirstOrDefault(t => t.Id.Equals(id)).Name;
                Dtos.Add(new DocumentTypeWorkflow_DTO(
                    id,
                    name,
                    d.EventName,
                    d.ProcessKey,
                    processList
                    ));
            }
            return Dtos;
        }

        //private async Task<Admin.Client> GetClientAsync()
        //{
        //    if (client != null)
        //    {
        //        return client;
        //    }
        //    else
        //    {
        //        var token = await _accessor.HttpContext.GetTokenAsync("access_token");
        //        var httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        //        client = new Admin.Client(AdminBaseUrl.Substring(0, AdminBaseUrl.IndexOf("/api")), httpClient);
        //        return client;
        //    }
        //}
    }
}
