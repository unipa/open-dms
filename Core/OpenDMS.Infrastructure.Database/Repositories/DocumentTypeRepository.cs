using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;

using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;
using System.Drawing.Drawing2D;


namespace OpenDMS.Infrastructure.Repositories;


public class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly ApplicationDbContext DS;
    private readonly IApplicationDbContextFactory contextFactory;

    public DocumentTypeRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
    }

    public async Task<DocumentType> GetById(string documentTypeId)
    {
        if (documentTypeId == null)
        {
            return new DocumentType() { DescriptionLabel="Descrizione", DocumentDateLabel="Data Documento" };
        }
        var t = (await DS.DocumentTypes.Include(t=>t.Fields).ThenInclude(t=>t.FieldType).AsNoTracking().FirstOrDefaultAsync(t=>t.Id == documentTypeId)) ?? new DocumentType();
        if (t.Fields != null) t.Fields.Sort((x,y)=> x.FieldIndex.CompareTo(y.FieldIndex));
        if (t.DocumentNumberDataType == "") t.DocumentNumberDataType = null;
        return t;
    }

    /// <summary>
    /// Recupera le tipologie accessibili ad un utente
    /// Sono accessibili le tipologie associate alle ACL in cui l'utente è inserito direttamente o tramite un gruppo
    /// </summary>
    /// <param name="userInfo"></param>
    /// <returns></returns>
    public async Task<List<DocumentType>> GetByUser(UserProfile userInfo)
    {
        List<DocumentType> TList =await DS.DocumentTypes.Include(t => t.ACL.Permissions.Where(a =>
            (a.ProfileType == ProfileType.User && userInfo.userId == a.ProfileId) ||
            (a.ProfileType == ProfileType.Role && userInfo.Roles.Select(s => s.Id).Contains(a.ProfileId)) ||
            (a.ProfileType == ProfileType.Group && userInfo.Groups.Select(s => s.Id).Contains(a.ProfileId))
           )).AsNoTracking().ToListAsync(); 
        return TList;
    }


    public async Task<List<DocumentType>> GetAll()
    {
        return await DS.DocumentTypes.AsNoTracking().OrderBy(o=>o.Name).ToListAsync();
    }





    public async Task<int> Insert(DocumentType NewType)
    {
        Int32 r = 0;
        NewType.Id = NewType.Id.ToUpper();
        String tipo = "";
        if (String.IsNullOrEmpty(NewType.ACLId)) NewType.ACLId = NewType.ACLId;
        if (String.IsNullOrEmpty(NewType.Owner)) NewType.Owner = "";
        if (String.IsNullOrEmpty(NewType.DescriptionLabel)) NewType.DescriptionLabel = "";
        if (String.IsNullOrEmpty(NewType.CategoryId)) NewType.CategoryId = "";
        if (String.IsNullOrEmpty(NewType.Description)) NewType.Description = "";
        //        if (String.IsNullOrEmpty(bd.EmailSource)) bd.EmailSource = "";
        //        if (String.IsNullOrEmpty(bd.TabellaStati)) bd.TabellaStati = "";
        //        if (String.IsNullOrEmpty(bd.StatoIniziale)) bd.StatoIniziale = "";
        DS.DocumentTypes.Add(NewType);
        try
        {
            r = await DS.SaveChangesAsync();
        } finally
        { 
            DS.Entry<DocumentType>(NewType).State = EntityState.Detached;
        }
        return r;
    }
    public async Task<int> AddField(DocumentTypeField definition)
    {
        Int32 r = 0;
        DS.Entry<DocumentTypeField>(definition).State = EntityState.Added;
        try
        {
            r = await DS.SaveChangesAsync();
        }
        finally
        {

            DS.Entry<DocumentTypeField>(definition).State = EntityState.Detached;
        }
        return r;
    }
    public async Task<int> Update(DocumentType NewType)
    {
        NewType.Id = NewType.Id.ToUpper();

        DocumentType PreviousType = await DS.DocumentTypes.Include(t=>t.Fields).AsNoTracking().FirstOrDefaultAsync (t=>t.Id == NewType.Id);
        Int32 r = 0;

        if (NewType.FileNamingTemplate == null) NewType.FileNamingTemplate = "";
        if (NewType.ACLId == null) NewType.ACLId = "";
        //if (NewType.ProtocolRegister == null) NewType.ProtocolRegister = "";

        var NewFields = NewType.Fields;
        NewType.Fields = null;// new List<DocumentTypeField>();
        DS.Entry<DocumentType>(NewType).State = EntityState.Modified;

        HashSet<string> FieldFound = new HashSet<string>();
        for (int i = 0; i < PreviousType.FieldCount; i++)
        {
            var uid = PreviousType.Fields[i].FieldName;
            FieldFound.Add(uid);
            var oldFieldOnNewType = NewFields.FirstOrDefault(f => f.FieldName == uid);
            if (oldFieldOnNewType == null)
            {
                // trovato campo non più disponibile
                // aggiorno i documenti impostando i campi associati come customized
                await DS.DocumentFields.Where(d => d.FieldName == uid && d.DocumentTypeId == PreviousType.Id).ExecuteUpdateAsync(x => x.SetProperty(u => u.Customized, u => true));
            }
        }

        for (int i = 0; i < NewFields.Count; i++)
        {
            var uid = NewFields[i].FieldName;
            //NewFields[i].DocumentType = NewType;
            //Se il campo esisteva già lo aggiorno e rimuovo l'indica dall'hastable
            // in modo tale che alla fine mi rimangano solo i vecchi campi non più presenti
            if (String.IsNullOrWhiteSpace(NewFields[i].FieldTypeId)) NewFields[i].FieldTypeId = null;

            // se il campo era già presente
            if (FieldFound.Contains(uid))
            {
                FieldFound.Remove(uid);
                DS.Entry<DocumentTypeField>(NewFields[i]).State = EntityState.Modified;
                //DS.DocumentTypeFields.Update(NewFields[i]);
            }
            else
            {
                NewFields[i].Id = Guid.NewGuid().ToString();
                DS.Entry<DocumentTypeField>(NewFields[i]).State = EntityState.Added;
            //    DS.DocumentTypeFields.Add(NewFields[i]);
                //                await DS.DocumentFields.Where(d => d.DocumentTypeId == NewType.Id).Execute UpdateAsync(x => x.SetProperty(u => u.Customized, u => true));
                // Trovato nuovo campo
                // inserisco il campo in tutti i documenti della tipologia

                //DS.Execute("INSERT INTO fields () SELECT FROM UPDATE DOCFLD0F SET FINDX=FINDX+" + NuoviCampi + " WHERE FTMP=1 AND FNURC IN (SELECT CNURC FROM DOCUM00F WHERE CTIPO=" + IDataSource.AsString(NewType.Id) + ")");
            }
        }
        // Rimuovo i campi non più presenti nella nuova configurazione
        foreach (var uid in FieldFound)
        {
            var f = NewFields.FirstOrDefault(f => f.FieldName == uid);
            if (f != null)
                DS.DocumentTypeFields.Remove(f);
        }
        try
        {
            r = await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<DocumentType>(NewType).State = EntityState.Detached;
            for (int i = 0; i < NewFields.Count; i++)
            {
                DS.Entry<DocumentTypeField>(NewFields[i]).State = EntityState.Detached;
            }
        }
        return r;
    }
    public async Task<int> Delete(string documentTypeId)
    {
        Int32 r = 0;
        //*Include() perché se no non cancellava i field da DocumentTypeFields e dava errore di foreign key constrain
        var doctype = await DS.DocumentTypes.Include(x => x.Fields).AsNoTracking().FirstOrDefaultAsync(t => t.Id == documentTypeId);
        try
        {
            DS.DocumentTypes.Remove(doctype);
            r = await DS.SaveChangesAsync();
        }
        finally
        {
            DS.Entry<DocumentType>(doctype).State = EntityState.Detached;
        }
        return r;
    }


    public async Task<List<DocumentTypeWorkflow>> GetAllWorkflows (string documentTypeId)
    {
        return await DS.DocumentTypeWorkflows.AsNoTracking().Where(t=>t.DocumentTypeId==documentTypeId).ToListAsync();
    }
    public async Task<List<DocumentTypeWorkflow>> GetAllTypesWorkflows()
    {
        return await DS.DocumentTypeWorkflows.AsNoTracking().ToListAsync();
    }

    public async Task<int> AddWorkflow(DocumentTypeWorkflow definition)
    {
        try
        {
            DS.DocumentTypeWorkflows.Add(definition);
            var r = await DS.SaveChangesAsync();
            return r;
        }
        finally
        {
            DS.Entry<DocumentTypeWorkflow>(definition).State = EntityState.Detached;
        }
    }
    public async Task<int> UpdateWorkflow(DocumentTypeWorkflow definition)
    {
        try
        {
            DS.DocumentTypeWorkflows.Update(definition);
            var r = await DS.SaveChangesAsync();
            return r;
        }
        finally
        {
            DS.Entry<DocumentTypeWorkflow>(definition).State = EntityState.Detached;
        }

    }
    public async Task<int> RemoveWorkflow(DocumentTypeWorkflow definition)
    {
        try
        {
            DS.DocumentTypeWorkflows.Remove(definition);
            var r = await DS.SaveChangesAsync();
            return r;
        }
        finally
        {
            DS.Entry<DocumentTypeWorkflow>(definition).State = EntityState.Detached;
        }
    }
    public async Task<DocumentTypeWorkflow> GetWorkflow(string documentTypeId, string eventName)
    {
        return await DS.DocumentTypeWorkflows.AsNoTracking().FirstOrDefaultAsync(t => t.DocumentTypeId == documentTypeId && t.EventName == eventName);

    }
}
