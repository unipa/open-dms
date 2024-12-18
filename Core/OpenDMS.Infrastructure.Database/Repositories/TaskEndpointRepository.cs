using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Workflow;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories
{
    public class TaskEndpointRepository : ITaskEndpointRepository
    {
        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;

        public TaskEndpointRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }

        public List<CustomTaskEndpoint> GetAll()
        {
            return ds.CustomTaskEndpoints.AsNoTracking().ToList();
        }

        public CustomTaskEndpoint GetByURL(string serviceURL)
        {
            return ds.CustomTaskEndpoints.AsNoTracking().FirstOrDefault(c => c.Endpoint == serviceURL);
        }

        public CustomTaskEndpoint GetById(string serviceId)
        {
            return ds.CustomTaskEndpoints.AsNoTracking().FirstOrDefault(c => c.Id == serviceId);
        }


        public void Add(CustomTaskEndpoint service)
        {
            if (String.IsNullOrEmpty(service.Id)) throw new ArgumentNullException(nameof(service.Id));
            if (String.IsNullOrEmpty(service.Endpoint)) throw new ArgumentNullException(nameof(service.Endpoint));
            service.CreationDate = DateTime.UtcNow;
            ds.CustomTaskEndpoints.Add(service);
            ds.SaveChanges();
            ds.Entry<CustomTaskEndpoint>(service).State = EntityState.Detached;
        }



        public void Update(string serviceId, string tasks)
        {
            var c = ds.CustomTaskEndpoints.FirstOrDefault(c => c.Id == serviceId);
            c.Tasks = tasks;
            c.LastUpdate = DateTime.UtcNow;
            ds.CustomTaskEndpoints.Update(c);
            ds.SaveChanges();
            ds.Entry<CustomTaskEndpoint>(c).State = EntityState.Detached;
        }

        public void Delete(string serviceId)
        {
            var c = ds.CustomTaskEndpoints.FirstOrDefault(c => c.Id == serviceId);
            if (c != null)
            {
                c.Deleted = true;
                c.LastUpdate = DateTime.UtcNow;
            c.DeletionDate= DateTime.UtcNow;
            ds.CustomTaskEndpoints.Update(c);
            ds.SaveChanges();
            ds.Entry<CustomTaskEndpoint>(c).State = EntityState.Detached;
            }
        }

        public void Restore (string serviceId)
        {
            var c = ds.CustomTaskEndpoints.FirstOrDefault(c => c.Id == serviceId);
            c.Deleted = false;
            c.LastUpdate = DateTime.UtcNow;
            ds.CustomTaskEndpoints.Update(c);
            ds.SaveChanges();
            ds.Entry<CustomTaskEndpoint>(c).State = EntityState.Detached;
        }
    }


}
