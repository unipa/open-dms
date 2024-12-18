import { createFormEditor } from "@bpmn-io/form-js";
import Swal from 'sweetalert2'
import "@bpmn-io/form-js/dist/assets/dragula.css";
import "@bpmn-io/form-js/dist/assets/form-js.css";
import "@bpmn-io/form-js/dist/assets/form-js-editor.css";
import "@bpmn-io/form-js/dist/assets/form-js-playground.css";
import "@bpmn-io/form-js/dist/assets/properties-panel.css";
import { SaveDiagramButton } from './custom/CustomButtons';

import "./styles.css";

import schema from "./descriptors/schema.json";

import config from "./descriptors/tenantConfig.json";

const bottoniapi = document.getElementById('apibuttons');

(async () => {
  const formEditor = await createFormEditor({
    container: document.getElementById("app"),
    schema,
    exporter: {
      name: "Foo Editor",
      version: "1.0.0"
    }
  });

  let myListener = (event) => {
    event.preventDefault();
    event.returnValue = '';
  };
  
  // Aggiungi l'ascoltatore d'evento
  window.addEventListener('beforeunload', myListener);

let doc =  document.getElementsByClassName("fjs-children fjs-drag-container fjs-vertical-layout");

doc[0].style.marginTop = '60px';

//import dei pulsanti custom per la gestione dell'XML
const saveDiagramButton = new SaveDiagramButton(formEditor, bottoniapi);

// const publishDiagramButton = new PublishDiagramButton(formEditor, bottoniapi);

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
  fetch(mypathbase+`api/getForm/${id}`)
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
          html: "Errore nell'apertura del form, ricaricare la pagina",
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
      data.content = data.content.replace(/'/g, "\"");
      let content = JSON.parse(data.content);
      //console.log(content);
      formEditor.importSchema(content).then(() => {
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
        });  
        Swal.fire({
          title: 'Errore!',
          html: "Errore nell'apertura del form, ricaricare la pagina",
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
        title: 'Form aperto correttamente'
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
        html: "Errore nell'apertura del form, ricaricare la pagina",
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
    formEditor.importSchema(schema).then(() => {
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
        html: "Errore nell'apertura del form, ricaricare la pagina",
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
      title: 'Nessun parametro di query fornito. Import del form di base'
    })
}

// ... Import statements e funzioni readFile e loadJsonFile ...

const importButton = document.getElementById("importButton");
const exportButton = document.getElementById("exportButton");

// Funzione per creare un elemento di input di tipo file
function createFileInput() {
  const input = document.createElement("input");
  input.type = "file";
  input.accept = "application/json";
  return input;
}

// Funzione per gestire l'importazione del file JSON tramite il pulsante Importa JSON
function handleImportButtonClick() {
  const input = createFileInput();
  input.addEventListener("change", async (event) => {
    const files = event.target.files;
    if (files.length === 1) {
      await loadJsonFile(files[0]);
    }
  });
  input.click();
}

// Funzione per gestire l'esportazione del file JSON tramite il pulsante Esporta JSON
async function handleExportButtonClick() {
  const schema = await formEditor.saveSchema();
  const blob = new Blob([JSON.stringify(schema, null, 2)], { type: "application/json" });
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = url;
  link.download = "form-schema.json";
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

importButton.addEventListener("click", handleImportButtonClick);
exportButton.addEventListener("click", handleExportButtonClick);

  const dropzone = document.getElementById("dropzone");

// Funzione per leggere il contenuto del file JSON
function readFile(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = (event) => resolve(event.target.result);
    reader.onerror = (error) => reject(error);
    reader.readAsText(file);
  });
}

// Funzione per gestire il caricamento del file JSON
async function loadJsonFile(file) {
  try {
    const fileContent = await readFile(file);
    const json = JSON.parse(fileContent);
    await formEditor.importSchema(json);
  } catch (error) {
    console.error("Errore durante il caricamento del file JSON:", error);
  }
}
  // Gestione degli eventi drag and drop
  document.addEventListener("dragenter", (event) => {
    event.preventDefault();
    dropzone.classList.remove("hidden");
  });

  dropzone.addEventListener("dragover", (event) => {
    event.preventDefault();
  });

  dropzone.addEventListener("dragleave", (event) => {
    event.preventDefault();
    dropzone.classList.add("hidden");
  });

  dropzone.addEventListener("drop", async (event) => {
    event.preventDefault();
    dropzone.classList.add("hidden");

    const files = event.dataTransfer.files;
    if (files.length === 1 && files[0].type === "application/json") {
      await loadJsonFile(files[0]);
    } else {
      console.error("Si prega di trascinare un file JSON valido.");
    }
  });

  function openJsonModal(json) {
    const jsonModal = document.getElementById("jsonModal");
    const jsonTextArea = document.getElementById("jsonTextArea");
    jsonTextArea.value = JSON.stringify(json, null, 2);
    jsonModal.style.display = "block";
  }
  
  function closeJsonModal() {
    const jsonModal = document.getElementById("jsonModal");
    jsonModal.style.display = "none";
  }

  const viewJsonButton = document.getElementById("viewJsonButton");
const closeModalButton = document.getElementById("closeModalButton");

viewJsonButton.addEventListener("click", async () => {
  const schema = await formEditor.saveSchema();
  openJsonModal(schema);
});

closeModalButton.addEventListener("click", () => {
  closeJsonModal();
});

formEditor.on("changed", () => {
  (async () => {
    const currentSchema = await formEditor.saveSchema();
    // Verifica se le proprietà personalizzate sono state aggiunte
    for (let i = 0; i < currentSchema.components.length; i++) {
      // Verifica che sia definito currentSchema.components[i]
      if (currentSchema.components[i] && typeof(currentSchema.components[i].properties) != 'undefined') {
        var currentComponentid = currentSchema.components[i].id;
        var currentComponentp = currentSchema.components[i].properties;
        //console.log(currentComponentp);
        //console.log(currentComponentid);
        var elements = document.querySelectorAll('[id*="' + currentComponentid + '"]');
        for(var j = 0; j < elements.length; j++) {
            //console.log("Elemento trovato: ", elements[j]);

            // Prima, rimuovi tutti gli attributi attuali
            Array.prototype.slice.call(elements[j].attributes).forEach(function(attribute) {
              if (attribute.name !== 'class' && attribute.name !== 'id' && attribute.name !== 'style' && attribute.name !== 'disabled' && attribute.name !== 'type' && attribute.name !== 'autocomplete' && attribute.name !== 'step' && attribute.name !== 'placeholder' && attribute.name !== 'data-input') {
                elements[j].removeAttribute(attribute.name);
              }
            });

            // Poi, aggiungi nuovi attributi da currentComponentp
            if (typeof(currentComponentp) === 'object') {
                for (var key in currentComponentp) {
                    if (currentComponentp.hasOwnProperty(key)) {
                        elements[j].setAttribute(key, currentComponentp[key]);
                    }
                }
            }
        }
      }
    }
  })();
});

// $(document).ready(function() {
//   // Creare un'istanza di MutationObserver
//   var observer = new MutationObserver(function(mutations) {
//       mutations.forEach(function(mutation) {
//           // Se la lista delle classi muta...
//           if (mutation.attributeName === "class") {
//               var classList = $(mutation.target).prop(mutation.attributeName);
//               // ...e contiene la classe "swal2-height-auto", allora rimuovila
//               if (classList.includes("swal2-height-auto")) {
//                   $(mutation.target).removeClass("swal2-height-auto");
//               }
//           }
//       });
//   });

//   // Inizia ad osservare il target con le configurazioni specificate
//   observer.observe(document.body, {
//       attributes: true // osserva solo le modifiche degli attributi
//   });
// });

})();
