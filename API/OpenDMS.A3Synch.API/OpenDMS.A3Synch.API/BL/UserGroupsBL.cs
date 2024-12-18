using A3Synch.Interfacce;
using A3Synch.Models;
using A3Synch.Utility;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.BL
{
    public class UserGroupsBL : IUserGroupsBL
    {
        private readonly IUserGroupsDAO _usergroupdao;
        private IConfiguration _config;
        private readonly IUtils _utils;

        public UserGroupsBL(IUserGroupsDAO usergroupdao, IConfiguration config, IUtils utils)
        {
            _usergroupdao = usergroupdao;
            _config = config;
            _utils = utils;
        }

        //Funzione per ciclare tutte le pagine delle strutture di A3
        public async Task<int> GetAllOrganizationsPages(List<Struttura> allUnits)
        {
            string url = (String)_config.GetValue(typeof(String), "A3_API_URL") + "/units?page=1";
            StrutturaInput input = await _utils.GetOrganizationPage(url);
            List<UserGroups> risultati = new List<UserGroups>();

            // Recupera tutti gli ExternalId dal database
            List<string> existingExternalIds = await _usergroupdao.GetAllExternalIds();

            // Crea una lista per salvare gli ExternalId in arrivo
            List<string> incomingExternalIds = new List<string>();

            int last_page = input.results.units.last_page;

            // Sincronizza le nuove strutture
            risultati = await SynchUserGroupsInDb(allUnits);

            // Salva gli ExternalId in arrivo
            incomingExternalIds.AddRange(risultati.Select(g => g.ExternalId));

            // Trova gli ExternalId che non sono più presenti
            var externalIdsToExpire = existingExternalIds.Except(incomingExternalIds).ToList();

            // Aggiorna la EndISODate per i gruppi di utenti non presenti
            if (externalIdsToExpire.Any())
            {
                await UpdateEndDateForMissingExternalIds(externalIdsToExpire);
            }

            int res = await _usergroupdao.UpdateUserGroups(risultati);
            return res;
        }

        // Funzione per sincronizzare la tabella UserGroups del database con le API di A3
        public async Task<List<UserGroups>> SynchUserGroupsInDb(List<Struttura> input)
        {
            List<UserGroups> risultati = new List<UserGroups>();

            foreach (var struttura in input)
            {
                UserGroups groups = new UserGroups
                {
                    Id = Guid.NewGuid().ToString(),
                    ExternalId = struttura.id.ToString(),
                    ExternalApp = Utils.A3Synch,
                    ShortName = struttura.denominazione,
                    Name = struttura.descrizione,
                    CreationDate = DateTime.Parse(struttura.inizio_validita),
                    ClosingDate = DateTime.Parse(struttura.fine_validita),
                    CreationUser = "",
                    ClosingUser = "",
                    NotificationStrategy = 0,
                    NotificationStrategyCC = 0,
                    Visible = true,
                    Closed = _utils.CheckClosed(struttura.fine_validita)
                };

                if (!groups.Closed)
                {
                    // aggiungi l'oggetto UserGroups alla lista dei risultati
                    risultati.Add(groups);
                }
            }

            return risultati;
        }

        // Nuovo metodo per aggiornare la EndISODate
        private async Task UpdateEndDateForMissingExternalIds(List<string> externalIds)
        {
            List<UserGroups> update_list = new List<UserGroups>();
            // Aggiorna la EndISODate per ogni ExternalId mancante
            foreach (var externalId in externalIds)
            {
                // Recupera l'UserGroup dal DB usando l'ExternalId
                string userGroupId = _usergroupdao.GetUserGroupsId(externalId);
                UserGroups userGroup = await _usergroupdao.GetUserGroupsById(userGroupId);

                if (userGroup != null)
                {
                    userGroup.ClosingDate = DateTime.UtcNow.AddDays(-1);
                    update_list.Add(userGroup);
                    await _usergroupdao.UpdateUserGroups(update_list);
                }
            }
        }

        public void ResetStatus()
        {
            SharedVariables.elaborated_groups_counter = 0;
            SharedVariables.total_groups_counter = 0;

        }
    }
}
