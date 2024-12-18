import { SelectEntry, isSelectEntryEdited } from '@bpmn-io/properties-panel';
import { useService } from 'bpmn-js-properties-panel';
import config from '../../../descriptors/tenantConfig.json';
//import config from '../../../../public/bpm/camunda/descriptors/tenantConfig.json';


export default function (element) {
  return [
    {
      id: 'iframe',
      element,
      component: Spell,
      isEdited: isSelectEntryEdited,
    }
  ];
}

// function Spell(props) {  
//   const { element, id } = props;
//   const translate = useService('translate');

//   var nomeTask = element.di?.bpmnElement?.ctype;

//   let extensionElements = element.di?.bpmnElement?.extensionElements;
//   let inputParametersElement;
  
//   if (extensionElements) {
//       // Trova il ModdleElement che contiene inputParameters
//       inputParametersElement = extensionElements.values.find(e => e.inputParameters);
//   }

//   if (inputParametersElement && inputParametersElement.inputParameters.length > 0) {
//       let moddleElements = inputParametersElement.inputParameters;
//       // Estrai i valori di target e costruisci un array di oggetti con le coppie key-value   
//       let parameters = moddleElements.map(element => {
//         if (element.source && element.source.startsWith('=')) {
//           element.source = element.source.substring(1);
//         } 
//           return {
//               key: element.target,
//               value: element.source // sostituisci '' con il valore effettivo che desideri assegnare
//           };
//       });

//       // Converti l'array di oggetti in una stringa e poi codificala per l'URL
//       let encodedParameters = encodeURIComponent(JSON.stringify(parameters));

//       // Costruisci l'URL finale
//       let domain = config.urls[0].iframepath;  // sostituisci con il tuo dominio effettivo
//       let url = `${domain}/BPMN/${nomeTask}?input=${encodedParameters}`;
//       //let url= " http://localhost:81/test_camunda.html"

//       const iframeURL = url;  // Inserisci qui l'URL del tuo iframe
      
//       return (
//         <>
//           <iframe 
//             src={iframeURL} 
//             width="100%" 
//             height="500px"  // Adatta queste dimensioni alle tue esigenze
//             frameborder="0">
//             {translate('Il tuo browser non supporta gli iframe.')}
//           </iframe>
//         </>
//       );
//   }
//   else{
//       let parameters = 
//         {
//             key: '',
//             value: '' // sostituisci '' con il valore effettivo che desideri assegnare
//         };

//     // Converti l'array di oggetti in una stringa e poi codificala per l'URL
//     let encodedParameters = encodeURIComponent(JSON.stringify(parameters));

//     // Costruisci l'URL finale
//     let domain = config.urls[0].iframepath;  // sostituisci con il tuo dominio effettivo
//     let url = `${domain}/BPMN/${nomeTask}?input=${encodedParameters}`;
//     //let url= " http://localhost:81/test_camunda.html"

//     const iframeURL = url;  // Inserisci qui l'URL del tuo iframe

//     if(nomeTask != undefined){
//       $('div[data-group-id="group-iframe"]').css("display","block")
//       return (
//         <>
//           <iframe 
//             src={iframeURL} 
//             width="100%" 
//             height="500px"  // Adatta queste dimensioni alle tue esigenze
//             frameborder="0">
//             {translate('Il tuo browser non supporta gli iframe.')}
//           </iframe>
//         </>
//       );
//     }
//     else{
//       $('div[data-group-id="group-iframe"]').css("display","none")
//     }
//   } 
// }
