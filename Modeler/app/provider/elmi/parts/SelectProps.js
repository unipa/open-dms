// import { SelectEntry, isSelectEntryEdited } from '@bpmn-io/properties-panel';
// import { useService } from 'bpmn-js-properties-panel';
// import config from '../../../descriptors/tenantConfig.json';
// import values from '../../../descriptors/customelements.json';

// export default function (element) {
//   return [
//     {
//       id: 'ctype',
//       element,
//       component: CustomSelect,
//       isEdited: isSelectEntryEdited, // o un'altra funzione appropriata per la select
//     }
//   ];
// }

// var counter = 0;

// function CustomSelect(props) {
//   const { element } = props;
//   const translate = useService('translate');
//   const modeling = useService('modeling');
//   const bpmnFactory = useService('bpmnFactory');

//   // Assicurati di avere un modo per gestire i cambiamenti
//   function handleChange(newValue) {
//     var container = document.getElementById('iframeContainer');
//     var apro = 0;
//     if ($('#iframeContainer').length == 1) {
//       $('#chiudiframe').click();
//       apro = 1;
//     }
//     const businessObject = element.businessObject;
//     businessObject.ctype = newValue;

//     let extensionElements = businessObject.extensionElements;

//     // Assicuriamoci che extensionElements esiste, altrimenti creiamolo
//     if (!extensionElements) {
//         extensionElements = bpmnFactory.create('bpmn:ExtensionElements', {
//             values: []
//         });
//         modeling.updateProperties(element, {
//             extensionElements: extensionElements
//         });
//     }

//     let ioMapping = extensionElements.values.find(e => e.$type === 'zeebe:IoMapping');

//     if (ioMapping) {
//         // Rimuovi tutte le variabili di input
//         ioMapping.inputParameters = [];
//     } 

//     // Controlla se esiste già un elemento zeebe:TaskDefinition
//     let taskDefinition = extensionElements.values.find(e => e.$type === 'zeebe:TaskDefinition');

//     if (!taskDefinition) {
//         taskDefinition = bpmnFactory.create('zeebe:TaskDefinition', {
//             type: newValue
//         });
//         extensionElements.values.push(taskDefinition);
//     } else {
//         taskDefinition.type = newValue;
//     }

//     // Cerca l'opzione corrispondente
//     const selectedOption = getOptionValuesFromConfig().find(option => option.value === newValue);
//     const labelValue = selectedOption ? selectedOption.label : '';

//     // Aggiorna l'elemento nel canvas per riflettere le modifiche
//     modeling.updateProperties(element, {
//         name: labelValue,
//         extensionElements: extensionElements
//     });
//     if ($('#iframeContainer').length == 1 || apro == 1) {
//       $('[data-action="append.iframe"]').trigger('click');
//       apro = 0;
//     }
// }


// function getOptionValuesFromConfig() {
//   let options = [];
  
//   if (Array.isArray(values)) {
//     values.forEach(item => {
//           item.items.forEach(subItem => {
//               // Rimuove il prefisso "elmi:" dalla stringa
//               let typeValue = subItem.target.type.replace('elmi:', '');
//               let label = subItem.label;
//               options.push({
//                   value: typeValue,
//                   label: label
//               });
//           });
//       });
//   }

//   // Ordina le opzioni in ordine alfabetico basandosi sulla proprietà 'label'
//   options.sort((a, b) => a.label.localeCompare(b.label));

//   return options;
// }



// return (
//   <SelectEntry 
//       id="customSelect" 
//       //label={translate('Selezione Personalizzata')} 
//       getOptions={getOptionValuesFromConfig}
//       getValue={() => element.businessObject.ctype || ''}
//       setValue={(newValue) => handleChange(newValue, modeling, bpmnFactory)}
//   />
// );


// }

