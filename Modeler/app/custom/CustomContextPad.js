import config from '../descriptors/tenantConfig.json';

export default class CustomContextPad {
  constructor(bpmnFactory, config, contextPad, create, elementFactory, injector, translate) {
    this.bpmnFactory = bpmnFactory;
    this.create = create;
    this.elementFactory = elementFactory;
    this.translate = translate;

    if (config.autoPlace !== false) {
      this.autoPlace = injector.get('autoPlace', false);
    }

    contextPad.registerProvider(this);

  }
  

  // getContextPadEntries(element) {
  //     // Se l'elemento non è di tipo bpmn:ServiceTask, ritorna un oggetto vuoto
  // if (!element.businessObject || element.businessObject.$type !== 'bpmn:ServiceTask') {
  //   return {};
  // }
  //   const {
  //     autoPlace,
  //     bpmnFactory,
  //     create,
  //     elementFactory,
  //     translate
  //   } = this;

  //   function _extends() {
  //     _extends = Object.assign || function (target) {
  //       for (var i = 1; i < arguments.length; i++) {
  //         var source = arguments[i];

  //         for (var key in source) {
  //           if (Object.prototype.hasOwnProperty.call(source, key)) {
  //             target[key] = source[key];
  //           }
  //         }
  //       }

  //       return target;
  //     };

  //     return _extends.apply(this, arguments);
  //   }

  //   function assign(target) {
  //     for (var _len = arguments.length, others = new Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
  //       others[_key - 1] = arguments[_key];
  //     }

  //     return _extends.apply(void 0, [target].concat(others));
  //   }

  //   function openIframe(event, element) {
  //     $("#properties-panel-container").css("display","none");
  //     var nomeTask = element.di?.bpmnElement?.ctype;
  //     if(nomeTask === undefined) {
  //       $("#properties-panel-container").css("display","block");
  //       alert("Selezionare prima il tipo di task!");
  //       return; // Uscire dalla funzione se nomeTask è undefined
  //   }
  //     var iframeURL = "";
  //     let extensionElements = element.di?.bpmnElement?.extensionElements;
  //     let inputParametersElement;
      
  //     if (extensionElements) {
  //         // Trova il ModdleElement che contiene inputParameters
  //         inputParametersElement = extensionElements.values.find(e => e.inputParameters);
  //     }

  //     if (inputParametersElement && inputParametersElement.inputParameters.length > 0) {
  //       let moddleElements = inputParametersElement.inputParameters;
  //       // Estrai i valori di target e costruisci un array di oggetti con le coppie key-value   
  //       let parameters = moddleElements.map(element => {
  //         // if (element.source && element.source.startsWith('=')) {
  //         //   element.source = element.source.substring(1);
  //         // } 
  //           return {
  //               key: element.target,
  //               value: element.source // sostituisci '' con il valore effettivo che desideri assegnare
  //           };
  //       });
  
  //       // Converti l'array di oggetti in una stringa e poi codificala per l'URL
  //       let encodedParameters = encodeURIComponent(JSON.stringify(parameters));
  //       console.log("Parametri che l'url passa all'iframe: "+JSON.stringify(parameters));
  //       // Costruisci l'URL finale
  //       let domain = config.urls[0].iframepath;  // sostituisci con il tuo dominio effettivo
  //       let url = `${domain}/BPMN/${nomeTask}?input=${encodedParameters}`;
  //       //let url= " http://localhost:81/test_camunda.html"
  
  //       iframeURL = url;  // Inserisci qui l'URL del tuo iframe
  //   }
  //   else{
  //       let parameters = 
  //         {
  //             key: '',
  //             value: '' // sostituisci '' con il valore effettivo che desideri assegnare
  //         };
  
  //     // Converti l'array di oggetti in una stringa e poi codificala per l'URL
  //     let encodedParameters = encodeURIComponent(JSON.stringify(parameters));
  //     console.log("Parametri che l'url passa all'iframe: "+JSON.stringify(parameters));
  //     // Costruisci l'URL finale
  //     let domain = config.urls[0].iframepath;  // sostituisci con il tuo dominio effettivo
  //     let url = `${domain}/BPMN/${nomeTask}?input=${encodedParameters}`;
  //     //let url= " http://localhost:81/test_camunda.html"
  
  //     iframeURL = url;  // Inserisci qui l'URL del tuo iframe
  //   } 

  //     $("#buttons-container").hide();
  //     // Prova a trovare un div contenitore esistente
  //     var container = document.getElementById('iframeContainer');
    
  //     if (container) {
  //       // Se il contenitore esiste già, ricarica l'iframe
  //       var iframe = container.querySelector('iframe');
  //       iframe.src = iframeURL;
  //       //iframe.contentWindow.location.reload();
  //     } else {
  //       // Se il contenitore non esiste, crealo
  //       container = document.createElement('div');
  //       container.id = 'iframeContainer'; // Assegna un ID per poterlo trovare facilmente in seguito
  //       container.style.position = 'absolute'; // Posiziona il contenitore (e quindi l'iframe e il pulsante) relativamente
  //       container.style.right = '0'; // Posiziona il contenitore (e quindi l'iframe e il pulsante) relativamente
  //       container.style.zIndex = '9999'; // Posiziona il contenitore (e quindi l'iframe e il pulsante) relativamente
  //       container.style.height = '100%'; // Posiziona il contenitore (e quindi l'iframe e il pulsante) relativamente

        
  //       // Crea l'iframe
  //       var iframe = document.createElement('iframe');
  //       iframe.src = iframeURL; // Sostituisci con l'URL del tuo iframe
  //       iframe.width = '500';
  //       iframe.height = '100%';
  //       iframe.id = "iframecustom";
  //       iframe.style.boxShadow= "-6px -1px 10px 0px #888888";

  //       // Crea il loader
  //       var loader = document.createElement('div');
  //       loader.id = 'iframeLoader';
  //       loader.style.position = 'absolute';
  //       loader.style.top = '0';
  //       loader.style.right = '0';
  //       loader.style.width = '100%';
  //       loader.style.height = '100%';
  //       loader.style.background = 'rgba(255, 255, 255, 0.9)'; // bianco semitrasparente
  //       loader.style.display = 'flex';
  //       loader.style.alignItems = 'center';
  //       loader.style.justifyContent = 'center';
  //       loader.innerHTML = '<div class="loader"></div>'; // Qui puoi inserire il tuo HTML per l'animazione del loader

  //       // Aggiungi il loader al container
  //       container.appendChild(loader);

  //       // Aggiungi l'event listener per l'evento load dell'iFrame
  //       iframe.addEventListener('load', function() {
  //           // Nascondi il loader una volta che l'iFrame è completamente caricato
  //           loader.style.display = 'none';
  //       });
  //       container.appendChild(iframe); // Aggiungi l'iframe al contenitore
        
  //       // Crea il pulsante di chiusura
  //       var closeButton = document.createElement('button');
  //       closeButton.innerText = 'X'; // Testo del pulsante
  //       closeButton.style.position = 'absolute'; // Posiziona il pulsante assolutamente all'interno del contenitore
  //       closeButton.style.right = '0'; // Allinea il pulsante al bordo destro del contenitore
  //       closeButton.style.top = '0'; // Allinea il pulsante al bordo superiore del contenitore
  //       closeButton.style.background = 'hsl(225, 10%, 35%)'; // Imposta il colore di sfondo del pulsante a rosso
  //       closeButton.style.color = 'white'; // Imposta il colore del testo del pulsante a bianco
  //       closeButton.style.border = 'none'; // Rimuovi il bordo del pulsante
  //       closeButton.style.cursor = 'pointer'; // Cambia il cursore al passaggio del mouse sul pulsante
  //       closeButton.style.padding = "10px";
  //       closeButton.setAttribute('id', 'chiudiframe');
  //       closeButton.addEventListener('click', function() {
  //         $("#properties-panel-container").css("display","block");
  //         $("#buttons-container").show();
  //         // Al click sul pulsante, rimuovi il contenitore (e quindi anche l'iframe e il pulsante) dal DOM
  //         document.body.removeChild(container);
  //       });
  //       container.appendChild(closeButton); // Aggiungi il pulsante di chiusura al contenitore
  //       // Aggiungi il contenitore al body della pagina o in un altro posto a tua scelta
  //       document.body.appendChild(container);
  //     }
  //   }

  //   var actions = {};

  //   assign(actions, {
  //     'append.iframe': {
  //       group: 'edit',
  //       className: 'fas fa-cog',
  //       title: translate('Imposta proprietà del task'),
  //       action: {
  //         click: openIframe,
  //       }
  //     }
  //   });    
  //   return actions;
  // }

}

CustomContextPad.$inject = [
  'bpmnFactory',
  'config',
  'contextPad',
  'create',
  'elementFactory',
  'injector',
  'translate'
];