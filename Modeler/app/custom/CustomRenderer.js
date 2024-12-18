import inherits from 'inherits';
import BaseRenderer from 'diagram-js/lib/draw/BaseRenderer';
import { append as svgAppend, create as svgCreate } from 'tiny-svg';
import elements from '../descriptors/customelements'

const HIGH_PRIORITY = 1500;

export default function CustomRenderer(eventBus, bpmnRenderer) {
  // BaseRenderer.call(this, eventBus, HIGH_PRIORITY);

  // this.bpmnRenderer = bpmnRenderer;

  // this.canRender = function (element) {
  //   return !element.labelTarget;
  // };

  // function createElement(parentNode, element, unicodeIcon) {
  //   const rect = svgCreate('rect');
    
  //   // Aggiungi un bordo arrotondato
  //   rect.setAttribute('rx', 10);
  //   rect.setAttribute('ry', 10);

  //   // Applica la sfumatura al rettangolo
  //   rect.setAttribute('fill', 'url(#gradient)');
    
  //   rect.setAttribute('width', element.width);
  //   rect.setAttribute('height', element.height);
  //   rect.setAttribute('stroke', 'black');
  //   rect.setAttribute('stroke-width', 2);

  //   svgAppend(parentNode, rect);

  //   const icon = svgCreate('text');
  //   icon.setAttribute('font-family', 'FontAwesome');
  //   icon.textContent = unicodeIcon;
  //   const margin = 10;
  //   icon.setAttribute('x', margin);
  //   icon.setAttribute('y', margin + 15);
  //   icon.setAttribute('font-size', '20px');

  //   svgAppend(parentNode, icon);

  //   return rect;
  // }

  // function updateLabelPosition(element, textElement) {
  //   let words = element.businessObject.name.split(' ');

  //   let dy = 0;
  //   let lineWidth = 0;
  //   let currentLine = [];

  //   words.forEach((word, i) => {
  //     if (lineWidth + word.length > element.width / 20) {
  //       currentLine = [word];
  //       lineWidth = word.length;
  //       dy += 20;
  //     } else {
  //       currentLine.push(word);
  //       lineWidth += word.length;
  //     }
  //   });

  //   let height = dy + 20; // Aggiunge la dimensione dell'ultima riga
  //   let y = (element.height - height) / 2; // Calcola la nuova posizione 'y' per centrare il testo
    
  //   // Aggiorna la posizione 'y' di ogni riga di testo
  //   Array.from(textElement.childNodes).forEach((tspan, i) => {
  //     tspan.setAttribute('dy', i === 0 ? y : 20);
  //   });
  // }

  // this.drawShape = function (parentNode, element) {
  //   let shape;
  
  //   // Cerca l'elemento corrispondente nel JSON
  //   let elementData = null;
  //   for (let group of elements) {
  //     for (let item of group.items) {
  //       if (item.target.type === element.type) {
  //         elementData = item;
  //         break;
  //       }
  //     }
  //     if (elementData) break;
  //   }
  
  //   // Se l'elemento corrispondente è stato trovato nel JSON
  //   if (elementData) {
  //     if (!element.businessObject.name) {
  //       // Imposta il nome dell'oggetto business sull'etichetta corrispondente
  //       element.businessObject.name = elementData.label;
  //     }
  //     shape = createElement(parentNode, element);
  //   } else if (element.type.includes('elmi')) {
  //     shape = createElement(parentNode, element, '');
  //   }
  
  //   if (element.businessObject.name) {
  //     let words = element.businessObject.name.split(' ');
  //     const text = svgCreate('text');
  
  //     // Troncare le parole e aggiungere "..." se necessario
  //     if (words.length > 2) {
  //       words = words.slice(0, 2);
  //       words.push("...");
  //     }
  
  //     text.setAttribute('x', element.width / 2);
  //     text.setAttribute('text-anchor', 'middle');
  //     text.setAttribute('dominant-baseline', 'central');
  
  //     let dy = 0;
  //     let lineWidth = 0;
  //     let currentLine = [];
  
  //     words.forEach((word, i) => {
  //       if (lineWidth + word.length > element.width / 20) {
  //         const lineTspan = svgCreate('tspan');
  //         lineTspan.textContent = currentLine.join(' ');
  //         lineTspan.setAttribute('x', element.width / 2);
  //         lineTspan.setAttribute('dy', dy === 0 ? '0' : '30');
  //         text.appendChild(lineTspan);
  
  //         currentLine = [word];
  //         lineWidth = word.length;
  //         dy += 20;
  //       } else {
  //         const lineTspan = svgCreate('tspan');
  //         lineTspan.textContent = currentLine.join(' ');
  //         lineTspan.setAttribute('x', element.width / 2);
  //         lineTspan.setAttribute('dy', dy === 0 ? '0' : '30');
  //         text.appendChild(lineTspan);
  
  //         currentLine = [word];
  //         lineWidth = word.length;
  //         dy += 20;
  //       }
  
  //       if (i === words.length - 1) {
  //         const lineTspan = svgCreate('tspan');
  //         lineTspan.textContent = currentLine.join(' ');
  //         lineTspan.setAttribute('x', element.width / 2);
  //         lineTspan.setAttribute('dy', dy === 0 ? '0' : '20');
  //         text.appendChild(lineTspan);
  //       }
  //     });
  
  //     svgAppend(parentNode, text);
  
  //     // Aggiorna la posizione della label
  //     updateLabelPosition(element, text);
  //   }
  
  //   return shape;
  // };
  

  // this.getShapePath = function (shape) {
  //   const { x, y, width, height } = shape;
  //   return [
  //     ['M', x, y],
  //     ['l', width, 0],
  //     ['l', 0, height],
  //     ['l', -width, 0],
  //     ['z']
  //   ];
  // };
}

// inherits(CustomRenderer, BaseRenderer);

// CustomRenderer.$inject = ['eventBus', 'bpmnRenderer'];
