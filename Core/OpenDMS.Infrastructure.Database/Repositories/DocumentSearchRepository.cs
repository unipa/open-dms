
using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OpenDMS.Infrastructure.Repositories;

//public class DocumentSearchRepository : IDocumentSearchRepository
//{
//    private readonly ApplicationDbContext repo;
////    private readonly ICustomFieldTypeRepository metaRepo;
//    private readonly IApplicationDbContextFactory contextFactory;

//    private Dictionary<string,  FieldType> metadati = new Dictionary<string, FieldType>();



//    public DocumentSearchRepository(IApplicationDbContextFactory contextFactory)
//    {
//        this.contextFactory = contextFactory;
//        this.repo = (ApplicationDbContext)contextFactory.GetDbContext();
//    }

//    public async Task<List<Dictionary<string, LookupTable>>> GetPage(UserProfile userInfo, List<SearchFilter> SearchFilters, List<string> columns, List<SortingType> orderby, int skip, int take)
//    {
//        string sql = @"
//SELECT /*FIELDS*/
//    FROM Documents D
//    LEFT JOIN Images I ON (D.ImageId=I.Id) 
//    LEFT JOIN DocumentTypes T ON (D.DocumentTypeId = T.Id) 
//    LEFT JOIN Companies B ON (D.CompanyId = B.Id) 
//    /*JOIN*/
//WHERE
//(
//    /* Permessi dell'utente, di all su ACL e documenti */
//	((
//	SELECT COUNT(*) FROM
//	    (
//			SELECT ProfileId  FROM 
//			(										
//				Select ProfileId, A.Authorization from ACLPermissions A 
//					WHERE A.ACLId=D.ACLID AND A.ProfileId in (/*USER*/,'*ALL') AND A.ProfileType=0 AND A.PermissionId=/*PERMISSION*/						
//			UNION
//			    Select DP.ProfileId, DP.Authorization from DocumentPermissions DP 
//			    	WHERE DP.DocumentId=D.Id AND DP.ProfileId = /*USER*/ AND DP.ProfileType=0 AND DP.PermissionId=/*PERMISSION*/									
//			UNION
//			    Select DPA.ProfileId,Min(DPA.Authorization) Authorization from DocumentPermissions DPA 
//			    	WHERE DPA.DocumentId=D.Id AND DPA.ProfileId in (/*USER*/,'*ALL')  AND DPA.ProfileType=0 AND DPA.PermissionId=/*PERMISSION*/ 
//			    	GROUP BY DPA.ProfileId								
//			) U
//			GROUP BY ProfileId HAVING Max(Authorization) = 1	
//		) UD  
//	) > 0)
//	OR
//	(
//	(( Select Max(DPA.Authorization) Authorization from DocumentPermissions DPA 
//			    	WHERE DPA.DocumentId=D.Id AND DPA.ProfileId in (/*USER*/,'*ALL')  AND DPA.ProfileType=0 AND DPA.PermissionId=/*PERMISSION*/ 
//			    	) < 2) AND
//	/* Permessi dei ruoli su ACL + permessi gruppi trasversali + permessi ruoli dell'utente nei gruppi + permessi ruoli w gruppi nei documenti */
//	((
//	SELECT COUNT(*) FROM
//	    (										
//			SELECT ProfileId FROM
//			(										
//				Select A.ProfileId, A.Authorization from ACLPermissions A 
//					WHERE A.ACLId =D.ACLID AND A.ProfileId in (/*ROLES*/) AND A.ProfileType=2 AND A.PermissionId=/*PERMISSION*/										
//			/* VISIBILITA TRASVERSALE */
//			UNION									
//				Select '$VISIBILITA$' ProfileId, 1 Authorization from UserGroups G  
//			        WHERE G.Id in (/*GROUPS*/) 
//			UNION		
//                Select G.RoleId ProfileId,COALESCE(P.Authorization,0) Authorization from ACLPermissions A  
//					JOIN UserGroupRoles G ON (A.ProfileId=G.UserGroupId AND A.ProfileType=1 AND A.PermissionId=/*PERMISSION*/)
//			        LEFT JOIN RolePermissions P ON (P.RoleId=G.RoleId AND P.PermissionId=/*PERMISSION*/) 
//			        WHERE (A.ACLId=D.AclId) AND (G.StartISODate <= /*DATA*/ AND G.EndISODate >= /*DATA*/) AND (G.UserId in (/*USER*/,'*ALL'))
//			UNION									
//				Select DP.ProfileId, DP.Authorization from DocumentPermissions DP 
//					WHERE DP.DocumentId=D.Id AND DP.ProfileId in (/*ROLES*/) AND DP.ProfileType=2 AND DP.PermissionId=/*PERMISSION*/									
//			UNION									
//				Select G.RoleId ProfileId, DPR.Authorization from DocumentPermissions DPR 
//			        LEFT JOIN UserGroupRoles G on (G.UserId in (/*USER*/,'*ALL') AND G.UserGroupId=DPR.ProfileId AND G.StartISODate <= /*DATA*/ AND G.EndISODate >= /*DATA*/) 
//			        WHERE DPR.DocumentId=D.Id AND DPR.ProfileId in (/*GROUPS*/) AND DPR.ProfileType=1 AND DPR.PermissionId=/*PERMISSION*/  
//			        GROUP BY G.RoleId							
//			) R 
//			GROUP BY ProfileId HAVING Max(Authorization) = 1										
//	    ) UC 
//	) > 0)
//	)
//)
//ORDER BY /*ORDERBY*/";
//        string user = userInfo.userId;
//        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
//        var query = repo.Documents
//            .Include(d => d.Image)
//            .Include(d => d.DocumentType)
//            .Include(d => d.Company)
//            .Where(d =>
//            (

//            (
//                from dp in repo.DocumentPermissions
//                where dp.ProfileId == user
//                    && dp.ProfileType == ProfileType.User
//                    && dp.PermissionId == PermissionType.CanView
//                    && dp.DocumentId == d.Id
//                    && dp.Authorization == AuthorizationType.Granted
//                select new { dp.Authorization }
//            ).Count() == 1 ||
//            (
//            (
//                from dp in repo.DocumentPermissions
//                where (dp.ProfileId == user || dp.ProfileId == "*ALL")
//                    && dp.ProfileType == ProfileType.User
//                    && dp.PermissionId == PermissionType.CanView
//                    && dp.DocumentId == d.Id
//                    && dp.Authorization == AuthorizationType.Denied
//                select new { dp.Authorization }
//            ).Count() == 0 &&
//            (
//                from dp in repo.DocumentPermissions
//                where dp.ProfileId == "*ALL"
//                    && dp.ProfileType == ProfileType.User
//                    && dp.PermissionId == PermissionType.CanView
//                    && dp.DocumentId == d.Id
//                    && dp.Authorization == AuthorizationType.Granted
//                select new { dp.Authorization }
//            ).Count() == 1
//            ) ||
//            // Permessi dell'utente, di all su ACL e documenti
//            (
//                (
//                    from a in repo.ACLPermissions
//                    where
//                        a.ACLId == d.ACLId
//                        && (userInfo.Roles.Contains(a.ProfileId))
//                        && a.ProfileType == ProfileType.Role
//                        && a.PermissionId == PermissionType.CanView
//                    select new { ProfileId = a.ProfileId, A = (int)a.Authorization }
//                ).Concat
//                (
//                    from g in repo.UserGroups
//                    where
//                        userInfo.Groups.Contains(g.Id)
//                    select new { ProfileId = "$VISIBILITA$", A = (int)AuthorizationType.Granted }
//                ).Concat
//                (
//                    from a in repo.ACLPermissions
//                    join g in repo.UserGroupRoles
//                    on new { id = a.ProfileId, ptype = a.ProfileType, pid = a.PermissionId } equals new { id = g.UserGroupId, ptype = ProfileType.Role, pid = PermissionType.CanView } into ags
//                    from ag in ags.DefaultIfEmpty()
//                    join p in repo.RolePermissions
//                    on new { id = ag.RoleId, pid = PermissionType.CanView } equals new { id = p.RoleId, pid = p.PermissionId }
//                    where a.ACLId == d.ACLId
//                                        && ag.StartISODate <= data
//                                        && ag.EndISODate >= data
//                                        && (ag.UserId == user || ag.UserId == "*ALL")
//                    select new { ProfileId = ag.RoleId, A = (int)AuthorizationType.Granted }
//                ).Concat
//                (
//                    from dp in repo.DocumentPermissions
//                    where
//                        dp.DocumentId == d.Id
//                        && userInfo.Roles.Contains(dp.ProfileId)
//                        && dp.ProfileType == ProfileType.Role
//                        && dp.PermissionId == PermissionType.CanView
//                    select new { ProfileId = dp.ProfileId, A = (int)dp.Authorization }
//                ).Concat
//                (
//                    from dp in repo.DocumentPermissions
//                    join ug in repo.UserGroupRoles
//                    on new { id = dp.ProfileId } equals new { id = ug.UserGroupId }
//                    into dps
//                    from dp1 in dps.DefaultIfEmpty()
//                    where
//                        dp1.StartISODate <= data
//                        && dp1.EndISODate >= data
//                        && (dp1.UserId == user || dp1.UserId == "*ALL")
//                        && dp.DocumentId == d.Id
//                        && userInfo.Groups.Contains(dp.ProfileId)
//                        && dp.ProfileType == ProfileType.Group
//                        && dp.PermissionId == PermissionType.CanView
//                    select new { dp1.RoleId, dp.Authorization } into pgs
//                    group pgs by pgs.RoleId into gp
//                    select new
//                    {
//                        ProfileId = gp.Key,
//                        A = gp.Max(gp => (int)gp.Authorization)
//                    }
//                )
//                .GroupBy(g => g.ProfileId).Where(a => a.Max(a => a.A) == 1)
//                .Select(g => g.Key)
//                .Count() > 0
//           )
//           )
//        ).AsQueryable();


//        StringBuilder join = new StringBuilder();
//        StringBuilder orderBy = new StringBuilder();
//        StringBuilder fields = new StringBuilder();

//        string roles = string.Join(",", userInfo.Roles.ConvertAll(c => c.Quoted()));
//        string groups = string.Join(",", userInfo.Groups.ConvertAll(c => c.Quoted()));

////        string where = @"
////SELECT ID FROM Documents D WHERE (ID = `d`.`Id`) AND
////(
////	((
////	SELECT COUNT(*) FROM
////	    (
////			SELECT ProfileId  FROM 
////			(										
////				Select ProfileId, A.Authorization from ACLPermissions A 
////					WHERE A.ACLId=D.ACLID AND A.ProfileId in (/*USER*/,'*ALL') AND A.ProfileType=0 AND A.PermissionId=/*PERMISSION*/						
////			UNION
////			    Select DP.ProfileId, DP.Authorization from DocumentPermissions DP 
////			    	WHERE DP.DocumentId=D.Id AND DP.ProfileId = /*USER*/ AND DP.ProfileType=0 AND DP.PermissionId=/*PERMISSION*/									
////			UNION
////			    Select DPA.ProfileId,Min(DPA.Authorization) Authorization from DocumentPermissions DPA 
////			    	WHERE DPA.DocumentId=D.Id AND DPA.ProfileId in (/*USER*/,'*ALL')  AND DPA.ProfileType=0 AND DPA.PermissionId=/*PERMISSION*/ 
////			    	GROUP BY DPA.ProfileId								
////			) U
////			GROUP BY ProfileId HAVING Max(Authorization) = 1	
////		) UD  
////	) > 0)
////	OR
////	(
////	(( Select Max(DPA.Authorization) Authorization from DocumentPermissions DPA 
////			    	WHERE DPA.DocumentId=D.Id AND DPA.ProfileId in (/*USER*/,'*ALL')  AND DPA.ProfileType=0 AND DPA.PermissionId=/*PERMISSION*/ 
////			    	) < 2) AND
////	/* Permessi dei ruoli su ACL + permessi gruppi trasversali + permessi ruoli dell'utente nei gruppi + permessi ruoli w gruppi nei documenti */
////	((
////	SELECT COUNT(*) FROM
////	    (										
////			SELECT ProfileId FROM
////			(										
////				Select A.ProfileId, A.Authorization from ACLPermissions A 
////					WHERE A.ACLId =D.ACLID AND A.ProfileId in (/*ROLES*/) AND A.ProfileType=2 AND A.PermissionId=/*PERMISSION*/										
////			/* VISIBILITA TRASVERSALE */
////			UNION									
////				Select '$VISIBILITA$' ProfileId, 1 Authorization from UserGroups G  
////			        WHERE G.Id in (/*GROUPS*/) 
////			UNION		
////                Select G.RoleId ProfileId,COALESCE(P.Authorization,0) Authorization from ACLPermissions A  
////					JOIN UserGroupRoles G ON (A.ProfileId=G.UserGroupId AND A.ProfileType=1 AND A.PermissionId=/*PERMISSION*/)
////			        LEFT JOIN RolePermissions P ON (P.RoleId=G.RoleId AND P.PermissionId=/*PERMISSION*/) 
////			        WHERE (A.ACLId=D.AclId) AND (G.VersionId='$CURRENT$') AND (G.UserId in (/*USER*/,'*ALL'))
////			UNION									
////				Select DP.ProfileId, DP.Authorization from DocumentPermissions DP 
////					WHERE DP.DocumentId=D.Id AND DP.ProfileId in (/*ROLES*/) AND DP.ProfileType=2 AND DP.PermissionId=/*PERMISSION*/									
////			UNION									
////				Select G.RoleId ProfileId, DPR.Authorization from DocumentPermissions DPR 
////			        LEFT JOIN UserGroupRoles G on (G.UserId in (/*USER*/,'*ALL') AND G.UserGroupId=DPR.ProfileId AND G.VersionId='$CURRENT$') 
////			        WHERE DPR.DocumentId=D.Id AND DPR.ProfileId in (/*GROUPS*/) AND DPR.ProfileType=1 AND DPR.PermissionId=/*PERMISSION*/  
////			        GROUP BY G.RoleId							
////			) R 
////			GROUP BY ProfileId HAVING Max(Authorization) = 1										
////	    ) UC 
////	) > 0)
////	)
////)".Replace("/*JOIN*/", join.ToString())
////                .Replace("/*USER*/", userInfo.userId.Quoted())
////                .Replace("/*GROUPS*/", groups)
////                .Replace("/*ROLES*/", roles)
////                .Replace("/*PERMISSION*/", ((int)PermissionType.CanView).ToString())
////                .Replace("/*ORDERBY*/", orderBy.ToString());

//        query = query.AsNoTracking().Where(GetWherePredicate(SearchFilters));

////        var query = ds.Documents.FromSqlRaw(where).Include(d => d.DocumentType).Include(d => d.Company).Include(d => d.Image).AsQueryable();

//        //        var f = await ConvertFilterToSql(SearchFilters);
//        //        string where = String.IsNullOrEmpty(f) ? "" : "AND(" + f+")";


//        Dictionary<int, string> Campi = new Dictionary<int, string>();
//        for (int i = 0; i < columns.Count(); i++)
//        {
            
//            var ExternalColumnName = columns[i].ToLower();
//            var ElencoCampi = GetFieldName(ExternalColumnName, i);
//            var fieldjoin = GetJoins(ExternalColumnName, i);

//            if (!String.IsNullOrEmpty(ElencoCampi))
//            {
//                List<string> clist = new List<string>( ElencoCampi.Split(','));
//                if (clist.Count < 2) clist.Add(clist[0]);
//                fields.Append($"{clist[0]} C{i}, {clist[1]} L{i}, '' T{i}, 0 S{i},");
//                Campi.Add(i, ExternalColumnName);
//            } else
//            {
//                if (fieldjoin.Length > 0)
//                {
//                    fields.Append($",D{i}.Value C{i},D{i}.LookupValue L{i},D{i}.FieldTypeId T{i},D{i}.Encrypted S{i}");
//                    Campi.Add(i, ExternalColumnName);
//                }
//            }
//            if (!string.IsNullOrEmpty(fieldjoin))
//                    join.Append(fieldjoin);
//            //if (!string.IsNullOrEmpty(campo))
//            //{
//            //    string[] clist = campo.Split(',');
//            //    fields.Append($"{clist[0]} C{i}, {clist[1]} L{i}, '' T{i}, 0 S{i},");
//            //    PrefissoCampi.Add($"D{i}", ExternalColumnName);
//            //}
//            //else
//            //{
//            //    if (fieldjoin.Length > 0)
//            //    {
//            //        fields.Append($",D{i}.FVALO C{i},D{i}.FVALD L{i},D{i}.FTIPC T{i},D{i}.FCRYP S{i}");
//            //        PrefissoCampi.Add($"D{i}", ExternalColumnName);
//            //    }
//            //}


//            SortingType stype = orderby[i];
//            if (stype != SortingType.None)
//                orderBy.Append($"L{i} {(stype == SortingType.Ascending ? "" : " desc")},");
//        }
//        orderBy.Append($"D.Id DESC");
//        fields.Append("D.Id K0,D.DocumentStatus K1");

////        Campi.Add(-1, "CNURC");
////        Campi.Add(-2, "CSTCO");
////        Campi.Add("P1", "P1");
////        Campi.Add("P2", "P2");
// //       sql = sql
// //           .Replace("/*FIELDS*/", fields.ToString())
// //               .Replace("/*JOIN*/", join.ToString())
// ////               .Replace("/*WHERE*/", where)
// //               .Replace("/*USER*/", userInfo.userId.Quoted())
// //               .Replace("/*GROUPS*/", groups)
// //               .Replace("/*ROLES*/", roles)
// //               .Replace("/*PERMISSION*/", ((int)PermissionType.CanView).ToString())
// //               .Replace("/*ORDERBY*/", orderBy.ToString());

//        List<Dictionary<string, LookupTable>> rows = new List<Dictionary<string, LookupTable>>();

//        //
//        string generatedSql = query.ToQueryString();// ds.Documents.FromSqlRaw(sql).AsNoTracking().Where(GetWherePredicate(SearchFilters)).ToQueryString();
//        //        int pos = generatedSql.IndexOf("FROM (");
//        //        generatedSql = "SELECT * "+generatedSql.Substring(pos);
//        //        generatedSql = generatedSql + " AND `d`.`Id` in (/*WHERE*/)".Replace("/*WHERE*/", where);
//        {
//            var cmd = repo.Database.GetDbConnection().CreateCommand();
//            cmd.CommandText = generatedSql;
//            cmd.Connection.Open();
//            using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
//            {
//                while (reader.Read())
//                {
//                    Dictionary<string, LookupTable> r = new Dictionary<string, LookupTable>(Campi.Count);
//                    for (int i = 0; i < Campi.Count; i++)
//                    {
//                        if (Campi.ContainsKey(i))
//                        {

//                            string c = reader[$"C{i}"].ToString();
//                            string l = reader[$"L{i}"].ToString();
//                            string t = reader[$"T{i}"].ToString();
//                            string s = reader[$"S{i}"].ToString();
//                            if (s == "1") // Se è cifrato
//                            {
//                                c = "*";
//                                l = "".PadRight(l.Length, '*');
//                            }
//                            LookupTable tb = new LookupTable();
//                            tb.Id = c;
//                            tb.Description = l;
//                            tb.TableId = t;
//                            var columnName = Campi[i];
//                            r.Add(columnName, tb);
//                        }
//                    }
//                    string k0 = reader["K0"].ToString();
//                    r.Add("K0", new LookupTable() { Id = k0 });
//                    string k1 = reader["K1"].ToString();
//                    r.Add("K1", new LookupTable() { Id = k1 });
//                    //                string p0 = reader["P1"].ToString();
//                    //                r.Add("P1", new LookupTable() { Id = p0 });
//                    //                string p1 = reader["P2"].ToString();
//                    //                r.Add("P2", new LookupTable() { Id = p1 });
//                    rows.Add(r);
//                }

//            }
//        }

////        using (var reader = ds.Database..Sql.ExecuteSqlRaw(.Select(sql, skip, take))
////        {
////            while (reader != null && reader.Read())
////            {
////                Dictionary<string, LookupTable> r = new Dictionary<string, LookupTable>(Campi.Count);
////                for (int i = 0; i < Campi.Count; i++)
////                {
////                    if (Campi.ContainsKey($"D{i}"))
////                    {
////                        string c = reader[$"C{i}"].ToString();
////                        string l = reader[$"L{i}"].ToString();
////                        string t = reader[$"T{i}"].ToString();
////                        string s = reader[$"S{i}"].ToString();
////                        if (s == "1")
////                        {
////                            c = "*";
////                            l = "".PadRight(l.Length, '*');
////                        }
////                        LookupTable tb = new LookupTable();
////                        tb.Id = c;
////                        tb.Description = l;
////                        tb.TableId = t;
////                        var columnName = Campi[$"D{i}"];
////                        r.Add(columnName, tb);
////                    }
////                }
////                string k0 = reader["K0"].ToString();
////                r.Add("K0", new LookupTable() { Id = k0 });
////                string k1 = reader["K1"].ToString();
////                r.Add("K1", new LookupTable() { Id = k1 });
//////                string p0 = reader["P1"].ToString();
//////                r.Add("P1", new LookupTable() { Id = p0 });
//////                string p1 = reader["P2"].ToString();
//////                r.Add("P2", new LookupTable() { Id = p1 });
////                rows.Add(r);
////            }
////        }
//        return rows;
//    }


//    public async Task<int> Count(UserProfile userInfo, List<SearchFilter> SearchFilters)
//    {

//        string sql = @"
//SELECT /*FIELDS*/ 
//    FROM Documents D
//    LEFT JOIN Images I ON (D.ImageId=I.Id) 
//    LEFT JOIN DocumentTypes T ON (D.DocumentTypeId = T.Id) 
//    /*JOIN*/
//WHERE
//(
//    /* Permessi dell'utente, di all su ACL e documenti */
//	((
//	SELECT COUNT(*) FROM
//	    (
//			SELECT ProfileId  FROM 
//			(										
//				Select ProfileId, A.Authorization from ACLPermissions A 
//					WHERE A.ACLId=D.ACLID AND A.ProfileId in (/*USER*/,'*ALL') AND A.ProfileType=0 AND A.PermissionId=/*PERMISSION*/						
//			UNION
//			    Select DP.ProfileId, DP.Authorization from DocumentPermissions DP 
//			    	WHERE DP.DocumentId=D.Id AND DP.ProfileId = /*USER*/ AND DP.ProfileType=0 AND DP.PermissionId=/*PERMISSION*/									
//			UNION
//			    Select DPA.ProfileId,Min(DPA.Authorization) Authorization from DocumentPermissions DPA 
//			    	WHERE DPA.DocumentId=D.Id AND DPA.ProfileId in (/*USER*/,'*ALL')  AND DPA.ProfileType=0 AND DPA.PermissionId=/*PERMISSION*/ 
//			    	GROUP BY DPA.ProfileId								
//			) U
//			GROUP BY ProfileId HAVING Max(Authorization) = 1	
//		) UD  
//	) > 0)
//	OR
//	(
//	(( Select Max(DPA.Authorization) Authorization from DocumentPermissions DPA 
//			    	WHERE DPA.DocumentId=D.Id AND DPA.ProfileId in (/*USER*/,'*ALL')  AND DPA.ProfileType=0 AND DPA.PermissionId=/*PERMISSION*/ 
//			    	) < 2) AND
//	/* Permessi dei ruoli su ACL + permessi gruppi trasversali + permessi ruoli dell'utente nei gruppi + permessi ruoli w gruppi nei documenti */
//	((
//	SELECT COUNT(*) FROM
//	    (										
//			SELECT ProfileId FROM
//			(										
//				Select A.ProfileId, A.Authorization from ACLPermissions A 
//					WHERE A.ACLId =D.ACLID AND A.ProfileId in (/*ROLES*/) AND A.ProfileType=2 AND A.PermissionId=/*PERMISSION*/										
//			/* VISIBILITA TRASVERSALE */
//			UNION									
//				Select '$VISIBILITA$' ProfileId, 1 Authorization from UserGroups G  
//			        WHERE G.Id in (/*GROUPS*/) 
//			UNION		
//                Select G.RoleId ProfileId,COALESCE(P.Authorization,0) Authorization from ACLPermissions A  
//					JOIN UserGroupRoles G ON (A.ProfileId=G.UserGroupId AND A.ProfileType=1 AND A.PermissionId=/*PERMISSION*/)
//			        LEFT JOIN RolePermissions P ON (P.RoleId=G.RoleId AND P.PermissionId=/*PERMISSION*/) 
//			        WHERE (A.ACLId=D.AclId) AND (G.VersionId='$CURRENT$') AND (G.UserId in (/*USER*/,'*ALL'))
//			UNION									
//				Select DP.ProfileId, DP.Authorization from DocumentPermissions DP 
//					WHERE DP.DocumentId=D.Id AND DP.ProfileId in (/*ROLES*/) AND DP.ProfileType=2 AND DP.PermissionId=/*PERMISSION*/									
//			UNION									
//				Select G.RoleId ProfileId, DPR.Authorization from DocumentPermissions DPR 
//			        LEFT JOIN UserGroupRoles G on (G.UserId in (/*USER*/,'*ALL') AND G.UserGroupId=DPR.ProfileId AND G.VersionId='$CURRENT$') 
//			        WHERE DPR.DocumentId=D.Id AND DPR.ProfileId in (/*GROUPS*/) AND DPR.ProfileType=1 AND DPR.PermissionId=/*PERMISSION*/  
//			        GROUP BY G.RoleId							
//			) R 
//			GROUP BY ProfileId HAVING Max(Authorization) = 1										
//	    ) UC 
//	) > 0)
//	)
//)
//";

        
////        var f = await ConvertFilterToSql(SearchFilters);
////        string where = String.IsNullOrEmpty(f) ? "" : "WHERE " + f;


//        string roles = string.Join("','", userInfo.Roles);
//        string groups = string.Join("','", userInfo.Groups);

//        sql = sql
//            .Replace("/*FIELDS*/", "*")
//                .Replace("/*JOIN*/", "") // join.ToString())
//                //               .Replace("/*WHERE*/", where)
//                .Replace("/*USER*/", userInfo.userId.Quoted())
//                .Replace("/*GROUPS*/", groups)
//                .Replace("/*ROLES*/", roles)
//                .Replace("/*PERMISSION*/", (PermissionType.CanView).Quoted());

//        dynamic result = repo.Documents .FromSqlRaw(sql).AsNoTracking().Where(GetWherePredicate(SearchFilters));
//        return (int)result;
//    }


//    private string GetFieldName(string ColumnId, int Index)
//    {
//        string column = ColumnId.ToLower();
//        switch (column)
//        {
//            case DocumentFields.ID: return "D.Id";
//            case DocumentFields.AZIENDA:
//            case DocumentFields.SISTEMA_INFORMATIVO:
//            case DocumentFields.BD: return "B.Id";
//            case DocumentFields.DESCRIZIONE: return "D.Description,D.Description";
//            case DocumentFields.TIPOLOGIA:
//            case DocumentFields.TIPO_DOCUMENTO: return "D.DocumentTypeId,T.Name";
//            case DocumentFields.TIPOCONTENUTO: return "D.ContentType,D.ContentType";
//            case DocumentFields.STATUS: return "DocumentStatus,DocumentStatus";
//            case DocumentFields.ND:
//            case DocumentFields.NUDC:
//            case DocumentFields.NUMERO_DOCUMENTO: return "D.DocumentNumber,D.DocumentFormattedNumber";
//            case DocumentFields.DA:
//            case DocumentFields.DATA_ARCHIVIAZIONE: return "D.CreationDate,D.CreationDate";
//            case DocumentFields.DS:
//            case DocumentFields.DATA_SCADENZA: return "D.ExpirationDate,D.ExpirationDate";
//            case DocumentFields.DM:
//            case DocumentFields.DATA_MODIFICA: return "D.LastUpdate,D.LastUpdate";
//            case DocumentFields.DD:
//            case DocumentFields.DATA_DOCUMENTO: return "D.DocumentDate,D.DocumentDate";
//            case DocumentFields.DC:
//            case DocumentFields.DATA_CANCELLAZIONE: return "D.DeletionDate,D.DeletionDate";
//            case DocumentFields.DE:
//            case DocumentFields.DATA_CONGELAMENTO: return "D.ConsolidationDate,D.ConsolidationDate";
//            case DocumentFields.DP:
//            case DocumentFields.DATA_PROTOCOLLO: return "D.ProtocolDate,D.ProtocolDate";
//            case DocumentFields.DPE:
//            case DocumentFields.DATA_PROTOCOLLO_ESTERNO: return "D.SenderProtocolDate,D.SenderProtocolDate";
//            case DocumentFields.PROPRIETARIO: return "D.Owner,D.Owner";
//            case DocumentFields.MEZZO: return "D.TransmissionChannelType,X.Descrption";
//            case DocumentFields.CONTENITORE:
//            case DocumentFields.NODO_PADRE: return "D.ParentId,D.ParentId";
//            case DocumentFields.FILENAME: return "I.FileName,I.FileName";
//            case DocumentFields.STATO_SPEDIZIONE: return "I.SendingStatus,I.SendingStatus";
//            case DocumentFields.STATO_SOTTOSCRIZIONE: return "I.SignatureStatus,I.SignatureStatus";
//            case DocumentFields.STATO_CONSERVAZIONE: return "I.PreservationStatus,I.PreservationStatus";
//            case DocumentFields.IMPRONTA: return "I.Hash,I.Hash";
//            case DocumentFields.SOGGETTO:
//            case DocumentFields.OGGETTO: return "D.ProtocolDescrption,D.ProtocolDescrption";
//            case DocumentFields.REGISTRO:
//            case DocumentFields.REGISTRO_PROTOCOLLO: return "D.ProtocolRegister,Y.Description";
//            case DocumentFields.PROTOCOLLO_ESTERNO: return "D.SenderProtocol,D.SenderProtocol";
//            case DocumentFields.DIREZIONE: return "D.Direction,D.Direction";
//            case DocumentFields.PROTOCOLLO: return "D.ProtocolNumber,D.ProtocolNumber";
//            case DocumentFields.STATO_INDICIZZAZIONE: return "I.IndexingStatus,I.IndexingStatus";
//            case DocumentFields.STATO_PREVIEW: return "I.PreviewStatus,I.PreviewStatus";
//            default:
//                return "";
//        }
//        return "";
//    }

//    private string GetJoins(string ColumnId, int Index)
//    {
//        string column = ColumnId.ToLower();
//        switch (column)
//        {
//            case DocumentFields.BD:
//                case DocumentFields.AZIENDA:
//                    case DocumentFields.SISTEMA_INFORMATIVO:
//                return "LEFT JOIN Company B ON (D.CompanyId=B.Id)";
//            //case "tipodocumento":
//            //case "tipologia": return "LEFT JOIN TIPOLOGIE T ON (D.CTIPO=T.OCDTP)";
//            //case "filename":
//            //case "statodistribuzione": 
//            //case "statosottoscrizione": 
//            //case "statoconservazione": 
//            //case "impronta": return "LEFT JOIN IMMAG00F I ON(D.CNMIM=I.BNURC)";
//            case DocumentFields.REGISTRO:
//            case DocumentFields.REGISTRO_PROTOCOLLO:
//                return "LEFT JOIN LookupTables Y ON (D.ProtocolRegister=Y.Id AND Y.TableId='REG')";
//            case DocumentFields.MEZZO:
//                return "LEFT JOIN LookupTables X ON (D.TransmissionChannelType=X.Id AND X.TableId='TPP')";
//            default:
//                string keyname = "";
//                bool Iskey = column.StartsWith("tipochiave");
//                if (Iskey) keyname = column.Substring(10);
//                else
//                {
//                    Iskey = column.StartsWith("tpk");
//                    if (Iskey) keyname = column.Substring(3);
//                    else
//                    {
//                        Iskey = column.StartsWith("$");
//                        if (Iskey) keyname = column.Substring(1);
//                    }
//                }
//                if (Iskey)
//                {
//                    string c = ColumnId.Quoted();
//                    return $"LEFT JOIN DocumentFields D{Index} ON (D.Id=D{Index}.DocumentId)AND(D{Index}.DocumentTypeId=${c})AND(D{Index}.Chunk=1)AND(D{Index}.Id=(SELECT MIN(Id) FROM DocumentFields WHERE (D.Id=DocumentId)AND(DocumentType={c})AND(Chunk=1)))";
//                }
//                else
//                {
//                    Iskey = column.StartsWith("metadato");
//                    if (Iskey) keyname = column.Substring(8);
//                    else
//                    {
//                        Iskey = column.StartsWith("#");
//                        if (Iskey) keyname = column.Substring(1);
//                    }
//                    if (Iskey)
//                    {
//                        string c = ColumnId.Quoted();
//                        return $"LEFT JOIN DocumentFields D{Index} ON (D.Id=D{Index}.DocumentId)AND(D{Index}.FieldIndex=${c})AND(D{Index}.Chunk=1)AND(D{Index}.Id=(SELECT MIN(Id) FROM DocumentFields WHERE (D.Id=DocumentId)AND(FieldIndex={c})AND(Chunk=1)))";
//                    }
//                }
//                break;
//        }
//        return "";
//    }


//    //private async Task<string> ConvertFilterToSql(List<SearchFilter> SearchFilters)
//    //{
//    //    string s = "";
//    //    foreach (var f in SearchFilters)
//    //    {
//    //        string column = f.ColumnName.ToLower();
//    //        switch (column)
//    //        {
//    //            case DocumentFields.ID:
//    //                f.CustomTypeId = "$$i";
//    //                s += GetSearchFilterValue("CNURC {0})", f);
//    //                break;
//    //            case DocumentFields.BD:
//    //            case DocumentFields.AZIENDA:
//    //                    case DocumentFields.SISTEMA_INFORMATIVO:
//    //                s += GetSearchFilterValue("CCDBD {0})", f);
//    //                break;
//    //            case DocumentFields.DESCRIZIONE:
//    //                s += GetSearchFilterValue("REPLACE(LOWER(CDESC),'.','') {0})", f);
//    //                break;
//    //            case DocumentFields.TIPOLOGIA:
//    //            case DocumentFields.TIPO_DOCUMENTO:
//    //                if (f.Values.Count > 0)
//    //                {
//    //                    if (f.Values.Count > 1 && f.Operator != OperatorType.Contains)
//    //                        f.Operator = OperatorType.Contains;
//    //                    s += GetSearchFilterValue("(CTIPO {0})", f);
//    //                }
//    //                else
//    //                    s += "AND((CTIPO ='')OR(CTIPO IS NULL))";
//    //                break;

//    //            case DocumentFields.TIPOCONTENUTO:
//    //                switch (f.Values[0].ToLower())
//    //                {
//    //                    case "f": s += "AND(CTPDC in ('F','D') "; break;
//    //                    case "d": s += "AND(CTPDC in ('') "; break;
//    //                    default:
//    //                        s += "AND(CTPDC='" + f.Values[0] + "')";
//    //                        break;
//    //                }
//    //                break;
//    //            case DocumentFields.STATUS:
//    //                switch (f.Values[0].ToLower())
//    //                {
//    //                    case "n": s += "AND(CSTCO<>'')AND(CSTCO<>'B')AND(NOT CSTCO IS NULL)"; break;
//    //                    case "": s += "AND(CSTCO='' OR CSTCO='B' OR CSTCO ='A' OR CSTCO IS NULL)"; break;
//    //                    case "all":
//    //                    case "a": s += ""; break;
//    //                    default:
//    //                        s += "AND(CSTCO='" + f.Values[0] + "')";
//    //                        break;
//    //                }
//    //                break;
//    //            case DocumentFields.ND:
//    //            case DocumentFields.NUDC:
//    //            case DocumentFields.NUMERO_DOCUMENTO:
//    //                s += GetSearchFilterValue("(CNUDC {0})", f);
//    //                break;
//    //            case DocumentFields.DA:
//    //            case DocumentFields.DATA_ARCHIVIAZIONE:
//    //                s += GetSearchFilterValue("(CDTAR {0})", f);
//    //                break;
//    //            case DocumentFields.DS:
//    //            case DocumentFields.DATA_SCADENZA:
//    //                s += GetSearchFilterValue("(CDTSC {0})", f);
//    //                break;
//    //            case DocumentFields.DM:
//    //            case DocumentFields.DATA_MODIFICA:
//    //                s += GetSearchFilterValue("(CDTUP {0})", f);
//    //                break;
//    //            case DocumentFields.DD:
//    //            case DocumentFields.DATA_DOCUMENTO:
//    //                s += GetSearchFilterValue("(CDTDC {0})", f);
//    //                break;
//    //            case DocumentFields.DC:
//    //            case DocumentFields.DATA_CANCELLAZIONE:
//    //                s += GetSearchFilterValue("(CDTPR {0})", f);
//    //                break;

//    //            case DocumentFields.PROPRIETARIO:
//    //                s += GetSearchFilterValue("(CPROP {0})", f);
//    //                break;
//    //            case DocumentFields.MEZZO:
//    //                s += GetSearchFilterValue("(CTRMS {0})", f);
//    //                break;
//    //            case DocumentFields.CONTENITORE:
//    //                case DocumentFields.NODO_PADRE:
//    //                s += GetSearchFilterValue("(CNUPD {0})", f);
//    //                break;
//    //            case DocumentFields.FASCICOLI:
//    //            case DocumentFields.RACCOGLITORI:
//    //                s += GetSearchFilterValue("(CNURC IN (SELECT HNMRC FROM DOCRC00F WHERE HNMDC {0}))", f);
//    //                break;
//    //            case "infascicolo":
//    //            case "inraccoglitore":
//    //                if (f.Values.Count > 0)
//    //                    s += GetSearchFilterValue("((CNUPD {0})OR(CNURC IN (SELECT HNMDC FROM DOCRC00F WHERE HNMRC {0})))", f);
//    //                else
//    //                    s += "AND(CNURC NOT IN (SELECT DISTINCT HNMDC FROM DOCRC00F)";
//    //                break;
//    //            case DocumentFields.POSTIT:
//    //            case DocumentFields.COMMENTO:
//    //                s += GetSearchFilterValue("(CNURC IN ( SELECT NNUDC FROM POSTIT0F WHERE NNOTA {0})", f);
//    //                break;
//    //            case DocumentFields.FILENAME:
//    //                s += GetSearchFilterValue("(CNMIM IN (SELECT BNURC FROM IMMAG00F WHERE BPTVS {0}))", f);
//    //                break;
//    //            case DocumentFields.STATO_SPEDIZIONE:
//    //                s += GetSearchFilterValue("(CNMIM IN (SELECT BNURC FROM IMMAG00F WHERE BSEMA {0}))", f);
//    //                break;
//    //            case DocumentFields.STATO_SOTTOSCRIZIONE:
//    //                s += GetSearchFilterValue("(CNMIM IN (SELECT BNURC FROM IMMAG00F WHERE BBURN {0}))", f);
//    //                break;
//    //            case DocumentFields.STATO_CONSERVAZIONE:
//    //                s += GetSearchFilterValue("(CNMIM IN (SELECT BNURC FROM IMMAG00F WHERE BFIDI {0}))", f);
//    //                break;
//    //            case DocumentFields.IMPRONTA:
//    //                s += GetSearchFilterValue("(CNMIM IN (SELECT BNURC FROM IMMAG00F WHERE BIMPR {0}))", f);
//    //                break;
//    //            case DocumentFields.CORRELATO:
//    //                s += GetSearchFilterValue("( (CNURC IN (SELECT DNMIM FROM IMMDC00F WHERE DNMDC {0} AND (DSEQU < 0))) OR (CNURC IN (SELECT DNMDC FROM IMMDC00F WHERE DNMIM {0} AND (DSEQU < 0)))) )", f);
//    //                break;
//    //            case DocumentFields.SOGGETTO:

//    //                string cs = "";
//    //                foreach (var v in f.Values)
//    //                {
//    //                    SearchFilter f2 = new SearchFilter();
//    //                    f2.Operator = f.Operator;
//    //                    using (var reader = ds.Select("SELECT Id FROM BUSINESSPARTNER Where IdMaster=" + v))
//    //                    {
//    //                        while (reader != null && reader.Read())
//    //                            f2.Values.Add(reader.GetString(0));
//    //                    }
//    //                    cs += "UNION (SELECT FNURC FROM DOCFLD0F WHERE (FTIPC='$bp')" + GetSearchFilterValue("(FVALO {0})", f2) + ")";
//    //                }
//    //                if (cs.Length > 0)
//    //                    s += "AND(CNURC IN(" + cs.Substring(7) + "))";
//    //                break;

//    //            case DocumentFields.STAR:
//    //                //TODO:
//    //                break;
//    //            case DocumentFields.NOIMAGE:
//    //            case DocumentFields.SENZA_IMMAGINI:
//    //                s += "AND(CNMIM IS NULL OR CNMIM = 0)";
//    //                break;

//    //            case DocumentFields.DP:
//    //            case DocumentFields.DATA_PROTOCOLLO:
//    //                s += GetSearchFilterValue("(REPLACE(LOWER(CDSPR),'.','') {0})", f);
//    //                break;
//    //            case DocumentFields.DPE:
//    //            case DocumentFields.DATA_PROTOCOLLO_ESTERNO:
//    //                s += GetSearchFilterValue("(CDPRE {0})", f);
//    //                break;
//    //            case DocumentFields.OGGETTO:
//    //                s += GetSearchFilterValue("(CDSPR {0})", f);
//    //                break;
//    //            case DocumentFields.REGISTRO:
//    //            case DocumentFields.REGISTRO_PROTOCOLLO:
//    //                s += GetSearchFilterValue("(CTIPO IN (SELECT OCDTP FROM Tipologie WHERE OTPRO {0}))", f);
//    //                break;
//    //            case DocumentFields.PROTOCOLLO_ESTERNO:
//    //                s += GetSearchFilterValue("(CNPRE {0})", f);
//    //                break;
//    //            case DocumentFields.DIREZIONE:
//    //                s += GetSearchFilterValue("(CDIRP {0})", f);
//    //                break;
//    //            case DocumentFields.PROTOCOLLO:
//    //                s += GetSearchFilterValue("(CNPRO {0})", f);
//    //                break;
//    //            case DocumentFields.PROTOCOLLATO:
//    //                if (f.Values[0].ToLower() == "s")
//    //                    s += "AND(CNPRO > 0)";
//    //                else
//    //                    s += "AND(CNPRO = 0)";
//    //                break;

//    //            //case "":
//    //            //    s += GetSearchFilterValue("(CTIPO {0})", f); 
//    //            //    break;

//    //            default:
//    //                // Verifico se si tratta di chiavi o metadati
//    //                string keyname = "";
//    //                bool Iskey = column.StartsWith("tipochiave");
//    //                if (Iskey) keyname = column.Substring(10);
//    //                else
//    //                {
//    //                    Iskey = column.StartsWith("tpk");
//    //                    if (Iskey) keyname = column.Substring(3);
//    //                    else
//    //                    {
//    //                        Iskey = column.StartsWith("$");
//    //                        if (Iskey) keyname = column.Substring(1);
//    //                    }
//    //                }
//    //                if (Iskey)
//    //                {
//    //                    string c1 = GetSearchFilterValue("(FVALO {0} AND FCRYP=0)", f) + ")";

//    //                    //TODO: Cifrare
//    //                    string c2 = "OR" + GetSearchFilterValue("(FVALO {0} AND FCRYP=1)", f) + ")".Substring(3);
//    //                    s += "AND(CNURC IN (SELECT FNURC FROM DOCFLD0F WHERE (FTIPC=" + keyname + ")" + c1 + c2 + ")";
//    //                }
//    //                else
//    //                {
//    //                    Iskey = column.StartsWith("metadato");
//    //                    if (Iskey) keyname = column.Substring(8);
//    //                    else
//    //                    {
//    //                        Iskey = column.StartsWith("#");
//    //                        if (Iskey) keyname = column.Substring(1);
//    //                    }
//    //                    if (Iskey)
//    //                    {
//    //                        string c1 = GetSearchFilterValue("(FVALO {0} AND FCRYP=0)", f) + ")";

//    //                        //TODO: Cifrare
//    //                        string c2 = "OR" + GetSearchFilterValue("(FVALO {0} AND FCRYP=1)", f) + ")".Substring(3);
//    //                        s += "AND(CNURC IN (SELECT FNURC FROM DOCFLD0F WHERE (FINDX=" + keyname + ")" + c1 + c2 + ")";
//    //                    }
//    //                }
//    //                break;
//    //        }
//    //    }
//    //    return s;
//    //}

//    //private async Task<string> GetSearchFilterValue(string statement, SearchFilter f)
//    //{
//    //    var M = new FieldType();
//    //    if (metadati.ContainsKey(f.CustomTypeId))
//    //        M = metadati[f.CustomTypeId];
//    //    else
//    //    {
//    //        M = await metaRepo.GetById(f.CustomTypeId);
//    //        metadati.Add(f.CustomTypeId, M);
//    //    }
//    //    //TODO: formattare i values

//    //    string s = "";
//    //    switch (M.DataType)
//    //    {
//    //        case DataType.NumeroDecimale:
//    //        case DataType.NumeroIntero:
//    //        case DataType.Valuta:
//    //            s = GetSearchNumberFilter(f);
//    //            break;
//    //        case DataType.Data:
//    //            s = GetSearchDateFilter(f);
//    //            break;
//    //        case DataType.DataComeNumero:
//    //            s = GetSearchNumberDateFilter(f);
//    //            break;
//    //        default:
//    //            s = GetSearchTextFilter(f);
//    //            break;
//    //    }
//    //    return "AND(" + statement.Replace("{0}", s) + ")";
//    //}

//    //private string GetSearchNumberFilter(SearchFilter f)
//    //{
//    //    string s;
//    //    switch (f.Operator)
//    //    {
//    //        case OperatorType.NotEqualTo:
//    //            s = "<>" + f.Values[0]; break;
//    //        case OperatorType.GreaterThan:
//    //            s = ">" + f.Values[0]; break;
//    //        case OperatorType.LessThan:
//    //            s = "<" + f.Values[0]; break;
//    //        case OperatorType.GreaterThanOrEqualTo:
//    //            s = ">=" + f.Values[0]; break;
//    //        case OperatorType.LessThanOrEqualTo:
//    //            s = "<=" + f.Values[0]; break;
//    //        case OperatorType.Contains:
//    //            s = " IN (" + string.Join(',', f.Values) + ")"; break;
//    //        default:
//    //            s = "=" + f.Values[0]; break;
//    //    }

//    //    return s;
//    //}
//    //private string GetSearchDateFilter(SearchFilter f)
//    //{
//    //    string s;
//    //    DateTime dt1 = DateTime.MinValue;
//    //    DateTime dt2 = DateTime.MaxValue;
//    //    for (int i = 0; i < f.Values.Count; i++)
//    //    {
//    //        string h = i > 0 ? " 23:59.59" : f.Values.Count > 0 ? " 00:00.00" : "";
//    //        string d = i > 0 ? "2999-31-12" : "1900-01-01";
//    //        if (DateTime.TryParse(f.Values[i], out dt1))
//    //            f.Values[i] = (dt1.ToString(IDataSource.DefaultDateFormat) + h).Quoted();
//    //        else
//    //            f.Values[i] = (d + h).Quoted();
//    //    }
//    //    if (f.Values.Count < 2) f.Values.Add("'2999-31-12 23:59.59'");
//    //    switch (f.Operator)
//    //    {
//    //        case OperatorType.NotEqualTo:
//    //            s = "<>" + f.Values[0]; break;
//    //        case OperatorType.GreaterThan:
//    //            s = ">" + f.Values[0]; break;
//    //        case OperatorType.LessThan:
//    //            s = "<" + f.Values[0]; break;
//    //        case OperatorType.GreaterThanOrEqualTo:
//    //            s = ">=" + f.Values[0]; break;
//    //        case OperatorType.LessThanOrEqualTo:
//    //            s = "<=" + f.Values[0]; break;
//    //        case OperatorType.Contains:
//    //            s = " BETWEEN " + f.Values[0] + " AND " + f.Values[1]; break;
//    //        default:
//    //            s = "=" + f.Values[0]; break;
//    //    }

//    //    return s;
//    //}
//    //private string GetSearchNumberDateFilter(SearchFilter f)
//    //{
//    //    DateTime dt1 = DateTime.MinValue;
//    //    DateTime dt2 = DateTime.MaxValue;
//    //    for (int i = 0; i < f.Values.Count; i++)
//    //    {
//    //        if (DateTime.TryParse(f.Values[i], out dt1))
//    //            f.Values[i] = dt1.ToString("yyyyMMdd");
//    //        else
//    //            f.Values[i] = i > 0 ? "999999999" : "0";
//    //    }
//    //    string s;
//    //    if (f.Values.Count < 2) f.Values.Add("99999999");
//    //    switch (f.Operator)
//    //    {
//    //        case OperatorType.NotEqualTo:
//    //            s = "<>" + f.Values[0]; break;
//    //        case OperatorType.GreaterThan:
//    //            s = ">" + f.Values[0]; break;
//    //        case OperatorType.LessThan:
//    //            s = "<" + f.Values[0]; break;
//    //        case OperatorType.GreaterThanOrEqualTo:
//    //            s = ">=" + f.Values[0]; break;
//    //        case OperatorType.LessThanOrEqualTo:
//    //            s = "<=" + f.Values[0]; break;
//    //        case OperatorType.Contains:
//    //            s = " BETWEEN " + f.Values[0] + " AND " + f.Values[1]; break;
//    //        default:
//    //            s = "=" + f.Values[0]; break;
//    //    }

//    //    return s;
//    //}
//    //private string GetSearchTextFilter(SearchFilter f)
//    //{
//    //    string s = "";
//    //    switch (f.Operator)
//    //    {
//    //        case OperatorType.EqualTo:
//    //            break;
//    //        case OperatorType.NotEqualTo:
//    //            s = "<>" + f.Values[0].Quoted(); break;
//    //        case OperatorType.GreaterThan:
//    //            s = ">" + f.Values[0].Quoted(); break;
//    //        case OperatorType.LessThan:
//    //            s = "<" + f.Values[0].Quoted(); break;
//    //        case OperatorType.GreaterThanOrEqualTo:
//    //            s = ">=" + f.Values[0].Quoted(); break;
//    //        case OperatorType.LessThanOrEqualTo:
//    //            s = "<=" + f.Values[0].Quoted(); break;
//    //        case OperatorType.Contains:
//    //            s = " LIKE " + ("%" + f.Values[0] + "%").Quoted(); break;
//    //        case OperatorType.NotContains:
//    //            s = " NOT LIKE " + ("%" + f.Values[0] + "%").Quoted(); break;
//    //        case OperatorType.StarstWith:
//    //            s = " LIKE " + (f.Values[0] + "%").Quoted(); break;
//    //        case OperatorType.EndsWith:
//    //            s = " LIKE " + ("%" + f.Values[0]).Quoted(); break;
//    //        default:
//    //            if (f.Values.Count > 1)
//    //                s = " IN (" + String.Join(',', f.Values.ConvertAll<string>(i => { return i.Quoted(); })) + ")";
//    //            else
//    //                s = "=" + f.Values[0].Quoted();
//    //            break;
//    //    }

//    //    return s;
//    //}

//    private Expression GetPropertyExpression(Expression pe, string propertyName)
//    {
//        var properties = propertyName.Split('.');
//        foreach (var property in properties)
//            pe = Expression.Property(pe, property);
//        return pe;
//    }

//    private Expression<Func<Document, Boolean>> GetWherePredicate(List<SearchFilter> SearchFilters)
//    {
//        //the 'IN' parameter for expression ie T=> condition
//        ParameterExpression pe = Expression.Parameter(typeof(Document), typeof(Document).Name);

//        //combine them with and 1=1 Like no expression
//        Expression combined = null;

//        if (SearchFilters != null)
//        {
//            foreach (var fieldItem in SearchFilters)
//            {
//                //Expression for accessing Fields name property
//                Expression columnNameProperty = GetPropertyExpression(pe, fieldItem.ColumnName);
//                //the name constant to match 
//                Expression e1 = GetFilter(columnNameProperty, fieldItem);
//                if (combined == null)
//                    combined = e1;
//                else
//                    combined = Expression.And(combined, e1);
//            }
//        }
//        //create and return the predicate
//        return Expression.Lambda<Func<Document, Boolean>>(combined, new ParameterExpression[] { pe });
//    }



//    private Expression GetFilter (Expression columnName, SearchFilter f)
//    {
//        Expression e = null;
//        Expression field = Expression.Constant(columnName.Type.Name=="Int32" ? Int32.Parse(f.Values[0]) : f.Values[0]);
//        Type c = columnName.Type;
//        switch (f.Operator)
//        {
//            case OperatorType.EqualTo:
//                e = Expression.Equal(columnName, field);
//                break;
//            case OperatorType.NotEqualTo:
//                e = Expression.NotEqual(columnName, field);
//                break;
//            case OperatorType.GreaterThan:
//                e = Expression.GreaterThan(columnName, field);
//                break;
//            case OperatorType.LessThan:
//                e = Expression.LessThan(columnName, field);
//                break;
//            case OperatorType.GreaterThanOrEqualTo:
//                e = Expression.GreaterThanOrEqual(columnName, field);
//                break;
//            case OperatorType.LessThanOrEqualTo:
//                e = Expression.LessThanOrEqual(columnName, field);
//                break;
//            case OperatorType.Contains:
//                if (columnName.Type.Name != "String")
//                {
//                    var eLeft = Expression.GreaterThanOrEqual(columnName, field);
//                    var eRight = Expression.LessThanOrEqual(columnName, field);
//                    e = Expression.And(eLeft, eRight);
//                }
//                else
//                {
//                    MethodInfo mi1 = c.GetMethod("Contains", new Type[] { c });
//                    e = Expression.Call(columnName, mi1, field);
//                }
//                break;
//            case OperatorType.NotContains:
//                if (columnName.Type.Name != "String")
//                {
//                    var eLeft = Expression.LessThan(columnName, field);
//                    var eRight = Expression.GreaterThan(columnName, field);
//                    e = Expression.Or(eLeft, eRight);
//                }
//                else
//                {
//                    MethodInfo mi2 = c.GetMethod("Contains", new Type[] { c });
//                    Expression ne = Expression.Call(columnName, mi2, field);
//                    e = Expression.IsFalse(ne);
//                }
//                break;
//            case OperatorType.StarstWith:
//                MethodInfo mi3 = c.GetMethod("StartsWith", new Type[] { c });
//                e = Expression.Call(columnName, mi3, field);

//                break;
//            case OperatorType.EndsWith:
//                MethodInfo mi4 = c.GetMethod("EndsWith", new Type[] { c });
//                e = Expression.Call(columnName, mi4, field); 
//                break;
//            default: // IN
//                Expression fields = Expression.Constant(f.Values.AsEnumerable());
//                var mi5 = typeof(Enumerable).GetRuntimeMethods().Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);
//                e = Expression.Call(columnName, mi5, fields);
//                break;
//        }
//        return e;
//        //                return Expression<Func<T, bool>> lambda =  Expression.Lambda<Func<T, bool>>(call, e);
//        //                return                                     Expression.Lambda<Func<T, bool>>(combined, new ParameterExpression[] { pe });
//    }
//}

