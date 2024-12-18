# Rilascio Sprint #2

## OpenDMS.Domain
Definisce il dominio applicativo, le entità di database, i modelli e le interfacce dei servizi esterni (es. invio posta, notifiche, mail, bpm, ecc)

## OpenDMS.MultiTenancy
Permette una gestione dinamica e multitenancy dei database e delle impostazioni del sistema. Le strategie di identificazione del tenant viene condizionata da una variabile d'ambiente che acetta i seguenti valori:
- [x] none = il sistema utilizza la stringa di connessione impostata nelle variabili di ambiente per accedere al database del sistema
- [x] host = il tenant viene identificato dal nome host e il database corrisponednete viene recuperato da un registro di tenant 
- [x] claim = il tenant viene identificato da un claim passato tra le informazioni di un utente loggato 
- [x] path = il tenant viene identificato dal valore del primo livello del path di una request

## OpenDMS.Infrastructure.Database
Implementa la persistenza dei modelli di sistema sui seguenti database relazionali:
- [x] SQL Server
- [x] MySql
- [x] In Memory (utilizzato per le unit test)

## OpenDMS.Infrastructure.Service
Implementa i seguenti servizi esterni:
- [x] Tracciamento degli eventi di sistema 
- [x] Gestione dell'invio delle Notifiche esterne 
- [x] Gestione dell'invio di Email
- [x] Gestione delle comunicazioni con il servizio di indicizzazione del contenuto dei documenti
- [x] Gestione delle comunicazioni con il servizio di generazione della preview dei documenti

## OpenDMS.AuthProviderManager
Gestiscse l'autenticazione e l'autorizzazione applicativa con un sistema IAM esterno tramite protocollo OpenID e OAUTH 2.0

## OpenDMS.Infrastructure.VirtualFileSystem
Gestisce l'astrazione dell'accesso al filesystem consentendo la memorizzazione dei documenti anche su altre strutture (es. database)

## OpenDMS.PdfManager
Gestisce la trasformazione di specifiche tipologie di file in pdf e la gestione di pagine (png) per la visualizzazione dei documenti sul web

## OpenDMS.Core
Contiene la logica applicativa del sistema DMS

## OpenDMS.Startup
Implementa le classi di startup comuni ai progetti API e Web

## Library
Librerie generiche per l'accesso a database, tipologie di file e MessageBus


