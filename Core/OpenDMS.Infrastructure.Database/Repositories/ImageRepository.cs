using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.Infrastructure.Repositories;



public partial class DocumentRepository : IDocumentRepository
{

    public async Task<DocumentImage> GetImage(int imageId = 0)
    {
        return await DS.Images.AsNoTracking().FirstOrDefaultAsync(f => f.Id == imageId);
    }
    public async Task<int> RemoveImage(int documentId, int imageId)
    {
        var version = await DS.ImageVersions.AsNoTracking().FirstOrDefaultAsync(i => i.ImageId == imageId && i.DocumentId == documentId);
        var images = await DS.ImageVersions.AsNoTracking().Where(i => i.ImageId == imageId).CountAsync();
        var document = await DS.Documents.FirstOrDefaultAsync(d=>d.Id==documentId);
        document.ImageId = DS.ImageVersions.AsNoTracking().Where(i => i.DocumentId == documentId).OrderByDescending(i=>i.CreationDate).Select(i=>i.ImageId).Skip(1).FirstOrDefault();
        DS.ImageVersions.Remove(version);
        if (images == 1)
        {
            DS.Images.Remove(DS.Images.AsNoTracking().FirstOrDefault(i => i.Id == imageId));
        }
        try
        {
            return await DS.SaveChangesAsync();
        } finally
        {
            DS.Entry<Document>(document).State = EntityState.Detached;
        }

    }
    public async Task<int> UpdateImage(DocumentImage image)
    {
        try
        {
            DS.Images.Update(image);
            var i = await DS.SaveChangesAsync();
            return i;
        }
        finally
        {
            DS.Entry<DocumentImage>(image).State = EntityState.Detached;

        }
    }

    public async Task<int> AddImage(int documentId, DocumentImage documentImage, string userId)
    {
        var i = 0;
        try
        {
            DS.Images.Add(documentImage);
            await DS.SaveChangesAsync();
            i = documentImage.Id;
            ImageVersion v = new ImageVersion();
            v.ImageId = i;
            v.DocumentId = documentId;
            v.CreationUser = userId;
            DS.ImageVersions.Add(v);
            try
            {
                await DS.SaveChangesAsync();
            } finally
            {
                DS.Entry<ImageVersion>(v).State = EntityState.Detached;
            }
        }
        finally
        {
            DS.Entry<DocumentImage>(documentImage).State = EntityState.Detached;

        }
        return i;
    }


    //public async Task<int> ChangePreservationState(int imageId, JobStatus status, string pda)
    //{
    //    var img = await DS.Images.FirstOrDefaultAsync(f => f.Id == imageId);
    //    img.PreservationStatus = status;
    //    img.PreservationPDV = pda;
    //    img.PreservationDate = DateTime.UtcNow;
    //    var i = await DS.SaveChangesAsync();
    //    DS.Entry<DocumentImage>(img).State = EntityState.Detached;
    //    return i;
    //}
    //public async Task<int> DeletePDA(string pda)
    //{
    //    var imgs = await DS.Images.Where(f => f.PreservationPDV == pda).ToListAsync();
    //    foreach (var img in imgs)
    //    {
    //        img.PreservationPDV = "";
    //        img.PreservationStatus = JobStatus.Queued;
    //    }
    //    DS.Images.UpdateRange(imgs);
    //    return await DS.SaveChangesAsync();
    //}

    public async Task<int> ChangeSignatureState(int imageId, JobStatus status, string userId, string signatureSession)
    {
        var img = await DS.Images.FirstOrDefaultAsync(f => f.Id == imageId);
        if (img != null)
        {
            img.SignatureStatus = status;
            img.SignatureSession = signatureSession;
            img.SignatureUser = userId;
            img.SignatureDate = DateTime.UtcNow;
            try
            {
                var i = await DS.SaveChangesAsync();
                return i;
            }
            finally
            {
                DS.Entry<DocumentImage>(img).State = EntityState.Detached;
            }

        }
        return 0;
    }
    public async Task<List<int>> GetImageOnSignatureSession(string userId, string signatureSession)
    {
        return await DS.Images.AsNoTracking().Where(i => i.SignatureUser == userId && i.SignatureSession == signatureSession).Select(i => i.Id).ToListAsync();
    }


    public async Task<int> ChangeSendingState(int imageId, JobStatus status)
    {
        var img = await DS.Images.FirstOrDefaultAsync(f => f.Id == imageId);
        img.SendingStatus = status;
        img.SendingDate = DateTime.UtcNow;
        try
        {
            var i = await DS.SaveChangesAsync();
            return i;
        }
        finally
        {
            DS.Entry<DocumentImage>(img).State = EntityState.Detached;
        }
    }

    public async Task<int> ChangePreviewState(int imageId, JobStatus status)
    {
        var img = await DS.Images.FirstOrDefaultAsync(f => f.Id == imageId);
        int i = 0;
        if (img != null) {
            try
            {
                img.PreviewStatus = status;
                i = await DS.SaveChangesAsync();
            }
            finally
            {
                DS.Entry<DocumentImage>(img).State = EntityState.Detached;
            }
        }
        return i;
    }

    public async Task<int> ChangeIndexingState(int imageId, JobStatus status)
    {
        var img = await DS.Images.FirstOrDefaultAsync(f => f.Id == imageId);
        img.IndexingStatus = status;
        try
        {
            var i = await DS.SaveChangesAsync();
            return i;
        }
        finally
        {
            DS.Entry<DocumentImage>(img).State = EntityState.Detached;
        }
    }


    public async Task<List<DocumentImage>> GetImages(int documentId)
    {
        return await DS.Images.AsNoTracking().Where(i => i.Versions.Any(v => v.DocumentId == documentId && v.ImageId == i.Id)).OrderByDescending(o => o.VersionNumber).ThenByDescending(o => o.RevisionNumber).ToListAsync();
    }
    public async Task<List<int>> GetDocumentsByImage(int imageId)
    {
        return await DS.ImageVersions.AsNoTracking().Where(v => v.ImageId == imageId).Select(o=>o.DocumentId).ToListAsync();
    }
    public async Task<DocumentImage> GetLastImage(int documentId)
    {
        return await DS.Images.AsNoTracking().Include(i => i.Versions.Where(v => v.DocumentId == documentId)).OrderByDescending(o => o.VersionNumber).ThenByDescending(o => o.RevisionNumber).FirstAsync();
    }



    public async Task<bool> CheckOut(int imageId, string userId, bool testCheckOut = false)
    {
        var img = await DS.Images.FirstOrDefaultAsync(i => i.Id == imageId);
        var blockedByUser = img.CheckOutUser;
        if (String.IsNullOrEmpty(blockedByUser) && !testCheckOut)
        {
            try
            {
                img.CheckOutUser = userId;
                return await DS.SaveChangesAsync() > 0;
            }
            finally
            {
                DS.Entry<DocumentImage>(img).State = EntityState.Detached;
            }
        }
        return false;
    }

    public async Task<bool> CheckIn(int imageId, string userId, bool forceCheckIn)
    {
        var img = await DS.Images.FirstOrDefaultAsync(i => i.Id == imageId);
        var blockedByUser = img.CheckOutUser;
        if ((blockedByUser.ToLower() == userId.ToLower()) || forceCheckIn)
        {
            try { 
                img.CheckOutUser = "";
                return await DS.SaveChangesAsync() > 0;
            }
            finally
            {
                DS.Entry<DocumentImage>(img).State = EntityState.Detached;
            }

        }
        return false;
    }



}

