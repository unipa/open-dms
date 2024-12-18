# Introduzione
OpenDMS.Workflow.API espone le API per l'avvio e la gestione dei processi documentali e implementa la logica di gestione dei task custom che vengono inseriti nei processi del DMS e del BPM.

Il worker gestisce le seguenti tipologie di task:
1. Task Utente
2. Task Documentali
3. Task di Accesso ai Dati
4. Integrazione Titulus 
5. Integrazione Maximo

  
# Task Utente
I task utente sono attiit� che possono essere richieste ad uno o pi� utenti (su nessuno o pi� documenti allegati). L'attivit� richiesta pu� essere descrittiva, può prevedere la compilazione di form o il verificarsi di un evento sugli allegati (es. firma o presa visione dei documenti).
Ogni task prevede dei parametri di input e pu� restituire dei parametri in output.

## Assegnazione Compito (io.camunda.zeebe:userTask)
Genera un task di tipo "Attività" nella tasklist degli utenti destinatari. Il task riporta il titolo, la descrizione ed un eventuale form. Il processo si ferma in attesa del completamento del task. E' possibile inserire un timer o altro evento per "sbloccare" l'attività. 
Se i destinatari in assegnazione sono pi� di uno, il task principale si completa quando tutti gli utenti avranno completato il loro "userTask".
In questi casi, le variabili del form vengono memorizzate sui singoli "usertask" e il task in uscita riporta le variabili dell'ultimo utente che ha completato il suo task.
Per catturare le variabili dei singoli utenti � possibile associare un messaggio in boundary in risposta all'evento "UserTask.Executed" che ricever� in input i dati dello usertask.
Quando si usano messaggi boundary per completare anticipatamente uno usertask, se si desidera annullare tutte le istanze attive del task dalla tasklist, si deve utilizzare il task "Completamento Task" indicando in input il jobkey del task o in alternativa, i destinatari,l'evento e gli allegati dei task da annullare.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Title | Testo | titolo dell'attivit� | Si |
| Description | Testo | descrizione estesa | No |
| ExpirationDate | Data e Ora | Data e ora di scadenza dell'attivit� | No |
| CategoriyId | Testo | Codice della Categoria | No |
| CompanyId | Numero Intero | Id del sistema informativo | No |
| ParentTaskId | Numero Intero | Id del TaskItem collegato | No |
| FormId | Numero Intero | Id del documento form colelgato | No |
| FormSchema | Testo | Form in formato json | No |
| NotifyTo | Testo | Elenco separato da vigole di profili (Tipo Profilo + Codice Profile) destinatari | No |
| NotifyCC | Testo | Elenco separato da vigole di profili (Tipo Profilo + Codice Profile) destinatari in CC | No |
| NotifiyExecution | Bit | 1 = Notifica l'esecuzione del task al richiedente | Si |
| NotifyExpiration | Bit | 1 = Notifica la scadenza del task al richiedente | Si |
| Proprity | Testo | Codice della priorit� | No |
| Sender | Testo | profilo (Tipo Profilo + Codice Profile) del richiedente (es. 0admin) | Si |
| Attachments | Testo | Elenco di ID documenti da collegare all'attivit� | No |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| TaskId | Numero Intero | ID del TaskItem generato |


## Invio Notifica (UserMessageWorker)
Genera un task di tipo "Messaggio" nella tasklist degli utenti destinatari. Il task riporta il titolo e la descrizione della attivit� e pu� essere completato con una nota (se richiesta). Il processo prosegue immediatamente senza attendere il completamento del task  

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Title | Testo | titolo dell'attivit� | Si |
| Description | Testo | descrizione estesa | No |
| ExpirationDate | Data e Ora | Data e ora di scadenza dell'attivit� | No |
| CategoriyId | Testo | Codice della Categoria | No |
| CompanyId | Numero Intero | Id del sistema informativo | No |
| ParentTaskId | Numero Intero | Id del TaskItem collegato | No |
| NotifyTo | Testo | Elenco separato da vigole di profili (Tipo Profilo + Codice Profile) destinatari | No |
| NotifyCC | Testo | Elenco separato da vigole di profili (Tipo Profilo + Codice Profile) destinatari in CC | No |
| Sender | Testo |profilo (Tipo Profilo + Codice Profile) del richiedente (es. 0admin) | Si |
| Attachments | Testo | Elenco di ID documenti da collegare all'attivit� | No |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| TaskId | Numero Intero | ID del TaskItem generato |  


## Richiesta Evento (RequestForEventTaskWorker)
Genera un task di tipo "Evento su documento" nella tasklist degli utenti destinatari. Il task riporta il titolo, la descrizione ed un eventuale form. Il form si rende utile quando si vuole offrire all'utente un'uscita alternativa (es. indicando che l'evento � gi� accaduto in passato e non pu� essere ripetuto). Se non si indica alcun form, il sistema propone un campo note facoltativo.
Questa tipologia di task pu� essere associata ad un solo documento.

Gli eventi supportati sono:
1. Visto su documenti (aprire scheda documento)
2. Firma Autografa (aprire scheda documento)
3. Firma Grafometrica ***(aprire client di firma)***
4. Firma Remota ***(aprire endpoint di firma)***
5. Firma Digitale ***(aprire client di firma)***
6. Visione documenti (aprire scheda documento)
7. Creazione nuova revisione (aprire scheda documento)
8. Creazione nuova versione (valido anche per un nuovo contenuto) (aprire scheda documento)
9. Fascicolazione (aprire scheda documento)
10. Protocollo documenti (aprire scheda documento)

Quando il task viene completato dalla tasklist il flusso procede in modo tradizionale e in uscita viene impostata la variabile "EventRaised=false".
Per ogni evento generato dal documentale, il sistema verifica se esistono "usertask" in attesa, collegati all'evento, e li completa impostando la variabile "EventRaised=true". Se il task ha diversi destinatari, viene generato un evento "UserTask.Execute" per ogni usertask completato.
Il task principale viene completato automaticamente quando tutti gli "usertask" sono completati.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Title | Testo | titolo dell'attivit� | Si |
| Description | Testo | descrizione estesa | No |
| ExpirationDate | Data e Ora | Data e ora di scadenza dell'attivit� | No |
| CategoriyId | Testo | Codice della Categoria | No |
| CompanyId | Numero Intero | Id del sistema informativo | No |
| ParentTaskId | Numero Intero | Id del TaskItem collegato | No |
| FormId | Numero Intero | Id del documento form colelgato | No |
| FormSchema | Testo | Form in formato json | No |
| NotifyTo | Testo | Elenco separato da vigole di profili (Tipo Profilo + Codice Profile) destinatari | No |
| NotifiyExecution | Bit | 1 = Notifica l'esecuzione del task al richiedente | Si |
| NotifyExpiration | Bit | 1 = Notifica la scadenza del task al richiedente | Si |
| Proprity | Testo | Codice della priorit� | No |
| Sender | Testo | profilo (Tipo Profilo + Codice Profile) del richiedente (es. 0admin) | Si |
| EventId | Testo | Codice Evento da gestire | Si |
| Attachments | Testo | Elenco di ID documenti da collegare all'attivit� | Si |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| TaskId | Numero Intero | ID del TaskItem generato |


## Completamento Task (CompleteTaskWorker)
Questo task deve essere utilizzato quando si vuole completare un'attivit� utente ancora in corso. Ad esempio, quando si richiede una approvazione a molteplici utenti in cui si vuole che il flusso proceda quando almeno uno dei destinatari effettua una scelta. In questi casi, � necessario informare gli altri partecipanti che la richiesta � stata soddisfatta e che non � pi� attiva. Questo task riceve in input l'ID del TaskItem da completare e un messaggio da far recapitar eai destinarati ancora attivi del Task.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| TaskId | Numero Intero | ID del TaskItem da completare | Si |
| JobKey | Numero Intero | ID del job da completare (In alternativa al TaskId) | No |
| FeedbackMessage | Testo | Messaggio da inviare ai destinatari del task da completare | No |


  
# Task Documentali
I seguenti task operano sui metadati e/o sui contenuti dei documenti

## Task di Creazione/Modifica Documento
Genera o modifica un documento attraverso il richiamo dell'api corrispondente. E' necessario passare un oggetto "CreateOrUpdateDocument" per creare un nuovo documento, oppure, indicando l'ID di un documento (DocumentId) o una query di ricerca (Filters) � possibile modificare uno o (maxResults) documenti.
Il task ritorna l'elenco dei documenti creati o modificati

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Document | CreateOrUpdateDocument | oggetto "CreateOrUpdateDocument" (vedere api Document) | Si |
| DocumentId | Numero | Id del documento da modificare | No |
| Filters | Array di SearchFilter | Filtri di ricerca di documenti | No |
| MaxResults | Numero Intero | Numero massimo di documenti estraibili dai filtri | No |

### Variabili di output
| Variabile | Tipo | Significato
|---|---|---|
| documents | Array di DocumentInfo | Elenco di oggetti "DocumentInfo" (vedere api Document) |


## Task di estrazione scheda documento
Estrae le informazioni di classificazione di un documento e del suo contenuto.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| DocumentId | Numero | Id del documento da leggere | Si |

### Variabili di output
| Variabile | Tipo | Significato
|---|---|---|
| Document | DocumentInfo | oggetto "DocumentInfo" (vedere api Document) 


## Task di inserimento documento in fascicolo
Inserisce un documento in un fascicolo esistente

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| DocumentId | Numero | Id del documento da fascicolare | No |
| FolderId | Numero | Id del fascicolo in cui inserire il documento | No |
| MoveToFolder | bit | Indica se spostare il documento sul fascicolo indicato | No |
| MaxResults | Numero Intero | Numero massimo di documenti estraibili dai filtri | No |

### Variabili di output
| Variabile | Tipo | Significato
|---|---|---|
| Errors | Array di BatchErrorResult | Elenco di oggetti "BatchErrorResult" (vedere api Document) |


## Task di ricerca documenti o fascicoli 
Ritorna i dati di uno o più documenti o fascicoli

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Filters | Array di SearchFilter | Filtri da applicare per la ricerca | No |
| MaxResults | Numero Intero | Numero massimo di documenti estraibili dai filtri | No |

### Variabili di output
| Variabile | Tipo | Significato
|---|---|---|
| DocumentIds | Array di Numeri | Elenco di Identificativi di documenti  |


## Task di gestione permessi su documenti o fascicoli
Permette di impostare o rimuovere i permessi ad uno o pi� utenti su uno o pi� documenti o fascicoli

DocumentId,Profiles,Permissions_Granted,Permissions_Removed,Permissions_Denied"
Granted,Removed,Denied (numero)


## Task di inserimento nuovo contenuto
Inserisce un nuovo contenuto su un documento creando una nuova versione o revisione
Il task ritorna l'ID del contenuto creato o genera un'eccezione

DocumentId,FileName,Data,IsBase64
ImageId


## Task di inserimento allegato/collegato
Inserisce un nuovo contenuto su un documento creando una nuova versione o revisione
Il task ritorna l'ID del contenuto creato o genera un'eccezione

DocumentId,AttachmentId,IsLink


## Task di inserimento da template 
Inserisce un nuovo contenuto a partire da un template.
Se il template � di tipo PDF, Word o Excel, il sistema tenta di inserire le variabili di processo ad ogni occorrenza simile a "{<nome  variabile>}" e i metadati del documento ad ogni occorrenza simile a "{doc:<propriet�>}".
Il task ritorna l'ID del contenuto creato o genera un'eccezione

DocumentId,TemplateId
ImageId

  
## Task di modifica dello stato di un documento (aka cancellazione)
Permette di modificare lo stato di un documento (attivo, bozza, archiviato, cancellato)

DocumentId,Status
NewStatus




# Task Accesso Dati
I task di accesso dati permette di leggere o scrivere le informazioni di processo e i metadati dei documenti da/su fonti dati esterne al sistema, utilizzando le connessioni a database definibili nell'area di amministrazione del documentale.

## Task di esecuzione SQL
Esegue una istruzione SQL di inserimento/aggiornamento/cancellazione su una fonte dati esterna.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| DatabaseConnectionId | Numero | Identiicativo della connessione a database da utilizzare | Si |
| SQL | Stringa | Istruzione SQL da eseguire. | Si |

### Variabili di output
| Variabile | Tipo | Significato 
|---|---|---|
| AffectedRows | Numero | Numero di righe modificate dall'esecuzione della SQL |


## Task di lettura dati
Esegue una istruzione di lettura dati da una fonte dati esterna.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| DatabaseConnectionId | Numero | Identiicativo della connessione a database da utilizzare | Si |
| SQL | Stringa | Istruzione select da eseguire | Si |

### Variabili di output
| Variabile | Tipo | Significato 
|---|---|---|
| Rows | Json | Array di righe estratte dall'istruzione SQL.



# Task di Integrazione con Titulus
I seguenti task permettono l'integrazione con il software Titulus.

## Crea un documento su Titulus (CreateDocumentInTitulus )
Questo task crea un documento all'interno di Titulus a partire dalla variabili di input passate allo stesso.

### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Debug | Boleano | Specifica se l'esecuzione è per scopi di debug o meno | Si |
| Bozza | Boleano | Specifica se il documento sarà una bozza o meno | Si |
| Oggetto | Testo | Specifica l'oggetto del documento | Si |
| Tipo | Testo | Specifica il tipo di documento | Si |
| Data_prot | Testo | Specifica la data di protocollazione del documento | Si |
| Data_arrivo | Testo | Specifica la data di arrivo del documento | Si |
| Commento_prot_differito | Testo | Commento se il documento presenta protocollo differito | No |
| Tipologia_cod | Testo | Specifica il codice della tipologia del documento | Si |
| Mezzo_trasmissione_costo | Testo  | Specifica il costo del mezzo di trasmissione | Si |
| Mezzo_trasmissione_valuta | Testo | Specifica la valuta del mezzo di trasmissione | Si |
| Mezzo_trasmissione_cod | Testo | Specifica il codice del mezzo di trasmissione | Si |
| Note | Testo | Note da inserire nel documento | Si |
| Riferimenti_innertext | Testo | Specifica il testo da inserire nel campo riferimenti | Si |
| Voce_indice_innertext | Testo | Specifica il testo da inserire nel campo voce indice | Si |
| Rif_esterno_dataprot | Testo | Specifica la data protocollazione da inserire nell'attributo data_prot del campo riferimento esterno  | Si |
| Rif_esterno_nome_cod| Testo | Specifica il nome del codice del riferimento esterno | Si |
| Rif_esterno_nome_innertext | Testo | Specifica il nome del riferimento esterno | Si |
| rif_esterno_referente_cod | Testo | Specifica il codice referente del riferimento esterno  | Si |
| Rif_esterno_referente_nominativo | Testo | Specifica il nominativo del riferimento esterno | Si |
| Rif_esterno_indirizzo_email | Testo | Specifica la mail del riferimento esterno | Si |
| Rif_esterno_indirizzo_email_certificata | Testo | Specifica la pec del riferimento esterno | Si |
| Rif_esterno_fax | Testo | Specifica il fax del riferimento esterno | Si |
| Rif_esterno_tel | Testo |  Specifica il numero di telefono del riferimento esterno | Si |
| Rif_esterno_indirizzo_innertext | Testo | Specifica l'indirizzo del riferimento esterno | Si |
| Rif_interno_diritto_rpa  | Testo | Inserisce RPA nell'attributo diritto del riferimento interno | Si |
| Rif_interno_codice_persona | Testo | Specifica il codice persona del riferimento interno | Si |
| Rif_interno_nome_persona | Testo |  Specifica il nome del riferimento interno | Si |
| Rif_interno_nome_ufficio | Testo | Specifica il nome dell'ufficio del riferimento interno  | Si |
| Attachment | Array di Attachment| Specifica la lsita degli allegati da inserire nel documento che si sta creando | Si |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| NewDocument | Testo | Risultato xml del nuovo documento creato su Titulus | 


## Recupera un documento da Titulus (GetDocumentFromTitulus)
Questo task recupera il documento da Titulus che ha il numero di protocollo corrispondente al valore che viene specificato nella variabile d'input.
  
### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| numero_protocollo | Testo | Numero identificativo del protocollo | Si |
| debug | Boleano | Specifica se l'esecuzione è per scopi di debug o meno | No |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| documento | Testo | Numero identificativo del protocollo | Si |
| debug | Boleano | Specifica se l'esecuzione è per scopi di debug o meno | No |


## Recupera un documento da Titulus (GetDocumentFromTitulus)
Questo task recupera il documento da Titulus che ha il numero di protocollo corrispondente al valore che viene specificato nella variabile d'input.
  
### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Numero_protocollo | Testo | Numero identificativo del protocollo | Si |
| Debug | Boleano | Specifica se l'esecuzione è per scopi di debug o meno | No |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| Documento | Testo | Numero identificativo del protocollo | 
| Debug | Boleano | Specifica se l'esecuzione è per scopi di debug o meno |


## Crea un documento su DMS (CreateDocumentInDMS )

Questo task crea un documento all'interno di DMS a partire dal un documento presente su Titulus.
  
### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Documento | Testo | JSON del documento da creare su DMS passato come testo | Si |
| Debug | Boleano | Specifica se l'esecuzione è per scopi di debug o meno | Si |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| Dms_id | Testo | Id del documento appena creato su DMS | 
| Titulus_document_files | Testo | JSON dei file presenti all'interno del documento recuperato da titulus | 
| Debug | Booleano | Specifica se l'esecuzione è per scopi di debug o meno | 


## Crea un documento su DMS (AddFilesToDocumentInDMS )
Questo task aggiunge i file recuperati da un documento di Titulus all'interno del documento DMS del quale si è passato l'id in input al task. 
  
### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Dms_id | Testo | Id del documento appena creato su DMS | Si |
| Titulus_document_files | Testo | JSON dei file presenti all'interno del documento recuperato da titulus | Si |
| Debug | Booleano | Specifica se l'esecuzione è per scopi di debug o meno | Si |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| Dms_id | Testo | Id del documento su DMS | 




# Task di Maximo
I seguenti task permettono l'integrazione con il software Maximo.

## Crea un documento su DMS (AddFilesToDocumentInDMS )
Questo task aggiunge i file recuperati da un documento di Titulus all'interno del documento DMS del quale si è passato l'id in input al task. 
  
### Variabili di input
| Variabile | Tipo | Significato | Obbligatorio |
|---|---|---|---|
| Assetnum | Testo | Identificativo dell'asset su maximo per il quale si apre una segnalazione | Si |
| Description | Testo | Descrizione del problema riscontrato per l'asset in questione | Si |
| Reportedpriority | Testo | prioritò della segnalazione (da 1 a 5) | Si |
| Studente | Testo | Username dello studente che apre la segnalazione | Si |
| Debug | Booleano | Specifica se l'esecuzione è per scopi di debug o meno | Si |

### Variabili di output
| Variabile | Tipo | Significato |
|---|---|---|
| maximoResponse | Testo | Risposta fornita dall'applicativo Maximo | 




