# Introduzione 
TenantManager gestisce il registro dei tenant applicativi che contiene le informazioni di collegamento al database di ciascun tenant, al realm di autenticazione e l'url di identificazione del tenant stesso.  

# Guida introduttiva
Il servizio può essere attivato come container docker o lanciato da console. La configurazione del database del registro è indicata nel file ***appsettings.json***
Al primo avvio il sistena crea il database del registro.

Le descrizioni delle API sono disponibili online, tramite [swagger](http://localhost/swagger/index.html) oppure sul file ***OpenDMS.TenantManager.API***
Oltre alle API è disponibile una interfaccia web che permette di gestire i tenant in modo grafico accessibile dalla root (/)

Per la creazione di un tenant è necessario specificare:
1. L'identificativo dell'host di riferimento. Ogni tenant viene identificato da un host differente (es. tenant1.unipa.it, tenant2.unipa.it)
2. Un nome descrittivo del tenant
3. Nome del database o stringa di collegamento ad database da dedicare al tenant. 
4. Strategia di collegamento al database. E' possibile scegliere di creare un nuovo database (restituendo un errore se dovesse esistere), collegarsi ad uno pre-esistente (restituendo un errore se non dovesse esistere) o tentare il collegamento/creazione al database indicato 
5. Nome del realm di autenticazione
6. Client Id del realm da associare al tenant
7. Client Secret del client indicato
8. URL del tenant documentale

## Installazione
E' necessario creare un client nel realm "Master" di Keycloak per abilitare l'accesso alle API e alle interfacce utente del servizio
Le informazioni del client devono essere inserite nel file ***appsettings.json*** oppure possono essere utilizzate come parametri in riga di comando o di ambiente in un file YAML

es.
`  "Keycloak": {
    "realm": "master",
    "auth-server-url": "http://172.25.0.145:8080/",
    "ssl-required": "external",
    "resource": "tenant-admin",
    "verify-token-audience": true,
    "credentials": {
      "secret": "J93HUyuig83PO5jSFZbVK31zybDH2KvM"
    },
    "confidential-port": 0,
    "policy-enforcer": {}
  }
`
Inoltre, l'accesso è consentito solamente ai ruoli ***admin*** e ***tenant-admin*** (quest'ultimo va creato se si vuole utilizzarlo)

## Avvio del servizio
Per impostazione predefinita, il servizio si attiva sulla porta 5000(http) e 5001(https).

Per modificare la porta di ascolto è sufficiente lanciare il servizio così:
`./OpenDMS.TenantManager.exe --urls http://localhost:80 https://localhost:441 `

``

# Compilazione e test
All'interno della cartella del progetto, lanciare:
`dotnet build` per compilare il servizio
`dotnet test` per eseguire le unit test
