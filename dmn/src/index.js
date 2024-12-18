import "./styles.css";
import Swal from 'sweetalert2'
import "dmn-js/dist/assets/diagram-js.css";
import "dmn-js/dist/assets/dmn-font/css/dmn-embedded.css";
import "dmn-js/dist/assets/dmn-js-decision-table-controls.css";
import "dmn-js/dist/assets/dmn-js-decision-table.css";
import "dmn-js/dist/assets/dmn-js-drd.css";
import "dmn-js/dist/assets/dmn-js-literal-expression.css";
import "dmn-js/dist/assets/dmn-js-shared.css";
import 'dmn-js-properties-panel/dist/assets/properties-panel.css';
import { SaveDiagramButton } from './custom/CustomButtons';

const dmn = `<?xml version="1.0" encoding="UTF-8"?>
<definitions xmlns="https://www.omg.org/spec/DMN/20191111/MODEL/" xmlns:dmndi="https://www.omg.org/spec/DMN/20191111/DMNDI/" xmlns:dc="http://www.omg.org/spec/DMN/20180521/DC/" id="Definitions_1lmea3i" name="DRD" namespace="http://camunda.org/schema/1.0/dmn">
  <decision id="Decision_0mqgtic" name="Decision 1">
    <decisionTable id="DecisionTable_11azyal">
      <input id="Input_1">
        <inputExpression id="InputExpression_1" typeRef="string">
          <text></text>
        </inputExpression>
      </input>
      <output id="Output_1" typeRef="string" />
    </decisionTable>
  </decision>
  <dmndi:DMNDI>
    <dmndi:DMNDiagram>
      <dmndi:DMNShape dmnElementRef="Decision_0mqgtic">
        <dc:Bounds height="80" width="180" x="160" y="100" />
      </dmndi:DMNShape>
    </dmndi:DMNDiagram>
  </dmndi:DMNDI>
</definitions>
`;

import DmnModeler from 'dmn-js/lib/Modeler';

import {
  DmnPropertiesPanelModule,
  DmnPropertiesProviderModule,
} from 'dmn-js-properties-panel';

let myListener = (event) => {
  event.preventDefault();
  event.returnValue = '';
};

// Aggiungi l'ascoltatore d'evento
window.addEventListener('beforeunload', myListener);

var dmnModeler = new DmnModeler({
  drd: {
    propertiesPanel: {
      parent: '#properties'
    },
    additionalModules: [
      DmnPropertiesPanelModule,
      DmnPropertiesProviderModule
    ]
  },
  container: '#canvas'
});

const bottoniapi = document.getElementById('apibuttons');
const controls = document.getElementById('controls');

dmnModeler.on("views.changed", ({ activeView }) => {
  if (activeView.type != "drd") {
    bottoniapi.style.display='none';
    controls.style.display='none';
  }
  else{
    bottoniapi.style.display='block';
    controls.style.display='block';
  }
}
);

//import dei pulsanti custom per la gestione dell'XML
const saveDiagramButton = new SaveDiagramButton(dmnModeler, bottoniapi);

// const publishDiagramButton = new PublishDiagramButton(dmnModeler, bottoniapi);

const style = document.createElement('style');
style.textContent = `
  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
`;
document.head.appendChild(style);

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

  let mypathbase = window.location.pathname;

  if (!mypathbase.endsWith('/')) {
    mypathbase += '/';
  }

  // Utilizza l'ID nella chiamata API
  fetch(mypathbase+`api/getDmn/${id}`)
    .then(response => {
      if (!response.ok) {
        const Toast = Swal.mixin({
          toast: true,
          position: 'top-end',
          showConfirmButton: false,
          timer: 4000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
          }
        })  
        Swal.fire({
          title: 'Errore!',
          html: "Errore nell'apertura del diagramma DMN, ricaricare la pagina",
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: "Ok, ricarica",
          confirmButtonColor: 'red',
          allowOutsideClick: false
        }).then((result) => {
          if (result.isConfirmed) {
            window.removeEventListener('beforeunload', myListener);
            location.reload();
          }
        }); 
        throw new Error("Errore HTTP: " + response.status+ " " +  response.statusText);
      }
      return response.json();
    })
    .then(data => {
      // Utilizza i dati dell'API
      //console.log(content);
      //var content = data.content.replace(/\\n/g, "\n").replace(/\\"/g, '"');
      dmnModeler.importXML(data.content).then(() => {
      }).catch((err) => {
        //console.error(err);
        const Toast = Swal.mixin({
          toast: true,
          position: 'top-end',
          showConfirmButton: false,
          timer: 4000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
          }
        })  
        Swal.fire({
          title: 'Errore!',
          html: "Errore nell'apertura del diagramma DMN, ricaricare la pagina",
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: "Ok, ricarica",
          confirmButtonColor: 'red',
          allowOutsideClick: false
        }).then((result) => {
          if (result.isConfirmed) {
            window.removeEventListener('beforeunload', myListener);
            location.reload();
          }
        }); 
      });
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 4000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.addEventListener('mouseenter', Swal.stopTimer)
          toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
      })
      Toast.fire({
        icon: 'success',
        title: 'Diagramma aperto correttamente'
      })
    })
    .catch(error => {
      // Gestisci eventuali errori che si verificano durante la chiamata all'endpoint
      //console.error('Si è verificato un errore durante la chiamata all\'endpoint:', error);
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 4000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.addEventListener('mouseenter', Swal.stopTimer)
          toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
      })
      Swal.fire({
        title: 'Errore!',
        html: "Errore nell'apertura del diagramma DMN, ricaricare la pagina",
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: "Ok, ricarica",
        confirmButtonColor: 'red',
        allowOutsideClick: false
      }).then((result) => {
        if (result.isConfirmed) {
          window.removeEventListener('beforeunload', myListener);
          location.reload();
        }
      }); 
    });
} else {
    // import XML base
    dmnModeler.importXML(dmn).then(() => {
    }).catch((err) => {
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 4000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.addEventListener('mouseenter', Swal.stopTimer)
          toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
      })
      Swal.fire({
        title: 'Errore!',
        html: "Errore nell'apertura del diagramma DMN, ricaricare la pagina",
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: "Ok, ricarica",
        confirmButtonColor: 'red',
        allowOutsideClick: false
      }).then((result) => {
        if (result.isConfirmed) {
          window.removeEventListener('beforeunload', myListener);
          location.reload();
        }
      }); 
    });
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 4000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    })
    Toast.fire({
      icon: 'success',
      title: 'Nessun parametro di query fornito. Import del diagramma DMN di base'
    })
}


// aggiungi un listener d'evento ai tuoi pulsanti
document.getElementById('importButton').addEventListener('click', importDiagram);
document.getElementById('exportButton').addEventListener('click', exportDiagram);

function importDiagram() {
  document.getElementById('fileInput').click();
}

document.getElementById('fileInput').addEventListener('change', function(evt) {
  var file = evt.target.files[0];

  var reader = new FileReader();

  reader.onload = function(e) {
    var xml = e.target.result;

    dmnModeler.importXML(xml, function(err) {
      if (err) {
        console.log('error rendering', err);
      } else {
        console.log('rendered');
      }
    });
  };

  reader.readAsText(file);
});


function exportDiagram() {
  dmnModeler.saveXML({ format: true }, function(err, xml) {
    if (err) {
      console.log('error exporting', err);
    } else {
      var blob = new Blob([xml], {type: "application/xml;charset=utf-8"});
      var link = document.createElement('a');
      link.href = URL.createObjectURL(blob);
      link.download = "diagram.dmn";
      link.click();
    }
  });
}

var canvas = document.getElementById('canvas');

canvas.addEventListener('dragover', function(evt) {
  evt.stopPropagation();
  evt.preventDefault();
  evt.dataTransfer.dropEffect = 'copy'; 
  canvas.classList.add('dragover'); // Aggiungi una classe per cambiare l'aspetto del dropzone
});

canvas.addEventListener('dragleave', function(evt) {
  canvas.classList.remove('dragover'); // Rimuovi la classe quando il drag finisce
});

canvas.addEventListener('drop', function(evt) {
  evt.stopPropagation();
  evt.preventDefault();
  canvas.classList.remove('dragover'); // Rimuovi la classe quando il file viene rilasciato
  var files = evt.dataTransfer.files;
  var file = files[0]; // Prendi solo il primo file
  var reader = new FileReader();
  reader.onload = function(e) {
    var xml = e.target.result;
    dmnModeler.importXML(xml, function(err) {
      if (err) {
        console.log('error rendering', err);
      } else {
        console.log('rendered');
      }
    }); 
  };
  reader.readAsText(file);
});


