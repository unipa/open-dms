using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.DAO
{
    public class OrganizationNodesDAO : IOrganizationNodesDAO
    {
        private readonly ILogger<OrganizationNodesDAO> _logger;
        private readonly IContext _ctx;

        public OrganizationNodesDAO(IContext ctx, ILogger<OrganizationNodesDAO> logger)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<List<OrganizationNodes>> GetAllOrganizationNodes()
        {
            try
            {
                var result = await _ctx.OrganizationNodes
                    .Where(u => u.UserGroupId != null)
                    .Select(u => new OrganizationNodes
                    {
                        Id = u.Id,
                        LeftBound = u.LeftBound,
                        RightBound = u.RightBound,
                        UserGroupId = u.UserGroupId,
                        ParentUserGroupId = u.ParentUserGroupId,
                        StartISODate = u.StartISODate,
                        EndISODate = u.EndISODate,
                        TaskReallocationProfile = u.TaskReallocationProfile,
                        TaskReallocationStrategy = u.TaskReallocationStrategy,
                        ClosingNote = u.ClosingNote,
                        CreationDate = u.CreationDate,
                        LastUpdate = u.LastUpdate
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in GetAllOrganizationNodes, errore: " + ex.Message);
            }
        }

		public async Task<List<OrganizationNodes>> GetOldOrganizationNodes()
		{
			try
			{
				var result = await _ctx.OrganizationNodes
					.Where(u => u.UserGroupId == null)
					.Select(u => new OrganizationNodes
					{
						Id = u.Id,
						LeftBound = u.LeftBound,
						RightBound = u.RightBound,
						UserGroupId = u.UserGroupId,
						ParentUserGroupId = u.ParentUserGroupId,
						StartISODate = u.StartISODate,
						EndISODate = u.EndISODate,
						TaskReallocationProfile = u.TaskReallocationProfile,
						TaskReallocationStrategy = u.TaskReallocationStrategy,
						ClosingNote = u.ClosingNote,
						CreationDate = u.CreationDate,
						LastUpdate = u.LastUpdate
					})
					.ToListAsync();

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
				throw new Exception("Errore in OrganizationNodesDAO in GetAllOrganizationNodes, errore: " + ex.Message);
			}
		}

		public async Task<int> UpdateOrganizationNode(List<OrganizationNodes> organizationNodes)
        {
            if (organizationNodes.Count > 1)
            {
                try
                {
                    SharedVariables.total_nodes_counter = organizationNodes.Count;
                    List<OrganizationNodes> oldList = await _ctx.OrganizationNodes.ToListAsync();
                    _ctx.OrganizationNodes.RemoveRange(oldList);
                    var res = 0;
                    foreach (var node in organizationNodes)
                    {
                        SharedVariables.elaborated_nodes_counter++;
                        _ctx.OrganizationNodes.Add(node);
                        res = await _ctx.SaveChangesAsync();
                        _ctx.Entry(node).State = EntityState.Detached;
                    }
                    return res;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                    throw new Exception("Errore in OrganizationNodesDAO in UpdateOrganizationNode, errore: " + ex.Message);
                }
                finally
                {
                    foreach (var organizationNode in organizationNodes)
                    {
                        _ctx.Entry(organizationNode).State = EntityState.Detached;
                    }
                }
            } else
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: Struttura vuota");
            }
            return 0;
        }
        public async Task<int> MaxRightBound()
        {
            try
            {
                // Trova il nodo con il right bound massimo
                int? maxRightBound = await _ctx.OrganizationNodes.MaxAsync(n => (int?)n.RightBound);
                return maxRightBound ?? 0;
            }
            catch(Exception ex)
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in MaxRightBound, errore: " + ex.Message);
            }
            
        }
        public async Task<int> AddNodeAsSiblingOfNode(OrganizationNodes newNode, int sibilingRightBound)
        {
            try
            {
                var res = 0;
                // Aggiorna i left e right bound degli altri nodi nel tree model
                var nodesToUpdate = await _ctx.OrganizationNodes
                    .Where(n => n.RightBound > sibilingRightBound)
                     .OrderByDescending(n => n.RightBound)
                     .ToListAsync();

                foreach (var node in nodesToUpdate)
                {
                    Console.WriteLine($"{node.RightBound}");
                    node.RightBound += 2;
                    node.LeftBound += (node.LeftBound > sibilingRightBound ? 2 : 0);
                    res = await _ctx.SaveChangesAsync();
                }

                // Aggiungi il nuovo nodo come fratello del nodo massimo
                newNode.LeftBound = sibilingRightBound + 1;
                newNode.RightBound = sibilingRightBound + 2;
                _ctx.OrganizationNodes.Add(newNode);

                res = await _ctx.SaveChangesAsync();
                return res;
            }
            catch(Exception ex) 
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in AddNodeAsSiblingOfNode, errore: " + ex.Message);
            }
            finally
            {
                _ctx.Entry(newNode).State = EntityState.Detached;
            }
        }
        public async Task<int> AddNodeAsChildOfNode(OrganizationNodes newNode, int parentLeftBound)
        {
            try
            {
                var res = 0;
                // Aggiorna i left e right bound degli altri nodi nel tree model
                var nodesToUpdate = await _ctx.OrganizationNodes
                    .Where(n => n.RightBound >= parentLeftBound)
                    .OrderByDescending(n => n.RightBound)
                    .ToListAsync();

                foreach (var node in nodesToUpdate)
                {
                    node.RightBound += 2;
                    node.LeftBound += (node.LeftBound > parentLeftBound ? 2 : 0);
                    res = await _ctx.SaveChangesAsync();
                }

                // Aggiungi il nuovo nodo come figlio del nodo di riferimento
                newNode.LeftBound = parentLeftBound + 1;
                newNode.RightBound = parentLeftBound + 2;
                _ctx.OrganizationNodes.Add(newNode);

                res = await _ctx.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in AddNodeAsChildOfNode, errore: " + ex.Message);
            }
            finally
            {
                _ctx.Entry(newNode).State = EntityState.Detached;
            }
        }
        
        public async Task<int> GetLeftBoundParent(string id)
        {
            try
            {
                // Trova il nodo con il right bound massimo
                var node = _ctx.OrganizationNodes.FirstOrDefault(node => node.UserGroupId == id);
                return node?.LeftBound ?? 0;
            }
            catch(Exception ex) 
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in GetLeftBoundParent, errore: " + ex.Message);
            }
        }

        public string? GetOrganizationNodeId(string externalId)
        {
            try
            {
                string? groupId = null;
                if (externalId != null)
                {
                    var group = _ctx.UserGroups.FirstOrDefault(node => node.ExternalId == externalId);
                    
                    
                    if(group.Id != null)
                    {
                        groupId = group.Id;
                        var node = _ctx.OrganizationNodes.FirstOrDefault(node => node.UserGroupId == groupId);
                        return groupId;
                    }
                }
                return groupId;
            }
            catch(Exception ex)
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in GetOrganizationNodeId, errore: " + ex.Message);
            }
            
        }
        public DateTime GetOrganizationCreationDate(string nodeId)
        {
            try
            {
                DateTime creationDate = DateTime.Now;
                if(nodeId != null)
                {
                    var node = _ctx.OrganizationNodes.FirstOrDefault(node => node.Id == nodeId);
                    if(node != null)
                    {
                        creationDate = node.CreationDate;
                    }
                    else 
                    { 
                        return DateTime.Now; 
                    }
                }
                return creationDate;
            }
            catch(Exception ex)
            {
                _logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
                throw new Exception("Errore in OrganizationNodesDAO in GetOrganizationCreationDate, errore: " + ex.Message);
            }
        }

		public async Task<int> GetStartISODate(string id)
		{
			try
			{
				// Trova il nodo con il right bound massimo
				var node = _ctx.OrganizationNodes.FirstOrDefault(node => node.Id == id);
				return node?.StartISODate ?? 0;
			}
			catch (Exception ex)
			{
				_logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
				throw new Exception("Errore in OrganizationNodesDAO in GetStartISODate, errore: " + ex.Message);
			}
		}

		public async Task<int> GetENDISODate(string id)
		{
			try
			{
				// Trova il nodo con il right bound massimo
				var node = _ctx.OrganizationNodes.FirstOrDefault(node => node.Id == id);
				return node?.EndISODate ?? 0;
			}
			catch (Exception ex)
			{
				_logger.LogError("Errore in OrganizationNodesDAO, errore: " + ex.Message);
				throw new Exception("Errore in OrganizationNodesDAO in GetENDISODate, errore: " + ex.Message);
			}
		}
	}
}
