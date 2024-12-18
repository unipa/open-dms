using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.Interfaces
{
    public interface IFormService
    {
        Task<FormSchema> GetByUid(string uid, UserProfile user);
        Task<FormSchema> GetById(int documentId, UserProfile user);
        Task<FormSchema> GetByTask(TaskItem TI, UserProfile user);
        Task<FormSchema> GetByImageId(DocumentInfo document, DocumentVersion image, UserProfile user);
        Task<List<FormSchema>> GetAll(UserProfile user);
        //Task<bool> Publish(int documentId, int versionNumber, int revisionNumber, UserProfile user);
    }
}
