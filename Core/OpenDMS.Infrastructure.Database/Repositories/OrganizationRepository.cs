using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IApplicationDbContextFactory contextFactory;
    private readonly ApplicationDbContext DS;

    public OrganizationRepository(IApplicationDbContextFactory contextFactory)
    {
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
        this.contextFactory = contextFactory;
    }

    public async Task<int> AddUser(UserGroupRole acl)
    {
        DS.UserGroupRoles.Add(acl);
        return await DS.SaveChangesAsync();
    }

    public async Task<int> RemoveUser(UserGroupRole acl)
    {
        DS.UserGroupRoles.Remove(acl);
        return await DS.SaveChangesAsync();
    }

    public async Task<int> UpdateUser(UserGroupRole acl)
    {
        DS.UserGroupRoles.Update(acl);
        return await DS.SaveChangesAsync();
    }
    public async Task<List<User>> GetInactiveUsers ()
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        return DS.UserGroupRoles.AsNoTracking().GroupBy(g => g.UserId).Where(grp => grp.Max(m => m.EndISODate) < data && grp.Min(m => m.EndISODate) > 0).SelectMany(grp => grp.Select(u => u.User)).ToList();
    }

    public async Task<List<OrganizationNode>> GetInactiveNodes()
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        return DS.OrganizationNodes.AsNoTracking().Where(grp => grp.EndISODate < data && grp.EndISODate > 0).ToList();
    }


    public async Task<int> Delete(OrganizationNode node)
    {
        using (var T = await DS.Database.BeginTransactionAsync())
        {
            try
            {
                var LeftBound = node.LeftBound;
                var RightBound = node.RightBound;

                DS.OrganizationNodes.Remove(node);
                await DS.SaveChangesAsync();
                await DS.Database.ExecuteSqlRawAsync("UPDATE OrganizationNodes SET RightBound=RightBound-2 WHERE RightBound>=" + LeftBound.ToString() + " ORDER BY RightBound");
                await DS.Database.ExecuteSqlRawAsync("UPDATE OrganizationNodes SET LeftBound=LeftBound-2 WHERE LeftBound>=" + RightBound.ToString() + " ORDER BY LeftBound");
                await T.CommitAsync();
            }
            catch (Exception ex)
            {
                await T.RollbackAsync();
                throw;
            }
        }
        return 1;
    }

    public async Task<List<UserGroupRole>> GetGroupsByRole(UserFilter filter)
    {
        var now = int.Parse(DateTime.UtcNow.Date.ToString("yyyyMMdd"));
        var users = DS.UserGroupRoles.Include(u => u.User).Include(u => u.Role).Include(u => u.UserGroup).AsQueryable();
        if (!filter.IncludeDeletes)
            users = users.Where(u => u.EndISODate >= now);
        if (filter.userGroupId != "*")
            users = users.Where(g => g.StartISODate <= now && g.EndISODate >= now && g.UserGroupId == filter.userGroupId);
        if (filter.roleId != "*")
            users = users.Where(g => g.StartISODate <= now && g.EndISODate >= now && g.RoleId == filter.roleId);
        if (!String.IsNullOrEmpty(filter.filter))
            users = users.Where(g => g.User.Contact.SearchName.Contains(filter.filter) || g.User.Contact.FiscalCode.StartsWith(filter.filter) || g.User.Contact.LicTradNum.StartsWith(filter.filter) || g.User.Contact.DigitalAddresses.Any(d => d.Name.Contains(filter.filter) || d.Address.Contains(filter.filter)));
        var list = await users.AsNoTracking().ToListAsync();
        return list;
        //return await users.OrderBy(u => u.Contact.FriendlyName).ToListAsync();
    }


    public async Task<List<OrganizationNode>> GetAll(int StartISODate = 0)
    {
        return await DS.OrganizationNodes.Include(o => o.UserGroup).AsNoTracking().Where(u => (u.StartISODate <= StartISODate && u.EndISODate >= StartISODate) || StartISODate == 0).OrderBy(o => o.LeftBound).ToListAsync();
    }


    public async Task<List<UserGroupRole>> GetUsersInGroup(int StartISODate, string groupId)
    {
        return await DS.UserGroupRoles.AsNoTracking().Where(u => u.UserGroupId == groupId && ((u.StartISODate <= StartISODate && u.EndISODate >= StartISODate) || StartISODate == 0)).ToListAsync();
    }

    public async Task<List<UserGroupRole>> GetUsersInGroup(string groupId)
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        return await DS.UserGroupRoles.AsNoTracking().Where(u => u.UserGroupId == groupId && u.StartISODate <= data && u.EndISODate >= data).ToListAsync();
    }
    public async Task<List<string>> GetUsersInRole(string roleId)
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        return await DS.UserGroupRoles.AsNoTracking().Where(u => u.RoleId == roleId && u.StartISODate <= data && u.EndISODate >= data).Select(g => g.UserId).ToListAsync();
    }
    public async Task<List<UserGroupRole>> GetGroupsByUser(string userId)
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        return await DS.UserGroupRoles.AsNoTracking().Where(u => u.UserId == userId && u.StartISODate <= data && u.EndISODate >= data).ToListAsync();
    }
    public async Task<OrganizationNode> GetById(int StartISODate, string GroupId)
    {
        return await DS.OrganizationNodes.Include(o => o.UserGroup).AsNoTracking().FirstOrDefaultAsync(u => ((u.StartISODate <= StartISODate && u.EndISODate >= StartISODate) || StartISODate == 0) && u.UserGroupId == GroupId);
    }
    public async Task<OrganizationNode> GetByExternalId(string ExternalId, int StartISODate = 0)
    {
        return await DS.OrganizationNodes.Include(o => o.UserGroup).AsNoTracking().FirstOrDefaultAsync(u => ((u.StartISODate <= StartISODate && u.EndISODate >= StartISODate) || StartISODate == 0) && u.UserGroup.ExternalId == ExternalId);
    }
    public async Task<OrganizationNode> GetByName(string shortName, int StartISODate = 0)
    {
        return await DS.OrganizationNodes.Include(o => o.UserGroup).AsNoTracking().FirstOrDefaultAsync(u => ((u.StartISODate <= StartISODate && u.EndISODate >= StartISODate) || StartISODate == 0) && (u.UserGroup.ShortName == shortName));
    }

    public async Task<UserGroupRole> GetUser(int StartISODate, string groupId, string userId)
    {
        return await DS.UserGroupRoles.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId && ((u.StartISODate <= StartISODate && u.EndISODate >= StartISODate) || StartISODate == 0) && u.UserGroupId == groupId);
    }

    public async Task<UserGroupRole> GetUser(string groupId, string userId)
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        return await DS.UserGroupRoles.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId /* && u.StartISODate <= data (commentato nella risoluzione ticket (16197) organigramma il 10/01/24) */ && u.EndISODate >= data && u.UserGroupId == groupId);
    }

    //public async Task<List<string>> GetUsersInGroup(string groupId)
    //{
    //    int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
    //    return await DS.UserGroupRoles.AsNoTracking().Where(u => u.StartISODate <= data && u.EndISODate >= data && u.UserGroupId == groupId).Select(s => s.UserId).ToListAsync();
    //}

    public async Task<int> Insert(OrganizationNode node)
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        using (var T = await DS.Database.BeginTransactionAsync())
        {
            try
            {
                if (node.UserGroup.Id == null)
                {
                    node.UserGroup.Id = Guid.NewGuid().ToString();
                    node.UserGroupId = node.UserGroup.Id;
                    DS.UserGroups.Add(node.UserGroup);
                }
                var LeftBound = 1;
                var RightBound = 2;
                OrganizationNode parent = null;
                if (!String.IsNullOrEmpty(node.ParentUserGroupId))
                    parent = DS.OrganizationNodes.AsNoTracking().FirstOrDefault(u => u.UserGroupId == node.ParentUserGroupId && u.StartISODate <= data && u.EndISODate >= data);
                if (parent != null)
                {
                    LeftBound = parent.RightBound;
                    RightBound = LeftBound + 1;
                }
                else
                {
                    parent = DS.OrganizationNodes.AsNoTracking().Where(u => string.IsNullOrEmpty(u.ParentUserGroupId)).OrderByDescending(o => o.RightBound).FirstOrDefault();
                    if (parent != null)
                    {
                        RightBound = parent.RightBound + 2;
                        LeftBound = RightBound - 1;
                    }
                }
                await DS.Database.ExecuteSqlRawAsync("UPDATE OrganizationNodes SET RightBound=RightBound+2 WHERE RightBound>=" + LeftBound.ToString()+ " ORDER BY RightBound DESC");
                await DS.Database.ExecuteSqlRawAsync("UPDATE OrganizationNodes SET LeftBound=LeftBound+2 WHERE LeftBound>=" + RightBound.ToString() + " ORDER BY LeftBound DESC");
                //DS.OrganizationNodes.Where(o => o.LeftBound >= RightBound).ExecuteUpdate(u => u.SetProperty(p => p.LeftBound, u => u.LeftBound + 2));
                node.LeftBound = LeftBound;
                node.RightBound = RightBound;
                DS.OrganizationNodes.Add(node);
                await DS.SaveChangesAsync();
                await T.CommitAsync();
            }
            catch (Exception ex)
            {
                await T.RollbackAsync();
                throw;
            }
        }
        return 1;
    }
    public async Task<int> Update(OrganizationNode node)
    {
        using (var T = await DS.Database.BeginTransactionAsync())
        {
            try
            {
                DS.OrganizationNodes.Update(node);
                var users = await DS.UserGroupRoles.Where(u => u.UserGroupId == node.UserGroupId && u.StartISODate <= node.StartISODate && u.EndISODate >= node.StartISODate).ToListAsync();
                foreach (var u in users)
                {
                    if (u.EndISODate > node.EndISODate)
                        u.EndISODate = node.EndISODate;
                    if (u.StartISODate > node.EndISODate)
                        u.StartISODate = node.EndISODate;
                }
                await DS.SaveChangesAsync();
                await T.CommitAsync();
            }
            catch (Exception ex)
            {
                await T.RollbackAsync();
                throw;
            }
        }

        return 1;
    }

    public async Task<int> Move(OrganizationNode OldNode, OrganizationNode NewNode)
    {
        int data = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        using (var T = await DS.Database.BeginTransactionAsync())
        {
            try
            {
                var users = await DS.UserGroupRoles.Where(u => u.UserGroupId == OldNode.UserGroupId && u.StartISODate <= OldNode.StartISODate && u.EndISODate >= OldNode.StartISODate).ToListAsync();
                OldNode.EndISODate = NewNode.StartISODate - 1;
                DS.OrganizationNodes.Update(OldNode);
                var LeftBound = 1;
                var RightBound = 2;
                OrganizationNode parent = null;
                if (!String.IsNullOrEmpty(NewNode.ParentUserGroupId))
                    parent = DS.OrganizationNodes.AsNoTracking().FirstOrDefault(u => u.UserGroupId == NewNode.ParentUserGroupId && u.StartISODate <= data && u.EndISODate >= data);
                if (parent != null)
                {
                    LeftBound = parent.RightBound;
                    RightBound = LeftBound + 1;
                }
                else
                {
                    parent = DS.OrganizationNodes.AsNoTracking().Where(u => string.IsNullOrEmpty(u.ParentUserGroupId)).OrderByDescending(o => o.RightBound).FirstOrDefault();
                    if (parent != null)
                    {
                        RightBound = parent.RightBound + 2;
                        LeftBound = RightBound - 1;
                    }
                }

                await DS.Database.ExecuteSqlRawAsync("UPDATE OrganizationNodes SET RightBound=RightBound+2 WHERE RightBound>=" + LeftBound.ToString() + " ORDER BY RightBound DESC");
                await DS.Database.ExecuteSqlRawAsync("UPDATE OrganizationNodes SET LeftBound=LeftBound+2 WHERE LeftBound>=" + RightBound.ToString() + " ORDER BY LeftBound DESC");

                NewNode.LeftBound = LeftBound;
                NewNode.RightBound = RightBound;
                DS.OrganizationNodes.Add(NewNode);
                foreach (var u in users)
                {
                    UserGroupRole UR = new();
                    UR.UserGroupId = NewNode.UserGroupId;
                    UR.RoleId = u.RoleId;
                    UR.UserId = u.UserId;
                    UR.StartISODate = NewNode.StartISODate;
                    UR.EndISODate = NewNode.EndISODate;
                    DS.UserGroupRoles.Add(UR);
                    if (u.EndISODate > OldNode.EndISODate)
                        u.EndISODate = OldNode.EndISODate;
                    if (u.StartISODate > OldNode.EndISODate)
                        u.StartISODate = OldNode.EndISODate;
                }
                await DS.SaveChangesAsync();
                await T.CommitAsync();
            }
            catch (Exception ex)
            {
                await T.RollbackAsync();
                throw;
            }
        }

        return 1;
    }

    /*
    public async Task<int> Insert(OrganizationNode node)
    {
        NestedDataSet NDS = new NestedDataSet(DS, node.VersionId);
        NestedTreeNode Parent = NDS.Get(node.ParentId);
        {
            try {
                if (Parent.Left <= 0)
                {
                    Parent.Left = 1;
                    Parent.Right = 2;
                    DS.Execute(@"INSERT INTO TREES (nodeid, rootid, lft, rgt, Extid,row_version) values (@nodeId, @rootId, @rgt, @rgt + 1, @extid, 1)",
                                    DS.Parameter("nodeId", DbType.Int32, Parent.NodeId),
                                    DS.Parameter("rootId", DbType.String, node.VersionId),
                                    DS.Parameter("rgt", DbType.Int32, Parent.Left),
                                    DS.Parameter("extid", DbType.String, ""));
                }
                var ExtId = node.UserGroupId;
                var Depth = Parent.Depth + 1;
                var NodeId = DS.NextID("Trees", node.VersionId, "");
                var Right = Parent.Right + 1;
                var Left = Right - 1;


                DS.Execute($"UPDATE TREES set rgt=rgt+2 where rgt >= {Parent.Right} and rootid='{node.VersionId}'");
                DS.Execute($"UPDATE TREES set lft=lft+2 where lft > {Parent.Right} and rootid='{node.VersionId}'");
                DS.Execute(@"INSERT INTO TREES (nodeid, rootid, lft, rgt, Extid,row_version) values (@nodeId, @rootId, @rgt, @rgt + 1, @extid, 1)",
                              DS.Parameter("nodeId", DbType.Int32, NodeId),
                              DS.Parameter("rootId", DbType.String, node.VersionId),
                              DS.Parameter("rgt", DbType.Int32, Left),
                              DS.Parameter("extid", DbType.String, node.UserGroupId));
                node.NodeId = NodeId;
                DS.SaveChanges();
            }
            catch (Exception ex) {
                DS.RollBack();
            }

            return node.NodeId;
        }
    }
   
    public async Task<int> Move(OrganizationNode node, OrganizationNode parentNode)
    {
        int r = 0;
        NestedDataSet NDS = new NestedDataSet(DS, node.VersionId);
        NestedTreeNode NodoDaSpostare = NDS.Get(node.NodeId);
        NestedTreeNode VecchioNodoPadre = NDS.Get(node.ParentId);
        NestedTreeNode NuovoNodoPadre = NDS.Get(parentNode.NodeId);
        if (VecchioNodoPadre.NodeId != NuovoNodoPadre.NodeId)
        {
            r = NDS.Move(NodoDaSpostare, NuovoNodoPadre);
            if (r > 0) node.ParentId = parentNode.NodeId;
        }
        return r;
    }
    public async Task<int> Delete(OrganizationNode node)
    {
        NestedDataSet NDS = new NestedDataSet(DS, node.VersionId);
        NestedTreeNode Nodo = NDS.Get(node.NodeId);
        Int32 r = 0;
        r = NDS.Remove(Nodo);
        return r;
    }

    public async Task<List<UserGroup>> GetAll(string VersionId, String userId)
    {
        return await DS.userGroups.Where (u=>u.).FirstOrDefaultAsync(u => u.Id == Id);

        List<UserGroup> etp = new List<UserGroup>();
        string s = "SELECT * FROM ACL_INFO LEFT JOIN ACL_USERS ON (AAULS=MAULS) WHERE MPROP=" + IDataSource.AsString(userId) + " ORDER BY ANAME";
        using (var dr = DS.Select(s))
        {
            while (dr != null && dr.Read())
            {
                etp.Add(ReadGroup(dr));
            };
        }
        return etp;
    }

    public async Task<List<OrganizationNode>> GetAll(string VersionId)
    {
        List<OrganizationNode> nodes = new List<OrganizationNode>();
        using (var dr = DS.Select($"SELECT T.*,P.nodeId ParentId FROM TREES T LEFT JOIN TREES P ON (P.lft < T.lft)AND(P.rgt> T.rght) WHERE T.RootId='{VersionId}' ORDER BY lft,nodeId"))
        {
            while (dr != null && dr.Read())
            {
                OrganizationNode N = new OrganizationNode();
                N.NodeId = int.Parse(dr["modeId"].ToString());
                N.ParentId = int.Parse(dr["ParentId"].ToString());
                N.VersionId = VersionId;
                N.UserGroupId = dr["extId"].ToString();
                nodes.Add(N);
            }
        }
        return nodes;
    }
    public async Task<OrganizationNodeTree> GetRootNode(string VersionId)
    {
        return await GetRootNodeByGroupId(VersionId, "");
    }
    public async Task<OrganizationNodeTree> GetRootNodeByGroupId(string VersionId, string GroupId)
    {
        Dictionary<string, OrganizationNodeTree> idlist = new Dictionary<string, OrganizationNodeTree>();

        OrganizationNodeTree root = new OrganizationNodeTree();
        root.UserGroupId = "";
        root.Id = 0;
        int left = 0;
        int right = int.MaxValue;
        using (var dr = DS.Select($"SELECT T.* FROM TREES T WHERE T.RootId='{VersionId}' AND T.ExtId={GroupId}"))
        {
            if (dr != null && dr.Read())
            {
                left = int.Parse(dr["lft"].ToString());
                right = int.Parse(dr["rgt"].ToString());
                root.Id = int.Parse(dr["nodeId"].ToString());
                root.UserGroupId = dr["ExtId"].ToString();
            }
        }
        idlist[root.Id.ToString()] = root;

        using (var dr = DS.Select($"SELECT T.*,P.nodeId ParentId FROM TREES T LEFT JOIN TREES P ON (P.lft < T.lft)AND(P.rgt> T.rgt) WHERE T.lft>{left} AND T.rgt<{right} T.RootId='{VersionId}'  ORDER BY lft,nodeId"))
        {
            while (dr != null && dr.Read())
            {
                string id = "" + dr["nodeId"].ToString();
                string pid = "" + dr["ParentId"].ToString();

                OrganizationNodeTree N = new OrganizationNodeTree();
                N.Id = int.Parse(id);

                if (!idlist.ContainsKey(pid))
                    pid = "";
                OrganizationNodeTree Parent = idlist[pid];
                idlist[id] = N;
                N.Parent = Parent;
                N.VersionId = VersionId;
                N.UserGroupId = dr["extId"].ToString();
                Parent.Nodes.Add(N);
            }
        }
        return root;
    }
    public async Task<List<OrganizationNodeTree>> GetRootNodeByGroups(string VersionId, List<string> Groups)
    {
        List<OrganizationNodeTree> roots = new List<OrganizationNodeTree>();
        Dictionary<string, OrganizationNodeTree> idlist = new Dictionary<string, OrganizationNodeTree>();
        using (var dr = DS.Select($"SELECT T.* FROM TREES T WHERE T.RootId='{VersionId}' AND T.ExtId in ({string.Join("','", Groups)})"))
        {
            while (dr != null && dr.Read())
            {
                OrganizationNodeTree root = new OrganizationNodeTree();
                root.Id = int.Parse(dr["nodeId"].ToString());
                root.UserGroupId = dr["ExtId"].ToString();
                idlist[root.Id.ToString()] = root;
                roots.Add(root);
            }
        }

        using (var dr = DS.Select($"SELECT T.*,P.nodeId ParentId FROM TREES T LEFT JOIN TREES P ON (P.lft < T.lft)AND(P.rgt> T.rgt) WHERE T.RootId='{VersionId}'  ORDER BY lft,nodeId"))
        {
            while (dr != null && dr.Read())
            {
                string id = "" + dr["nodeId"].ToString();
                string pid = "" + dr["ParentId"].ToString();

                // verifico che il nodo corrente sia figlio di un nodo radice o
                // di un suo sotto figlio
                if (idlist.ContainsKey(pid))
                {
                    OrganizationNodeTree N = new OrganizationNodeTree();
                    N.Id = int.Parse(id);
                    OrganizationNodeTree Parent = idlist[pid];
                    if (idlist.ContainsKey(id))
                    {
                        // Solo un nodo radice può essere presente nella lista degli Id insieme al nodo padre
                        //In questo caso, tolgo il nodo dalle radici
                        roots.Remove(idlist[id]);
                    }
                    else
                        idlist[id] = N;
                    N.Parent = Parent;
                    N.VersionId = VersionId;
                    N.UserGroupId = dr["extId"].ToString();
                    Parent.Nodes.Add(N);
                }
            }
        }
        return roots;
    }


    public async Task<OrganizationNode> GetById(string VersionId, string GroupId)
    {
        Dictionary<string, OrganizationNodeTree> idlist = new Dictionary<string, OrganizationNodeTree>();

        OrganizationNode node = new OrganizationNode();
        node.UserGroupId = "";
        node.NodeId = 0;
        return node;
    }

    public async Task<List<string>> GetUsersById(string Id)
    {
        List<String> etp = new List<String>();
        using (IDataReader dr = DS.Select($"SELECT DISTINCT MPROP FROM ACL_USERS WHERE MAULS={Id}"))
        {
            while (dr != null && dr.Read())
            {
                etp.Add(dr[0].ToString());
            };
        }
        return etp;
    }
    public async Task<UserGroupPermissionSet> GetUserPermissions(string versionId, string groupId, string userId)
    {
        UserGroupPermissionSet etp = new UserGroupPermissionSet();
        using (IDataReader dr = DS.Select($"SELECT * FROM ACL_USERS WHERE MAULS={groupId} AND MPROP={userId}"))
        {
            if (dr != null && dr.Read())
                etp = ReadUser(dr);
        }
        return etp;
    }
    public async Task<List<UserGroupPermissionSet>> GetAllUsersPermissions(string versionId, string groupID)
    {
        List<UserGroupPermissionSet> etp = new List<UserGroupPermissionSet>();
        using (IDataReader dr = DS.Select($"SELECT * FROM ACL_USERS WHERE MAULS={groupID}"))
        {
            while (dr != null && dr.Read())
            {
                etp.Add(ReadUser(dr));
            };
        }
        return etp;

    }


    public async Task<int> AddUser(UserGroupPermissionSet acl)
    {
        Int32 r = DS.Execute(string.Format("INSERT INTO ACL_USERS (MAULS,MPROP,MVIEW,MREAD,MWRITE,MEDIT,MDELETE,MADDIMAGE,MREMOVEIMAGE,MAUTH,MSEND,MPROTOCOL,MVIEWUP,MVIEWDN,MVIEWSIDE,MINBOX,MDATA,MORA) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17})"

            , acl.GroupId
            , IDataSource.AsString(acl.UserId)
            , acl.DocumentPermissions.CanRead ? 1 : 0
            , acl.DocumentPermissions.CanView ? 1 : 0
            , acl.DocumentPermissions.CanCreate ? 1 : 0
            , acl.DocumentPermissions.CanEdit ? 1 : 0
            , acl.DocumentPermissions.CanDelete ? 1 : 0
            , acl.DocumentPermissions.CanAddImage ? 1 : 0
            , acl.DocumentPermissions.CanRemoveImage ? 1 : 0
            , acl.DocumentPermissions.CanAuthorize ? 1 : 0
            , acl.DocumentPermissions.CanShare ? 1 : 0
            , acl.DocumentPermissions.CanProtocol ? 1 : 0
            , acl.CanReadInbox ? 1 : 0
            , acl.CanViewUp ? 1 : 0
            , acl.CanViewDown ? 1 : 0
            , acl.CanViewSide ? 1 : 0
            , DateTime.UtcNow.ToString("yyyyMMdd")
            , DateTime.UtcNow.ToString("HHmmss")
            ));
        return r;
    }
    public async Task<int> UpdateUser(UserGroupPermissionSet acl)
    {
        Int32 r = DS.Execute(string.Format("UPDATE ACL_USERS SET MVIEW={3},MREAD={4},MWRITE={5},MEDIT={6},MDELETE={7},MADDIMAGE={8},MREMOVEIMAGE={9}," +
            "MAUTH={10},MSEND={11},MPROTOCOL={12},MVIEWUP={13},MVIEWDN={14},MVIEWSIDE={15},MDATA={16},MORA={17},MINBOX={2} " +
            "WHERE MAULS={0} AND MPROP={1}"

            , acl.GroupId
            , IDataSource.AsString(acl.UserId)
            , acl.CanReadInbox ? 1 : 0
            , acl.DocumentPermissions.CanRead ? 1 : 0
            , acl.DocumentPermissions.CanView ? 1 : 0
            , acl.DocumentPermissions.CanCreate ? 1 : 0
            , acl.DocumentPermissions.CanEdit ? 1 : 0
            , acl.DocumentPermissions.CanDelete ? 1 : 0
            , acl.DocumentPermissions.CanAddImage ? 1 : 0
            , acl.DocumentPermissions.CanRemoveImage ? 1 : 0
            , acl.DocumentPermissions.CanAuthorize ? 1 : 0
            , acl.DocumentPermissions.CanShare ? 1 : 0
            , acl.DocumentPermissions.CanProtocol ? 1 : 0
            , acl.CanReadInbox ? 1 : 0
            , acl.CanViewUp ? 1 : 0
            , acl.CanViewDown ? 1 : 0
            , acl.CanViewSide ? 1 : 0
            , DateTime.UtcNow.ToString("yyyyMMdd")
            , DateTime.UtcNow.ToString("HHmmss")
            ));
        return r;
    }
    public async Task<int> RemoveUser(UserGroupPermissionSet acl)
    {
        Int32 r = 0;

        if (!String.IsNullOrEmpty(acl.UserId))
        {
            r = DS.Execute(string.Format("DELETE FROM ACL_USERS WHERE MAULS={0} AND MPROP={1}",
                IDataSource.AsString(acl.GroupId), IDataSource.AsString(acl.UserId)));
        }
        else
            r = DS.Execute(string.Format("DELETE FROM ACL_USERS WHERE MAULS={0}", IDataSource.AsString(acl.GroupId)));
        return r;
    }


}

*/
}