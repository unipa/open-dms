using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories
{
    public class CustomDataTypeRepository : ICustomFieldTypeRepository
    {
        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;
        private readonly IDataTypeFactory customfieldFactory;

        public CustomDataTypeRepository (IApplicationDbContextFactory contextFactory, IDataTypeFactory customfieldFactory)
        {
            this.contextFactory = contextFactory;
            this.customfieldFactory = customfieldFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }




        public async Task<FieldType> GetById(string id)
        {
            FieldType r = new FieldType();
            if (string.IsNullOrEmpty(id)) id = "$$t";
            string c = (id ?? "").ToLower();
            //if (c.StartsWith("$") || string.IsNullOrEmpty(c))
            //    r = customfieldFactory.GetAllTypes().FirstOrDefault(f=>f.Id == c);
            //else
                r = await ds.FieldTypes.AsNoTracking().FirstOrDefaultAsync(f=>f.Id == c);
            return r;
        }


        public async Task<List<FieldType>> GetAll()
        {
            var list = (await ds.FieldTypes.AsNoTracking().ToListAsync());
            //list.AddRange(customfieldFactory.GetAllTypes().Where(m=>m.Customized).ToList());
            return list.OrderBy(c => c.Name).ToList();    
        }

        public async Task<int> Insert(FieldType bd)
        {
            bd.Id = bd.Id.ToLower();
            ds.FieldTypes.Add(bd);
            var r = await ds.SaveChangesAsync();
            ds.Entry<FieldType>(bd).State = EntityState.Detached;
            return r;
        }
        public async Task<int> Update(FieldType bd)
        {
            //ds.Attach(bd);
            bd.Id = bd.Id.ToLower();
            ds.Entry<FieldType>(bd).State = EntityState.Modified;
            var r = await ds.SaveChangesAsync();
            ds.Entry<FieldType>(bd).State = EntityState.Detached;
            return r;
        }
        public async Task<int> Delete(FieldType bd)
        {
            if (!ds.Database.IsInMemory())
            {
                ds.DocumentTypeFields.Where(t => t.FieldTypeId == bd.Id).ExecuteUpdate(u => u.SetProperty(u => u.FieldTypeId, u => "").SetProperty(u => u.Title, u => bd.Name));
                ds.DocumentFields.Where(t => t.FieldTypeId == bd.Id).ExecuteUpdate(u => u.SetProperty(u => u.FieldTypeId, u => ""));
            }
            ds.Entry<FieldType>(bd).State = EntityState.Deleted;
            ds.FieldTypes.Remove(bd);
            return await ds.SaveChangesAsync();
        }
        public async Task<int> Delete(string id)
        {
            var r = await ds.FieldTypes.FirstOrDefaultAsync(c => c.Id == id);
            if (!ds.Database.IsInMemory())
            {
                ds.DocumentTypeFields.Where(t => t.FieldTypeId == id).ExecuteUpdate(u => u.SetProperty(u => u.FieldTypeId, u => "").SetProperty(u => u.Title, u => r.Name));
                ds.DocumentFields.Where(t => t.FieldTypeId == id).ExecuteUpdate(u => u.SetProperty(u => u.FieldTypeId, u => ""));
            }
            ds.Entry<FieldType>(r).State = EntityState.Deleted;
            ds.FieldTypes.Remove(r);
            return await ds.SaveChangesAsync();
        }
    }
}