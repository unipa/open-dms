import xmlFormat from 'xml-formatter';
import Swal from 'sweetalert2'

export class UploadXmlButton {
    constructor(bpmnModeler, bottoni) {
        const uploadXmlButtonEl = document.createElement('input');
        uploadXmlButtonEl.type = 'file';
        uploadXmlButtonEl.accept = '.xml, .bpmn';
        uploadXmlButtonEl.style.display = 'none';
        uploadXmlButtonEl.addEventListener('change', () => {
        const fileReader = new FileReader();
        fileReader.readAsText(uploadXmlButtonEl.files[0], 'UTF-8');
        fileReader.onload = (event) => {
            const xml = event.target.result;
            bpmnModeler.importXML(xml).catch((err) => {
                console.error(err);
            });
        };
        });
        const uploadXmlLabelEl = document.createElement('label');
        uploadXmlLabelEl.textContent = 'IMPORT';
        uploadXmlLabelEl.className = 'panel';
        uploadXmlLabelEl.style.marginLeft = '5px';
        uploadXmlLabelEl.style.zIndex = '10';
        uploadXmlLabelEl.style.cursor = 'pointer';
        uploadXmlLabelEl.addEventListener('click', () => {
            uploadXmlButtonEl.click();
        });
        bottoni.appendChild(uploadXmlButtonEl);
        bottoni.appendChild(uploadXmlLabelEl);
    }
}

export class DownloadXmlButton {
    constructor(bpmnModeler, bottoni) {
        const downloadXmlButtonEl = document.createElement('button');
        downloadXmlButtonEl.textContent = 'EXPORT';
        downloadXmlButtonEl.className = 'panel';
        downloadXmlButtonEl.style.marginLeft = '5px';
        downloadXmlButtonEl.style.zIndex = '10';
        downloadXmlButtonEl.style.cursor = 'pointer';
        downloadXmlButtonEl.addEventListener('click', () => {
            bpmnModeler.saveXML({ format: true }, (err, xml) => {
            if (err) {
            console.error(err);
            return;
            }
            const blob = new Blob([xml], { type: 'application/xml' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'diagram.bpmn';
            a.click();
            URL.revokeObjectURL(url);
            });
        });
        bottoni.appendChild(downloadXmlButtonEl);
    }
}

export class DownloadSVGButton {
    constructor(bpmnModeler, bottoni) {
        const downloadSvgButtonEl = document.createElement('button');
        downloadSvgButtonEl.textContent = 'SVG';
        downloadSvgButtonEl.className = 'panel';
        downloadSvgButtonEl.style.marginLeft = '5px';
        downloadSvgButtonEl.style.zIndex = '10';
        downloadSvgButtonEl.style.cursor = 'pointer';
        downloadSvgButtonEl.addEventListener('click', () => {
            bpmnModeler.saveSVG((err, svg) => {
            if (err) {
                console.error(err);
                return;
            }
            const blob = new Blob([svg], { type: 'image/svg+xml' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'diagram.svg';
            a.click();
            URL.revokeObjectURL(url);
            });
        });
        bottoni.appendChild(downloadSvgButtonEl);
    }
}

export class EditXMLButton {
    constructor(bpmnModeler, bottoni) {
      const editXmlButtonEl = document.createElement('button');
      editXmlButtonEl.textContent = 'XML';
      editXmlButtonEl.className = 'panel';
      editXmlButtonEl.style.marginLeft = '5px';
      editXmlButtonEl.style.zIndex = '10';
      editXmlButtonEl.style.cursor = 'pointer';
      editXmlButtonEl.addEventListener('click', () => {
        editXml();
      });
      bottoni.appendChild(editXmlButtonEl);
      async function editXml() {
        try {
          const result = await bpmnModeler.saveXML();
          const { xml } = result;
          const formattedXml = xmlFormat(xml);
      
          const overlayEl = document.createElement('div');
          overlayEl.classList.add('fullscreen-overlay');
      
          const editXmlContainerEl = document.createElement('div');
          editXmlContainerEl.classList.add('fullscreen-xml-container');
      
          const titleEl = document.createElement('h2');
          titleEl.textContent = 'Modifica XML';
          titleEl.style.margin = '10px';
          titleEl.style.fontSize = '24px';
          editXmlContainerEl.appendChild(titleEl);
      
          const xmlTextAreaEl = document.createElement('textarea');
          xmlTextAreaEl.textContent = formattedXml;
          xmlTextAreaEl.style.width = '100%';
          xmlTextAreaEl.style.height = 'calc(100% - 100px)';
          xmlTextAreaEl.style.resize = 'none';
          xmlTextAreaEl.style.fontSize = '16px';
          xmlTextAreaEl.style.fontFamily = 'monospace';
          xmlTextAreaEl.style.padding = '10px';
          xmlTextAreaEl.style.boxSizing = 'border-box';
          xmlTextAreaEl.style.border = '2px solid #ccc';
          xmlTextAreaEl.style.borderRadius = '5px';
          xmlTextAreaEl.style.marginTop = '10px';
          xmlTextAreaEl.style.marginBottom = '10px';
          editXmlContainerEl.appendChild(xmlTextAreaEl);
      
          const buttonContainerEl = document.createElement('div');
          buttonContainerEl.style.display = 'flex';
          buttonContainerEl.style.justifyContent = 'flex-end';
          buttonContainerEl.style.marginBottom = '10px';
      
          const saveButtonEl = document.createElement('button');
          saveButtonEl.textContent = 'Salva';
          saveButtonEl.style.marginRight = '10px';
          saveButtonEl.style.padding = '10px 20px';
          saveButtonEl.style.background = '#4CAF50';
          saveButtonEl.style.color = '#fff';
          saveButtonEl.style.fontSize = '16px';
          saveButtonEl.style.border = 'none';
          saveButtonEl.style.borderRadius = '5px';
          saveButtonEl.style.cursor = 'pointer';
          saveButtonEl.addEventListener('click', () => {
            const editedXml = xmlTextAreaEl.value;
            bpmnModeler.importXML(editedXml, (err) => {
              if (err) {s
                alert("Errore nel salvataggio dell'XML, assicurati che il contenuto sia corretto!" + err);
                return;
              }
              overlayEl.classList.add('hidden');
            });
          });
          buttonContainerEl.appendChild(saveButtonEl);
      
          const closeButtonEl = document.createElement('button');
          closeButtonEl.textContent = 'Chiudi';
          closeButtonEl.style.padding = '10px 20px';
          closeButtonEl.style.fontSize = '16px';
          closeButtonEl.style.border = '2px solid #ccc';
          closeButtonEl.style.borderRadius = '5px';
          closeButtonEl.style.cursor = 'pointer';
          closeButtonEl.addEventListener('click', () => {
            overlayEl.classList.add('hidden');
          });
          buttonContainerEl.appendChild(closeButtonEl);
      
          editXmlContainerEl.appendChild(buttonContainerEl);
      
          overlayEl.appendChild(editXmlContainerEl);
          document.body.appendChild(overlayEl);
        } catch (err) {
          console.log(err);
        }
      }      
    }
}

export class ZoomButtons {
  constructor(bpmnModeler, containerEl, canvasEl, canvasEl2) {
    const zoomInButton = document.createElement('button');
    zoomInButton.innerHTML = '+';
    zoomInButton.addEventListener('click', function() {
      // aumenta il livello di zoom
      bpmnModeler.get('zoomScroll').zoom(0.1, {x: canvasEl.offsetWidth/2, y: canvasEl.offsetHeight/2});
      bpmnModeler.get('zoomScroll').zoom(0.1, {x: canvasEl2.offsetWidth/2, y: canvasEl2.offsetHeight/2});
    });
    
    const zoomOutButton = document.createElement('button');
    zoomOutButton.innerHTML = '-';
    zoomOutButton.addEventListener('click', function() {
      // diminuisci il livello di zoom
      bpmnModeler.get('zoomScroll').zoom(-0.1, {x: canvasEl.offsetWidth/2, y: canvasEl.offsetHeight/2});
      bpmnModeler.get('zoomScroll').zoom(-0.1, {x: canvasEl2.offsetWidth/2, y: canvasEl2.offsetHeight/2});
    });
    
    // aggiungi i bottoni alla pagina
    const buttonsContainer = document.createElement('div');
    buttonsContainer.setAttribute('id', 'buttons-container');
    buttonsContainer.appendChild(zoomInButton);
    buttonsContainer.appendChild(zoomOutButton);
    containerEl.appendChild(buttonsContainer);

  }
}

export class SaveDiagramButton {
  constructor(bpmnModeler, containerEl) {
    this._bpmnModeler = bpmnModeler;
    this._containerEl = containerEl;

    this._createButton();
    this._createLoader(); 
  }

  _createButton() {
    const button = document.createElement('button');
    button.id = "saveDiagram";
    button.textContent = "SALVA";
    button.className = 'panel';
    button.style.marginRight = '5px';
    button.style.zIndex = '10';
    button.style.cursor = 'pointer';
    // button.style.backgroundColor = 'lightgreen';
    //button.addEventListener('click', () => this._saveDiagram());
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
    text.style.top = '57%';  // Posiziona il testo un po' sotto al spinner
    text.style.left = '50%';
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
    // Estrai il diagramma come XML
    const resultXML = await this._bpmnModeler.saveXML({ format: true });
    let content = resultXML.xml;
  
    // Rimuovi tutti i ritorni a capo e sostituisci tutte le doppie virgolette con singoli apici
    content = content.replace(/\n/g, "\n").replace(/"/g, '\"');
  
    // Estrai il diagramma come SVG
    const resultSVG = await this._bpmnModeler.saveSVG();
    const svgStr = resultSVG.svg;
  
    // Converte la stringa SVG in una stringa base64
    const previewInBase64 = btoa(unescape(encodeURIComponent(svgStr)));
  
    postDiagram(content, previewInBase64, false)
      .then(data => {
        if(data == 200){
          document.getElementById('saveLoader').style.display = 'none';
          document.getElementById('saveText').style.display = 'none';  // Nascondi il testo
          window.parent.postMessage({ op: "checkin"}, "*");
          Swal.fire({
            title: 'Salvato!',
            html: "Il tuo diagramma è stato salvato.",
            icon: 'success',
            showCancelButton: false,
            confirmButtonColor: 'green',
          });
        }
      })
      .catch(error => {
        document.getElementById('saveLoader').style.display = 'none';
        document.getElementById('saveText').style.display = 'none';  // Nascondi il testo
        Swal.fire({
          title: 'Errore!',
          html: "Si è verificato un errore durante la chiamata: " + "<b>" + error + "</b>",
          icon: 'error',
          showCancelButton: false,
          confirmButtonColor: 'green',
        });   
      });
  }
    
}

export class PublishDiagramButton {
  constructor(bpmnModeler, containerEl) {
    this._bpmnModeler = bpmnModeler;
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
    text.style.top = '57%';  // Posiziona il testo un po' sotto al spinner
    text.style.left = '50%';
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
    // Estrai il diagramma come XML
    const resultXML = await this._bpmnModeler.saveXML({ format: true });
    let content = resultXML.xml;
  
    // Rimuovi tutti i ritorni a capo e sostituisci tutte le doppie virgolette con singoli apici
    content = content.replace(/\n/g, "\n").replace(/"/g, '\"');
  
    // Estrai il diagramma come SVG
    const resultSVG = await this._bpmnModeler.saveSVG();
    const svgStr = resultSVG.svg;
  
    // Converte la stringa SVG in una stringa base64
    const previewInBase64 = btoa(unescape(encodeURIComponent(svgStr)));
  
    postDiagram(content, previewInBase64, true)
      .then(data => {
        if(data == 200){
          document.getElementById('publishLoader').style.display = 'none';
          document.getElementById('publishText').style.display = 'none';  // Nascondi il testo
          window.parent.postMessage({ op: "checkin"}, "*");
          Swal.fire({
            title: 'Pubblicato!',
            html: "Il tuo diagramma è stato pubblicato.",
            icon: 'success',
            showCancelButton: false,
            confirmButtonColor: 'blue',
          });
        }
      })
      .catch(error => {
        document.getElementById('publishLoader').style.display = 'none';
        document.getElementById('publishText').style.display = 'none';  // Nascondi il testo
        Swal.fire({
          title: 'Errore!',
          html: "Si è verificato un errore durante la chiamata: " + "<b>" + error + "</b>",
          icon: 'error',
          showCancelButton: false,
          confirmButtonColor: 'blue',
        });   
      });
  }
    
}


//funzione per chiamata api che salva il diagramma
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
      previewExtension: "svg",
      toBePublished: toBePublished
    }

    let mypathbase = window.location.pathname;

    if (!mypathbase.endsWith('/')) {
      mypathbase += '/';
    }

    // Effettua la chiamata POST all'API
    const response = await fetch(mypathbase+`api/postDiagram/${id}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data) // Converte i dati in una stringa JSON
    });

    //console.log(JSON.stringify(data));

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

  