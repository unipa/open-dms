# Rilascio Sprint #1

## OpenDMS.TenantManager.API
Al suo interno è presente il progetto del microservizio che assolve il requisito:

- [x] Sviluppo DMS - Definizione tenants: Saranno rilasciate delle API per la gestione dei tenant applicatiivi (creazione, modifca, cancellazione)
- [x] Sviluppo DMS - Integrazione con IAM: Saranno implementate le verifiche dell'autenticazione sulle API precedentemente rilasciate. L'accesso al sistema DMS sarà possibile solo attraverso l'autenticazione su Keycloak. Un tentativo di accesso non autorizzato reindirizzerà verso la pagina di login di Keycloak
Il servizio, oltre alle API, espone anche una interfaccia web di amministrazione dei tenant.

## OpenDMS.Admin.API
Al suo interno è presente il progetto del microservizio che assolve i seguenti requisiti:

- [x] Sviluppo DMS - Definizione ACL: Saranno rilasciate API per la definizione di ACL per la gestione dei permessi sui documenti, da associare a tipologie documentali
- [x] Sviluppo DMS - Definizione Dizionario Metadati: Saranno rilasciate API per la definizione di un dizionario di metadati custom per gestire informazioni collegate a fonti dati esterne o tabellari
- [x] Sviluppo DMS - Definizione delle tipologie documentali: Saranno rilasciate API per la defininzione di tipologie documentali organizzate per categoria. Ogni tipologia dovrà possedere un set personalizzato di metadati e criteri di gestione associate ad ACL 
- [x] Sviluppo DMS - Integrazione con IAM: Saranno implementate le verifiche dell'autenticazione sulle API precedentemente rilasciate. L'accesso al sistema DMS sarà possibile solo attraverso l'autenticazione su Keycloak. Un tentativo di accesso non autorizzato reindirizzerà verso la pagina di login di Keycloak

## OpenDMS.DocumentManager.API
Al suo interno è presente il progetto del microservizio che assolve i requisiti:

- [x] Sviluppo DMS - Archiviazione documenti: Saranno rilasciate API per l'archiviaizone di un documento generico, accessibile all'utente creatore (proprietario)
- [x] Sviluppo DMS - Integrazione con IAM: Saranno implementate le verifiche dell'autenticazione sulle API precedentemente rilasciate. L'accesso al sistema DMS sarà possibile solo attraverso l'autenticazione su Keycloak. Un tentativo di accesso non autorizzato reindirizzerà verso la pagina di login di Keycloak


# Rilascio Sprint #2

## OpenDMS.Admin.API
Sono state integrate ulteriori API per poter gestire applicativamente le seguenti funzionalità:

- [x] Sviluppo DMS - Api AppSettings: Permette la gestione delle impostazioni di sistema (es. template dei messaggi di notifica, destinatari di messaggi di errore, ecc.)
- [x] Sviluppo DMS - Api ExternalDatasource: Permette il collegamento di fonti dati esterne al DMS da utilizzare nella gestione dei metadati dei documenti
- [x] Sviluppo DMS - Api LookupTable: Permette la gestione di tabelle di decodifica interne al DMS da utilizzare nella gestione dei metadati dei documenti
- [x] Sviluppo DMS - Api Role & RolePermissions: Permette la gestione dei ruoli "documentali" all'interno del sistema DMS (i ruoli documentali definiscono i profili autorizzativi sui documenti, a differenza dei ruoli applicativi gestiti su Keycloak per l'accesso alle funzionalità del sistema)

## OpenDMS.DocumentManager.API
Sono state integrate ulteriori API per poter gestire applicativamente le seguenti funzionalità:

- [x] Sviluppo DMS - Gestione versioning dei file: Saranno rilasciate API per la archiviaizone e riarchiviazione di un file associato ad un documento. Il sistema deve verificare la sua impronta e sostituilo al precedete solo in caso di variazione. 
- [x] Sviluppo DMS - Rimozione contenuto di un documento: Saranno rilasciate API per la per rimozione di un file e il tracciamento del'operaizone. 
- [x] Sviluppo DMS - Tracciatura delle azioni sui documenti: Saranno rilasciate API per il tracciamento completo di tutte le attività eseguite sui documenti come la visualizzaizone, la modifica di metadati, la revisione di file e la modifica di permessi
- [x] Sviluppo DMS - Classificazione di un documento: Saranno rilasciate API per la classificaizone di un qualsiasi documento (in cui si abbiano sufficienti permessi) indicando un tipologia documentale e i relativi metadati obbligatori e facoltativi. La definizione delle tipologie e dei metadati viene gestita da interfaccia di amministrazione

## OpenDMS.TaskLst.API
Al suo interno è presente il progetto del microservizio che assolve i requisiti:

- [x] Sviluppo BPMS - Sviluppo Tasklist : Saranno rilasciate le interfacce utente per gestire la tasklist delle attività di tipo utente. Tali attività potranno essere assegnate ad un utente specifico o ad un gruppo(ufficio). Le attività assegnate all'utente saranno visibili in un'area specifica e resteranno in gestione all'utente finchè non le prenderà in carico e le eseguirà, oppure potrà rifiutarle o inotrarle ad altro utente. Le attività di gruppo saranno visibili a tutti gli utenti che disporranno di tale permesso e dovranno essere presi in carico per essere spostati nella propria cassetta di attività personali.
- [x] Sviluppo BPMS - Funzionalità di Esecuzione di una attività: Saranno rilasciate le interfacce utente per l'esecuzione delle attività (task utente) prese in carico. L'esecuzione può essere eseguita in 3 modalità: 1. attraverso una conferma motivata di svolgimento della attiivtà richiesta. 2. attraverso la compilazione di un form. 3. attraverso la firma di un documento
- [x] Sviluppo BPMS - Funzionalità di rifiuto di un'attività assegnata: Saranno rilasciate le interfacce utente per rifiutare una attività assegnata. L'utente che ritiene di non essere il destinatario di un'attività può rifiutarla indicandone una motivazione che sarà recapitata al mittente dell'attività
- [x] Sviluppo BPMS (1) - Funzionalità avviso di ricezione di una nuova attività: Sarà rilasciata la funzionalità di invio di una email per ogni attività assegnata ad un utente. L'avviso conterrà un link all'attività stessa. Le attività assegnate ad un gruppo/ufficio producono una mail ad ogni utente autorizzato all'accesso alle attività di gruppo

(1) Il rilascio di questa attività è condizionato da sviluppi successivi. La mail di notifica viene prodotta (come contenuti) e serializzata su database in attesa di essere inviata da un servizio che sarà realizzato nelle fasi successive (Fase 2 - Sprint #4 - Integrazione PEO e PEC)

## OpenDMS.Identity.API
Al suo interno è presente il progetto del microservizio che assolve i requisiti:

- [x] Sviluppo BPMS - Sviluppo Tasklist: Api che fornisce le immagini Avatar degli utenti e informazioni sugli utenti di sistema

## OpenDMS.Search.API
Al suo interno è presente il progetto del microservizio che assolve i requisiti:

- [x] Sviluppo BPMS - Sviluppo Tasklist: Api che fornisce le funzionalità di interrogazione dei Task e dei documenti del sistema DPM e BPM.

## OpenDMS.Organizazion.API
Al suo interno è presente il progetto del microservizio che assolve i requisiti:

- [x] Integrazione con IAM (A3) di UNIPA - Profilazione utenti: Api che permette la gestione della struttura organizzativa che sarà utilizzata come middleware applicativo dai servizi della piattaforma 
