# Introduzione 
OpenDMS.Tasklist.API fornisce le API per la gestione dei task.
I task sono messaggi scambiati tra gli utenti con la finalità di gestire una attività.

Le descrizioni delle API sono disponibili online, tramite [swagger](http://localhost/swagger/index.html) oppure sul file ***OpenDMS.Tasklist.API***


# Guida introduttiva
Il compito richiesto da un task è composto da un titolo e da una eventuale descrizione estesa.
Ogni utente può assegnare un task ad uno o più utenti, ruoli o strutture di organigramma per competenza e per conoscenza.

## Task per competenza
I task assegnati per competenza devono essere presi in carico e gestiti attraverso gli stati d'avanzamento e il completamento dell'attività. Se assegnati a ruoli o gruppi, devono essere presti in carico da un utente che si farà carico di gestire l'attività richiesta. 
Ogni attività può evere una scadenza ed anche uno o più documenti/fascicoli a cui fa riferimento.
Nella gestione di un task, ciascun destinatario può scambiare messaggi con gli altri coinvolti nello stesso task e documentare lo stato d'avanzamento attraverso delle note e una percentuale di completamento della propria parte di attività. Le percentuali di completamento indicate dai vari destinatari di un task vanno a comporre la percentuale complessiva di un task. Solo quando questa raggiunge il 100%, il task viene concluso.

Ciascun destinatario di un task può anche rifiutarsi di gestirlo, desrivendone le motivazioni. In questo caso, la parte di attività dell'utente sarà riassegnata al richiedente che potrà a sua votla riassegnarla ad altri.
In alternativa, un destinatario che ritiene di non poter gestire un task, può riassegnarlo ad altri colleghi, motivandone la scelta.

## Task per conoscenza
I task inviati per conoscenza rappresentano informazioni che i destinatari devono solo visualizzare. Se inviati a ruoli o gruppi, vengono assegnati singolarmente a tutti gli utenti sottostanti.

## Messaggi
I messaggi scambiati all'interno di un task vengono gestiti come task in conoscenza a tutti i destinatari del task principale. A differenza dei task in competenza, i messaggi non devono essere presi in carico, non hanno scadenza, non possono essere rifiutati o riassegnati.

### Generazione Task
Un task può essere generato da tre eventi:
1. Un utente richiede un'attività ad uno o più colleghi
2. Un processo richiede un'attività ad un o più utenti
3. Un errore interno, non previsto, informa uno o più utenti del problema riscontrato.


## Installazione
Tutti i parametri di configurazione sono presenti nel file ***appsettings.json*** e possono essere sotituiti da corrispondenti variabili d'ambiente.

Per impostazione predefinita, il servizio si attiva sulla porta 7000(http) e 7001(https).
Per modificare la porta di ascolto è sufficiente lanciare il servizio così:
`./OpenDMS.Tasklist.API.exe --urls http://localhost:80 https://localhost:441 `

Al lancio della applicazione, nella console vengono mostrate le voci di configurazioni mancanti.


## Avvio del servizio
Il servizio può essere attivato come container docker o lanciato da console. 
Il database di riferimento viene creato al primo avvio se non presente.

Nella console vengono mostrati i log delle richieste ed eventuali errori applicativi.

``

# Compilazione e test
All'interno della cartella del progetto, lanciare:
`dotnet build` per compilare il servizio
`dotnet test` per eseguire le unit test

