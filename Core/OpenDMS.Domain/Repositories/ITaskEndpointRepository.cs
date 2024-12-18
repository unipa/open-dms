using OpenDMS.Domain.Entities.Workflow;

namespace OpenDMS.Domain.Repositories
{
    public interface ITaskEndpointRepository
    {
        CustomTaskEndpoint GetByURL(string serviceURL);
        public CustomTaskEndpoint GetById(string serviceId);

        List<CustomTaskEndpoint> GetAll ();
        void Add(CustomTaskEndpoint service);
        void Update(string serviceId, string tasks);
        void Delete(string serviceId);
        void Restore(string serviceId);
    }
}