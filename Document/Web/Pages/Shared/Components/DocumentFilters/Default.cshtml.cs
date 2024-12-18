using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Constants;
using org.apache.poi.sl.usermodel;
using OpenDMS.Domain.Services;
using Microsoft.IdentityModel.Tokens;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Entities.Schemas;

namespace Web.Pages.Shared.Components.DocumentFilters
{
    public class DocumentFilters : ViewComponent
    {
        private readonly IDocumentTypeService documentTypeService;
        private readonly IDataTypeFactory dataTypeFactory;
        private readonly ICustomFieldService customFieldService;
        private readonly ILoggedUserProfile userContext;

        public int ParentId { get; set; } = 0;
        public string DocumentTypeId { get; set; }

        public string NumberLabel { get; set; }
        public string DateLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public UserProfile UserProfile { get; set; }


        public FieldType DocumentNumberType { get; set; } = new();
        public List<FieldType> MetaList { get; set; } = new();
        public List<DocumentTypeField> FieldList { get; set; } = new();
        public DateTime dc1 { get; set; } = DateTime.MinValue;
        public DateTime dc2 { get; set; } = DateTime.MaxValue;
        public string Status { get; set; }
        public string PreservationStatus { get; set; }
        public string SignatureStatus { get; set; }
        public string SendingStatus { get; set; }
        public string SearchType { get; set; }
        public string DateType { get; set; }
        public string PlaceHolder { get; set; }
        public string FreeText { get; set; }
        public string DocumentNumber { get; set; }
        public string Utente { get; set; }
        public string Filters { get; set; } = "";
        public string SelectedCompanies { get; set; }

        public DocumentFilters(
            IDocumentTypeService documentType, 
            IDataTypeFactory dataTypeFactory,
            ICustomFieldService customFieldService,
            ILoggedUserProfile usercontext)
        {
            this.documentTypeService = documentType;
            this.dataTypeFactory = dataTypeFactory;
            this.customFieldService = customFieldService;
            this.userContext = usercontext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserProfile = userContext.Get();
            int n = 0;
            DescriptionLabel = "Descrizione";
            DateLabel = "Data Documento";
            NumberLabel = "Numero Documento";
            DocumentNumberType.DataType = "$$t";
            if (Request.Query.ContainsKey("DateType"))
                DateType = Request.Query["DateType"].ToString();

            if (Request.Query.ContainsKey("dt1"))
                try
                {
                    dc1 = Convert.ToDateTime(Request.Query["dt1"].ToString());
                }
                catch (Exception)
                {
                }
            if (Request.Query.ContainsKey("dt2"))
                try
                {
                    dc2 = Convert.ToDateTime(Request.Query["dt2"].ToString());
                }
                catch (Exception)
                {
                }

            if (dc1 > DateTime.MinValue.Date || (dc2 > DateTime.MinValue.Date  && dc2 < DateTime.MaxValue.Date)) n++;

            if (Request.Query.ContainsKey("FreeTextType"))
                SearchType = Request.Query["FreeTextType"].ToString();
            if (!string.IsNullOrEmpty(SearchType)) n++;

            if (Request.Query.ContainsKey("Status"))
                Status = Request.Query["Status"].ToString();
            else
                Status = "1";
            if (Status != "1") n++;

            if (Request.Query.ContainsKey("Companies"))
                SelectedCompanies = Request.Query["Companies"].ToString();
            else
                SelectedCompanies = "";

            if (Request.Query.ContainsKey("PreservationStatus"))
                PreservationStatus = Request.Query["PreservationStatus"].ToString();
            if (!string.IsNullOrEmpty(PreservationStatus )) n++;
            if (Request.Query.ContainsKey("SignatureStatus"))
                SignatureStatus = Request.Query["SignatureStatus"].ToString();
            if (!string.IsNullOrEmpty(SignatureStatus )) n++;
            if (Request.Query.ContainsKey("SendingStatus"))
                SendingStatus = Request.Query["SendingStatus"].ToString();
            if (!string.IsNullOrEmpty(SendingStatus )) n++;

            if (Request.Query.ContainsKey("Owner"))
                Utente = Request.Query["Owner"].ToString();
            if (!string.IsNullOrEmpty(Utente)) n++;

            if (Request.Query.ContainsKey("nd"))
                DocumentNumber = Request.Query["nd"].ToString();
            if (!string.IsNullOrEmpty(DocumentNumber)) n++;

            if (Request.Query.ContainsKey("FreeText"))
                FreeText = Request.Query["FreeText"].ToString();
            if (!string.IsNullOrEmpty(FreeText)) n++;

            if (Request.Query.ContainsKey("Id") && int.TryParse(Request.Query["Id"], out int id))
                ParentId = id;

            if (Request.Query.ContainsKey("DocumentType") && !String.IsNullOrEmpty(Request.Query["DocumentType"]))
                DocumentTypeId = Request.Query["DocumentType"].ToString();
            if (!string.IsNullOrEmpty(DocumentTypeId) && ParentId != 0) n++;


            PlaceHolder = "Cerca su...";
            if (!String.IsNullOrEmpty(DocumentTypeId))
            {
                var d = await documentTypeService.GetById(DocumentTypeId);
                foreach (var field in d.Fields.Where(f=>!f.Deleted))
                {
                    field.FieldType = await customFieldService.GetById(field.FieldTypeId);
                    // mostro solo i campi ricercabili
                    if (field.FieldType.Searchable)
                    {
                        if (string.IsNullOrEmpty(field.Title))
                            field.Title = field.FieldType.Title;
                        FieldList.Add(field);
                    }
                }
                NumberLabel = d.DocumentNumberLabel;
                DateLabel = d.DocumentDateLabel;
                DescriptionLabel = d.DescriptionLabel;
                DocumentNumberType = await customFieldService.GetById(d.DocumentNumberDataType);
                DocumentNumberType.ControlType = (await dataTypeFactory.Instance(DocumentNumberType.DataType)).ControlType;
                PlaceHolder = "Cerca su "+d.Name+"...";
            }
            else
            {
                if (ParentId == 0)
                {
                    PlaceHolder = "Cerca in tutto l'archivio...";
                }
                foreach (var meta in (await customFieldService.GetAll()).Where(m => m.Searchable))
                {
                    // mostro solo i campi ricercabili
                    if (meta.Searchable)
                    {
                        meta.ControlType = (await dataTypeFactory.Instance(meta.DataType)).ControlType;
                        MetaList.Add(meta);
                    }
                }
            }
            int m = 0;
            if (int.TryParse(Request.Query["metas"].ToString(), out m))
            {
                for (int i = 0; i < m; i++)
                {
                    var mi = Request.Query["mi" + (i + 1)].ToString().ToLowerInvariant();
                    var mv = Request.Query["mv" + (i + 1)];
                    var meta = MetaList.FirstOrDefault(m => m.Id.ToLower() == mi);
                    if (meta != null)
                    {
                        meta.DefaultValue = mv;
                        n++;
                    }
                }
            }
            int f = 0;
            if (int.TryParse(Request.Query["fields"].ToString(), out f))
            {
                for (int i = 0; i < f; i++)
                {
                    var fi = Request.Query["fi" + (i + 1)].ToString().ToLowerInvariant();
                    var fv = Request.Query["fv" + (i + 1)];
                    var field = FieldList.FirstOrDefault(f => f.FieldName.ToLower() == fi);
                    if (field != null)
                    {
                        field.DefaultValue = fv;
                        n++;
                    }
                }
            }
            if (n > 0) Filters = n.ToString();
            return View(this);
        }
    }

}
