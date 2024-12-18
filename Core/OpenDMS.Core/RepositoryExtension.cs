using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Core.Builders;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DataTypes;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Managers;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Core
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection Services)
        {
            Services.AddTransient<IDataTypeManager, OrganizationProfileFieldType>();
            Services.AddTransient<IDataTypeManager, UserProfileFieldType>();
            Services.AddTransient<IDataTypeManager, GroupProfileFieldType>();
            Services.AddTransient<IDataTypeManager, RoleProfileFieldType>();
            Services.AddTransient<IDataTypeManager, TextFieldType>();
            Services.AddTransient<IDataTypeManager, ParagraphFieldType>();
            Services.AddTransient<IDataTypeManager, DateFieldType>();
            Services.AddTransient<IDataTypeManager, DatabaseFieldType>();
            Services.AddTransient<IDataTypeManager, MailAddressFieldType>();
            Services.AddTransient<IDataTypeManager, InternalTableFieldType>();
            Services.AddTransient<IDataTypeManager, RestAPIFieldType>();
            Services.AddTransient<IDataTypeFactory, DataTypeFactory>();
            Services.AddTransient<IDataTypeManager, IntegerFieldType>();
            Services.AddTransient<IDataTypeManager, NumberFieldType>();
            Services.AddTransient<IDataTypeManager, JsonFieldType>();

            Services.AddTransient<IProcessMonitorService, ProcessMonitorService>();
            Services.AddTransient<IUserService, UserService>();
            
            Services.AddTransient<IACLService, ACLService>();
            Services.AddTransient<ICompanyService, CompanyService>();
            Services.AddTransient<IUserFilterService, UserFiltersService>();
            Services.AddTransient<ICustomFieldService, CustomFieldService>();
            Services.AddTransient<IDocumentTypeService, DocumentTypeService>();
            Services.AddTransient<ILookupTableService, LookupTableService>();
            Services.AddTransient<IRoleService, RoleService>();
            Services.AddTransient<IOrganizationUnitService, OrganizationUnitService>();
            Services.AddTransient<IDocumentService, DocumentService>();
//            Services.AddTransient<IApplicationAuthorizationService, AuthorizationService>();
            Services.AddTransient<IUserTaskService, UserTaskService>();
            Services.AddTransient<IFormService, FormService>();
            Services.AddTransient<IMailboxService, MailboxService>();

            Services.AddTransient<IViewServiceResolver, ViewServiceResolver>();
            Services.AddTransient<IViewManager, ViewManager>();

            Services.AddTransient<ISearchService, TaskListSearchService>();
            Services.AddTransient<ISearchService, DocumentSearchService>();

            Services.AddTransient<IUpdateIdentityService, UpdateIdentityService>();
            Services.AddTransient<ILoggedUserProfile, LoggedUserProfile>();
            Services.AddTransient<IMessageBuilder, MessageBuilder>();

            //Services.AddTransient<IProcessDataService, ProcessDataService>();


            return Services;
        }

        public static async Task<IApplicationBuilder> AddTemplates(this IApplicationBuilder app)
        {

            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>().GetDbContext();
            var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();
            var customfields = scope.ServiceProvider.GetRequiredService<ICustomFieldService>();
            var dataTypeFactory = scope.ServiceProvider.GetRequiredService<IDataTypeFactory>();
            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
            //var _companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
            foreach (var f in dataTypeFactory.GetAllTypes())
            {
                if (await customfields.GetById(f.Id) == null)
                {
                    await customfields.Insert(f);
                }
            }
            
            //var company = await _companyRepository.GetById(0);
            //if (company == null)
            //{
            //    company = new Domain.Entities.Settings.Company() { Id = 0, Description = "Ambiente Condiviso", OffLine = false };
            //    await _companyRepository.Insert(company);
            //}


            var ProcessFolderId = await documentService.FindByUniqueId(null, "$WORKFLOW-FOLDER$", ContentType.Folder);
            if (ProcessFolderId <= 0)
            {
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Archivio Processi", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$WORKFLOW-FOLDER$", Icon = "fa fa-cogs", ContentType = ContentType.Folder, Owner = SpecialUser.SystemUser, Authorize = "2admin,2workflow-architect" };
                ProcessFolderId = await documentService.Create(p, UserProfile.SystemUser());
            }
            var ProcessTemplateFolderId = await documentService.FindByUniqueId(null, "$WORKFLOW-TEMPLATE-FOLDER$", ContentType.Folder);
            if (ProcessTemplateFolderId <= 0)
            {
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Modelli", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$WORKFLOW-TEMPLATE-FOLDER$", Icon = "fa fa-edit", ContentType = ContentType.Folder, Owner = SpecialUser.SystemUser, Authorize = "2admin", FolderId = ProcessFolderId };
                ProcessTemplateFolderId = await documentService.Create(p, UserProfile.SystemUser());
            }

            var ReportFolderId = await documentService.FindByUniqueId(null, "$REPORT-FOLDER$", ContentType.Folder);
            if (ReportFolderId <= 0)
            {
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Archivio Reports", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$REPORT-FOLDER$", Icon = "fa fa-pie-chart", ContentType = ContentType.Folder, Owner = SpecialUser.SystemUser, Authorize = "2admin" };
                ReportFolderId = await documentService.Create(p, UserProfile.SystemUser());
            }

            var HelpFolderId = await documentService.FindByUniqueId(null, "$HELP-FOLDER$", ContentType.Folder);
            if (HelpFolderId <= 0)
            {
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "", Description = "Guide Online", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$HELP-FOLDER$", Icon = "fa fa-life-ring", ContentType = ContentType.Folder, Owner = SpecialUser.SystemUser, Authorize = "2admin,2user" };
                ReportFolderId = await documentService.Create(p, UserProfile.SystemUser());
            }

            var NewDiagramId = await documentService.FindByUniqueId(null, "$NEW-DIAGRAM-TEMPLATE$", ContentType.Form);
            if (NewDiagramId <= 0)
            {
                FileContent FC = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "wwwroot", "templates", "NewProcess.html"))),
                    FileName = "NewProcess.html",
                };
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Modello Creazione Diagrammi", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$NEW-DIAGRAM-TEMPLATE$", Icon = "fa fa-cogs", ContentType = ContentType.Form, Owner = SpecialUser.SystemUser, Authorize = "2admin,2workflow-architect", Content = FC, FolderId= ProcessTemplateFolderId };
                NewDiagramId = await documentService.Create(p, UserProfile.SystemUser());
            }

            var NewFormId = await documentService.FindByUniqueId(null, "$NEW-FORM-TEMPLATE$", ContentType.Form);
            if (NewFormId <= 0)
            {
                FileContent FC = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "wwwroot", "templates", "NewForm.html"))),
                    FileName = "NewForm.html",
                };
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Modello Creazione Form", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$NEW-FORM-TEMPLATE$", Icon = "fa fa-table", ContentType = ContentType.Form, Owner = SpecialUser.SystemUser, Authorize = "2admin,2workflow-architect", Content = FC, FolderId = ProcessTemplateFolderId };
                NewFormId = await documentService.Create(p, UserProfile.SystemUser());
            }

            var NewDMNId = await documentService.FindByUniqueId(null, "$NEW-DMN-TEMPLATE$", ContentType.Form);
            if (NewDMNId <= 0)
            {
                FileContent FC = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "wwwroot", "templates", "NewDMN.html"))),
                    FileName = "NewDMN.html",
                };
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Modello Creazione DMN", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$NEW-DMN-TEMPLATE$", Icon = "fa fa-question-circle", ContentType = ContentType.Form, Owner = SpecialUser.SystemUser, Authorize = "2admin,2workflow-architect", Content = FC, FolderId = ProcessTemplateFolderId };
                NewDMNId = await documentService.Create(p, UserProfile.SystemUser());
            }

            var NewTemplateId = await documentService.FindByUniqueId(null, "$NEW-TEMPLATE-TEMPLATE$", ContentType.Form);
            if (NewTemplateId <= 0)
            {
                FileContent FC = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "wwwroot", "templates", "NewTemplate.html"))),
                    FileName = "NewTemplate.html",
                };
                var p = new CreateOrUpdateDocument() { DocumentTypeId = null, CompanyId = 0, ACLId = "$WORKFLOW$", Description = "Modello Creazione Template", DocumentDate = DateTime.MinValue, DocumentNumber = "", ExternalId = "$NEW-TEMPLATE-TEMPLATE$", Icon = "fa fa-edit", ContentType = ContentType.Form, Owner = SpecialUser.SystemUser, Authorize = "2admin,2workflow-architect", Content = FC, FolderId = ProcessTemplateFolderId };
                NewTemplateId = await documentService.Create(p, UserProfile.SystemUser());
            }
            var role = await roleService.GetById("FAQAdmin");
            if (role == null)
            {
                await roleService.Create(new Role() { Id = "FAQAdmin", RoleName = "Amministratore FAQ" });
            }
            return app;

        }
    }
}
