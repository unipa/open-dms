using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.PdfManager;

namespace OpenDMS.Core.Interfaces
{
    public interface IDocumentService
    {
        Task<int> GetUserFolder(UserProfile userId);
        Task<int> FindInFolderByUniqueId(int folderId, string externalId, ContentType contentType);
        Task<int> FindByUniqueId(string docType, string externalId, ContentType contentType);

        Task<DocumentField> AddField(DocumentInfo document, DocumentTypeField DocumentTypeField, string DefaultValue);
        Task<DocumentField> UpdateField(DocumentInfo document, DocumentTypeField DocumentTypeField, string Value);

        Task<DocumentVersion> GetPublished(int documentId);
        IEnumerable<int> GetContentsToPreview();
        //IEnumerable<int> GetContentsToPreserve();
        IEnumerable<int> GetContentsToIndex();

        Task<int> GetByDocumentTypeAndNumber(string documentTypeId, string documentNumber);

        Task<DocumentImage> AddContent(int documentId, UserProfile userInfo, FileContent content, bool createNewVersion = true, bool checkIn = false);
//        Task AddPreview(int documentId, int imageId, UserProfile userInfo, FileContent content);
        Task<bool> AddLink(int documentId, int AttachmentId, UserProfile userInfo, bool IsAttachment = false);

        Task SetPermission(int documentId, UserProfile userInfo, string ProfileId, ProfileType ProfileType, string permissionId, AuthorizationType authorization, bool RaiseEvents = true);
        Task SetPermissions(int documentId, UserProfile userInfo, string ProfileId, ProfileType ProfileType, Dictionary<string, AuthorizationType> Permissions);
        Task SetPermissions(List<int> documentIdList, UserProfile userInfo, string ProfileId, ProfileType ProfileType, string permissionId, AuthorizationType authorization);
        Task SetPermissions(List<int> documentIdList, UserProfile userInfo, string ProfileId, ProfileType ProfileType, Dictionary<string, AuthorizationType> Permissions);
        Task RemovePermissions(int documentId, ProfileType profileType, string profileId);

        Task<List<BatchErrorResult>> AddToFolder(int folderId, List<int> documentList, UserProfile userInfo, bool moveTofolder = false);
        Task ChangeStatus(int documentId, UserProfile UserInfo, DocumentStatus newStatus);
        Task<int> Create(CreateOrUpdateDocument document, UserProfile userInfo);
        Task<DocumentInfo> CreateAndRead(CreateOrUpdateDocument document, UserProfile userInfo);
        Task<DocumentInfo> ChangeType(int documentId, string newTypeId, UserProfile userInfo);
        Task<DocumentInfo> DocumentSchema(string DocumentTypeId, UserProfile userInfo, ContentType ContentType = ContentType.Document);

        Task<Document> Get(int documentId);
        Task<List<LookupTable>> FullPath(Document r2, UserProfile userInfo);

        Task View(DocumentInfo document, UserProfile userInfo);

        Task Delete(int documentId, UserProfile user, string motivation = "", bool recursive = true);
        Task<bool> Exists(int documentId);
        Task<byte[]> GetContent(int imageId);
        Task<byte[]> GetPreview(DocumentImage image);
        Task<DocumentImage> GetContentInfo(int imageId);

        Task<List<int>> GetDocumentFolders(int documentId, UserProfile userInfo);
        Task<List<int>> GetDocumentsFromContentId(int imageId, UserProfile userInfo);
        Task<List<int>> GetExpiredDocuments(int Top = 50);
        Task<List<int>> GetFolderDocuments(int folderId, UserProfile userInfo);
        Task<Permission> GetPermission(int documentId, UserProfile userInfo, string permissionId);
        Task<List<Permission>> GetPermissions(int documentId, UserProfile userInfo);
        Task<List<DocumentVersion>> Images(int documentId, UserProfile userInfo);
        Task<List<DocumentLink>> Links(int documentId, UserProfile userInfo, bool IsAttachment = false);
        Task<List<DocumentLink>> LinkedIn(int documentId, UserProfile userInfo, bool IsAttachment = false);
        Task<DocumentInfo> Load(int documentId, UserProfile userInfo);
        Task<bool> RemoveContent(int documentId, UserProfile userInfo);
        Task<List<BatchErrorResult>> RemoveFromFolder(int folderId, List<int> documentList, UserProfile userInfo);
        Task<bool> RemoveLink(int documentId, int AttachmentId, UserProfile userInfo, bool IsAttachment = false);

        Task<Permission> GetProfilePermission(int documentId, ProfileType profileType, string profileId, string permissionId);
        Task<ProfilePermissions> GetProfilePermissions(int documentId, ProfileType profileType, string profileId);
        Task<List<ProfilePermissions>> GetDocumentPermissions(int documentId);
        Task<DocumentInfo> Protocol(int documentId, ProtocolInfo info, UserProfile userInfo);



        Task UnDelete(int documentId, UserProfile user);
        Task<DocumentInfo> Update(int documentId, CreateOrUpdateDocument document, UserProfile userInfo);


        Task<int> UpdateSendingStatus(int imageId, JobStatus status, UserProfile userInfo);
        Task<int> UpdateIndexingStatus(int imageId, JobStatus status, UserProfile userInfo);
        Task<int> UpdatePreviewStatus(int imageId, JobStatus status, UserProfile userInfo);
        //Task<int> UpdatePreservationStatus(int imageId, JobStatus status, string PDA, UserProfile userInfo);
        Task<int> UpdateSignatureStatus(int imageId, JobStatus status, UserProfile userInfo, string signatureSession);
        Task<List<int>> GetImagesBySignatureSession(string userName, string signatureSession);
        Task<int> Publish(int imageId, UserProfile userInfo);
        Task<bool> Share(int[] documentIds, UserProfile userInfo, List<ProfileInfo> To, List<ProfileInfo> Cc, ActionRequestType Request, bool AssignToAllUsers, string Title, string Message="");

        Task<byte[]> CheckOut(int imageId, string user);
        Task CheckIn(int imageId, string user, bool ForceCheckIn);

        Task ClearIndexing();
        Task AddBlankSignField(DocumentInfo doc, UserProfile userInfo, int pageIndex, float xPercentage, float yPercentage, string nomeCampoFirma);
        Task<List<FieldPosition>> GetBlankSignFields(Document doc);
        Task<bool> RemoveBlankSignField(DocumentInfo doc, UserProfile userInfo, string FieldName);

        //Task<int> Count(UserProfile userInfo, List<SearchFilter> filters);
        //Task<List<int>> Find(UserProfile userInfo, List<SearchFilter> filters);

        Task<DocumentImage> AddContentFromTemplate(int documentId, string templateKey, string Variables, string OutputExtension = "");

        Task<DocumentImage> ConvertTo(int documentId, String OutputExtension);
        Task<FolderExportModel> GetFolderContentRecursive(DocumentInfo folder, UserProfile user);
        Task ProcessImportedFolderFirstPass(int rootFolderId, FolderExportModel folderExportModel, UserProfile user, Dictionary<int, int> documentIdMap);
        Task ProcessImportedFolderSecondPass(FolderExportModel folderExportModel, UserProfile user, Dictionary<int, int> documentIdMap);
    }
}