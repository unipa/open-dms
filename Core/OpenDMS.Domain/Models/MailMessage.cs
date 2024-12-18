using OpenDMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Models
{
    public class CreateOrUpdateMailMessage
    {

        public int EntryId { get; set; }

        public int MailboxId { get; set; }

        public string FromUser { get; set; }
        public List<string> To { get; set; } = new();
        public List<string> CC { get; set; } = new();
        public List<string> CCr { get; set; } = new();

        public string Subject { get; set; }
        public string Body { get; set; }

        public List<int> Attachments { get; set; } = new();
        
        public bool LinkAttachments { get; set; } = false;
        public bool IncludePDFPreview { get; set; } = false;

        public DateTime SendDate { get; set; }

        public MailStatus Status { get; set; }

        /// <summary>
        /// True = l'invio avvieme in modo interattivo
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// In caso di errori la mail non viene memorizzata
        /// </summary>
        public bool AbortOnError { get; set; }
    }
}
