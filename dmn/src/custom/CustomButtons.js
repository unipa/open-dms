import html2canvas from 'html2canvas';
import Swal from 'sweetalert2'

async function getFormPreviewInBase64() {
  const formElement = document.getElementById("canvas"); // ID del tuo form
  const canvas = await html2canvas(formElement);
  const prebase64 = canvas.toDataURL();
  const base64ImageCleaned = prebase64.replace(/^data:image\/(png|jpg);base64,/, '');
  return base64ImageCleaned; // Ritorna la stringa base64
}

export class SaveDiagramButton {
  constructor(dmnModeler, containerEl) {
    this._dmnModeler = dmnModeler;
    this._containerEl = containerEl;

    this._createButton();
    this._createLoader(); 
  }

  _createButton() {
    const button = document.createElement('button');
    button.id = "saveDiagram";
    button.textContent = "Salva Diagramma";
    button.className = 'panel';
    button.style.marginRight = '5px';
    button.style.zIndex = '10';
    button.style.cursor = 'pointer';
    button.style.backgroundColor = 'lightgreen';
    button.addEventListener('click', () => this._confirmSave());

    this._containerEl.appendChild(button);
  }

  _createLoader() {
    const loader = document.createElement('div');
    loader.id = "saveLoader";
    loader.style.display = 'none';  // Il loader è nascosto di default
    loader.style.position = 'fixed';
    loader.style.zIndex = '1000';
    loader.style.left = '0';
    loader.style.top = '0';
    loader.style.width = '100%';
    loader.style.height = '100%';
    loader.style.backgroundColor = 'rgba(0,0,0,0.5)';  // Un sfondo semi-trasparente nero

    const spinner = document.createElement('div');
    spinner.style.position = 'absolute';
    spinner.style.top = '50%';
    spinner.style.left = '50%';
    spinner.style.margin = '-30px 0 0 -30px';
    spinner.style.width = '60px';
    spinner.style.height = '60px';
    spinner.style.border = '16px solid #f3f3f3';
    spinner.style.borderRadius = '50%';
    spinner.style.borderTop = '16px solid lightgreen';  // verde
    spinner.style.animation = 'spin 2s linear infinite';  // Usa l'animazione "spin" definita sotto

    const text = document.createElement('p');
    text.id = "saveText";
    text.textContent = "Salvataggio in corso..";
    text.style.position = 'absolute';
    text.style.top = '62%';  // Posiziona il testo un po' sotto al spinner
    text.style.left = '51%';
    text.style.transform = 'translate(-50%, -50%)';
    text.style.color = '#f3f3f3';
    text.style.fontSize = '20px';
    text.style.display = 'none';  // Nascondi il testo di default

    loader.appendChild(text);
    loader.appendChild(spinner);
    document.body.appendChild(loader);
  }

  async _confirmSave() {
    const { isConfirmed } = await Swal.fire({
      title: 'Sei sicuro?',
      text: "Sei sicuro di voler salvare il diagramma?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: 'green',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sì, salva!'
    });

    if (isConfirmed) {
      this._saveDiagram();
    }
  }

  async _saveDiagram() {
    // Mostra il loader
    document.getElementById('saveLoader').style.display = 'block';
    document.getElementById('saveText').style.display = 'block';
  
    try {
      // Estrai il diagramma come XML
      const resultXML = await this._dmnModeler.saveXML({ format: true });
      let content = resultXML.xml;

      // Rimuovi tutti i ritorni a capo e sostituisci tutte le doppie virgolette con singoli apici
      content = content.replace(/\n/g, "\n").replace(/"/g, '\"');
        
      // Converte la stringa SVG in una stringa base64
      let previewString = await getFormPreviewInBase64();
      const previewInBase64 = previewString;
  
      const response = await postDiagram(content, previewInBase64, false);
      
      if (response == 200) {
        window.parent.postMessage({ op: "checkin"}, "*");
        Swal.fire({
          title: 'Salvato!',
          text: "Il tuo diagramma è stato salvato correttamente.",
          icon: 'success',
          confirmButtonColor: 'green',
        });
      } else {
        Swal.fire({
          title: 'Errore!',
          html: "Errore: risposta non valida dal server",
          icon: 'error',
          showCancelButton: false,
          confirmButtonColor: 'green',
        });  
        throw new Error('Errore: risposta non valida dal server');
      }
    } catch (error) {
      Swal.fire({
        title: 'Errore!',
        html: "Si è verificato un errore durante la chiamata: " + "<b>" + error + "</b>",
        icon: 'error',
        confirmButtonColor: 'green',
      }); 
    } finally {
      // Nascondi il loader e il testo indipendentemente dal risultato
      document.getElementById('saveLoader').style.display = 'none';
      document.getElementById('saveText').style.display = 'none';
    }
  }
}


export class PublishDiagramButton {
  constructor(dmnModeler, containerEl) {
    this._dmnModeler = dmnModeler;
    this._containerEl = containerEl;

    this._createButton();
    this._createLoader(); 
  }

  _createButton() {
    const button = document.createElement('button');
    button.id = "publishDiagram";
    button.textContent = "Pubblica diagramma";
    button.className = 'panel';
    button.style.marginRight = '5px';
    button.style.zIndex = '10';
    button.style.cursor = 'pointer';
    button.style.backgroundColor = 'lightblue';
    button.addEventListener('click', () => this._confirmSave());

    this._containerEl.appendChild(button);
  }

  _createLoader() {
    const loader = document.createElement('div');
    loader.id = "publishLoader";
    loader.style.display = 'none';  // Il loader è nascosto di default
    loader.style.position = 'fixed';
    loader.style.zIndex = '1000';
    loader.style.left = '0';
    loader.style.top = '0';
    loader.style.width = '100%';
    loader.style.height = '100%';
    loader.style.backgroundColor = 'rgba(0,0,0,0.5)';  // Un sfondo semi-trasparente nero

    const spinner = document.createElement('div');
    spinner.style.position = 'absolute';
    spinner.style.top = '50%';
    spinner.style.left = '50%';
    spinner.style.margin = '-30px 0 0 -30px';
    spinner.style.width = '60px';
    spinner.style.height = '60px';
    spinner.style.border = '16px solid #f3f3f3';
    spinner.style.borderRadius = '50%';
    spinner.style.borderTop = '16px solid #3498db';  // Blu
    spinner.style.animation = 'spin 2s linear infinite';  // Usa l'animazione "spin" definita sotto

    const text = document.createElement('p');
    text.id = "publishText";
    text.textContent = "Pubblicazione in corso..";
    text.style.position = 'absolute';
    text.style.top = '62%';  // Posiziona il testo un po' sotto al spinner
    text.style.left = '51%';
    text.style.transform = 'translate(-50%, -50%)';
    text.style.color = '#f3f3f3';
    text.style.fontSize = '20px';
    text.style.display = 'none';  // Nascondi il testo di default

    loader.appendChild(text);
    loader.appendChild(spinner);
    document.body.appendChild(loader);
  }

  async _confirmSave() {
    const { isConfirmed } = await Swal.fire({
      title: 'Sei sicuro?',
      text: "Sei sicuro di voler pubblicare il diagramma?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: 'blue',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sì, pubblica!'
    });

    if (isConfirmed) {
      this._saveDiagram();
    }
  }

  async _saveDiagram() {
    // Mostra il loader
    document.getElementById('publishLoader').style.display = 'block';
    document.getElementById('publishText').style.display = 'block';
  
    try {
      // Estrai il diagramma come XML
      const resultXML = await this._dmnModeler.saveXML({ format: true });
      let content = resultXML.xml;

      // Rimuovi tutti i ritorni a capo e sostituisci tutte le doppie virgolette con singoli apici
      content = content.replace(/\n/g, "\n").replace(/"/g, '\"');
  
      // Converte la stringa SVG in una stringa base64
      let previewString = await getFormPreviewInBase64();
      const previewInBase64 = previewString
  
      const response = await postDiagram(content, previewInBase64, true);
      
      if (response == 200) {
        window.parent.postMessage({ op: "checkin"}, "*");
        Swal.fire({
          title: 'Pubblicato!',
          text: "Il tuo diagramma è stato pubblicato correttamente.",
          icon: 'success',
          confirmButtonColor: 'blue',
        });
      } else {
        throw new Error('Errore: risposta non valida dal server');
      }
    } catch (error) {
      Swal.fire({
        title: 'Errore!',
        html: "Si è verificato un errore durante la chiamata: " + "<b>" + error + "</b>",
        icon: 'error',
        confirmButtonColor: 'blue',
      });
    } finally {
      // Nascondi il loader e il testo indipendentemente dal risultato
      document.getElementById('publishLoader').style.display = 'none';
      document.getElementById('publishText').style.display = 'none';
    }
  }
}


//funzione per chiamata api che salva il form
async function postDiagram(content, previewInBase64, toBePublished) {
  // Ottieni la stringa query dall'URL
  const queryString = window.location.search;

  // Crea un'istanza di URLSearchParams
  const urlParams = new URLSearchParams(queryString);

  // Ottieni un iteratore per le coppie chiave/valore
  const entries = urlParams.entries();

  // Ottieni la prima coppia chiave/valore
  const firstEntry = entries.next().value;

  if (firstEntry) {
    var id = firstEntry[1];  // Il valore del parametro è il secondo elemento della coppia chiave/valore

    // Dati da inviare con la chiamata POST
    const data = {
      content: content,
      previewInBase64: previewInBase64,
      previewExtension: "png",
      toBePublished: toBePublished
    }

    //console.log(JSON.stringify(data));

    let mypathbase = window.location.pathname;

    if (!mypathbase.endsWith('/')) {
      mypathbase += '/';
    }

    // Effettua la chiamata POST all'API
    const response = await fetch(mypathbase+`api/postDmn/${id}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data) // Converte i dati in una stringa JSON
    });

    // Gestisci la risposta
    if (!response.ok) {
      throw new Error("Errore HTTP: " + response.status + " " + response.statusText);
    }

    const responseData = await response.json(); // Estrae i dati JSON dalla risposta

    // Ritorna i dati ricevuti
    //console.log(responseData);

    return responseData;

  } else {
    document.getElementById('saveLoader').style.display = 'none';
    document.getElementById('publishLoader').style.display = 'none';
    Swal.fire({
      title: 'Errore!',
      html: "Nessun parametro di query fornito. Il salvataggio non è stato possibile",
      icon: 'error',
      showCancelButton: false,
      confirmButtonColor: 'green',
    });
  }
}

  