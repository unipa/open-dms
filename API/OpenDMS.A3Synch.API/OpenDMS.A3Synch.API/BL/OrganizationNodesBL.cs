using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Models;
using OpenDMS.A3Synch.API.Utility;
using System.Text.Json;

namespace A3Synch.BL
{
    public class OrganizationNodesBL : IOrganizationNodesBL
    {
        private readonly IOrganizationNodesDAO _organizationnodesDAO;
        private readonly IUserGroupsDAO _usergroupsDAO;
        private readonly IUtils _utils;
		private List<Nodo> listaNuova = new List<Nodo>();

		public OrganizationNodesBL(IOrganizationNodesDAO organizationnodesDAO, IUserGroupsDAO usergroupsDAO, IUtils utils)
        {
            _organizationnodesDAO = organizationnodesDAO;
            _usergroupsDAO = usergroupsDAO;
            _utils = utils;
        }

		public List<Nodo>  GetListaNuova()
		{
			return this.listaNuova;
		}

		public void SetListaNuova(List<Nodo> newListValue)
		{
			this.listaNuova = newListValue;
		}

		//public async Task<List<Struttura>> GetAllStructures()
  //      {
		//	try
		//	{
		//		string jsonString = await _utils.GetAllOrganizationsJson();
		//		List<Struttura> strutture = JsonSerializer.Deserialize<List<Struttura>>(jsonString);
		//		return strutture;
		//	}
		//	catch
		//	{
		//		throw new Exception("Errore in OrganizationNodesBL");
		//	}
            
  //      }

		public List<Nodo> ConvertStructuresInNode(List<Struttura> strutture)
		{
			try
			{
				//Ordino la lista in modo che i padri siano messi all'inizio della lista
				strutture = strutture.OrderBy(s => s.id_struttura == null ? 0 : 1).ToList();
				List<Nodo> nodi = new List<Nodo>();
				int parentId = 0;
				foreach (Struttura struttura in strutture)
				{
					if (struttura.id_struttura != null)
					{
						parentId = int.Parse(struttura.id_struttura);
					}

					Nodo nodo = new Nodo()
					{
						Id = struttura.id,
						ParentId = parentId,
						LeftBound = 0,
						RightBound = 0,
					};
					nodi.Add(nodo);
				}
				return nodi;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public List<Nodo> ConvertOrganizationNodesInNode(List<OrganizationNodes> organizationNodes )
		{
			try
			{
				int seed_id = 10000;
				List<Nodo> nodi = new List<Nodo>();
				int parentId = 0;
				foreach (OrganizationNodes nodoOrg in organizationNodes)
				{
					if (nodoOrg.ParentUserGroupId != null)
					{
						parentId = int.Parse(_usergroupsDAO.GetExternalId(nodoOrg.ParentUserGroupId));
					}

					Nodo nodo = new Nodo()
					{
						Id = seed_id+1,
						ParentId = parentId,
						LeftBound = 0,
						RightBound = 0,
						oldId = nodoOrg.Id
					};
					nodi.Add(nodo);
					seed_id++;
				}
				return nodi;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}
		
		public List<Nodo> MergeNodeList(List<Nodo> lista1, List<Nodo> lista2)
		{
			try
			{
				List<Nodo> mergedList = new List<Nodo>();
				mergedList = lista1.Concat(lista2).ToList();
				return mergedList;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		static List<Nodo> OrderRecords(List<Nodo> records)
		{
			try
			{
				var orderedRecords = new List<Nodo>();

				// Aggiungi i record con parentId = 0
				orderedRecords.AddRange(records.Where(r => r.ParentId == 0));

				// Aggiungi i record con parentId corrispondente ad un Id già presente nella lista
				foreach (var record in records)
				{
					if (record.ParentId != 0)
					{
						var parentRecord = orderedRecords.FirstOrDefault(r => r.Id == record.ParentId);
						if (parentRecord != null)
						{
							var index = orderedRecords.IndexOf(parentRecord);
							orderedRecords.Insert(index + 1, record);
						}
						else
						{
							orderedRecords.Add(record);
						}
					}
				}

				return orderedRecords;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public static List<Nodo> BuildTree(List<Nodo> records)
		{
			try
			{
				var nodes = new Dictionary<int, Nodo>();

				// Costruisci un dizionario dei nodi
				foreach (var record in records)
				{
					nodes[record.Id] = record;
				}
				records = OrderRecords(records);

				// Costruisci l'albero collegando i nodi ai loro genitori
				var roots = new List<Nodo>();
				foreach (var record in records)
				{
					if (record.ParentId == 0)
					{
						roots.Add(nodes[record.Id]);
					}

					if (nodes.ContainsKey(record.ParentId.Value))
					{
						nodes[record.ParentId.Value].Figli.Add(record);
					}
					else if (!nodes.ContainsKey(record.ParentId.Value) && record.ParentId != 0)
					{
						// Se il nodo genitore non è presente SKIPPA IL RECORD (manca il padre da UNIPA NON CONSIDERO IL RECORD)
						continue;
					}
				}

				return roots;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public static List<List<int>> CountPaths(Nodo tree)
		{
			try
			{
				var paths = new List<List<int>>();

				void Dfs(Nodo node, List<int> path)
				{
					if (node.Figli.Count == 0)
					{
						paths.Add(path);
						return;
					}

					foreach (var child in node.Figli)
					{
						var newPath = new List<int>(path);
						newPath.Add(child.Id);
						Dfs(child, newPath);
					}
				}

				Dfs(tree, new List<int> { tree.Id });
				return paths;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}

		}

		public static Nodo SetFirstFatherBound(Nodo node)
		{
			try
			{
				node.LeftBound = 1;
				node.RightBound = 2;
				return node;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public static bool IsFather(Nodo node)
		{
			try
			{
				if (node.ParentId == 0)
					return true;
				else
					return false;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}

		}


		public static bool IsFirstInList(List<Nodo> lista)
		{
			try
			{
				if (lista.Count == 0)
					return true;
				else
					return false;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}

		}

		public static int? MaxBound(List<Nodo> lista)
		{
			try
			{
				if (lista == null || lista.Count == 0)
					return null;

				var rightBounds = lista.Select(node => node.RightBound).Where(rb => rb != 0);
				return rightBounds.Any() ? rightBounds.Max() : 0;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public static Nodo TrovaRecordPerId(List<Nodo> records, int idCercato)
		{
			try
			{
				foreach (var record in records)
				{
					if (record.Id == idCercato)
					{
						return record;
					}
				}
				return null;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public static bool NodeIsInList(int nodeId, List<Nodo> list)
		{
			try
			{
				if (list.Count > 0)
				{
					foreach (var item in list)
					{
						if (item.Id == nodeId)
						{
							return true;
						}
					}
					return false;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}

		}

		public async Task<List<Nodo>> CostruisciAlberoCompletoConBound(List<List<int>> percorsi, List<Nodo> listaNuova, List<Nodo> listaOriginale)
		{
			try
			{
				foreach (var percorso in percorsi)
				{
					foreach (var id in percorso)
					{
						Nodo current_node = TrovaRecordPerId(listaOriginale, id);
						bool isInList = NodeIsInList(id, listaNuova);

						if (IsFather(current_node) && !isInList)
						{
							if (IsFirstInList(listaNuova))
							{
								current_node = SetFirstFatherBound(current_node);
								if (!isInList)
								{
									listaNuova.Add(current_node);
								}

							}
							else
							{
								if (!isInList)
								{
									int? sibillingRightBound = MaxBound(listaNuova);
									listaNuova = await AddNodeAsSiblingOfNode(listaNuova, current_node, sibillingRightBound);
								}

							}
						}
						else
						{
							if (!isInList)
							{
								var fatherNode = TrovaRecordPerId(listaOriginale, current_node.ParentId.Value);
								listaNuova = await AddNodeAsChildOfNode(listaNuova, current_node, fatherNode.LeftBound);
							}

						}
					}
				}
				return listaNuova;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public async Task<List<Nodo>> AddNodeAsSiblingOfNode(List<Nodo> nodeList, Nodo newNode, int? sibilingRightBound)
		{
			try
			{
				await Task.Yield(); // Consente ad altre attività di eseguire il codice prima di continuare

				// Esegui le operazioni aritmetiche in parallelo su tutti gli elementi della lista
				nodeList.AsParallel().ForAll(node =>
				{
					if (node.RightBound > sibilingRightBound)
					{
						node.RightBound += 2;
						node.LeftBound += (node.LeftBound > sibilingRightBound ? 2 : 0);
					}
				});
				// Aggiungi il nuovo nodo come fratello del nodo massimo
				newNode.LeftBound = (int)(sibilingRightBound + 1);
				newNode.RightBound = (int)(sibilingRightBound + 2);
				nodeList.Add(newNode);

				return nodeList;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}

		}
		public async Task<List<Nodo>> AddNodeAsChildOfNode(List<Nodo> nodeList, Nodo newNode, int parentLeftBound)
		{
			try
			{
				await Task.Yield(); // Consente ad altre attività di eseguire il codice prima di continuare

				// Esegui le operazioni aritmetiche in parallelo su tutti gli elementi della lista
				nodeList.AsParallel().ForAll(node =>
				{
					if (node.RightBound > parentLeftBound)
					{
						node.RightBound += 2;
						node.LeftBound += (node.LeftBound > parentLeftBound ? 2 : 0);
					}
				});

				// Aggiungi il nuovo nodo come figlio del nodo di riferimento
				newNode.LeftBound = parentLeftBound + 1;
				newNode.RightBound = parentLeftBound + 2;
				nodeList.Add(newNode);

				return nodeList;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public async Task<List<Nodo>> StartProcess(List<Struttura> strutture)
		{
			try
			{
				//List<Struttura> strutture = await GetAllStructures();
				List<Nodo> listaA3 = ConvertStructuresInNode(strutture);
				List<OrganizationNodes> nodi_vecchi = await _organizationnodesDAO.GetOldOrganizationNodes();
				List<Nodo> lista_nodi_vecchi = ConvertOrganizationNodesInNode(nodi_vecchi);
				List<Nodo> listaOriginale = MergeNodeList(lista_nodi_vecchi, listaA3);


				List<Nodo> trees = BuildTree(listaOriginale);
				List<Nodo> listaNuova = GetListaNuova();
				List<Nodo> nuova_lista = new List<Nodo>();
				foreach (Nodo tree in trees)
				{

					List<List<int>> percorsi = CountPaths(tree);
					nuova_lista = await CostruisciAlberoCompletoConBound(percorsi, listaNuova, listaOriginale);
					SetListaNuova(nuova_lista);
				}
				nuova_lista = GetListaNuova();
				return nuova_lista;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

		public async Task<List<OrganizationNodes>> CreateOrganizationNodes(List<Nodo> nodi)
		{
			try
			{
				List<OrganizationNodes> organizationNodes = new List<OrganizationNodes>();

				foreach (Nodo nodo in nodi)
				{
					string userGroupId = _usergroupsDAO.GetUserGroupsId(nodo.Id.ToString());

					if (userGroupId != null && nodo.oldId == null)
					{
						OrganizationNodes organizationNode = new OrganizationNodes()
						{
							Id = Guid.NewGuid().ToString(),
							LeftBound = nodo.LeftBound,
							RightBound = nodo.RightBound,
							ParentUserGroupId = _usergroupsDAO.GetUserGroupsId(nodo.ParentId.ToString()),
							UserGroupId = _usergroupsDAO.GetUserGroupsId(nodo.Id.ToString()),
							StartISODate = _utils.ISOnumericDate(_usergroupsDAO.GetCreationDate(nodo.Id.ToString())),
							EndISODate = _utils.ISOnumericDate(_usergroupsDAO.GetClosingDate(nodo.Id.ToString())),
							TaskReallocationStrategy = 0,
							TaskReallocationProfile = null,
							ClosingNote = null,
							CreationDate = DateTime.Now,
							LastUpdate = DateTime.Now
						};
						organizationNodes.Add(organizationNode);
					}
					else
					{
						OrganizationNodes organizationNode = new OrganizationNodes()
						{
							Id = nodo.oldId,
							LeftBound = nodo.LeftBound,
							RightBound = nodo.RightBound,
							ParentUserGroupId = _usergroupsDAO.GetUserGroupsId(nodo.ParentId.ToString()),
							UserGroupId = null,
							StartISODate = await _organizationnodesDAO.GetStartISODate(nodo.oldId),
							EndISODate = await _organizationnodesDAO.GetENDISODate(nodo.oldId),
							TaskReallocationStrategy = 0,
							TaskReallocationProfile = null,
							ClosingNote = null,
							CreationDate = DateTime.Now,
							LastUpdate = DateTime.Now
						};
						organizationNodes.Add(organizationNode);
					}


				}
				return organizationNodes;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

        public async Task<int> SyncOrganigrammaNodes(List<Struttura> allUnits)
        {
			try
			{
				List<Nodo> lista_nodi = await StartProcess(allUnits);
				List<OrganizationNodes> organizationNodes = await CreateOrganizationNodes(lista_nodi);
				int res = await _organizationnodesDAO.UpdateOrganizationNode(organizationNodes);

				return res;
			}
			catch
			{
				throw new Exception("Errore in OrganizationNodesBL");
			}
		}

        public void ResetStatus()
        {
            SharedVariables.total_nodes_counter = 0;
            SharedVariables.elaborated_nodes_counter = 0;
        }
    }
}
