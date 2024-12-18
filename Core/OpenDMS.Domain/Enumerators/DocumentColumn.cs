using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Enumerators
{
    public static class DocumentColumn
    {
        public const string Permissions = "Document.Permissions";

        public const string Id = "Document.Id";
        public const string Rank = "Document.Rank";
        public const string ContentType = "Document.ContentType";
        public const string DocumentDate = "Document.DocumentDate";
        public const string Description = "Document.Description";
        public const string DocumentNumber = "Document.DocumentNumber";
        public const string Company = "Document.Company";
        public const string DocumentType = "Document.DocumentType";
        public const string ExpirationDate = "Document.ExpirationDate";
        public const string FileName = "Document.FileName";
        public const string Status = "Document.Status";
        public const string FileSize = "Document.FileSize";
        public const string CreationDate = "Document.CreationDate";
        public const string Owner = "Document.Owner";
        public const string Revision = "Document.RevisionNumber";
        public const string Version = "Document.VersionNumber";
        public const string VersionNumber = "Document.Version";
        public const string Indexing = "Document.IndexingStatus";
        public const string Preservation = "Document.Preservation";
        public const string Signature = "Document.SignatureStatus";
        public const string Preview = "Document.PreviewStatus";
        public const string Sending = "Document.SendingStatus";
        public const string ProtocolNumber = "Document.ProtocolNumber";
        public const string ProtocolFormattedNumber = "Document.ProtocolFormattedNumber";
        public const string ProtocolDate = "Document.ProtocolDate";
        public const string ViewDate = "Document.ViewUser.";
        public const string ExternalId = "Document.ExternalId";
        public const string SendingDate = "Document.SendingDate";
        public const string PreservationDate = "Document.PreservationDate";
        public const string CancellationDate = "Document.CancellationDate";
        public const string PDV = "Document.PDV";
        public const string Icon = "Document.Icon";
        public const string IconColor = "Document.Color";
        public const string Folder = "Document.FolderId";

        public const string FreeText = "Document.Freetext";
        public const string Parent = "Document.Parent";

        // * Campi Dinamici * //
        public const string Meta = "Document.Meta.";
        public const string Field = "Document.Field.";


        public static List<string> RankIcons = new() {
            "",
            "<i class='rank0'></i>",
            "<i class='rank1'></i>",
            "<i class='rank2'></i>",
            "<i class='rank3'></i>",
            "<i class='rank4'></i>",
            "<i class='rank5'></i>",
            "<i class='rank6'></i>"
        };

        public static List<string> StatusIcons = new() {
            "",
            "",
            "<i class='fa fa-pencil'></i>",
            "<i class='fa fa-hdd-o'></i>",
            "<i class='fa fa-lock'></i>",
            "<i class='fa fa-trash-o'></i>",
        };

        public static List<string> JobStatusIcons = new() {
            "<i class='fa fa-clock-o' style='color:#aab'></i>",
            "<i class='fa fa-cog' style='color:#a88ce5'></i>",
            "<i class='fa fa-check-sign' style='color:#4d4'></i>",
            "<i class='fa fa-bug' style='color:#edb3be'></i>",
            "<i class='fa fa-stop' style='color:#aab'></i>",
            "<i class='fa fa-eye-slash' style='color:#aab'></i>",
            ""
        };

        public static List<string> StatusTooltips = new() {
            "",  
            "Attivo",
            "Bozza",
            "Conservabile",
            "Conservato",
            "Cancellato"
        };
        public static List<string> JobStatusTooltips = new() {
            "In Attesa",
            "In Corso",
            "Completata",
            "Fallita",
            "Annullata",
            "Ignorata",
            "Non Necessaria"
        };
        public static List<string> TaskStatusTooltips = new() {
            "Non Assegnata",
            "Assegnata",
            "In Corso",
            "Sospesa",
            "Eseguita"
        };
    }
}
