using OpenDMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class MailMessageEntry
    {
        public int Id { get; set; }
        public MailSubType SubType { get; set; }

        public MailStatus Status { get; set; }
        public string InternalMailAddress { get; set; }
        public string ExternalMailAddress { get; set; }
        public string MessageDate { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int NumberOfAttachments { get; set; }
        public bool IsSPAM { get; set; }
        public bool IsInfected { get; set; }
        public int DocumentId { get; set; }
        public string Company { get; set; }
        public string UserId { get; set; }
        public string ExternalContactId { get; set; }
        public string InternalName { get; set; }
        public string ExternalName { get; set; }
        public string ClaimUser{ get; set; }
        public string ClaimDate { get; set; }
        public string ArchiveUser { get; set; }
        public string ArchiveDate { get; set; }
        public string DeleteUser { get; set; }
        public string DeleteDate { get; set; }
        public MailSubType DeliveryStatus { get; set; }
    }
}
