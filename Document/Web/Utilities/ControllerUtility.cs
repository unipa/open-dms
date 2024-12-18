using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using org.apache.james.mime4j.dom.field;
using Web.DTOs;

namespace Web.Utilities
{
    public static class ControllerUtility
    {
        public static string GetFirstExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Il nome del file non può essere vuoto o nullo.", nameof(fileName));

            string[] parts = fileName.Split('.');

            if (parts.Length >= 2)
            {
                string firstExtension = "." + parts[1];
                return firstExtension;
            }
            else
                throw new Exception("Non ci sono estensioni in questo fileName.");
        }

        public static string GetAllExtensions(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Il nome del file non può essere vuoto o nullo.", nameof(fileName));

            string[] parts = fileName.Split('.');

            if (parts.Length >= 2)
            {
                string Extensions = "";

                parts = parts.Skip(1).ToArray();

                foreach (var ext in parts)
                {
                    Extensions += "." + ext;
                }
                return Extensions;
            }
            else
                throw new Exception("Non ci sono estensioni in questo fileName.");
        }

        public static string GetErrorsString(this ModelStateDictionary modelState)
        {
            return string.Join(" ; ", modelState.Values
                                        .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
        }

        public static async Task<FileExplorerModel> GetFilters(HttpRequest Request, string Title, DocumentType docType = null)
        {
            string DocumentType = docType == null ? "" : docType.Id;
            int n = 0;
            var viewId = "doc.all";
            var FileFilters = new FileExplorerModel();
            FileFilters.Request.Filters = new List<SearchFilter>();
            if (Request.Query.ContainsKey("pageIndex"))
                FileFilters.Request.PageIndex = int.Parse(Request.Query["pageIndex"].ToString());
            if (Request.Query.ContainsKey("pageSize"))
                FileFilters.Request.PageSize = int.Parse(Request.Query["pageSize"].ToString());

            string searchType = "";
            if (Request.Query.ContainsKey("FreeTextType"))
                searchType = Request.Query["FreeTextType"].ToString();

            if (Request.Query.ContainsKey("id") && Request.Query["id"] != "0" && Request.Query["id"] != "")
            {
                var id= Request.Query["id"].ToString();
                //if (status != "0" && !String.IsNullOrEmpty(status))
                FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { id } });
                viewId = "doc.folder." + id;
            } 
            else
            if (!String.IsNullOrEmpty(DocumentType))
            {
                //var doctype = await documentTypeService.GetById(DocumentType);
                //                DocumentTypeName = doctype.Name;
                //                GroupType = doctype.GroupName;
                //                var L = await lookupTableService.GetById("CLA", doctype.GroupName);
                //                GroupName = string.IsNullOrEmpty(L.Id) ? Web.Constant.Labels.VirtualFolder : L.Description;
                FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OperatorType.EqualTo, Values = new() { DocumentType } });
                FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)docType.ContentType).ToString() } });
                viewId = "doc.type." + DocumentType;
            }
            if (Request.Query.ContainsKey("Companies"))
            {
                var Companies = Request.Query["Companies"].ToString();
                if (Companies != "0" && !String.IsNullOrEmpty(Companies))
                {
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Company, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { Companies } });
                    n++;
                }
            }

            if (Request.Query.ContainsKey("Status"))
            {
                var status = Request.Query["Status"].ToString();
                if (status != "0" && !String.IsNullOrEmpty(status))
                {
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { status } });
                    n++;
                }
            }
            else
            {
                if (searchType != "5")
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { "1" } });
            }
            if (Request.Query.ContainsKey("FreeText"))
            {
                Title = "Risultati della ricerca";
                string freeText = Request.Query["FreeText"].ToString();
                if (!String.IsNullOrEmpty(freeText)) n++;
                if (searchType == "1")
                {
                    viewId = "doc.content." + DocumentType;
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.FreeText, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { freeText } });
                }
                else
                if (searchType == "3")
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ProtocolNumber, Operator = OpenDMS.Domain.Enumerators.OperatorType.StarstWith, Values = new() { freeText } });
                else
                if (searchType == "4")
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.PDV, Operator = OpenDMS.Domain.Enumerators.OperatorType.StarstWith, Values = new() { freeText } });
                else
                if (searchType == "5")
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Id, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { freeText } });
                else
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.FreeText, Operator = OpenDMS.Domain.Enumerators.OperatorType.Contains, Values = new() { freeText } });
            }
            if (Request.Query.ContainsKey("DateType"))
            {
                n++;
                string FieldName = "";
                switch (Request.Query["DateType"])
                {
                    case "0": FieldName = DocumentColumn.CreationDate; break;
                    case "1": FieldName = DocumentColumn.DocumentDate; break;
                    case "2": FieldName = DocumentColumn.ExpirationDate; break;
                    case "3": FieldName = DocumentColumn.ProtocolDate; break;
//TODO:
                    //case "4": FieldName = DocumentColumn.SendingDate; break;
                    //case "5": FieldName = DocumentColumn.PreservationDate; break;
                    //case "9": FieldName = DocumentColumn.CancellationDate; break;
                    default:
                        break;
                }
                if (!String.IsNullOrEmpty(FieldName))
                {
                    string dt1 = Request.Query["dt1"].ToString();
                    string dt2 = Request.Query["dt2"].ToString();
                    if (dt2.Length < 4 || int.Parse(dt2.Substring(0, 4)) < 1900)
                        dt2 = "2999-12-31";
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = FieldName, Operator = OpenDMS.Domain.Enumerators.OperatorType.In, Values = new() { dt1, dt2 } });
                }
            }
            //TODO: gestire le chiavi
            if (Request.Query.ContainsKey("metas"))
            {
                var m = int.Parse(Request.Query["metas"].ToString());
                for (int i=0; i < m; i++)
                {
                    if (Request.Query.ContainsKey("mi"+(i+1).ToString()))
                    {
                        var meta_id = Request.Query["mi" + (i + 1).ToString()].ToString();
                        var meta_val = Request.Query["mv" + (i + 1).ToString()].ToString();
                        n++;
                        FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Meta+meta_id, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { meta_val } });

                    }

                }
            }
            if (Request.Query.ContainsKey("fields"))
            {
                var m = int.Parse(Request.Query["fields"].ToString());
                for (int i = 0; i < m; i++)
                {
                    if (Request.Query.ContainsKey("fi" + (i + 1).ToString()))
                    {
                        var meta_id = Request.Query["fi" + (i + 1).ToString()].ToString();
                        var meta_val = Request.Query["fv" + (i + 1).ToString()].ToString();
                        n++;
                        FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Field + meta_id, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { meta_val } });

                    }

                }
            }
            foreach (var q in Request.Query)
            {
                if (q.Key.StartsWith("fi_"))
                {
                    var meta_id = q.Key.Substring(3);
                    var meta_val = q.Value.ToString().Split(",").ToList();
                    var meta_op = meta_val.Count > 1 ? OpenDMS.Domain.Enumerators.OperatorType.In : OpenDMS.Domain.Enumerators.OperatorType.EqualTo;
                    n++;
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Field + meta_id, Operator = meta_op, Values = meta_val });
                }
                if (q.Key.StartsWith("mt_"))
                {
                    var meta_id = q.Key.Substring(3);
                    var meta_val = q.Value.ToString().Split(",").ToList();
                    var meta_op = meta_val.Count > 1 ? OpenDMS.Domain.Enumerators.OperatorType.In : OpenDMS.Domain.Enumerators.OperatorType.EqualTo;
                    n++;
                    FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Meta + meta_id, Operator = meta_op, Values = meta_val });
                }
            }

            FileFilters.Title = Title;
            FileFilters.ShowRemoveFromFolder = false;
            FileFilters.ShowDelete = true;
            FileFilters.Request.PageSize = 25;
            FileFilters.Request.PageSize = 0;
            FileFilters.Filters = n;

            FileFilters.Request.ViewId = viewId;
            return FileFilters;
        }
    }
}
