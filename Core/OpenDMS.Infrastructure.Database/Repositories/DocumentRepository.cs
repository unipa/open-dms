using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;

namespace OpenDMS.Infrastructure.Repositories;

public partial class DocumentRepository : IDocumentRepository
{

    private readonly ApplicationDbContext DS;
    private IApplicationDbContextFactory contextFactory;

    public DocumentRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
    }


    public async Task<int> Create(Document document)
    {
        try
        {
            document.CreationDate = DateTime.UtcNow;
            DS.Entry<Document>(document).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            if (document.Fields != null)
                foreach (var meta in document.Fields)
                {
                    DS.Entry<DocumentField>(meta).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    if (meta.Blob != null) DS.Entry<DocumentBlobField>(meta.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }

            var obj = await DS.Documents.AddAsync(document);
            await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<Document>(document).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            if (document.Fields != null)
                foreach (var meta in document.Fields)
                {
                    DS.Entry<DocumentField>(meta).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    if (meta.Blob != null) DS.Entry<DocumentBlobField>(meta.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
        }
        return document.Id;
    }
    public async Task Update(Document document)
    {
        try
        {
            DS.Entry<Document>(document).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            if (document.Fields != null)
            {
                if (!DS.Database.IsInMemory())
                {
                    await DS.DocumentFields.AsNoTracking().Where(d => d.DocumentId == document.Id).ExecuteDeleteAsync();
                    await DS.DocumentBlobFields.AsNoTracking().Where(d => d.DocumentId == document.Id).ExecuteDeleteAsync();
                }
                foreach (var meta in document.Fields)
                {
                    DS.Entry<DocumentField>(meta).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    if (meta.Blob != null) DS.Entry<DocumentBlobField>(meta.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                }
            }
            await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<Document>(document).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            if (document.Fields != null)
                foreach (var meta in document.Fields)
                {
                    DS.Entry<DocumentField>(meta).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    if (meta.Blob != null) DS.Entry<DocumentBlobField>(meta.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
        }
    }
    public async Task SaveFields(int Id, List<DocumentField> Metadati)
    {
        try
        {
            foreach (DocumentField field in Metadati)
            {
                field.DocumentId = Id;
                DS.DocumentFields.Add(field);
                DS.Entry<DocumentField>(field).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                if (field.Blob != null) DS.Entry<DocumentBlobField>(field.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Added;

            }
            await DS.SaveChangesAsync();
        }
        finally
        {
            foreach (DocumentField field in Metadati)
            {
                DS.Entry<DocumentField>(field).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                if (field.Blob != null) DS.Entry<DocumentBlobField>(field.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
        }
    }

    public async Task AddField(DocumentField Metadato)
    {
        DS.Entry<DocumentField>(Metadato).State = EntityState.Added;
        if (Metadato.Blob != null) DS.Entry<DocumentBlobField>(Metadato.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        try
        {
            var r = await DS.SaveChangesAsync();
        }
        finally {
            DS.Entry<DocumentField>(Metadato).State = EntityState.Detached;
            if (Metadato.Blob != null) DS.Entry<DocumentBlobField>(Metadato.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

    }
    public async Task UpdateField(DocumentField Metadato)
    {
        DS.Entry<DocumentField>(Metadato).State = EntityState.Modified;
        if (Metadato.Blob != null) DS.Entry<DocumentBlobField>(Metadato.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        try
        {
            var r = await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<DocumentField>(Metadato).State = EntityState.Detached;
            if (Metadato.Blob != null) DS.Entry<DocumentBlobField>(Metadato.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }
    }
    public async Task RemoveField(DocumentField Metadato)
    {
        DS.Entry<DocumentField>(Metadato).State = EntityState.Deleted;
        if (Metadato.Blob != null) DS.Entry<DocumentBlobField>(Metadato.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        try
        {
            var r = await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<DocumentField>(Metadato).State = EntityState.Detached;
            if (Metadato.Blob != null) DS.Entry<DocumentBlobField>(Metadato.Blob).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }
    }

    public async Task SaveRecipients(int Id, List<DocumentRecipient> Contacts)
    {
        try
        {
            Contacts.ForEach(m => m.DocumentId = Id);
            await DS.DocumentRecipients.AsNoTracking().Where(d => d.DocumentId == Id).ExecuteDeleteAsync();
            await DS.DocumentRecipients.AddRangeAsync(Contacts);
            await DS.SaveChangesAsync();
        }
        finally {
            Contacts.ForEach(m => DS.Entry<DocumentRecipient>(m).State = EntityState.Detached);
        }
    }
    public async Task<int> SaveChanges()
    {
        return await DS.SaveChangesAsync();
    }

    public async Task<int> Delete(int documentId, string userId, string motivation, bool isRecoverable)
    {
        var doc = await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == documentId);
        if (doc != null)
            return await Delete(doc, userId, motivation, isRecoverable);
        else
            return 0;
    }
    public async Task<int> Delete(Document doc, string userId, string motivation, bool isRecoverable)
    {
        try
        {
            if (!isRecoverable)
            {
                DS.Entry<Document>(doc).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                return await DS.SaveChangesAsync();
            }
            else
            {
                if (doc.DocumentStatus != DocumentStatus.Deleted)
                {
                    var DataCancellazione = Int32.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
                    doc.DocumentStatus = DocumentStatus.Deleted;
                    if (!String.IsNullOrEmpty(motivation))
                    {
                        doc.Description = motivation.Substring(0, Math.Min(motivation.Length, 128)) + " - " + doc.Description;
                        if (doc.Description.Length > 255) doc.Description = doc.Description.Substring(0, 255);
                    }
                    doc.LastUpdateUser = userId;
                    doc.DeletionDate = DateTime.UtcNow;
                    DS.Entry<Document>(doc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    return await DS.SaveChangesAsync();
                }
                else return 0;
            }
        }
        finally {
                DS.Entry<Document>(doc).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        };
    }
    public async Task<int> Restore(int documentId)
    {
        var doc = await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == documentId);
        doc.DocumentStatus = DocumentStatus.Active;
        DS.Entry<Document>(doc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        try
        {
            return await DS.SaveChangesAsync();
        } finally
        {
            DS.Entry<Document>(doc).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }
    }

    public async Task ClearIndexing()
    {
        await DS.Images.ExecuteUpdateAsync(d => d.SetProperty(i => i.IndexingStatus, i => JobStatus.Queued));
    }


    public IEnumerable<int> GetContentsToPreview()
    {
        return DS.Images.AsNoTracking().Where(i => i.PreviewStatus == JobStatus.Queued || i.PreviewStatus == JobStatus.Failed).Select(i=>i.Id).AsEnumerable();
    }
    public IEnumerable<int> GetContentsToIndex()
    {
        return DS.Documents.Include(i=>i.Image).AsNoTracking().Where(d=>d.Image != null && (d.Image.IndexingStatus== JobStatus.Queued || d.Image.IndexingStatus == JobStatus.Failed)).Select(i => i.Image.Id).AsEnumerable();
    }
    //public IEnumerable<int> GetContentsToPreserve()
    //{
    //    return DS.Documents.Include(i => i.Image).AsNoTracking().Where(d => d.Image != null && (d.Image.PreservationStatus == JobStatus.Queued || d.Image.PreservationStatus == JobStatus.Failed)).Select(i => i.Image.Id).AsEnumerable();
    //}

    public async Task<Document> GetById(int documentId)
    {
        return await DS.Documents.Include(d=>d.Image).AsNoTracking().FirstOrDefaultAsync(d => d.Id == documentId);
    }
    public async Task<List<int>> GetExpiredDocuments(int maxResults = 50)
    {
        return await DS.Documents.AsNoTracking().Where(d => d.ExpirationDate < DateTime.UtcNow && d.ExpirationDate > DateTime.MinValue && d.DocumentStatus != DocumentStatus.Deleted).Select<Document, int>(i => i.Id).Take(maxResults).ToListAsync();
    }

    public async Task<int> FindByUniqueId(string docType, string uniqueId, ContentType contentType)
    {
        if (docType != null)
            return (await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentTypeId == docType && d.ExternalId == uniqueId && d.DocumentStatus != DocumentStatus.Deleted && (d.ContentType == contentType || ContentType.Any == contentType)))?.Id ?? 0;
        else 
            return (await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.ExternalId == uniqueId && d.DocumentStatus != DocumentStatus.Deleted && (d.ContentType == contentType || ContentType.Any == contentType)))?.Id ?? 0;
    }

    public async Task<int> GetByDocumentTypeAndNumber(string documentTypeId, string documentNumber, ContentType contentType = ContentType.Document )
    {
        if (documentTypeId.Contains(","))
        {
            var values = documentTypeId.Split(","); 
            return (await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => values.Contains(d.DocumentTypeId) && d.DocumentNumber == documentNumber && d.DocumentStatus != DocumentStatus.Deleted && d.ContentType == contentType))?.Id ?? 0;
        }
        else
            return (await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentTypeId == documentTypeId && d.DocumentNumber == documentNumber && d.DocumentStatus != DocumentStatus.Deleted && d.ContentType == contentType))?.Id ?? 0;

    }


    public async Task<int> FindInFolderByUniqueId(string docType, string uniqueId, int folderId, ContentType contentType)
    {
        return (await DS.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentTypeId == docType && d.ExternalId == uniqueId && d.FolderId == folderId && d.DocumentStatus != DocumentStatus.Deleted && d.ContentType == contentType))?.Id ?? 0;
    }
    public async Task<List<int>> GetDocumentFolders(int documentId)
    {
        var lista = await DS.FolderContents.Where(d => d.DocumentId == documentId).AsNoTracking().Select<FolderContent, int>(i => i.FolderId).ToListAsync();
        lista.AddRange
            (
            await DS.Documents.Where(d => d.Id == documentId && d.FolderId > 0).AsNoTracking().Select(i => i.FolderId ?? 0).ToListAsync()
            );
        return lista;
    }
    public async Task<List<int>> GetFolderContent(int folderId)
    {
        var lista = await DS.FolderContents.Where(d => d.FolderId == folderId).AsNoTracking().Select<FolderContent, int>(i => i.DocumentId).ToListAsync();
        lista.AddRange
            (
            await DS.Documents.Where(d => d.FolderId == folderId).AsNoTracking().Select(i => i.Id).ToListAsync()
            );
        return lista;
    }

    public async Task<ContentType> GetContentType(int documentId)
    {
        return (await DS.Documents.FindAsync(documentId)).ContentType;
    }


public async Task<List<int>> Copies(Int32 Id)
    {
        return await DS.Documents.AsNoTracking().Where(d => d.MasterDocumentId == Id && d.DocumentStatus != DocumentStatus.Deleted).AsNoTracking().Select<Document, int>(i => i.Id).ToListAsync();
    }

    public async Task<List<int>> GetLinks(Int32 Id, bool AsAttachment)
    {
        if (AsAttachment)
            return await DS.DocumentRelationships.AsNoTracking().Where(d => (d.DocumentId == Id) && d.IsLinked == false).Select<DocumentRelationship, int>(i => i.AttachmentId).ToListAsync();
        else
            return await DS.DocumentRelationships.AsNoTracking().Where(d => (d.AttachmentId == Id || d.DocumentId == Id) && d.IsLinked == true).Select<DocumentRelationship, int>(i =>  i.AttachmentId == Id ? i.DocumentId : i.AttachmentId).ToListAsync();
    }
    public async Task<List<int>> GetLinkedToDocuments(Int32 Id, bool AsAttachment)
    {
        return await DS.DocumentRelationships.AsNoTracking().Where(d => d.AttachmentId == Id && d.IsLinked == !AsAttachment).Select<DocumentRelationship, int>(i => i.DocumentId).ToListAsync();
    }


    public async Task<int> AddToFolder(Int32 Id, Int32 FolderId, string UserId, bool MoveToThisFolder)
    {
        if (FolderId <= 0) throw new ArgumentException(nameof(FolderId));
        if (Id <= 0) throw new ArgumentException(nameof(Id));
        var folder = DS.FolderContents.AsNoTracking().FirstOrDefault(f => f.DocumentId == Id && f.FolderId == FolderId);
        try
        {
            if (folder == null)
            {
                folder = new FolderContent() { DocumentId = Id, FolderId = FolderId, CreationUser = UserId };
                DS.FolderContents.Add(folder);
            }
            return await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<FolderContent>(folder).State = EntityState.Detached;
        }
    }
    public async Task<int> RemoveFromFolder(Int32 Id, Int32 FolderId)
    {
        if (FolderId <= 0) throw new ArgumentException(nameof(FolderId));
        if (Id <= 0) throw new ArgumentException(nameof(Id));
        Document doc = null;
        var folder = await DS.FolderContents.AsNoTracking().FirstOrDefaultAsync(d => d.FolderId == FolderId && d.DocumentId == Id);
        var docfolder = folder;
        if (folder != null)
            DS.FolderContents.Remove(folder);
        else
        {
            doc = DS.Documents.Find(Id);
            if (doc.FolderId == FolderId)
            {
                docfolder = await DS.FolderContents.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == Id);
                if (docfolder != null)
                {
                    doc.FolderId = docfolder.FolderId;
                    DS.FolderContents.Remove(docfolder);
                }
                else
                    doc.FolderId = 0;
            }
        }
        try
        {
            return await DS.SaveChangesAsync();
        }
        finally {
            if (folder != null)
            {
                DS.Entry<FolderContent>(folder).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            else
            {
                if (doc != null) DS.Entry<Document>(doc).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                if (docfolder != null) DS.Entry<FolderContent>(docfolder).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

        };
    }

    public async Task<int> AddLink(Int32 Id, Int32 LinkId, string UserId, bool AsAttachment = false)
    {
        if (LinkId <= 0) throw new ArgumentException(nameof(LinkId));
        if (Id <= 0) throw new ArgumentException(nameof(Id));
        var link = new DocumentRelationship() { DocumentId = Id, AttachmentId = LinkId, IsLinked = !AsAttachment, CreationUser = UserId };
        try
        {
            if (DS.DocumentRelationships.AsNoTracking().FirstOrDefault(f =>
                f.DocumentId == Id &&
                f.AttachmentId == LinkId 
                //&& f.IsLinked == !AsAttachment
                ) == null)
            {
                DS.DocumentRelationships.Add(link);

                return await DS.SaveChangesAsync();
            }
        }
        finally
        {
            DS.Entry<DocumentRelationship>(link).State = EntityState.Detached;
        }
        return 0;
    }
    public async Task<int> RemoveLink(Int32 Id, Int32 LinkId, bool AsAttachment = false)
    {
        var att = await DS.DocumentRelationships.AsNoTracking().FirstOrDefaultAsync(a => a.AttachmentId == LinkId && a.DocumentId == Id && a.IsLinked == !AsAttachment);
        DS.DocumentRelationships.Remove(att);
        try { 
            return await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<DocumentRelationship>(att).State = EntityState.Detached;
        }
    }



    public async Task<List<DocumentField>> GetFields(int documentId)
    {
        return await DS.DocumentFields.Include(f=>f.Blob).AsNoTracking().Where(f => f.DocumentId == documentId).OrderBy(o => o.FieldIndex).ToListAsync();
    }
    public async Task<List<DocumentRecipient>> GetRecipients(int documentId)
    {
        return await DS.DocumentRecipients.AsNoTracking().Where(f => f.DocumentId == documentId).OrderBy(o => o.CreationDate).ToListAsync();
    }
    public IDbContextTransaction BeginTransaction()
    {
        return DS.Database.BeginTransaction();
    }
    public async Task Commit(IDbContextTransaction Transaction)
    {
        await Transaction.CommitAsync();
    }
    public async Task Rollback(IDbContextTransaction Transaction)
    {
        await Transaction.RollbackAsync();
    }

    public async Task<DocumentPermission> GetPermission(int id, string Username, ProfileType ptype, string permissionId)
    {
        if (!String.IsNullOrEmpty(Username)) Username = Username.ToLower();
        DocumentPermission p = await DS.DocumentPermissions.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == id && d.ProfileId == Username && d.ProfileType == ptype && d.PermissionId == permissionId);
        return p;
    }
    public async Task RemovePermissions(int documentId, ProfileType profileType, string profileId)
    {
        var permissions = DS.DocumentPermissions.AsNoTracking().Where(p => (p.DocumentId == documentId) && (p.ProfileType == profileType) && (p.ProfileId == profileId)).ToList();
        DS.DocumentPermissions.RemoveRange(permissions);
        await DS.SaveChangesAsync();
    }


    public async Task SetPermission(int documentId, string profileId, ProfileType profileType, string permissionId, AuthorizationType authorization)
    {
        DocumentPermission p = await DS.DocumentPermissions.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == documentId && d.ProfileId == profileId && d.ProfileType == profileType && d.PermissionId == permissionId);
        try
        {
            if (p == null)
            {
                if (authorization == AuthorizationType.None) return;
                p = new DocumentPermission() { DocumentId = documentId, ProfileType = profileType, ProfileId = profileId, PermissionId = permissionId };
                p.Authorization = authorization;
                DS.Entry<DocumentPermission>(p).State = EntityState.Added;
            }
            else
            {
                if (authorization != AuthorizationType.None)
                {
                    p.Authorization = authorization;
                    DS.DocumentPermissions.Update(p);
                }
                else
                {
                    DS.DocumentPermissions.Remove(p);
                    p = null;
                }

            }
            await DS.SaveChangesAsync();
        }
        finally
        {
            if (p != null) DS.Entry<DocumentPermission>(p).State = EntityState.Detached;
        }
    }
    public async Task SetPermission(DocumentPermission permission)
    {
        DocumentPermission p = await DS.DocumentPermissions.FirstOrDefaultAsync(d => d.DocumentId == permission.DocumentId && d.ProfileId == permission.ProfileId && d.ProfileType == permission.ProfileType && d.PermissionId == permission.PermissionId);
        try
        {
            if (p == null)
            {
                p = permission;
                //DS.DocumentPermissions.Add(p);
                DS.Entry<DocumentPermission>(p).State = EntityState.Added;
            }
            else
            {
                DS.Entry<DocumentPermission>(p).State = EntityState.Modified;
            }
            p.Authorization = permission.Authorization;
            await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<DocumentPermission>(p).State = EntityState.Detached;
        }
    }

    //public async Task SetPermissions(int documentId, string profileId, ProfileType profileType, Dictionary<string, AuthorizationType> permissions)
    //{
    //    List<DocumentPermission> saved = new();
    //    try
    //    {
    //        foreach (var permission in permissions)
    //        {
    //            DocumentPermission p = await DS.DocumentPermissions.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == documentId && d.ProfileId == profileId && d.ProfileType == profileType && d.PermissionId == permission.Key);
    //            if (p == null)
    //            {
    //                if (permission.Value != AuthorizationType.None)
    //                {
    //                    p = new DocumentPermission() { DocumentId = documentId, ProfileType = profileType, ProfileId = profileId, PermissionId = permission.Key };
    //                    p.Authorization = permission.Value;
    //                    DS.Entry<DocumentPermission>(p).State = EntityState.Added;
    //                }
    //            }
    //            else
    //            {
    //                if (permission.Value != AuthorizationType.None)
    //                {
    //                    p.Authorization = permission.Value;
    //                    DS.DocumentPermissions.Update(p);
    //                }
    //                else
    //                {
    //                    DS.DocumentPermissions.Remove(p);
    //                }
    //            }
    //            saved.Add(p);
    //        }
    //        await DS.SaveChangesAsync();
    //    }
    //    finally
    //    {
    //        foreach (var p in saved)
    //            DS.Entry<DocumentPermission>(p).State = EntityState.Detached;
    //    }
    //}

    //public async Task SetPermissions(List<Int32> documentIdList, string profileId, ProfileType profileType, Dictionary<string, AuthorizationType> permissions)
    //{
    //    List<DocumentPermission> saved = new();
    //    try
    //    {
    //        foreach (var documentId in documentIdList)
    //        {
    //            foreach (var permission in permissions)
    //            {
    //                DocumentPermission p = await DS.DocumentPermissions.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == documentId && d.ProfileId == profileId && d.ProfileType == profileType && d.PermissionId == permission.Key);
    //                if (p == null)
    //                {
    //                    if (permission.Value != AuthorizationType.None)
    //                    {
    //                        p = new DocumentPermission() { DocumentId = documentId, ProfileType = profileType, ProfileId = profileId, PermissionId = permission.Key };
    //                        p.Authorization = permission.Value;
    //                        DS.Entry<DocumentPermission>(p).State = EntityState.Added;
    //                    }
    //                }
    //                else
    //                {
    //                    if (permission.Value != AuthorizationType.None)
    //                    {
    //                        p.Authorization = permission.Value;
    //                        DS.DocumentPermissions.Update(p);
    //                    }
    //                    else
    //                    {
    //                        DS.DocumentPermissions.Remove(p);
    //                    }
    //                }
    //                saved.Add(p);
    //            }
    //        }
    //        await DS.SaveChangesAsync();
    //    }
    //    finally
    //    {
    //        foreach (var p in saved)
    //            DS.Entry<DocumentPermission>(p).State = EntityState.Detached;
    //    }
    //}



    //public async Task SetPermissionOnDocuments(List<Int32> idList, string Username, ProfileType ptype, string permissionId, AuthorizationType authorization)
    //{
    //    List<DocumentPermission> saved = new();
    //    try
    //    {
    //        foreach (var id in idList)
    //        {
    //            DocumentPermission p = await DS.DocumentPermissions.FirstOrDefaultAsync(d => d.DocumentId == id && d.ProfileId == Username && d.ProfileType == ptype && d.PermissionId == permissionId);
    //            if (p == null)
    //            {
    //                p = new DocumentPermission() { DocumentId = id, ProfileType = ptype, ProfileId = Username, PermissionId = permissionId };
    //                DS.DocumentPermissions.Add(p);
    //            }
    //            p.Authorization = authorization;
    //            saved.Add(p);
    //        }
    //        await DS.SaveChangesAsync();
    //    }
    //    finally
    //    {
    //        foreach (var p in saved)
    //            DS.Entry<DocumentPermission>(p).State = EntityState.Detached;
    //    }
    //}



    public async Task<List<DocumentPermission>> GetPermissionsByDocumentId(int id)
    {
        return await DS.DocumentPermissions.AsNoTracking().Where(d => d.DocumentId == id).OrderBy(o => o.ProfileId).ThenBy(o => o.ProfileType).ToListAsync();
    }

    public async Task<List<DocumentPermission>> GetPermissionByProfileId(int id, string Username, ProfileType ptype)
    {
        Username = Username.ToLower();
        return await DS.DocumentPermissions.AsNoTracking().Where(d => d.DocumentId == id && d.ProfileId == Username && d.ProfileType == ptype).ToListAsync();
    }


    public async Task<List<int>> GetDocumentsToPreserve(string type, int gap)
    {
        return await DS.Documents.AsNoTracking()
            .Where(d => d.DocumentTypeId == type
            && d.DocumentDate <= DateTime.Now.AddDays((-1) * gap)
            && (d.Image != null && d.Image.PreservationStatus != JobStatus.Failed && d.Image.PreservationStatus != JobStatus.Running
            && String.IsNullOrEmpty(d.Image.PreservationPDV))).Select(i => i.Id).ToListAsync();
    }







}


