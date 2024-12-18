using OpenDMS.Domain;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using Org.BouncyCastle.Asn1.Esf;

namespace OpenDMS.Infrastructure.Database.Builder
{
    public partial class QueryBuilder
    {

        public string[] getTablePrefix(string fieldId)
        {
            switch (fieldId)
            {
                case DocumentColumn.Permissions: return null;
                case DocumentColumn.Company:
                    return new[] { "-D", "D-C" };
                case DocumentColumn.DocumentNumber:
                case DocumentColumn.ProtocolNumber:
                case DocumentColumn.ProtocolFormattedNumber:
                case DocumentColumn.Folder:
                case DocumentColumn.Parent:
                case DocumentColumn.CreationDate:
                case DocumentColumn.DocumentDate:
                case DocumentColumn.ProtocolDate:
                case DocumentColumn.Description:
                case DocumentColumn.Id:
                case DocumentColumn.Owner:
                case DocumentColumn.ExternalId:
                    return new[] { "-D" };
                case DocumentColumn.ContentType:
                    return new[] { "-D", "D-I" };
                case DocumentColumn.DocumentType:
                    return new[] { "-D", "D-TP" };
                case DocumentColumn.VersionNumber:
                case DocumentColumn.FileName:
                case DocumentColumn.FileSize:
                case DocumentColumn.Indexing:
                case DocumentColumn.Preservation:
                case DocumentColumn.Revision:
                case DocumentColumn.Sending:
                case DocumentColumn.Signature:
                case DocumentColumn.Version:
                case DocumentColumn.Preview:
                case DocumentColumn.SendingDate:
                case DocumentColumn.PreservationDate:
                case DocumentColumn.CancellationDate:
                case DocumentColumn.PDV:
                    return new[] { "-D", "D-I" };

                case TaskColumn.Permissions: return null;
                case TaskColumn.Id:
                case TaskColumn.Assignment:
                case TaskColumn.Group:
                case TaskColumn.Role:
                case TaskColumn.Status:
                case TaskColumn.User:
                case TaskColumn.CreationDate:
                case TaskColumn.ExpirationDate:
                case TaskColumn.Attachment:
                case TaskColumn.FromUser:
                case TaskColumn.UserPercentage:
                case TaskColumn.Percentage:
                case TaskColumn.Parent:
                case TaskColumn.EventId:
                case TaskColumn.FormKey:
                case TaskColumn.TaskType:
                case TaskColumn.Title:
                    return new[] { "-UT" , "UT-TI"};
                case TaskColumn.Category:
                    return new[] { "-UT", "UT-TI", "TI-CT" };
                case TaskColumn.Company:
                    return new[] { "-UT", "UT-TI", "TI-C" };
                case TaskColumn.Priority:
                    return new[] { "-UT", "UT-TI", "TI-PR" };

                case TaskColumn.Destinatario:
                    return new[] { "-UT", "UT-DEST" };


                case ProcessColumn.Permissions: return null;
                case ProcessColumn.Id:
                case ProcessColumn.Name:
                case ProcessColumn.SchemaId: return new[] { "-P" };

                case ProcessColumn.ClaimDate:
                case ProcessColumn.ClaimTime:
                case ProcessColumn.CreationDate:
                case ProcessColumn.DelayTime:
                case ProcessColumn.EventId:
                case ProcessColumn.ExecutionDate:
                case ProcessColumn.ExecutionTime:
                case ProcessColumn.ExpirationDate:
                case ProcessColumn.ExpirationTime:
                case ProcessColumn.JobTime:
                case ProcessColumn.TaskId:
                case ProcessColumn.FromUser:
                case ProcessColumn.Group:
                case ProcessColumn.Role:
                case ProcessColumn.User:
                case ProcessColumn.Percentage:
                case ProcessColumn.Title:
                case ProcessColumn.Status:
                case ProcessColumn.TaskType:
                case ProcessColumn.Assignment: return new[] { "-P", "P-VUT" };
                case ProcessColumn.Category: return new[] { "-P", "P-VUT", "VUT-CT" };
                case ProcessColumn.Company: return new[] { "-P", "P-VUT", "VUT-C" };
                case ProcessColumn.Priority: return new[] { "-P", "P-VUT", "VUT-PR" };


                default:
                    if (fieldId.StartsWith(ProcessColumn.Variables)) return new[] { "-P", "P-PD" };
                    if (fieldId.StartsWith(ProcessColumn.Dimensions)) return new[] { "-P", "P-PL" };
                    if (fieldId.StartsWith(DocumentColumn.Meta)) return new[] { "-D", "D-Meta" };
                    if (fieldId.StartsWith(DocumentColumn.Field)) return new[] { "-D", "D-Field" };
                    if (fieldId.StartsWith(DocumentColumn.ViewDate)) return new[] { "-D" };
                    return null;
            }
        }

        public string[] getSelectFields(string fieldId)
        {
            var model = fieldId.Split('.')[0];
            switch (fieldId)
            {
                case DocumentColumn.Rank: return new[] { "1" };
                case DocumentColumn.Id: return new[] { "D.Id" };
                case DocumentColumn.Status: return new[] { "D.DocumentStatus" };
                case DocumentColumn.Permissions: return null;
                case DocumentColumn.Company:
                    return new[] { "D.CompanyId", "C.Description" };
                case DocumentColumn.ContentType:
                    return new[] { "D.ContentType", "D.Icon", "D.IconColor", "I.OriginalFileName" };
                case DocumentColumn.DocumentNumber:
                    return new[] { "D.DocumentNumber", "D.DocumentFormattedNumber" };
                case DocumentColumn.DocumentType:
                    return new[] { "D.DocumentTypeId", "TP.Name" };
                case DocumentColumn.ProtocolNumber:
                    return new[] { "D.ProtocolNumber" };
                case DocumentColumn.ProtocolFormattedNumber:
                    return new[] { "D.ProtocolNumber" };
                case DocumentColumn.VersionNumber:
                    return new[] { "I.VersionNumber", "I.RevisionNumber" };
                case DocumentColumn.FileName:
                    return new[] { "I.OriginalFileName" };
                case DocumentColumn.Parent:
                case DocumentColumn.Folder:
                    return new[] { "D.FolderId" };
                case DocumentColumn.Owner:
                    return new[] { "D.Owner" };
                case DocumentColumn.CreationDate:
                    return new[] { "D.CreationDate" };
                case DocumentColumn.DocumentDate:
                    return new[] { "D.DocumentDate" };
                case DocumentColumn.ProtocolDate:
                    return new[] { "D.ProtocolDate" };
                case DocumentColumn.ExpirationDate:
                    return new[] { "D.ExpirationDate" };
                case DocumentColumn.Description:
                    return new[] { "D.Description" };
                case DocumentColumn.ExternalId:
                    return new[] { "D.ExternalId" };
                case DocumentColumn.FileSize:
                    return new[] { "I.FileSize" };
                case DocumentColumn.Indexing:
                    return new[] { "I.IndexingStatus" };
                case DocumentColumn.Preservation:
                    return new[] { "I.PreservationStatus" };
                case DocumentColumn.Revision:
                    return new[] { "I.RevisionNumber" };
                case DocumentColumn.Sending:
                    return new[] { "I.SendingStatus" };
                case DocumentColumn.Signature:
                    return new[] { "I.SignatureStatus" };
                case DocumentColumn.Version:
                    return new[] { "I.VersionNumber" };
                case DocumentColumn.Preview:
                    return new[] { "I.PreviewStatus" };

                case DocumentColumn.SendingDate:
                    return new[] { "I.SendingDate" };
                case DocumentColumn.PreservationDate:
                    return new[] { "I.PreservationDate" };
                case DocumentColumn.CancellationDate:
                    return new[] { "I.CancellationDate" };
                case DocumentColumn.PDV:
                    return new[] { "I.PDV" };


                case TaskColumn.Permissions: return null;
                case TaskColumn.Id:
                    return new[] { "UT.Id" };
                case TaskColumn.Assignment:
                    return new[] { "UT.GroupId", "UT.RoleId", "UT.UserId" };
                case TaskColumn.Category:
                    return new[] { "TI.CategoryId", "CT.Description" };
                case TaskColumn.Company:
                    return new[] { "TI.CompanyId", "C.Description" };
                case TaskColumn.FromUser:
                    return new[] { "TI.FromUserId" };
                case TaskColumn.Group:
                    return new[] { "UT.GroupId" };
                case TaskColumn.Role:
                    return new[] { "UT.RoleId" };
                case TaskColumn.Priority:
                    return new[] { "TI.Priority", "PR.Description" };
                case TaskColumn.Status:
                    return new[] { "UT.Status", "UT.CC", "UT.GroupId", "UT.RoleId" };
                case TaskColumn.TaskType:
                    return new[] { "TI.TaskType" };
                case TaskColumn.Title:
                    return new[] { "TI.Title" };
                case TaskColumn.Description:
                    return new[] { "TI.Description" };
                case TaskColumn.User:
                    return new[] { "UT.UserId" };
                case TaskColumn.CreationDate:
                    return new[] { "UT.CreationDate" };
                case TaskColumn.ExpirationDate:
                    return new[] { "UT.ExpirationDate" };
                case TaskColumn.Percentage:
                    return new[] { "TI.ExecutionPercentage" };
                case TaskColumn.UserPercentage:
                    return new[] { "UT.Percentage" };
                case TaskColumn.Parent:
                    return new[] { "TI.ParentId" };
                case TaskColumn.EventId:
                    return new[] { "TI.EventId" };
                case TaskColumn.FormKey:
                    return new[] { "TI.FormKey" };

                case TaskColumn.Destinatario:
                    return new[] { "Coalesce(DEST.Id/DEST.Id,0)" };
                case TaskColumn.Attachment:
                    return new[] { "(SELECT COUNT(TA.DocumentId) FROM TaskItems TI LEFT JOIN TaskAttachments TA ON (TI.Id=TA.TaskItemId) WHERE (UT.TaskItemId=TI.Id))" };


                case ProcessColumn.Permissions: return null;
                case ProcessColumn.Id: return new[] { "P.Id" };
                case ProcessColumn.Name: return new[] { "P.Name" };
                case ProcessColumn.SchemaId: return new[] { "P.SchemaId" };
                case ProcessColumn.ClaimDate: return new[] { "VUT.ClaimDate" };
                case ProcessColumn.ClaimTime: return new[] { "VUT.ClaimTime" };
                case ProcessColumn.CreationDate: return new[] { "VUT.CreationDate" };
                case ProcessColumn.DelayTime: return new[] { "VUT.DelayTime" };
                case ProcessColumn.EventId: return new[] { "VUT.EventId" };
                case ProcessColumn.ExecutionDate: return new[] { "VUT.ExecutionDate" };
                case ProcessColumn.ExecutionTime: return new[] { "VUT.ExecutionTime" };
                case ProcessColumn.ExpirationDate: return new[] { "VUT.ExpirationDate" };
                case ProcessColumn.ExpirationTime: return new[] { "VUT.ExpirationTime" };
                case ProcessColumn.JobTime: return new[] { "VUT.JobTime" };
                case ProcessColumn.TaskId: return new[] { "VUT.TaskId" };
                case ProcessColumn.FromUser: return new[] { "VUT.FromUserId" };
                case ProcessColumn.Group: return new[] { "VUT.GroupId" };
                case ProcessColumn.Role: return new[] { "VUT.RoleId" };
                case ProcessColumn.User: return new[] { "VUT.UserId" };
                case ProcessColumn.Percentage: return new[] { "VUT.Percentage" };
                case ProcessColumn.Title: return new[] { "VUT.Title" };
                case ProcessColumn.Assignment: return new[] { "VUT.Assignment" };
                case ProcessColumn.Category: return new[] { "VUT.CategoryId", "CT.Description" };
                case ProcessColumn.Company: return new[] { "VUT.CompanyId", "C.Description" };
                case ProcessColumn.Priority: return new[] { "VUT.Priority", "PR.Description" };
                case ProcessColumn.Status: return new[] { "VUT.Status" };
                case ProcessColumn.TaskType: return new[] { "VUT.TaskType" };


                default:
                    if (fieldId.StartsWith(ProcessColumn.Variables) ||
                        fieldId.StartsWith(ProcessColumn.Dimensions) ||
                        fieldId.StartsWith(DocumentColumn.Meta) ||
                        fieldId.StartsWith(DocumentColumn.Field))
                    {
                        var P = "F" + fieldId.Split('.')[2].Replace("-", "").Replace("$", "").Replace("%", "").Replace(" ", "").Replace(".", "").Replace(",", "");
                        return new[] { $"{P}.Value", $"{P}.LookupValue" };
                    }
                    else
                    if (fieldId.StartsWith(DocumentColumn.ViewDate))
                        return new[] { "(SELECT MAX(H.CreationDate) FROM Histories H INNER JOIN HistoryDocuments HD ON (H.Id=HD.EntryId) WHERE (D.Id=HD.DocumentId)AND(H.EventType=" + EventType.View.Quoted() + ")AND(H.UserId=" + fieldId.Split('.')[2].Quoted() + "))" };

                    return null;// new[] { getTablePrefix(fieldId) + "." + fieldId.Split(',')[1] };
            }
        }

        public string[] getSortFields(string fieldId)
        {
            //var model = fieldId.Split('.')[0];
            switch (fieldId)
            {
                case DocumentColumn.Company:
                    return new[] { "C.Description" };
                case DocumentColumn.ContentType:
                    return new[] { "D.ContentType" };
                case DocumentColumn.DocumentNumber:
                    return new[] { "D.DocumentFormattedNumber" };
                case DocumentColumn.DocumentType:
                    return new[] { "TP.Name" };
                case DocumentColumn.ProtocolNumber:
                    return new[] { "D.ProtocolNumber" };
                case DocumentColumn.ProtocolFormattedNumber:
                    return new[] { "D.ProtocolNumber" };
                case DocumentColumn.VersionNumber:
                    return new[] { "I.VersionNumber", "I.RevisionNumber" };
                case DocumentColumn.FileName:
                    return new[] { "I.OriginalFileName" };
                case DocumentColumn.Folder:
                case DocumentColumn.Parent:
                    return null;
                case DocumentColumn.Owner:
                    return new[] { "D.Owner" };
                case DocumentColumn.CreationDate:
                    return new[] { "D.CreationDate" };
                case DocumentColumn.DocumentDate:
                    return new[] { "D.DocumentDate" };
                case DocumentColumn.ProtocolDate:
                    return new[] { "D.ProtocolDate" };
                case DocumentColumn.ExpirationDate:
                    return new[] { "D.ExpirationDate" };
                case DocumentColumn.Description:
                    return new[] { "D.Description" };
                case DocumentColumn.ExternalId:
                    return new[] { "D.ExternalId" };
                case DocumentColumn.FileSize:
                    return new[] { "I.FileSize" };
                case DocumentColumn.Indexing:
                    return new[] { "I.IndexingStatus" };
                case DocumentColumn.Preservation:
                    return new[] { "I.PreservationStatus" };
                case DocumentColumn.Revision:
                    return new[] { "I.RevisionNumber" };
                case DocumentColumn.Sending:
                    return new[] { "I.SendingStatus" };
                case DocumentColumn.Signature:
                    return new[] { "I.SignatureStatus" };
                case DocumentColumn.Version:
                    return new[] { "I.VersionNumber" };
                case DocumentColumn.Preview:
                    return new[] { "I.PreviewStatus" };

                case DocumentColumn.SendingDate:
                    return new[] { "I.SendingDate" };
                case DocumentColumn.PreservationDate:
                    return new[] { "I.PreservationDate" };
                case DocumentColumn.CancellationDate:
                    return new[] { "I.CancellationDate" };
                case DocumentColumn.PDV:
                    return new[] { "I.PDV" };



                case TaskColumn.Id:
                    return new[] { "UT.Id" };
                case TaskColumn.Assignment:
                    return null;
                case TaskColumn.Category:
                    return new[] { "CT.Description" };
                case TaskColumn.Company:
                    return new[] { "C.Description" };
                case TaskColumn.FromUser:
                    return new[] { "TI.FromUserId" };
                case TaskColumn.Group:
                    return new[] { "UT.GroupId" };
                case TaskColumn.Role:
                    return new[] { "UT.RoleId" };
                case TaskColumn.Priority:
                    return new[] { "PR.Description" };
                case TaskColumn.Status:
                    return null;
                case TaskColumn.EventId:
                    return new[] { "TI.EventId" };
                case TaskColumn.FormKey:
                    return new[] { "TI.FormKey" };
                case TaskColumn.TaskType:
                    return new[] { "TI.TaskType" };
                case TaskColumn.Title:
                    return new[] { "TI.Title" };
                case TaskColumn.Description:
                    return new[] { "TI.Description" };
                case TaskColumn.User:
                    return new[] { "UT.UserId" };
                case TaskColumn.CreationDate:
                    return new[] { "UT.CreationDate" };
                case TaskColumn.ExpirationDate:
                    return new[] { "UT.ExpirationDate" };
                case TaskColumn.Percentage:
                    return new[] { "TI.ExecutionPercentage" };
                case TaskColumn.UserPercentage:
                    return new[] { "UT.Percentage" };
                case TaskColumn.Parent:
                    return new[] { "TI.ParentId" };

                case TaskColumn.Destinatario:
                    return null;
                case TaskColumn.Attachment:
                    return null;


                case ProcessColumn.Permissions: return null;
                case ProcessColumn.Id: return new[] { "P.Id" };
                case ProcessColumn.Name: return new[] { "P.Name" };
                case ProcessColumn.SchemaId: return new[] { "P.SchemaId" };
                case ProcessColumn.ClaimDate: return new[] { "VUT.ClaimDate" };
                case ProcessColumn.ClaimTime: return new[] { "VUT.ClaimTime" };
                case ProcessColumn.CreationDate: return new[] { "VUT.CreationDate" };
                case ProcessColumn.DelayTime: return new[] { "VUT.DelayTime" };
                case ProcessColumn.EventId: return new[] { "VUT.EventId" };
                case ProcessColumn.ExecutionDate: return new[] { "VUT.ExecutionDate" };
                case ProcessColumn.ExecutionTime: return new[] { "VUT.ExecutionTime" };
                case ProcessColumn.ExpirationDate: return new[] { "VUT.ExpirationDate" };
                case ProcessColumn.ExpirationTime: return new[] { "VUT.ExpirationTime" };
                case ProcessColumn.JobTime: return new[] { "VUT.JobTime" };
                case ProcessColumn.TaskId: return new[] { "VUT.TaskId" };
                case ProcessColumn.FromUser: return new[] { "VUT.FromUserId" };
                case ProcessColumn.Group: return new[] { "VUT.GroupId" };
                case ProcessColumn.Role: return new[] { "VUT.RoleId" };
                case ProcessColumn.User: return new[] { "VUT.UserId" };
                case ProcessColumn.Percentage: return new[] { "VUT.Percentage" };
                case ProcessColumn.Title: return new[] { "VUT.Title" };
                case ProcessColumn.Assignment: return new[] { "VUT.Assignment" };
                case ProcessColumn.Category: return new[] { "CT.Description" };
                case ProcessColumn.Company: return new[] { "C.Description" };
                case ProcessColumn.Priority: return new[] { "PR.Description" };
                case ProcessColumn.Status: return new[] { "VUT.Status" };
                case ProcessColumn.TaskType: return new[] { "VUT.TaskType" };


                default:
                    if (fieldId.StartsWith(ProcessColumn.Variables) ||
                        fieldId.StartsWith(ProcessColumn.Dimensions) ||
                        fieldId.StartsWith(DocumentColumn.Meta) ||
                        fieldId.StartsWith(DocumentColumn.Field))
                    {
                        var P = "F" + fieldId.Split('.')[2].Replace("-", "").Replace("$", "").Replace("%", "").Replace(" ", "").Replace(".", "").Replace(",", "");
                        return new[] { $"{P}.LookupValue" };
                    }
                    else
                    if (fieldId.StartsWith(DocumentColumn.ViewDate))
                        return new[] { "(SELECT MAX(H.CreationDate) FROM Histories H INNER JOIN HistoryDocuments HD ON (H.Id=HD.EntryId) WHERE (D.Id=HD.DocumentId)AND(H.EventType=" + EventType.View.Quoted() + ")AND(H.UserId=" + fieldId.Split('.')[2].Quoted() + "))" };
                    else
                        return null;
            }
        }

        public string GetTableRelations(string  relationId, string fieldId)
        {
            var parts = fieldId.Split('.');
            switch (relationId)
            {
                case "-D": return "Documents D";
                case "-UT": return "UserTasks UT";
                case "-TI": return "TaskItems TI";
                case "-C": return "Companies C";
                case "-I": return "Images I";
                case "-P": return "vProcesses P";
                case "-T": return "vUserTasks T";
                case "D-C":
                    return "LEFT JOIN Companies C ON (D.CompanyId=C.Id)";
                case "D-I":
                    return "LEFT JOIN Images I ON (D.ImageId=I.Id)";
                case "D-TP":
                    return "LEFT JOIN DocumentTypes TP ON (D.DocumentTypeId=TP.Id)";
                case "C-D":
                    return "LEFT JOIN Documents D ON (C.Id=D.CompanyId)";
                case "I-D":
                    return "LEFT JOIN Documents D ON (I.Id=D.ImageId)";

                case "UT-TI":
                    return "LEFT JOIN TaskItems TI ON (UT.TaskItemId=TI.Id)";
                case "TI-UT":
                    return "LEFT JOIN UserTasks UT ON (TI.Id=UT.TaskItemId)";
                case "UT-C":
                    return "LEFT JOIN Companies C ON (TI.CompanyId=C.Id)";
                case "TI-CT":
                    return "LEFT JOIN LookupTables CT ON (TI.CategoryId=CT.Id AND CT.TableId='" + TaskConstants.CONST_TASK_GROUPS + "')";
                case "TI-PR":
                    return "LEFT JOIN LookupTables PR ON (TI.Priority=PR.Id AND PR.TableId='" + TaskConstants.CONST_TASK_PRIORITIES + "')";
                case "UT-DEST":
                    var tuid = Var("uid");// filter.Values[0];
                    var troles = Var("roles");// filter.Values[1];
                    var groles = Var("globalroles");// filter.Values[1];
                    var tgroups = Var("groups");// filter.Values[2];
                    var tcompanies = Var("companies");// filter.Values[4];
                    if (string.IsNullOrEmpty(tcompanies)) tcompanies = "0,99999";

                    if (string.IsNullOrEmpty(troles)) troles = "''";
                    if (string.IsNullOrEmpty(groles)) groles = "''";
                    if (string.IsNullOrEmpty(tgroups)) tgroups = "''";

                    return $"LEFT JOIN UserTasks DEST ON (UT.Id = DEST.Id) AND (DEST.UserId={tuid} OR ((CONCAT(DEST.RoleId,CONCAT('\\', DEST.GroupId))) IN ({troles}) AND (DEST.RoleId<>'') AND (DEST.GroupId<>'')) OR (DEST.RoleId IN ({groles}) AND (DEST.RoleId<>'') AND (DEST.GroupId='')) OR (DEST.GroupId IN ({tgroups}) AND DEST.RoleId='' AND DEST.GroupId<>'') )";
                case "P-VUT":
                    return "LEFT JOIN vUserTasks VUT ON (VUT.ProcessId = P.Id)";
                case "VUT-C":
                    return "LEFT JOIN Companies C ON (VUT.CompanyId=C.Id)";
                case "VUT-CT":
                    return "LEFT JOIN LookupTables CT ON (VUT.CategoryId=CT.Id AND CT.TableId='" + TaskConstants.CONST_TASK_GROUPS + "')";
                case "VUT-PR":
                    return "LEFT JOIN LookupTables PR ON (VUT.Priority=PR.Id AND PR.TableId='" + TaskConstants.CONST_TASK_PRIORITIES + "')";
                default:
                    var split = fieldId.Split('.');
                    var alias  = "F" + split[2].Replace("-", "").Replace("$", "").Replace("%", "").Replace(" ", "").Replace(".", "").Replace(",", "");
                    if (relationId == "D-Meta")
                        return $"LEFT JOIN DocumentFields {alias} ON (D.Id={alias}.DocumentId AND {alias}.Chunk=1 AND {alias}.FieldTypeId={split[2].Quoted()})";
                    else
                    if (relationId == "D-Field")
                        return $"LEFT JOIN DocumentFields {alias} ON (D.Id={alias}.DocumentId AND {alias}.Chunk=1 AND {alias}.FieldName={split[2].Quoted()})";
                    else
                    if (relationId == "P-PD")
                        return $"LEFT JOIN ProcessData {alias} ON (P.Id={alias}.ProcessId AND {alias}.VariableId={split[2].Quoted()})";
                    else
                        if (relationId == "P-PL")
                        return $"LEFT JOIN ProcessData {alias} ON (P.Id={alias}.ProcessId AND {alias}.VariableName={split[2].Quoted()})";
                    break;
            }
            return "";
        }


        public void getFilter(FilterRule filter)//, string[] Variables)
        {
            var v = filter.FirstValue;
            var id = filter.Element;
//            var model = id.Split(',')[0];
            switch (id)
            {
                case DocumentColumn.Id:
                case DocumentColumn.Status:
                case DocumentColumn.FileSize:
                case DocumentColumn.Revision:
                case DocumentColumn.Version:
                case DocumentColumn.Indexing:
                case DocumentColumn.Preservation:
                case DocumentColumn.Signature:
                case DocumentColumn.Preview:
                case DocumentColumn.Sending:
                case DocumentColumn.ProtocolNumber:

                case TaskColumn.Destinatario:
                case TaskColumn.Id:
                case TaskColumn.Assignment:
                case TaskColumn.Percentage:
                case TaskColumn.UserPercentage:
                case TaskColumn.Status:
                case TaskColumn.TaskType:
                case TaskColumn.Parent:

                case ProcessColumn.Id:
                case ProcessColumn.TaskId:
                case ProcessColumn.Percentage:
                case ProcessColumn.Assignment:
                case ProcessColumn.Status:
                case ProcessColumn.TaskType:
                case ProcessColumn.DelayTime:
                case ProcessColumn.ExecutionTime:
                case ProcessColumn.ExpirationTime:
                case ProcessColumn.JobTime:

                    sqlBuilder.AddNumericFilter(getSelectFields(id)[0], filter.OperatorType, filter.Values);
                    break;

                case ProcessColumn.Permissions:

                    var puid = filter.Values[0];
                    var proles = filter.Values[1];
                    var pgroups = filter.Values[2];
                    if (string.IsNullOrEmpty(proles)) proles = "''";
                    if (string.IsNullOrEmpty(pgroups)) pgroups = "''";

                    sqlBuilder.AddFilter(@$"
(
    (
	    SELECT COUNT(*) FROM ProcessPermissions R 
		WHERE (
                (R.ProfileId = ({puid}) AND R.ProfileType=0)
                OR
                (R.ProfileId in ({proles}) AND R.ProfileType=2)
                OR
				(R.ProfileId in ({pgroups}) AND R.ProfileType=1)
              )
    ) > 0 
)

");
                    break;


                case DocumentColumn.ContentType:
                case DocumentColumn.Description:
                case DocumentColumn.DocumentNumber:
                case DocumentColumn.Company:
                case DocumentColumn.FileName:
                case DocumentColumn.Owner:
                case DocumentColumn.ProtocolFormattedNumber:
                case DocumentColumn.ExternalId:
                case DocumentColumn.Icon:
                case DocumentColumn.IconColor:
                case DocumentColumn.PDV:

                case TaskColumn.Category:
                case TaskColumn.Company:
                case TaskColumn.Description:
                case TaskColumn.FromUser:
                case TaskColumn.Group:
                case TaskColumn.Role:
                case TaskColumn.Title:
                case TaskColumn.User:
                case TaskColumn.EventId:
                case TaskColumn.FormKey:

                case ProcessColumn.Name:
                case ProcessColumn.SchemaId:
                case ProcessColumn.EventId:
                case ProcessColumn.FromUser:
                case ProcessColumn.Group:
                case ProcessColumn.Role:
                case ProcessColumn.User:
                case ProcessColumn.Title:
                case ProcessColumn.Category:
                case ProcessColumn.Company:
                case ProcessColumn.Priority:
                    sqlBuilder.AddTextFilter(getSelectFields(id)[0], filter.OperatorType, filter.Values);
                    break;


                case DocumentColumn.DocumentDate:
                case DocumentColumn.ExpirationDate:
                case DocumentColumn.CreationDate:
                case DocumentColumn.ProtocolDate:
                case DocumentColumn.ViewDate:
                case DocumentColumn.SendingDate:
                case DocumentColumn.PreservationDate:
                case DocumentColumn.CancellationDate:

                case TaskColumn.CreationDate:
                case TaskColumn.ExpirationDate:

                case ProcessColumn.ClaimDate:
                case ProcessColumn.ClaimTime:
                case ProcessColumn.CreationDate:
                case ProcessColumn.ExecutionDate:
                case ProcessColumn.ExpirationDate:
                    sqlBuilder.AddDateFilter(getSelectFields(id)[0], filter.OperatorType, filter.Values);
                    break;


                case DocumentColumn.FreeText:
                    v = ("%" + v.Replace(" ", "%") + "%").Quoted();
                    sqlBuilder.AddFilter(@$"(D.Description LIKE {v})
                    OR(D.DocumentFormattedNumber LIKE {v})
                    OR(I.FileName LIKE {v})
                    OR(D.Id IN (SELECT DocumentId FROM DocumentFields DF WHERE DF.LookupValue LIKE {v}))");
                    break;
                case DocumentColumn.Folder:
                case DocumentColumn.Parent:
                    sqlBuilder.AddFilter(@$"(D.FolderId={v})OR(D.Id IN (SELECT FC.DocumentId FROM FolderContents FC WHERE FC.FolderId={v}))");
                    break;
                case DocumentColumn.DocumentType:
                    v = v != null ? string.Join(",", filter.Values.Select(c => c.Quoted())) : null;
                    sqlBuilder.AddFilter(filter.Values[0] != "''" && !String.IsNullOrEmpty(filter.Values[0]) ? @$"(D.DocumentTypeId IN ({v}))" : @$"(D.DocumentTypeId IS NULL OR D.DocumentTypeId='')");
                    break;
                case DocumentColumn.Rank:
                    v = v != null ? string.Join(",", filter.Values.Select(c => c.Quoted())) : null;
                    sqlBuilder.AddFilter($"(D.Id IN ({v}))");
                    break;


                case TaskColumn.Attachment:
                    sqlBuilder.AddFilter($"TA.DocumentId={filter.FirstValue}");
                    break;
                case TaskColumn.Direction:
                    bool Inbox = filter.Values[0] == "0";
                    if (Inbox)
                        sqlBuilder.AddFilter($"Coalesce(DEST.Id/DEST.Id,0)=1");
                    else
                        sqlBuilder.AddFilter($"Coalesce(DEST.Id/DEST.Id,0)=0");
                    break;
                case TaskColumn.FreeText:
                    v = ("%" + v.Replace(" ", "%") + "%").Quoted();
                    sqlBuilder.AddFilter(@$"(TI.Title LIKE {v})
                    OR(TI.Description LIKE {v})
                    OR(TI.FromUserId LIKE {v})
                    OR(EXISTS
                        (SELECT TA.TaskItemId 
                            FROM TaskAttachments TA 
                            LEFT JOIN Documents DA ON (DA.Id=TA.DocumentId)
                            LEFT JOIN DocumentFields DF ON (DF.DocumentId=DA.Id) 
                            WHERE (TA.TaskItemId=TI.Id)AND((DA.Description LIKE {v})OR(DF.LookupValue LIKE {v}))
                        )
                      )");
                    break;


                case DocumentColumn.Permissions:
                    var uid = Var("uid");// filter.Values[0];
                    var roles = Var("roles");// filter.Values[1];
                    var droles = Var("globalroles");// filter.Values[1];
                    var groups = Var("groups");// filter.Values[2];
                    var companies = Var("companies");// filter.Values[4];
                    var date = Var("date");
                    var Permission = Var("permission").Quoted();

                    if (string.IsNullOrEmpty(companies)) companies = "0,99999";
                    if (string.IsNullOrEmpty(roles)) roles = "''";
                    if (string.IsNullOrEmpty(droles)) droles = "''";
                    if (string.IsNullOrEmpty(groups)) groups = "''";

                    //var uid = filter.Values[0];
                    //var roles = filter.Values[1];
                    //var groups = filter.Values[2];
                    //var date = filter.Values[3];
                    //var Permission = filter.Values[4].Quoted();
                    //var companies = filter.Values[5];
                    //if (string.IsNullOrEmpty(companies)) companies = "0,99999";

                    sqlBuilder.AddFilter(@$"
(D.CompanyId IN ({companies})) 
AND 
(
	/* Verifico che non esista una negazione per l'utente sul documento*/
	NOT EXISTS 
	(
		SELECT 1 FROM DocumentPermissions DPA WHERE(DPA.DocumentId = D.Id)AND(DPA.ProfileId ={uid})AND(DPA.ProfileType = 0)AND(DPA.PermissionId ={Permission})AND(DPA.Authorization=2) 
    )
)
AND
(
	/* Verifico che l'utente disponga di un permesso sulle ACL che non sia revocato nelle singole istanze documentali */
	EXISTS  
	(
        SELECT 1 FROM ACLPermissions A 
            WHERE (
                    ((A.ProfileId in ({roles}) AND A.ProfileType = 2) OR (A.ProfileId in ({groups}) AND A.ProfileType = 1) OR (A.ProfileId in ({uid}) AND A.ProfileType = 0))

                AND (
                    NOT EXISTS 
                      (
                         SELECT 1 FROM DocumentPermissions DP
                         WHERE 
                            (DP.ProfileId = A.ProfileId)
                            AND (DP.ProfileType = A.ProfileType)
                            AND (DP.Authorization = 2)
                            AND (DP.DocumentId = D.Id)
							AND (DP.PermissionId = {Permission})
                         
                      )
                    )
                AND (A.Authorization = 1)
				AND (A.ACLId = D.ACLId  OR A.ACLId = '$GLOBAL$')
			    AND (A.PermissionId ={Permission})
			) 
	) 
	OR
	/* Verifico che l'utente disponga di un permesso sulle singole istanze documentali che non sia revocato nelle ACL */
	EXISTS  
	(
        SELECT 1 FROM DocumentPermissions DP 
            WHERE (
                    ((DP.ProfileId in ({roles}) AND DP.ProfileType = 2) 
                    OR
                    (DP.ProfileId in ({groups}) AND DP.ProfileType = 1)
                    OR
                    (DP.ProfileId in ({uid}) AND DP.ProfileType = 0))
                AND (
                    NOT EXISTS 
                      (
                         SELECT 1 FROM ACLPermissions A
                         WHERE 
                            (DP.ProfileId = A.ProfileId)
                            AND (DP.ProfileType = A.ProfileType)
                            AND (A.Authorization = 2)
							AND (A.ACLId = D.ACLId OR A.ACLId = '$GLOBAL$')
			    			AND (A.PermissionId = {Permission})
                      )
                    )
                	AND (DP.Authorization = 1)
                    AND (DP.DocumentId = D.Id)
					AND (DP.PermissionId = {Permission})
			) 
	) 
)");
                    break;
                case TaskColumn.Permissions:
                    var tuid = Var("uid");// filter.Values[0];
                    var troles = Var("roles");// filter.Values[1];
                    var groles = Var("globalroles");// filter.Values[1];
                    var tgroups = Var("groups");// filter.Values[2];
                    var tcompanies = Var("companies");// filter.Values[4];
                    if (string.IsNullOrEmpty(tcompanies)) tcompanies = "0,99999";

                    if (string.IsNullOrEmpty(troles)) troles = "''";
                    if (string.IsNullOrEmpty(groles)) troles = "''";
                    if (string.IsNullOrEmpty(tgroups)) tgroups = "''";
                    sqlBuilder.AddFilter(@$"
(TI.CompanyId IN ({tcompanies})) 
AND 
(
        (TI.FromUserId={tuid}) OR 
        ( UT.UserId={tuid} ) OR 
        ( (UT.RoleId IN ({groles})) AND (UT.RoleId<>'') AND (UT.GroupId='') ) OR 
        ( ( CONCAT(UT.RoleId, CONCAT('\',UT.GroupId)) IN ({troles})) AND (UT.RoleId<>'') AND (UT.GroupId<>'') ) OR 
        ( (UT.GroupId IN ({tgroups})) AND (UT.GroupId<>'') AND (UT.RoleId='') )
)");
                    //sqlBuilder.AddFilter($"(DEST.UserId={tuid} OR ((CONCAT(DEST.RoleId,CONCAT('\\', DEST.GroupId))) IN ({troles}) AND (DEST.RoleId<>'') AND (DEST.GroupId<>'')) OR (DEST.RoleId IN ({troles}) AND (DEST.RoleId<>'') AND (DEST.GroupId='')) OR (DEST.GroupId IN ({tgroups}) AND DEST.RoleId='' AND DEST.GroupId<>'') )");
                    break;
     


                     default:
                    if (id.StartsWith(DocumentColumn.Field))
                    {
                        var g = id.Split('.')[2];
                        var P = "F" + g.Replace("-", "").Replace("$", "").Replace("%", "").Replace(" ", "").Replace(".", "").Replace(",", "");
                        //var dt = await DataTypeFactory.Instance(fieldType.DataType);
                        //v = dt.Serialize(v);
                        sqlBuilder.AddFilter($"(D.Id IN (SELECT DocumentId FROM DocumentFields {P} WHERE D.Id={P}.DocumentId AND {P}.FieldName={g.Quoted()} AND " + sqlBuilder.GetTextFilter($"{P}.Value", filter.OperatorType, filter.Values)+ "))");
                        break;
                    }
                    else
                        if (id.StartsWith(DocumentColumn.Meta))
                    {
                        var g = id.Split('.')[2];
                        var P = "F" + g.Replace("-", "").Replace("$", "").Replace("%", "").Replace(" ", "").Replace(".", "").Replace(",", "");

                        //var dt = await DataTypeFactory.Instance(fieldType.DataType);
                        //v = dt.Serialize(v);
                        sqlBuilder.AddFilter($"(D.Id IN (SELECT DocumentId FROM DocumentFields {P} WHERE D.Id={P}.DocumentId AND {P}.FieldTypeId={g.Quoted()} AND " + sqlBuilder.GetTextFilter($"{P}.Value", filter.OperatorType, filter.Values)+ "))");
                        break;
                    };
                    break;
            }
        }
    }
}
