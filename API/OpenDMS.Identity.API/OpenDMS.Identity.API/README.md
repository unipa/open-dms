# Introduzione 
Admin.API espone le API di amministrazione del DMS

Attualmente il sistema espone le seguenti API:
1. ACL
2. AppSettings
3. CustomFields
4. DocumentTypes
5. LookupTables
6. Roles

# Guida introduttiva
Il servizio può essere attivato come container docker o lanciato da console. Il sistema si collega al database del registro dei tenant per determinare, attraverso il nome host, il proprio database di gestione. 
La configurazione del database del registro dei tenant è indicata nel file ***appsettings.json***

Le descrizioni delle API sono disponibili online, tramite [swagger](http://localhost/swagger/index.html) oppure sul file ***OpenDMS.TenantManager.API***

## API

### ACL
Le ACL definiscono una lista di autorizzazioni per utenti/struttura e ruoli che vengono applicate sulle tipologie documentali per definire i criteri di accesso e gestione dei documenti. 
Ogni ACL può essere associata a pià tipologie documentali e definisce quali autorizzazioni vengono concesse agli utenti, ai gruppi o ai ruoli sulle tipologie documentali associate.

es.
Autorizzare studenti e docenti alla visualizzazione del documento "Comunicazioni dalla Biblioteca" e permettere al personale della biblioteca di creare nuovi documenti

`
***ACL     Profilo     Codice Profilo      Permesso    Valore Permesso***     
ACL1    Ruolo       Studente            CanView     Granted             
ACL1    Ruolo       Docente             CanView     Granted             
ACL1    Struttura   Biblioteca          CanCreate   Granted
`

### AppSettings
AppSettings definiscono le configurazioni del tenant documentale.
Ogni configurazione è caratterizzata da una chiave e un valore.

es.
Autorizzare studenti e docenti alla visualizzazione del documento "Comunicazioni dalla Biblioteca" e permettere al personale della biblioteca di creare nuovi documenti

`
***Key                  Value***     
Tenant.RootPath         /tenant1/
Document.SoftDelete     Y
`

### CustomFields
Definiscono un dizionario di metadati che possono essere associati alle tipologie documentali.

### DocumentTypes
Definiscono le tipologie documentali per documenti e fascicoli. 

### LookupTables
Sono tabelle interne che definiscono ad esempio i registri di protocollo, i repertori, i canali di trasmissione di un documento, ...

### Roles
Definisce i ruoli applicativi e i permessi associati a ciascun ruolo. I ruoli possono essere assocati agli utenti esclusivamente tramite l'appartenenza ad uno o più gruppi nella struttura di organigramma.
I permessi associati ad un ruolo possono essere sovrascritti esclusivamente nell'ACL di una tipologia documentale o nelle ACL delle istanze dei documenti (non ancora gestita).

## Installazione
E' necessario creare un client nel realm di autenticazione di Keycloak.
Le informazioni del client devono essere inserite nel file ***appsettings.json*** oppure possono essere utilizzate come parametri in riga di comando o di ambiente in un file YAML

es.
`  "Keycloak": {
    "realm": "test",
    "auth-server-url": "http://172.25.0.145:8080/",
    "ssl-required": "external",
    "resource": "test-admin",
    "verify-token-audience": true,
    "credentials": {
      "secret": "J93HUyuig83PO5jSFZbVK31zybDH2KvM"
    },
    "confidential-port": 0,
    "policy-enforcer": {}
  }
`
Inoltre, l'accesso è consentito solamente ai ruoli ***admin*** e ***tenant-admin*** (quest'ultimo va creato se si vuole utilizzarlo)

***Per poter utilizzare il servizio è necessario aver definito un tenant attraverso l'API TenantManager***

## Avvio del servizio
Per impostazione predefinita, il servizio si attiva sulla porta 5000(http) e 5001(https).

Per modificare la porta di ascolto è sufficiente lanciare il servizio così:
`./OpenDMS.TenantManager.exe --urls http://localhost:80 https://localhost:441 `

``

# Compilazione e test
All'interno della cartella del progetto, lanciare:
`dotnet build` per compilare il servizio
`dotnet test` per eseguire le unit test
