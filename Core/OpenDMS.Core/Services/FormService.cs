using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDMS.Core.BusinessLogic
{
    public class FormService : IFormService
    {
        private readonly IDocumentService repository;
        private readonly IAppSettingsRepository appSettings;
        private readonly IUserService userService;
        private readonly ILookupTableService lookupTableService;
        private readonly ISearchService searchService;

        public FormService(IDocumentService repository, 
            IAppSettingsRepository appSettings,
            IUserService userService,
            ILookupTableService lookupTableService,
            ISearchService searchService)
        {
            this.repository = repository;
            this.appSettings = appSettings;
            this.userService = userService;
            this.lookupTableService = lookupTableService;
            this.searchService = searchService;
        }

        public async Task<FormSchema> GetByUid(string uid, UserProfile user)
        {
            var formId = await repository.FindByUniqueId(null, uid, ContentType.Form);
            return await GetById(formId, user);
        }

        public async Task<FormSchema> GetById(int documentId, UserProfile user)
        {
            FormSchema F = new FormSchema();
            var document = await repository.Load(documentId, UserProfile.SystemUser());
            if (document != null)
            {
                // recupero l'ultimo form pubblicato
                var image = document.Image; 
                if (image != null)
                {
                    var formData = await repository.GetContent(image.Id);
                    if (formData == null) return F;
                    F = new FormSchema();
                    F.Id = documentId;
                    F.Key = document.ExternalId;
                    F.Version = image.VersionNumber.ToString() + "." + image.RevisionNumber.ToString("00");
                    F.LastUpdate = image.CreationDate;
                    F.Name = document.Description;
                    var fname = image.FileName.ToLower();
                    F.Schema = (Encoding.UTF8.GetString(formData));//.Parse(user, "UserProfile");
                    if (fname.EndsWith(".formjs"))
                        F.FormType = "FORMJS";
                    if (fname.EndsWith(".formio"))
                        F.FormType = "FORMIO";
                    else
                        F.FormType = "HTML";

                    F.Schema = await ParseSettings(F.Schema, user);
                }
            }
            return F;
        }
        private async Task<string> ParseSettings(string value, UserProfile user)
        {
            var text = Regex.Replace(value, @"{{Settings.\((?<exp>.+)\)}}",
            match =>
            {
                var setting = match.Groups[1].Value;
                var v = appSettings.Get(setting).GetAwaiter().GetResult();
                return v;
            });

            text = Regex.Replace(text, @"{{HasRole\((?<exp>.+)\)}}",
            match =>
            {
                var setting = match.Groups[1].Value;
                var v = user.Roles.FirstOrDefault(r => r.Id.Equals(setting, StringComparison.InvariantCultureIgnoreCase));
                if (v == null && setting.Contains("\\"))
                {
                    var role = setting.Split("\\")[0];
                    v = user.GlobalRoles.FirstOrDefault(r => r.Id.Equals(role,StringComparison.InvariantCultureIgnoreCase));
                }
                return v != null ? "1" : "0";
            });
            
            text = Regex.Replace(text, @"{{IsInGroup\((?<exp>.+)\)}}",
            match =>
            {
                var setting = match.Groups[1].Value;
                var v = user.Groups.FirstOrDefault(g => g.TableId.Equals(setting, StringComparison.InvariantCultureIgnoreCase));
                return v != null ? "1" : "0";
            });

            var Roles = string.Join("\n", user.Roles.Where(r => r.Id.Contains("\\")).Select(r => $"<option eid='{r.TableId}' value='{r.Id}'>{r.Annotation}</option>").ToArray());
            var Groups = string.Join("\n", user.Groups.Select(r => $"<option eid='{r.TableId}' value='{r.Id}'>{r.Annotation}</option>").ToArray());
            var Companies = string.Join("\n", user.Companies.Select(r => $"<option aoo='{r.AOO}'  eid='{r.ExternalReference}' value='{r.Id}'>{r.Description}</option>").ToArray());
            
            //TODO: Usare RegEx
            while (text.Contains("{{Lookup:"))
            {
                var i = text.IndexOf("{{Lookup:"); // lungo 9
                var j = text.IndexOf("}}", i);
                if (j < 0) break;
                var tableId = text.Substring(i + 9, j - i - 9);
                StringBuilder options = new StringBuilder();
                if (!string.IsNullOrEmpty(tableId))
                {
                    var lista = await lookupTableService.GetAll(tableId);
                    foreach (var l in lista)
                    {
                        options.AppendLine($"<option value='{l.Id}'>{l.Description}</option>");
                    }
                }
                text = text.Replace("{{Lookup:" + tableId + "}}", options.ToString());
            }

            var RemoteSignService = await userService.GetAttribute(user.userId, UserAttributes.CONST_REMOTESIGNATURE_SERVICE);


            text = text
                    .Parse(user, "UserProfile")
                    .Parse(user.UserInfo.Contact, "User")
                    
                    .Replace("{{FirmaRemota}}", RemoteSignService)
                    .Replace("{{FirmaLocale}}", "")
                    .Replace("{{yyyy-MM-dd}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{YYYY-MM-DD}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{yyyy-mm-dd}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{Today}}", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("{{Date}}", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("{{Now}}", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"))
                    .Replace("{{Roles}}", Roles)
                    .Replace("{{Groups}}", Groups)
                    .Replace("{{Companies}}", Companies)
                    .Replace("{{UserId}}", user.userId)
                    .Replace("{{UserName}}", user.UserInfo.Contact.FullName)
                    .Replace("{{EMail}}", user.UserInfo.Contact.DigitalAddresses.FirstOrDefault(c => c.DigitalAddressType == DigitalAddressType.Email && string.Compare(c.SearchName,"PEC", true)==0)?.Address ?? "")
                    .Replace("{{PEC}}", user.UserInfo.Contact.DigitalAddresses.FirstOrDefault(c => c.DigitalAddressType == DigitalAddressType.Pec && string.Compare(c.SearchName,"Mail", true)==0)?.Address ?? "");

            return text;
        }


        public async Task<FormSchema> GetByTask(TaskItem TI, UserProfile user)
        {
            FormSchema FormInfo = new FormSchema();
            FormInfo.FormType = "HTML";
            FormInfo.Version = "1.00";
            FormInfo.LastUpdate = DateTime.UtcNow;
            //            if (TI.TaskType != TaskType.Message)
            {
                if (String.IsNullOrEmpty(TI.FormKey))
                {
                    FormInfo.Data = "{ \"Justification\" : \"\" }";
                    FormInfo.Key = TI.FormKey;

                    if (!String.IsNullOrEmpty(TI.EventId))
                        FormInfo.Schema = await ParseSettings(FormSchema.GetTemplate(TI.EventId), user);
                    else
                        FormInfo.Schema = await ParseSettings(FormSchema.Default(), user);

                }
                else
                    if (!String.IsNullOrEmpty(TI.FormKey))
                        FormInfo = await GetByUid(TI.FormKey, user);


                FormInfo.Schema = FormInfo.Schema
                        .Parse(TI, "Task")
                        .Replace("{{Attachments}}", String.Join(",", TI.Attachments.Where(a => !a.IsLinked).Select(a => a.DocumentId.ToString())))
                        .Replace("{{Attachments.Count}}", TI.Attachments.Where(a => !a.IsLinked).Count().ToString())
                        .Replace("{{Link}}", String.Join(",", TI.Attachments.Where(a => a.IsLinked).Select(a => a.DocumentId.ToString())))
                        .Replace("{{Links.Count}}", TI.Attachments.Where(a => a.IsLinked).Count().ToString());
                        //.Replace("{{Mandatory}}", TI.NotifyExecution ? "required" : "");
            }
            return FormInfo;
        }

        public async Task<FormSchema> GetByImageId(DocumentInfo document, DocumentVersion image, UserProfile user)
        {
            FormSchema F = null;
            if (document != null)
            {
                // recupero l'ultimo form pubblicato
                var formData = await repository.GetContent(image.ImageId);
                if (formData == null) return null;
                F = new FormSchema();
                F.Id = document.Id;
                F.Key = document.ExternalId;
                F.Version = image.VersionNumber.ToString() + "." + image.RevisionNumber.ToString("00");
                F.LastUpdate = image.CreationDate;
                F.Name = document.Description;
                var fname = image.FileName.ToLower();
                F.Schema = await ParseSettings(Encoding.UTF8.GetString(formData), user);

                if (fname.EndsWith(".formhtml") || fname.EndsWith(".html"))
                    F.FormType = "HTML";
                if (fname.EndsWith(".formjs"))
                    F.FormType = "FORMJS";
                if (fname.EndsWith(".formio"))
                    F.FormType = "FORMIO";
            }
            return F;
        }



        public async Task<List<FormSchema>> GetAll(UserProfile user)
        {
            List<FormSchema> FList = new();
            List<SearchFilter> filters = new();
            filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)OpenDMS.Domain.Enumerators.ContentType.Form).ToString() } });
            filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            foreach (var d in await searchService.Find(filters, user, 512))
            {
                var p = await repository.GetPermission(d, user, PermissionType.CanExecute);
                if (p != null && p.Authorization == AuthorizationType.Granted)
                {
                    var doc = await repository.Get(d);
                    var F = new FormSchema();
                    F.Id = d;
                    F.Key = doc.ExternalId;
                    if (doc.Image != null)
                        F.Version = doc.Image.VersionNumber.ToString() + "." + doc.Image.RevisionNumber.ToString("00");
                    else
                        F.Version = "";
                    F.LastUpdate = doc.CreationDate;
                    F.Name = doc.Description;
                    FList.Add(F);
                }
            }
            return FList;
        }

        //public async Task<bool> Publish(int documentId, int versionNumber, int revisionNumber, UserProfile user)
        //{
        //    var document = await repository.Load(documentId, user);
        //    if (document != null)
        //    {
        //        var image = (await repository.Images(documentId, user)).FirstOrDefault(i => i.VersionNumber == versionNumber && i.RevisionNumber == revisionNumber);
        //        return (await repository.Publish(image.Id, user)) > 0;
        //    }
        //    return false;
        //}


    }
}
