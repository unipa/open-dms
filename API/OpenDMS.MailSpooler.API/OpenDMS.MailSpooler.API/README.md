# Introduzione 
MailSpooler.API espone le API per l'invio e la gestione dei messaggi di posta elettronica ordinaria e certificata in uscita.

# Guida introduttiva
Il servizio può essere attivato come container docker o lanciato da console. Il sistema si collega al database del registro dei tenant per determinare, attraverso il nome host, il proprio database di gestione. 
La configurazione del database del registro dei tenant è indicata nel file ***appsettings.json***

Le descrizioni delle API sono disponibili online, tramite [swagger](http://localhost/swagger/index.html) oppure sul file ***OpenDMS.MailSpooler.API***

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
L'accesso è consentito solamente agli utenti autenticati.
***Per poter utilizzare il servizio è necessario aver definito un tenant attraverso l'API TenantManager***

## Avvio del servizio
Per impostazione predefinita, il servizio si attiva sulla porta 5000(http) e 5001(https).

Per modificare la porta di ascolto è sufficiente lanciare il servizio così:
`./OpenDMS.MailSpooler.exe --urls http://localhost:80 https://localhost:441 `

``

# Compilazione e test
All'interno della cartella del progetto, lanciare:
`dotnet build` per compilare il servizio
`dotnet test` per eseguire le unit test
