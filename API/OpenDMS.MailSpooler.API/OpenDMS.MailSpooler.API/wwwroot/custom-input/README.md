# Custom Input

Come da richiesta, custom-input con 4 tipi principali: **text, textarea, lookup e date**.

**Il tag**: `<custom-input></custom-input>`

## Installazione

Per installare il custom input esistono 2 approcci: o manualmente o tramite npm:

- Tramite NPM: Prendere la cartella custom-input e metterla nella root del progetto, eseguire quindi il comando `npm install ./custom-input` per installare il pacchetto, nella cartella node_modules verranno inseriti solo i files necessari. Cancellare quindi la cartella nella root del progetto.
- Manualmente: Copiare soltanto le cartelle dist, che contiene il file principale e lib che contiene tutte le sottoclassi.

Per utilizzarlo tramite node module bisogna chiamarlo con `import "custom-input"` altrimenti chiamare il file custom-script.js in pagina tramite tag.
Per React è consigliato installarlo come modulo node.

## Attributi presenti

- **width**: si può dare una larghezza customizzata, di base è 100%.
- **name**: per il form
- **value**: il valore iniziale dell'input
- **id** //
- **instanceId**: l'id dell'istanza, utilizzato per il lookup
- **placeholder** //
- **type**: l'attributo che permette di scegliere quale input o textarea si deve creare.
- **visible**: true o false, mette a display o meno il campo
- **label**: il testo che si vuole visualizzare sopra all'input
- **required**: true o false, se true il campo è obbligatorio
- **disabled**: true o false, se true il campo è disabilitato
- **multiValue**: true o false, se true il campo è multiselect
- **label**: il testo che si vuole visualizzare sopra l'input

## Eventi

In questo momento è presente un evento definito come **getValue** che permette di estrarre i dati dai campi di input. Per quanto riguarda la searchbar, getValue restituisce l'oggetto della ricerca, che nell'esempio in cartella presenta un id, un nome ed un codice.

## Valori di ritorno delle componenti nel form

Il **lookup** ritorna un oggetto che corrisponde a quello selezionato tramite click successivamente alla ricerca, altrimenti torna vuoto.

Il **textarea** , il **text** ed il **date**, **number**, \* si comportano as usual.

## Comandi per avviare il json server di prova

    npm install

    npm run test

Usare l'estensione di VS [Live Server](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) per test.

## Tasks

- [x] Creare un componente per la textarea

- [x] Creare un componente per l'input text

- [x] Creare un componente per la searchbar

- [x] Dividere in classi le varie componenti

- [x] Creare richiesta api nel componente searchbar

- [x] Creazione attributi visible, width, name, value, id, placeholder

- [x] Renderizzazione condizionale dei componenti in base al tipo

- [x] Esportare l'oggetto con i dati del campo di ricerca tramite output

- [x] Inserire Debounce alla digitazione per la ricerca

- [x] Permettere alle componenti di essere gestite dal form tag

- [x] Aggiungere a custom-input max-length di caratteri

- [x] Aggiungere lookup gerarchica con ul li ul li

- [x] Aggiungere componente date che restituisca dato in formato americano senza spazio

- [x] Aggiungere obbligatorio o non obbligatorio a campo input e textarea

- [x] Aggiungere multiselect a lookup che si attiva solo se multivalue è true
- [x] Aggiungere possibilità di disabilitare il campo

- [x] Aggiungere selezione campi multipli

- [x] Fare chiamata api su attributechangedcallback se value esterna cambia riportarsi nome/nomi dentro array/value del multiselect/select
- [x] Nominare oggetti array in modo tale che se in futuro cambia, si cambia solo quel valore
- [x] Aggiungere multiselect/singleSelect in base ad attributo multivalue true/false alla modifica del documento
- [x] Aggiungere watching attributo value in custom-input che aggiorna la lista di valori refetchando i dati in base all'attributo value

- [x] Creare oggetto che traduce i nomi degli attributi in modo tale da poterli usare in modo più semplice

- [x] Modificare API call, inserire funzione di creazione oggetto da passare come parametro per interpretare i dati in ingresso
- [x] Risolvere eventualità in cui il parametro inserito all'interno del lookup ritorna undefined, vedi note
- [x] Aggiungere valore grezzo in rosso in multiselect e valore grezzo in select se richiesta non produce alcun risultato.

## Tasks future (ipotizzate ma non in programma)

- [ ] Cambio input in base ad attributo type in tempo reale tramite manipolazione DOM

## Bugs da sistemare

- [x] La searchbar ritorna undefined fuori dal componente

- [x] bug che non trasmette dato da custom-input a form tag
- [x] bug multiselect che non aggiunge il valore corretto o lo resetta
- [x] bug textarea che non aggiunge valore da attributo value
- [x] Sistemato switch multiselect/single select in lookup. Al cambio di stato true/false di multivalue, ora cambia tipo di lookup
- [x] bug che non chiude i search results quando si cliccava fuori dal campo di ricerca

- [x] bug che non aggiorna il valore del campo di ricerca quando si clicca su un elemento della lista

## Note

- Lo stile è stato inserito nel file mixinstyle.js tramite classe, custom-input.css è un file per il trasferimento del codice css in classe.

- Configurare l'api call in lib/api/fetchlookup.js
- Nel file fetchlookupdata.js in newObj vengono definiti i nomi degli delle chiavi che verranno restituite dall'oggetto, in questo modo è possibile cambiare i nomi delle chiavi senza dover modificare il codice. action_nome ed action_codice corrispondono ai parametri della chiamata GET, nel try si può rimodulare la chiamata se è di POST o se ha bisogno di altri parametri.

### Riguardo campi multipli:
