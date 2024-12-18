
using OpenDMS.Domain.Entities.Workflow;
using OpenDMS.Workflow.API.DTOs.Palette;

namespace OpenDMS.Workflow.API.BusinessLogic.CustomTask
{
    public interface ICustomTaskProvider
    {

        List<TaskGroup> GetPalette();

        CustomTaskEndpoint GetByURL(string serviceURL);
        void Delete(string serviceURL);
        void Restore(string serviceURL);
        void Update(string serviceURL, string Tasks);
        bool ImportSwagger(string swaggerURL, byte[] swaggerContent);
        Task ImportPalette(IServiceProvider serviceProvider);

    }
}