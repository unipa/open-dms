using Microsoft.EntityFrameworkCore.Storage;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Security;

namespace OpenDMS.Domain.Repositories
{
    public interface IDocumentRepository
    {
        Task<int> AddImage(int documentId, DocumentImage documentImage, string userId);
        Task AddField(DocumentField Metadato);
        Task UpdateField(DocumentField Metadato);
        Task RemoveField(DocumentField Metadato);

        Task<int> GetByDocumentTypeAndNumber(string documentTypeId, string documentNumber, ContentType contentType = ContentType.Document);

        IEnumerable<int> GetContentsToPreview();
        IEnumerable<int> GetContentsToIndex();
        //IEnumerable<int> GetContentsToPreserve();

        Task<int> UpdateImage(DocumentImage image);
         Task<int> AddToFolder(int Id, int FolderId, string UserId, bool MoveToThisFolder);
        Task<int> RemoveFromFolder(int Id, int FolderId);
        Task<int> AddLink(int Id, int LinkId, string UserId, bool AsAttachment = false);
        //Task<int> ChangePreservationState(int imageId, JobStatus status, string pda);
        Task<int> ChangeSendingState(int imageId, JobStatus status);
        Task<int> ChangePreviewState(int imageId, JobStatus status);
        Task<int> ChangeIndexingState(int imageId, JobStatus status);
        Task<int> ChangeSignatureState(int imageId, JobStatus status, string userId, string signatureSession);
        Task<List<int>> GetImageOnSignatureSession(string userId, string signatureSession);
        Task<bool> CheckIn(int imageId, string userId, bool forceCheckIn);
        Task<bool> CheckOut(int imageId, string userId, bool testCheckOut = false);
        Task ClearIndexing();
        Task<List<int>> Copies(int Id);
        Task<int> Create(Document document);
        Task<int> Delete(int documentId, string userId, string motivation, bool isRecoverable);
        Task<int> Delete(Document document, string userId, string motivation, bool isRecoverable);
        //Task<int> DeletePDA(string pda);
        Task<int> FindByUniqueId(string docType, string uniqueId, ContentType contentType);
        Task<int> FindInFolderByUniqueId(string docType, string uniqueId, int folderId, ContentType contentType);
        Task<Document> GetById(int documentId);
        Task<List<int>> GetDocumentsByImage(int imageId);
        Task<List<DocumentRecipient>> GetRecipients(int documentId);
        Task<List<int>> GetDocumentFolders(int documentId);
        Task<List<int>> GetExpiredDocuments(int maxResults = 50);
        Task<List<DocumentField>> GetFields(int documentId);
        Task<List<int>> GetFolderContent(int folderId);
        Task<ContentType> GetContentType(int documentId);
        Task<DocumentImage> GetImage(int imageId = 0);
        Task<List<DocumentImage>> GetImages(int documentId);
        Task<DocumentImage> GetLastImage(int documentId);
        Task<List<int>> GetLinks(int Id, bool AsAttachment);
        Task<List<int>> GetLinkedToDocuments(int Id, bool AsAttachment);
        //        Task<List<int>> Link(int Id, bool AsAttachment = false);
        Task<int> RemoveImage(int documentId, int imageId);
        Task<int> RemoveLink(int Id, int LinkId, bool AsAttachment = false);
        Task<int> Restore(int documentId);
        Task<int> SaveChanges();
        Task SaveRecipients(int Id, List<DocumentRecipient> Contacts);
        Task SaveFields(int Id, List<DocumentField> Metadati);
        Task Update(Document document);

        Task<DocumentPermission> GetPermission(int documentId, string profileId, ProfileType profileType, string permissionId);
        Task SetPermission(DocumentPermission p);
        Task SetPermission(int documentId, string profileId, ProfileType profileType, string permissionId, AuthorizationType authorization);
        //      Task SetPermissions(int documentId, string profileId, ProfileType profileType, Dictionary<string, AuthorizationType> permissions);
        Task RemovePermissions(int documentId, ProfileType profileType, string profileId);

        Task<List<DocumentPermission>> GetPermissionsByDocumentId(int documentId);
        Task<List<DocumentPermission>> GetPermissionByProfileId(int documentId, string profileId, ProfileType ptype);

//        Task SetPermissionOnDocuments(List<Int32> idList, string profileId, ProfileType profileType, string permissionId, AuthorizationType authorization);
//        Task SetPermissionOnDocuments(List<Int32> idList, DocumentPermission p);


        IDbContextTransaction BeginTransaction();

        Task Commit(IDbContextTransaction Transaction);

        Task Rollback(IDbContextTransaction Transaction);

        Task<List<int>> GetDocumentsToPreserve(string type, int gap);


        //Task<int> Count(UserProfile userInfo, List<SearchFilter> filters);
        //Task<List<int>> Find(UserProfile userInfo, List<SearchFilter> filters);
        //Task<List<object[]>> PagedData(UserProfile userInfo, string[] columns, List<SearchFilter> filters, int PageIndex, int PageSize, List<SortingColumn> SortColumns);

    }
}