import BpmnModeler from "bpmn-js/lib/Modeler";

import resizeTask from "bpmn-js-task-resize/lib";

import BpmnColorPickerModule from "bpmn-js-color-picker";

import minimapModule from "diagram-js-minimap";

import Swal from "sweetalert2";

import BpmnModdle from "bpmn-moddle";

import camundaModdlePackage from "camunda-bpmn-moddle/resources/camunda.json";

import zeebeModdle from "zeebe-bpmn-moddle/resources/zeebe.json";

import customelem from "../app/descriptors/customelements.json";

import zeebeModdleExtensions from "../app/descriptors/prova.json";

import {
	UploadXmlButton,
	DownloadXmlButton,
	DownloadSVGButton,
	EditXMLButton,
	ZoomButtons,
	SaveDiagramButton,
} from "./custom/CustomButtons";

import {
	BpmnPropertiesPanelModule,
	BpmnPropertiesProviderModule,
	ZeebePropertiesProviderModule,
	ElementTemplatesPropertiesProviderModule,
} from "bpmn-js-properties-panel";

import propertiesProviderModule from "bpmn-js-properties-panel/dist/zeebe";

import magicPropertiesProviderModule from "./provider/elmi";

import magicModdleDescriptor from "./descriptors/elmiAttr.json";

import diagramXML from "../resources/diagram.bpmn";

import customModule from "./custom";

import elmiExtension from "./descriptors/elmi.json";

import customelements_ from "./descriptors/customelements.json";
import { forEach } from "min-dash";
import { isPaste } from "diagram-js/lib/features/keyboard/KeyboardUtil";

//recupero il file json con i mattoncini da importare in base al tenant che mi sta chiamando(al momento controlliamo il dominio che chiama)
async function fetchApiData() {
	try {
		const currentDomain = window.location.hostname + ":" + window.location.port;
		let apiUrl;

		// Leggi il file tenantConfig.JSON
		let path = window.location.pathname;
		const tenantConfigResponse = await fetch(
			path + "/descriptors/tenantConfig.json"
		);
		if (!tenantConfigResponse.ok) {
			//throw new Error(`Errore HTTP: ${tenantConfigResponse.status}`);
			console.log(`Errore HTTP: ${tenantConfigResponse.status}`);
		}
		const tenantConfig = await tenantConfigResponse.json();

		// Crea un oggetto per mappare il dominio all'URL appropriato
		const domainUrlMap = {};
		for (const entry of tenantConfig.urls) {
			domainUrlMap[entry.domain] = entry.types_url;
		}

		// Ottieni l'URL corrispondente al dominio corrente
		apiUrl = domainUrlMap[currentDomain];
		apiUrl = path + apiUrl;

		const response = await fetch(apiUrl);

		if (!response.ok) {
			console.log(`Errore HTTP: ${response.status}`);
			//throw new Error(`Errore HTTP: ${response.status}`);
		}

		const data = await response.json();
		return data;
	} catch (error) {
		console.error(
			"Si è verificato un errore durante il recupero dei dati degli attributi:",
			error
		);
	}
}

// document.getElementById("process-name").innerHTML="NOME PROCESSO";
document.getElementById("process-version").innerHTML = "Versione: ";	
document.getElementById("process-revision").innerHTML = "Revisione: ";

//recupero il file json con gli attributi personalizzati da importare nei mattoncini custom in base al tenant che mi sta chiamando(al momento controlliamo il dominio che chiama)
async function fetchApiDataAttr() {
	try {
		const currentDomain = window.location.hostname + ":" + window.location.port;
		let apiUrl;

		// Leggi il file tenantConfig.JSON
		let path = window.location.pathname;
		const tenantConfigResponse = await fetch(
			path + "/descriptors/tenantConfig.json"
		);
		if (!tenantConfigResponse.ok) {
			console.log(`Errore HTTP: ${tenantConfigResponse.status}`);
			//throw new Error(`Errore HTTP: ${tenantConfigResponse.status}`);
		}
		const tenantConfig = await tenantConfigResponse.json();

		// Crea un oggetto per mappare il dominio all'URL appropriato
		const domainUrlMap = {};
		for (const entry of tenantConfig.urls) {
			domainUrlMap[entry.domain] = entry.AttrUrl;
		}

		// Ottieni l'URL corrispondente al dominio corrente
		apiUrl = domainUrlMap[currentDomain];

		apiUrl = path + apiUrl;
		const response = await fetch(apiUrl);

		if (!response.ok) {
			console.log(`Errore HTTP: ${response.status}`);
			//throw new Error(`Errore HTTP: ${response.status}`);
		}

		const data = await response.json();
		return data;
	} catch (error) {
		console.error(
			"Si è verificato un errore durante il recupero dei dati degli attributi:",
			error
		);
	}
}

(async function main() {
	//recupero mattonci custom e attributi custom
	const data = await fetchApiData();
	const dataAttr = await fetchApiDataAttr();

	const moddle = new BpmnModdle({ elmi: data });

	let myListener = (event) => {
		event.preventDefault();
		event.returnValue = "";
	};

	function recuperamattoncini() {
		let mypathbase = window.location.pathname;

		if (!mypathbase.endsWith("/")) {
			mypathbase += "/";
		}

		const apiUrl = mypathbase + "api/getPalette";
		return fetch(apiUrl, {
			method: "GET",
			headers: {
				Accept: "application/json",
			},
		})
			.then((response) => {

				if (!response.ok) {
					throw new Error("Network response was not ok");
				}
				return response.json();
			})
			.catch((error) => {
				console.error("There was a problem with the fetch operation:", error);
				return [];
			});
	}

	recuperamattoncini();

	// Aggiungi l'ascoltatore d'evento
	window.addEventListener("beforeunload", myListener);

	const containerEl = document.getElementById("container");
	const bottoni = document.getElementById("hint");
	const bottoniapi = document.getElementById("apibuttons");

	var gruppi = customelem;
	// Utilizza un oggetto Set per tracciare i gruppi già aggiunti
	var gruppiAggiunti = new Set();

	gruppi.forEach((element) => {
		// Converti il nome del gruppo in minuscolo per mantenere la consistenza
		var groupId = element.name.toLowerCase().replaceAll(" ", "");

		// Se il gruppo non è ancora stato aggiunto, procedi con l'aggiunta
		if (!gruppiAggiunti.has(groupId)) {
			$("#tabs_").append(
				"<div id='" +
					groupId +
					'\' class="tab disattivato" onclick="filter(\'' +
					groupId +
					"')\">" +
					element.name +
					"</div>"
			);
			// Aggiungi il gruppo all'insieme dei gruppi già aggiunti
			gruppiAggiunti.add(groupId);
		}
	});

	// create modeler
	const bpmnModeler = new BpmnModeler({
		container: containerEl,
		keyboard: {
			bindTo: document,
		},
		additionalModules: [
			resizeTask,
			BpmnColorPickerModule,
			minimapModule,
			customModule,
			BpmnPropertiesProviderModule,
			BpmnPropertiesPanelModule,
			ZeebePropertiesProviderModule,
			//ElementTemplatesPropertiesProviderModule,
			//magicPropertiesProviderModule
		],
		taskResizingEnabled: true,
		eventResizingEnabled: true,
		moddleExtensions: {
			zeebe: zeebeModdleExtensions,
			elmi: data,
			camunda: camundaModdlePackage,
			zeebe: zeebeModdle,
			//magic: dataAttr,
		},
		propertiesPanel: {
			parent: "#properties-panel-container",
		},
	});

	var commandStack = bpmnModeler.get("commandStack");

	let errorsByElement = {};
	bpmnModeler.on("import.done", function (event) {
		if (!event.error) {
			const elementRegistry = bpmnModeler.get("elementRegistry");
			const allElements = elementRegistry.getAll();
			verificaErroriEAggiornaUI(allElements);
			elementRegistry.forEach(function (element) {
				const businessObject = element.businessObject;
				if (businessObject.$type == "bpmn:Process") {
					const processName = businessObject.name;
					document.getElementById("process-name").innerHTML =
						processName || " ";
				}
	
				// Inizializza gli array per raccogliere i nomi delle variabili mancanti
				let missingRequiredInputs = [];
				let missingRequiredOutputs = [];
	
				businessObject.extensionElements?.values?.forEach(
					(extensionElement) => {
						// Controllo per inputParameters
						extensionElement.inputParameters?.forEach((inputParameter) => {
							const isRequired = inputParameter.$attrs["required"] === "true";
							const isPopulated =
								inputParameter.source && inputParameter.source.trim() !== "";
							if (isRequired && !isPopulated) {
								missingRequiredInputs.push(inputParameter.target || "Nome non specificato");
							}
						});
	
						// Controllo per outputParameters
						extensionElement.outputParameters?.forEach((outputParameter) => {
							const isRequired = outputParameter.$attrs["required"] === "true";
							const isPopulated =
								outputParameter.source && outputParameter.source.trim() !== "";
							if (isRequired && !isPopulated) {
								missingRequiredOutputs.push(outputParameter.target || "Nome non specificato");
							}
						});
					}
				);
	
				// Passa le variabili di input e output mancanti alla funzione addWarningOverlay
				if (missingRequiredInputs.length > 0 || missingRequiredOutputs.length > 0) {
					addWarningOverlay(element, missingRequiredInputs, missingRequiredOutputs);
				}
	
				verificaConflittiEAggiornaUI();
				verificaEAggiornaStatoWarnings();
			});
		} else {
			console.error("Errore durante l'importazione del diagramma:", event.error);
		}
	});
	
	

	bpmnModeler.on("elements.changed", function (event) {
		const elements = event.elements;
		const elementRegistry = bpmnModeler.get("elementRegistry");
	
		elements.forEach(function (element) {
			const existingElement = elementRegistry.get(element.id);
			if (!existingElement) {
				// Elemento non presente, esce dalla funzione per questo elemento
				return; // Continua con il prossimo elemento
			}
	
			// Reinizializza gli errori per l'elemento attuale
			errorsByElement[element.id] = {};
	
			let missingRequiredInputs = [];
			let missingRequiredOutputs = [];
			const businessObject = element.businessObject;
	
			// Controllo per inputParameters
			businessObject.extensionElements?.values?.forEach((extensionElement) => {
				extensionElement.inputParameters?.forEach((inputParameter) => {
					const isRequired =
						inputParameter.$attrs["zeebe:required"] === true ||
						inputParameter.$attrs["required"] === "true";
					const isPopulated =
						inputParameter.source && inputParameter.source.trim() !== "";
	
					if (isRequired && !isPopulated) {
						missingRequiredInputs.push(inputParameter.target);
						errorsByElement[element.id][inputParameter.target] = {
							elementName: businessObject.name || "<i>Task senza nome</i>",
							variableName: inputParameter.target,
							variableType: "input",
						};
					}
				});
	
				// Controllo per outputParameters
				extensionElement.outputParameters?.forEach((outputParameter) => {
					const isRequired =
						outputParameter.$attrs["zeebe:required"] === true ||
						outputParameter.$attrs["required"] === "true";
					const isPopulated =
						outputParameter.source && outputParameter.source.trim() !== "";
	
					if (isRequired && !isPopulated) {
						missingRequiredOutputs.push(outputParameter.target);
						errorsByElement[element.id][outputParameter.target] = {
							elementName: businessObject.name || "<i>Task senza nome</i>",
							variableName: outputParameter.target,
							variableType: "output",
						};
					}
				});
			});
	
			// Passa le variabili di input e output mancanti alla funzione addWarningOverlay
			if (missingRequiredInputs.length > 0 || missingRequiredOutputs.length > 0) {
				addWarningOverlay(element, missingRequiredInputs, missingRequiredOutputs);
			} else {
				removeWarningOverlay(element);
			}
	
			// Aggiorna il nome del processo nel DOM
			if (
				element.type === "bpmn:Process" ||
				(element.businessObject &&
					element.businessObject.$instanceOf("bpmn:Process"))
			) {
				const processName = element.businessObject.name;
				document.getElementById("process-name").innerHTML = processName || " ";
			}
		});
	
		// Aggiorna i contatori di errori e warnings
		aggiornaContatori();
	
		// Popola il pannello degli errori e warnings
		popolaErroriEWarning(flattenErrors(errorsByElement));
	
		// Verifica e aggiorna eventuali conflitti di output
		verificaConflittiEAggiornaUI();
	
		// Verifica e aggiorna lo stato dei warnings
		verificaEAggiornaStatoWarnings();
	});
	


	function verificaErroriEAggiornaUI(elements) {
		errorsByElement = {};
		elements.forEach(function (element) {
			// Reinizializza gli errori per l'elemento attuale
			errorsByElement[element.id] = {};
	
			let missingRequiredInputs = [];
			let missingRequiredOutputs = [];
			const businessObject = element.businessObject;
	
			// Controllo per inputParameters
			businessObject.extensionElements?.values?.forEach((extensionElement) => {
				extensionElement.inputParameters?.forEach((inputParameter) => {
					const isRequired =
						inputParameter.$attrs["zeebe:required"] === true ||
						inputParameter.$attrs["required"] === "true";
					const isPopulated =
						inputParameter.source && inputParameter.source.trim() !== "";
	
					if (isRequired && !isPopulated) {
						missingRequiredInputs.push(inputParameter.target);
						errorsByElement[element.id][inputParameter.target] = {
							elementName: businessObject.name || "Task senza nome",
							variableName: inputParameter.target,
							variableType: "input",
						};
					}
				});
	
				// Controllo per outputParameters
				extensionElement.outputParameters?.forEach((outputParameter) => {
					const isRequired =
						outputParameter.$attrs["zeebe:required"] === true ||
						outputParameter.$attrs["required"] === "true";
					const isPopulated =
						outputParameter.source && outputParameter.source.trim() !== "";
	
					if (isRequired && !isPopulated) {
						missingRequiredOutputs.push(outputParameter.target);
						errorsByElement[element.id][outputParameter.target] = {
							elementName: businessObject.name || "Task senza nome",
							variableName: outputParameter.target,
							variableType: "output",
						};
					}
				});
			});
	
			// Passa le variabili di input e output mancanti alla funzione addWarningOverlay
			if (missingRequiredInputs.length > 0 || missingRequiredOutputs.length > 0) {
				addWarningOverlay(element, missingRequiredInputs, missingRequiredOutputs);
			} else {
				removeWarningOverlay(element);
			}
		});
	
		aggiornaContatori();
		popolaErroriEWarning(flattenErrors(errorsByElement));
	}
	
	

	function flattenErrors(errorsByElement) {
		let flatErrors = [];
		for (let elementId in errorsByElement) {
			for (let variableName in errorsByElement[elementId]) {
				let error = errorsByElement[elementId][variableName];
				flatErrors.push({
					elementId: elementId,
					elementName: error.elementName || "<i>Task senza nome</i>",
					variableName: variableName,
					variableType: error.variableType,
				});
			}
		}
		return flatErrors;
	}

	function addWarningOverlay(element, missingInputVariables, missingOutputVariables) {
		const overlays = bpmnModeler.get("overlays");
		const overlayId = `warning-overlay-${element.id}`;
	
		// Rimuovi un overlay esistente per assicurarti che le informazioni siano aggiornate
		removeWarningOverlay(element);
	
		const overlayHtml = document.createElement("div");
		overlayHtml.className = "warning-overlay";
		overlayHtml.id = overlayId;
	
		overlayHtml.innerHTML =
			'<i class="fa fa-exclamation-circle" style="color: white;"></i>';
	
		// Costruisci i messaggi distinti per input e output
		let titleText = "";
	
		if (missingInputVariables.length > 0) {
			titleText +=
				"Le seguenti variabili di input non sono valorizzate: " +
				missingInputVariables.join(", ") + ".";
		}
	
		if (missingOutputVariables.length > 0) {
			if (titleText.length > 0) titleText += "\n"; // Aggiungi una nuova riga tra i messaggi se entrambi sono presenti
			titleText +=
				"Le seguenti variabili di output non sono valorizzate: " +
				missingOutputVariables.join(", ") + ".";
		}
	
		// Imposta il titolo con le variabili mancanti di input e output
		overlayHtml.title = titleText;
	
		overlays.add(element, {
			position: {
				top: -5,
				left: -5,
			},
			html: overlayHtml,
		});
	}
	

	function addConflictWarningOverlay(element, conflictVariables) {
		const overlays = bpmnModeler.get("overlays");
		const overlayId = `conflict-warning-overlay-${element.id}`;

		// Rimuovi un overlay esistente per assicurarti che le informazioni siano aggiornate
		//removeWarningOverlay(element);  // Assicurati che questa funzione rimuova tutti i tipi di overlays o crea una nuova funzione specifica per rimuovere gli overlay di conflitto

		const overlayHtml = document.createElement("div");
		overlayHtml.className = "warning-overlay2";
		overlayHtml.id = overlayId;

		overlayHtml.innerHTML = '<i class="fa fa-exclamation-triangle"></i>';

		// Unisci i nomi delle variabili in conflitto per mostrare nel titolo
		const titleText =
			"Conflitto: la variabile di output " +
			conflictVariables.join(", ") +
			" è condivisa da più elementi.";
		overlayHtml.title = titleText;

		overlays.add(element, {
			position: {
				bottom: 15,
				left: -3,
			},
			html: overlayHtml,
		});
	}

	function removeWarningOverlay(element) {
		const overlays = bpmnModeler.get("overlays");
		const overlayId = `warning-overlay-${element.id}`;

		const overlayElement = document.getElementById(overlayId);
		if (
			overlayElement &&
			overlayElement.className.includes("warning-overlay")
		) {
			overlayElement.parentNode.removeChild(overlayElement);
			overlays.remove({ element: element, html: overlayElement });
		}
	}

	bpmnModeler.on("import.done", function (event) {
		aggiornaContatori();
		// Nascondi il popup se presente
		let popup = document.querySelector(".helper-popup");
		if (popup) {
			popup.style.display = "none";
		}
	});

	bpmnModeler.on("shape.added", function (event) {
		let element = event.element;
		customelements_.forEach(function (category) {
			category.tasks.forEach(function (item) {
				if (element.businessObject.customId === item.id) {
					let businessObject = element.businessObject;
					businessObject.name = item.label;
					
					// Assicurati che l'elemento extensionElements esista
					if (!businessObject.extensionElements) {
						businessObject.extensionElements = bpmnModeler
							.get("moddle")
							.create("bpmn:ExtensionElements");
					}
	
					let inputs = item.inputs.map((input) => {
						let inputParameter = bpmnModeler.get("moddle").create("zeebe:Input", {
							source: input.defaultValue || "",
							target: input.name,
						});
	
						// Aggiungi l'attributo 'required' agli input
						inputParameter.$attrs["zeebe:required"] = input.required;
	
						return inputParameter;
					});
	
					// Aggiorna anche gli output per aggiungere 'required'
					let outputs = item.outputs.map((output) => {
						let outputParameter = bpmnModeler.get("moddle").create("zeebe:Output", {
							source: output.defaultValue || "",
							target: output.name,
						});
	
						// Aggiungi l'attributo 'required' agli output, se necessario
						outputParameter.$attrs["zeebe:required"] = output.required;
	
						return outputParameter;
					});
	
					// Crea l'IO mapping
					let ioMapping = bpmnModeler.get("moddle").create("zeebe:IoMapping", {
						outputParameters: outputs,
						inputParameters: inputs,
					});
	
					let taskDefinition = bpmnModeler
						.get("moddle")
						.create("zeebe:TaskDefinition", {
							type: "JobWorker",
							retries: item.jobWorker,
						});
	
					// Aggiungi zeebe:taskDefinition e ioMapping agli extensionElements
					businessObject.extensionElements
						.get("values")
						.push(taskDefinition, ioMapping);
				}
			});
		});
	});
	

	bpmnModeler
		.get("eventBus")
		.on("commandStack.shape.create.postExecuted", function (event) {
			const element = event.context.shape;
			const businessObject = element.businessObject;
			//aggiungicheck(element);
			// Assumiamo che `customelements_` sia il tuo array JSON caricato
			const item = customelements_.flatMap((element) => element.tasks)
			.find((task) => businessObject.customId === task.id);
		

			if (item) {
				const modeling = bpmnModeler.get("modeling");
				modeling.setColor([element], {
					stroke: item.colorStroke,
					fill: item.colorFill
				});
			}
		});

	bpmnModeler.on("selection.changed", function (e) {
		if (e.newSelection.length > 0) {
			const element = e.newSelection[0];
			const businessObject = element.businessObject;
			//aggiungicheck(element);
			aggiornaContatori();
			if (businessObject) {
				aggiornaScritteAiuto();
			}
		}
	});

	function aggiornaScritteAiuto() {
		var checkExist = setInterval(function () {
			var $inputTitles = $(
				'div[data-group-id="group-inputs"] .bio-properties-panel-collapsible-entry-header-title'
			);
			var $outputTitles = $(
				'div[data-group-id="group-outputs"] .bio-properties-panel-collapsible-entry-header-title'
			);
	
			if ($inputTitles.length || $outputTitles.length) {
				clearInterval(checkExist);
	
				var addCustomText = function ($titles, elementsArray) {
					$titles.each(function (index) {
						const currentTitle = $(this).text().trim().toLowerCase(); // Normalizza il titolo
	
						// Cerca la corrispondenza basandoti su label o nome
						const elementIndex = elementsArray.findIndex(
							(e) =>
								e.label?.trim().toLowerCase() === currentTitle ||
								e.name?.trim().toLowerCase() === currentTitle
						);
	
						if (elementIndex !== -1) {
							const descriptionObject = elementsArray[elementIndex];
	
							// Mostra sempre l'helper, anche se description è vuota
							var uniqueId = `${descriptionObject.name}-${index}`;
							var defaultValue = descriptionObject.defaultValue || "";
							var description = descriptionObject.description || "Nessuna descrizione disponibile";
							var name = descriptionObject.name || "";
	
							// Crea elemento "Aiuto"
							var $helpText = $(
								'<span title="Mostra un popup di aiuto per la seguente variabile" style="margin-left: 5px; font-size: smaller; color: #138ed3;text-decoration:underline;cursor:pointer;right:32px;position:absolute;">Aiuto</span>'
							);
	
							// Aggiungi l'evento per mostrare l'aiuto
							$helpText.on("click", function (event) {
								event.stopPropagation(); // Evita l'attivazione dell'apertura/chiusura dell'accordion
								// Chiamata alla tua funzione helper
								helper(name, defaultValue, description, uniqueId);
							});
	
							// Aggiungi l'helper accanto all'elemento solo se non è stato aggiunto prima
							if (!$(this).attr("data-custom-text-added")) {
								$(this).after($helpText);
								$(this).attr("data-custom-text-added", "true");
							}
						}
					});
				};
	
				// Funzione per ottenere inputs o outputs dai task
				const getElementsFromTasks = (type) => {
					return customelements_.flatMap((category) =>
						category.tasks.flatMap((task) =>
							task[type].map((element, index) => ({
								...element,
								Index: index,
							}))
						)
					);
				};
	
				// Usa getElementsFromTasks per inputs e outputs e aggiungi gli helper
				addCustomText($inputTitles, getElementsFromTasks("inputs"));
				addCustomText($outputTitles, getElementsFromTasks("outputs"));
			}
		}, 100); // Controlla ogni 100 millisecondi fino a quando gli elementi non sono disponibili
	}
	

	function aggiungicheck(element) {
		var checkExist = setInterval(function () {
			var $inputTitles = $(
				'div[data-group-id="group-inputs"] .bio-properties-panel-collapsible-entry-header-title'
			);

			if ($inputTitles.length) {
				clearInterval(checkExist);
				//qua l'indice è errato, fixare
				$inputTitles.each(function (index) {
					if (!$(this).attr("data-checkbox-added")) {
						// Trova la variabile di input corrispondente basata sull'indice
						const inputVar =
							element.businessObject.extensionElements.values.find(
								(val) => val.$type === "zeebe:IoMapping"
							)?.inputParameters[index];
						//console.log(inputVar?.$attrs);
						const isRequired =
							inputVar?.$attrs["zeebe:required"] === true ||
							inputVar?.$attrs["required"] == "true";
						const $label = $(
							'<label style="margin-left: 5px;">Required</label>'
						);
						const $checkbox = $(
							'<input type="checkbox" style="margin-left: 5px;" title="Rendi questa variabile obbligatoria">'
						).prop("checked", isRequired);
						$checkbox.on("click", function (event) {
							event.stopPropagation();
						});
						$checkbox.on("change", function () {
							const isChecked = $(this).is(":checked");
							// Assumi che "inputVar" sia la variabile corrente gestita da questa checkbox
							if (inputVar) {
								inputVar.$attrs["zeebe:required"] = isChecked;
								inputVar.$attrs["required"] = isChecked;
							}
							// Ora triggeriamo l'evento 'elements.changed' utilizzando modeling.updateProperties
							const modeling = bpmnModeler.get("modeling");
							modeling.updateProperties(element, {});
							//console.log("Cambiato stato required a:", isChecked, "per", inputVar);
						});

						$(this).after($checkbox).after($label);
						$(this).attr("data-checkbox-added", "true");
					}
				});
			}
		}, 100); // Controlla ogni 100 millisecondi fino a quando gli elementi non sono disponibili
	}

	function aggiornaContatori() {
		let errorCount = 0;
		let warningCount = conteggioConflittiOutput(); // Ottieni il conteggio dei warnings da conflitti di output
	
		const elementRegistry = bpmnModeler.get("elementRegistry");
		elementRegistry.forEach(function (element) {
			const businessObject = element.businessObject;
			
			// Controllo per inputParameters
			businessObject.extensionElements?.values?.forEach((extensionElement) => {
				extensionElement.inputParameters?.forEach((inputParameter) => {
					const isRequired =
						inputParameter.$attrs["zeebe:required"] === true ||
						inputParameter.$attrs["required"] === "true";
					const isPopulated =
						inputParameter.source && inputParameter.source.trim() !== "";
					if (isRequired && !isPopulated) {
						errorCount++;  // Aggiungi agli errori per input mancanti
					}
				});
	
				// Controllo per outputParameters
				extensionElement.outputParameters?.forEach((outputParameter) => {
					const isRequired =
						outputParameter.$attrs["zeebe:required"] === true ||
						outputParameter.$attrs["required"] === "true";
					const isPopulated =
						outputParameter.source && outputParameter.source.trim() !== "";
					if (isRequired && !isPopulated) {
						errorCount++;  // Aggiungi agli errori per output mancanti
					}
				});
			});
		});
	
		// Aggiorna i contatori visualizzati nell'UI
		document.getElementById("errorCount").textContent = errorCount;
		document.getElementById("errorscount").textContent = errorCount;
		document.getElementById("warningCount").textContent = warningCount;
		document.getElementById("warningscount").textContent = warningCount;
	}

	function conteggioConflittiOutput() {
		let conflittiOutput = {};
		const elementRegistry = bpmnModeler.get("elementRegistry");
		elementRegistry.forEach(function (element) {
			const businessObject = element.businessObject;
			businessObject.extensionElements?.values?.forEach((extensionElement) => {
				extensionElement.outputParameters?.forEach((outputParameter) => {
					const target = outputParameter.target;
					if (!conflittiOutput[target]) {
						conflittiOutput[target] = [];
					}
					conflittiOutput[target].push(element.id); // Salviamo gli ID degli elementi per evitare duplicati
				});
			});
		});

		// Contiamo solo i target con più di un elemento
		let warningCount = 0;
		Object.keys(conflittiOutput).forEach((target) => {
			if (conflittiOutput[target].length > 1) {
				warningCount += conflittiOutput[target].length;
			}
		});
		return warningCount;
	}

	document
		.getElementById("statusIndicators")
		.addEventListener("click", function () {
			document.getElementById("logPanel").style.display = "block";
			openTab(null, "Errors"); // Apre esplicitamente il tab "Errors"
		});

	document
		.getElementsByClassName("close")[0]
		.addEventListener("click", function () {
			document.getElementById("logPanel").style.display = "none";
		});

	function popolaErroriEWarning(errors, warnings = []) {
    const errorList = document.getElementById("errorList");
    const warningList = document.getElementById("warningList");

    // Resetta le liste
    errorList.innerHTML = "";
    warningList.innerHTML = "";

    // Filtra gli errori per variabili di input e output
    const inputErrors = errors.filter(error => error.variableType == "input");
    const outputErrors = errors.filter(error => error.variableType == "output");

    // Gestisci gli errori di input
    if (inputErrors.length === 0 && outputErrors.length === 0) {
        errorList.innerHTML = '<li class="listaerr">Nessun errore</li>';
    } else {
        if (inputErrors.length > 0) {
            const inputErrorHeader = document.createElement("li");
            inputErrorHeader.className = "listaerr-header";
            inputErrorHeader.innerHTML = `<b><h3>Errori nelle variabili di input:</h3></b>`;
            errorList.appendChild(inputErrorHeader);

            inputErrors.forEach(function (error) {
                const li = document.createElement("li");
                li.className = "listaerr";
                li.title = "Clicca per selezionare l'elemento";
                if (error.variableName === "undefined") {
                    error.variableName = "<i>Nessun nome variabile impostato</i>";
                }
                li.innerHTML = `Element(<b>${error.elementId})</b> - Errore nel task <i><b>'${error.elementName}'</b></i>: La variabile di input <i><u><b>'${error.variableName}'</b></i></u> è required ma non risulta popolata.`;
                li.setAttribute("data-element-id", error.elementId); // Imposta l'id dell'elemento come attributo
                li.addEventListener("click", function () {
                    selezionaElemento(this.getAttribute("data-element-id"));
                });
                errorList.appendChild(li);
            });
        }

        // Gestisci gli errori di output
        if (outputErrors.length > 0) {
            const outputErrorHeader = document.createElement("li");
            outputErrorHeader.className = "listaerr-header";
            outputErrorHeader.innerHTML = `<br><b><h3>Errori nelle variabili di output:</h3></b>`;
            errorList.appendChild(outputErrorHeader);

            outputErrors.forEach(function (error) {
                const li = document.createElement("li");
                li.className = "listaerr";
                li.title = "Clicca per selezionare l'elemento";
                if (error.variableName === "undefined") {
                    error.variableName = "<i>Nessun nome variabile impostato</i>";
                }
                li.innerHTML = `Element(<b>${error.elementId})</b> - Errore nel task <i><b>'${error.elementName}'</b></i>: La variabile di output <i><u><b>'${error.variableName}'</b></i></u> è required ma non risulta popolata.`;
                li.setAttribute("data-element-id", error.elementId); // Imposta l'id dell'elemento come attributo
                li.addEventListener("click", function () {
                    selezionaElemento(this.getAttribute("data-element-id"));
                });
                errorList.appendChild(li);
            });
        }
    }

    // Gestisci i warnings, se presenti
    if (warnings.length > 0) {
        warningList.innerHTML = ''; // Resetta la lista dei warning, se necessario
        warnings.forEach(function (warning) {
            const li = document.createElement("li");
            li.className = "listawarn";
            li.title = "Clicca per selezionare l'elemento";
            li.innerHTML = `Warning: ${warning}`;
            warningList.appendChild(li);
        });
    } else {
        warningList.innerHTML = '<li class="listawarn">Nessun warning</li>';
    }
}


	function selezionaElemento(elementId) {
		const elementRegistry = bpmnModeler.get("elementRegistry");
		const selection = bpmnModeler.get("selection");
		const canvas = bpmnModeler.get("canvas");

		const element = elementRegistry.get(elementId);
		if (element) {
			selection.select(element);
			// Calcola il centro dell'elemento
			const elementMid = {
				x: element.x + element.width / 2,
				// Modifica qui per alzare la posizione verticale
				y: element.y + element.height / 2 + 180, // Sposta la visuale più in alto rispetto al centro dell'elemento
			};
			// Ottieni il viewbox corrente
			const viewbox = canvas.viewbox();
			// Calcola il nuovo centro, tenendo conto della modifica
			const newCenter = {
				x: elementMid.x - viewbox.outer.width / 2,
				y: elementMid.y - viewbox.outer.height / 2,
			};
			// Imposta il viewbox per centrare l'elemento con la nuova posizione Y
			canvas.viewbox({
				x: newCenter.x,
				y: newCenter.y,
				width: viewbox.width,
				height: viewbox.height,
			});
		}
	}

	function raccogliVariabiliOutput() {
		let outputTargets = {};
		const elementRegistry = bpmnModeler.get("elementRegistry");
		elementRegistry.forEach(function (element) {
			const businessObject = element.businessObject;
			businessObject.extensionElements?.values?.forEach((extensionElement) => {
				extensionElement.outputParameters?.forEach((outputParameter) => {
					const target = outputParameter.target;
					if (!outputTargets[target]) {
						outputTargets[target] = [];
					}
					outputTargets[target].push(element);
				});
			});
		});
		return outputTargets;
	}

	function rimuoviTuttiGliOverlayDiConflitto() {
		const elementRegistry = bpmnModeler.get("elementRegistry");
		elementRegistry.forEach(function (element) {
			removeConflictWarningOverlay(element);
		});
	}

	function removeConflictWarningOverlay(element) {
		const overlays = bpmnModeler.get("overlays");
		const overlayId = `conflict-warning-overlay-${element.id}`;

		const overlayElement = document.getElementById(overlayId);
		if (
			overlayElement &&
			overlayElement.className.includes("warning-overlay2")
		) {
			// overlayElement.parentNode.removeChild(overlayElement);
			overlays.remove({ element: element, html: overlayElement });
		}
	}

	function verificaConflittiEAggiornaUI() {
		rimuoviTuttiGliOverlayDiConflitto();
		const outputTargets = raccogliVariabiliOutput();
		let warningCount = 0;
		const elementRegistry = bpmnModeler.get("elementRegistry");
		const allElements = elementRegistry.getAll();
		verificaErroriEAggiornaUI(allElements);

		let elementConflicts = new Map(); // Nuova mappa per tenere traccia dei conflitti per elemento

		Object.keys(outputTargets).forEach((target) => {
			if (outputTargets[target].length > 1) {
				// Se la variabile di output è condivisa da più di un elemento
				outputTargets[target].forEach((element) => {
					if (!elementConflicts.has(element)) {
						elementConflicts.set(element, []);
					}
					elementConflicts.get(element).push(target); // Aggiungi il target al set di conflitti per l'elemento
				});
			}
		});

		elementConflicts.forEach((conflicts, element) => {
			addConflictWarningOverlay(element, conflicts); // Passa tutti i conflitti trovati
			conflicts.forEach((conflict) => {
				aggiornaLogWarnings(element, conflict); // Aggiorna il pannello di log per ogni conflitto
			});
			warningCount += conflicts.length;
		});

		verificaEAggiornaStatoWarnings(warningCount);
	}

	function aggiornaLogWarnings(element, conflictVariables) {
		const warningList = document.getElementById("warningList");

		// Assicurati che conflictVariables sia sempre un array
		conflictVariables = Array.isArray(conflictVariables)
			? conflictVariables
			: [conflictVariables];

		// Rimuovi il messaggio "Nessun warning" se presente
		if (
			warningList.children.length === 1 &&
			warningList.children[0].innerText === "Nessun warning"
		) {
			warningList.innerHTML = "";
		}

		conflictVariables.forEach((variableName) => {
			const li = document.createElement("li");
			li.className = "listaerr listawarn";
			li.innerHTML = `Element(<b>${element.id})</b> - Warning nel task <i><b>'${
				element.businessObject.name || "Task senza nome"
			}'</b></i>: Conflitto sulla variabile di output <i><u><b>'${variableName}'</b></i></u> usata da i seguenti task.`;

			const ul = document.createElement("ul");
			ul.style.paddingLeft = "20px";
			ul.style.listStyleType = "circle";

			const elementsUsingVariable = findElementsUsingVariable(variableName);
			elementsUsingVariable.forEach((el) => {
				const subLi = document.createElement("li");
				var nometask = el.businessObject.name || "Task senza nome";
				subLi.innerHTML = `Element ID: <b>${el.id}</b>, Task Name: <i><b>${nometask}</b></i>`;
				subLi.className = "perhover";
				subLi.style.cursor = "pointer";
				subLi.title = "Clicca per selezionare l'elemento";
				subLi.onclick = () => {
					selezionaElemento(el.id);
				};
				ul.appendChild(subLi);
			});

			li.appendChild(ul);
			warningList.appendChild(li);
		});
	}

	function findElementsUsingVariable(variableName) {
		const elementRegistry = bpmnModeler.get("elementRegistry");
		let elementsUsingVariable = [];
		elementRegistry.forEach(function (element) {
			// Assumi che l'elemento possa avere un campo che indica le variabili di output
			if (element.businessObject && element.businessObject.extensionElements) {
				element.businessObject.extensionElements.values.forEach((ext) => {
					if (
						ext.outputParameters &&
						ext.outputParameters.some((param) => param.target === variableName)
					) {
						elementsUsingVariable.push(element);
					}
				});
			}
		});
		return elementsUsingVariable;
	}

	function verificaEAggiornaStatoWarnings() {
		const warningList = document.getElementById("warningList");
		if (warningList.children.length === 0) {
			const li = document.createElement("li");
			li.className = "listaerr"; // Usa la stessa classe degli errori se vuoi lo stesso stile
			li.innerHTML = `Nessun warning`;
			warningList.appendChild(li);
		}
	}

	const eventBus = bpmnModeler.get("eventBus");

	eventBus.on("copyPaste.elementsCopied", (event) => {
		let elementsArray;

		// Controlla e assegna gli elementi copiati in base alla presenza della chiave nell'oggetto evento
		if (event.tree && event.tree["0"]) {
			elementsArray = event.tree["0"];
		} else if (Array.isArray(event.elements)) {
			// Assumi che gli elementi siano direttamente sotto una chiave 'elements' se 'tree' non esiste
			elementsArray = event.elements;
		} else {
			console.log("La struttura dei dati copiati non è come previsto.");
			return;
		}

		if (Array.isArray(elementsArray)) {
			const elementIds = elementsArray.map((copiedElement) => copiedElement.id);
			// Memorizza gli ID degli elementi nel localStorage o in una variabile
			localStorage.setItem("copiedElementIds", JSON.stringify(elementIds));
			showToast();
		} else {
			console.log(
				"Gli elementi copiati non sono in formato array come previsto."
			);
		}
	});

	function showToast() {
		var toast = document.getElementById("toast");
		toast.className = "toast show";
		setTimeout(function () {
			toast.className = toast.className.replace("show", "");
		}, 3000);
	}

	const keyboard = bpmnModeler.get("keyboard");
	keyboard.addListener(2000, (event) => {
		const { keyEvent } = event;

		if (!isPaste(keyEvent)) {
			return;
		}

		event.preventDefault(); // Impedisce l'azione di incolla predefinita
		event.stopPropagation();
		// Recupera gli ID degli elementi originali copiati
		const originalElementIds = JSON.parse(
			localStorage.getItem("copiedElementIds") || "[]"
		);

		const elementRegistry = bpmnModeler.get("elementRegistry");
		const modeling = bpmnModeler.get("modeling");

		originalElementIds.forEach((id) => {
			const originalElement = elementRegistry.get(id);
			if (originalElement) {
				// Crea una copia dell'elemento originale (omesso: clonazione e assegnazione di un nuovo ID)
				// Supponendo che hai una funzione cloneElement che clona l'elemento e assegna un nuovo ID
				const clonedElement = cloneElement(originalElement);
				// Aggiungi l'elemento clonato al modello (specificare la posizione, se necessario)
				//modeling.createShape(clonedElement, { x: clonedElement.x + 10, y: clonedElement.y + 10 }, originalElement.parent);
			}
		});

		// Dopo l'incolla, potresti voler pulire o aggiornare il localStorage
	});

	function cloneElement(element) {
		if (element.type === "label") {
			return;
		}
	
		const moddle = bpmnModeler.get("moddle");
		const modeling = bpmnModeler.get("modeling");
	
		// Genera un nuovo ID univoco per l'elemento clonato
		const newId =
			element.id + "_copy_" + Math.random().toString(36).substr(2, 9);
	
		// Clona il businessObject con un nuovo ID e altri attributi necessari
		const newBusinessObject = moddle.create(element.businessObject.$type, {
			...element.businessObject,
			id: newId,
		});
	
		// Aggiungi il nome solo se presente nell'elemento originale
		if (element.businessObject.name) {
			newBusinessObject.name = element.businessObject.name + " (Copy)";
		}
	
		// Clona extensionElements, inclusi inputParameters e outputParameters con 'required'
		if (element.businessObject.extensionElements) {
			const newExtensionElements = moddle.create("bpmn:ExtensionElements");
			newBusinessObject.extensionElements = newExtensionElements;
	
			element.businessObject.extensionElements.values.forEach((ext) => {
				if (ext.$type === "zeebe:IoMapping") {
					const newIoMapping = moddle.create("zeebe:IoMapping", {
						inputParameters: ext.inputParameters.map((input) => {
							const newInput = moddle.create("zeebe:Input", {
								source: input.source,
								target: input.target,
							});
							// Clona l'attributo 'required' sugli input
							if (
								input.$attrs["zeebe:required"] === true ||
								input.$attrs["required"] === "true"
							) {
								newInput.$attrs["zeebe:required"] = true;
							}
							else{
								newInput.$attrs["zeebe:required"] = false;
							}
							return newInput;
						}),
						outputParameters: ext.outputParameters.map((output) => {
							const newOutput = moddle.create("zeebe:Output", {
								source: output.source,
								target: output.target,
							});
							// Clona l'attributo 'required' sugli output
							if (
								output.$attrs["zeebe:required"] === true ||
								output.$attrs["required"] === "true"
							) {
								newOutput.$attrs["zeebe:required"] = true;
							}
							else{
								newOutput.$attrs["zeebe:required"] = false;
							}
							return newOutput;
						}),
					});
					newExtensionElements.get("values").push(newIoMapping);
				} else if (ext.$type === "zeebe:TaskDefinition") {
					const newTaskDefinition = moddle.create("zeebe:TaskDefinition", {
						type: ext.type,
						retries: ext.retries,
					});
					newExtensionElements.get("values").push(newTaskDefinition);
				} else if (ext.$type === "zeebe:TaskHeaders") {
					const newTaskHeaders = moddle.create("zeebe:TaskHeaders", {});
					if (Array.isArray(ext.values)) {
						newTaskHeaders.values = ext.values.map((header) =>
							moddle.create("zeebe:Header", {
								key: header.key,
								value: header.value,
							})
						);
					}
					newExtensionElements.get("values").push(newTaskHeaders);
				} else if (ext.$type === "zeebe:Properties") {
					const newProperties = moddle.create("zeebe:Properties", {});
					newProperties.properties = ext.properties.map((property) =>
						moddle.create("zeebe:Property", {
							name: property.name,
							value: property.value,
						})
					);
					newExtensionElements.get("values").push(newProperties);
				}
			});
		}
	
		const parent = element.parent || element.businessObject.$parent;
		let newElement;
	
		if (element.waypoints) {
			// Gestione delle connessioni
			const waypoints = element.waypoints.map((wp) => ({
				x: wp.x + 200,
				y: wp.y + 40,
			})); // Sposta anche i waypoints se necessario
			newElement = modeling.createConnection(
				element.source,
				element.target,
				{
					type: element.type,
					businessObject: newBusinessObject,
					waypoints: waypoints,
				},
				parent
			);
		} else {
			// Gestione delle forme
			const position = { x: element.x + 200, y: element.y + 40 }; // Modifica la posizione se necessario
			newElement = modeling.createShape(
				{ type: element.type, businessObject: newBusinessObject },
				position,
				parent
			);
		}
	
		// Assicurati che gli attributi visivi siano copiati se necessario
		if (element.di) {
			newElement.di.fill = element.di.fill;
			newElement.di.stroke = element.di.stroke;
		}
		newElement.height = element.height;
		newElement.width = element.width;
		newElement.businessObject.customId = element.businessObject.customId;
		modeling.updateProperties(newElement, {});
	
		return newElement;
	}
	

	// Ascolta l'evento 'commandStack.elements.delete.preExecute', che viene emesso prima che gli elementi vengano effettivamente rimossi
	eventBus.on("commandStack.elements.delete.preExecute", function (event) {
		const elements = event.context.elements;
		elements.forEach(function (element) {
			// Verifica se ci sono errori registrati per l'elemento rimosso e, in caso affermativo, eliminali
			if (errorsByElement[element.id]) {
				delete errorsByElement[element.id];

				// Dopo aver rimosso gli errori per l'elemento, aggiorna l'UI per riflettere i cambiamenti
				popolaErroriEWarning(flattenErrors(errorsByElement));
				verificaConflittiEAggiornaUI();
				verificaEAggiornaStatoWarnings();
				aggiornaContatori();
			}
		});
	});

	const canvasEl = containerEl.querySelector(".djs-overlay-container");
	const canvasEl2 = containerEl.querySelector(".djs-tooltip-container");

	//import dei pulsanti custom per la gestione dell'XML
	const saveDiagramButton = new SaveDiagramButton(bpmnModeler, bottoni);

	// const publishDiagramButton = new PublishDiagramButton(bpmnModeler, bottoniapi);

	const uploadXmlButton = new UploadXmlButton(bpmnModeler, bottoni);

	const downloadXmlButton = new DownloadXmlButton(bpmnModeler, bottoni);

	const editXMLbutton = new EditXMLButton(bpmnModeler, bottoni);

	const downloadSVGButton = new DownloadSVGButton(bpmnModeler, bottoni);

	const zoombutton = new ZoomButtons(
		bpmnModeler,
		containerEl,
		canvasEl,
		canvasEl2
	);

	const style = document.createElement("style");
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
		var id = firstEntry[1]; // Il valore del parametro è il secondo elemento della coppia chiave/valore

		// Utilizza l'ID nella chiamata API
		let mypathbase = window.location.pathname;

		if (!mypathbase.endsWith("/")) {
			mypathbase += "/";
		}

		fetch(mypathbase + `api/getDiagram/${id}`)
			.then((response) => {
				if (!response.ok) {
					const Toast = Swal.mixin({
						toast: true,
						position: "top-end",
						showConfirmButton: false,
						timer: 4000,
						timerProgressBar: true,
						didOpen: (toast) => {
							toast.addEventListener("mouseenter", Swal.stopTimer);
							toast.addEventListener("mouseleave", Swal.resumeTimer);
						},
					});
					Swal.fire({
						title: "Errore!",
						html: "Errore nell'apertura del diagramma BPMN, provare a ricaricare la pagina",
						icon: "error",
						showCancelButton: false,
						confirmButtonText: "Ok, ricarica",
						confirmButtonColor: "red",
						allowOutsideClick: false,
					}).then((result) => {
						if (result.isConfirmed) {
							window.removeEventListener("beforeunload", myListener);
							location.reload();
						}
					});

					throw new Error("Errore HTTP: " + " " + response.statusText);
				}
				return response.json();
			})
			.then((data) => {
				document.getElementById("process-name").innerHTML = data;
				document.getElementById("process-version").innerHTML = "Versione: <b style='color:black'>" + data.versionNumber +"</b>";	
				document.getElementById("process-revision").innerHTML = "Revisione: <b style='color:black'>" + data.revisionNumber +"</b>";
				// Utilizza i dati dell'API
				//var content = data.content.replace(/\\n/g, "\n").replace(/\\"/g, '"');
				bpmnModeler
					.importXML(data.content)
					.then(() => {})
					.catch((err) => {
						//console.error(err);
						const Toast = Swal.mixin({
							toast: true,
							position: "top-end",
							showConfirmButton: false,
							timer: 4000,
							timerProgressBar: true,
							didOpen: (toast) => {
								toast.addEventListener("mouseenter", Swal.stopTimer);
								toast.addEventListener("mouseleave", Swal.resumeTimer);
							},
						});
						Swal.fire({
							title: "Errore!",
							html: "Errore nell'apertura del diagramma BPMN, provare a ricaricare la pagina",
							icon: "error",
							showCancelButton: false,
							confirmButtonText: "Ok, ricarica",
							confirmButtonColor: "red",
							allowOutsideClick: false,
						}).then((result) => {
							if (result.isConfirmed) {
								window.removeEventListener("beforeunload", myListener);
								location.reload();
							}
						});
					});
				const Toast = Swal.mixin({
					toast: true,
					position: "top-end",
					showConfirmButton: false,
					timer: 4000,
					timerProgressBar: true,
					didOpen: (toast) => {
						toast.addEventListener("mouseenter", Swal.stopTimer);
						toast.addEventListener("mouseleave", Swal.resumeTimer);
					},
				});
				Toast.fire({
					icon: "success",
					title: "Diagramma BPMN aperto correttamente",
				});
			})
			.catch((error) => {
				// Gestisci eventuali errori che si verificano durante la chiamata all'endpoint
				//console.error('Si è verificato un errore durante la chiamata all\'endpoint:', error);
				const Toast = Swal.mixin({
					toast: true,
					position: "top-end",
					showConfirmButton: false,
					timer: 4000,
					timerProgressBar: true,
					didOpen: (toast) => {
						toast.addEventListener("mouseenter", Swal.stopTimer);
						toast.addEventListener("mouseleave", Swal.resumeTimer);
					},
				});
				Swal.fire({
					title: "Errore!",
					html: "Errore nell'apertura del diagramma BPMN, provare a ricaricare la pagina",
					icon: "error",
					showCancelButton: false,
					confirmButtonText: "Ok, ricarica",
					confirmButtonColor: "red",
					allowOutsideClick: false,
				}).then((result) => {
					if (result.isConfirmed) {
						window.removeEventListener("beforeunload", myListener);
						location.reload();
					}
				});
			});
	} else {
		// import XML base
		bpmnModeler
			.importXML(diagramXML)
			.then(() => {})
			.catch((err) => {
				const Toast = Swal.mixin({
					toast: true,
					position: "top-end",
					showConfirmButton: false,
					timer: 4000,
					timerProgressBar: true,
					didOpen: (toast) => {
						toast.addEventListener("mouseenter", Swal.stopTimer);
						toast.addEventListener("mouseleave", Swal.resumeTimer);
					},
				});
				Swal.fire({
					title: "Errore!",
					html: "Errore nell'apertura del diagramma BPMN, provare a ricaricare la pagina",
					icon: "error",
					showCancelButton: false,
					confirmButtonColor: "red",
				});
			});
		const Toast = Swal.mixin({
			toast: true,
			position: "top-end",
			showConfirmButton: false,
			timer: 4000,
			timerProgressBar: true,
			didOpen: (toast) => {
				toast.addEventListener("mouseenter", Swal.stopTimer);
				toast.addEventListener("mouseleave", Swal.resumeTimer);
			},
		});
		Toast.fire({
			icon: "success",
			title:
				"Nessun parametro di query fornito. Import del diagramma BPMN di base",
		});
	}

	//gestisco il drag&drop dei file per mostrarli nell'editor
	function registerFileDrop(containerEl, callback) {
		function handleFileSelect(e) {
			e.stopPropagation();
			e.preventDefault();

			var files = e.dataTransfer.files;

			var file = files[0];

			var reader = new FileReader();

			reader.onload = function (e) {
				var xml = e.target.result;

				callback(xml);
			};

			reader.readAsText(file);
		}

		function handleDragOver(e) {
			e.stopPropagation();
			e.preventDefault();

			e.dataTransfer.dropEffect = "copy";
		}

		containerEl.addEventListener("dragover", handleDragOver, false);
		containerEl.addEventListener("drop", handleFileSelect, false);
	}

	async function openDiagram(xml) {
		try {
			await bpmnModeler.importXML(xml);
		} catch (err) {
			alert(
				"Errore durante l'apertura del file xml, assicurati che il file sia formattato correttamente " +
					err
			);
		}
	}

	////// file drag / drop ///////////////////////
	if (!window.FileList || !window.FileReader) {
		console.log("Drag&Drop non supportato dal browser!");
	} else {
		registerFileDrop(containerEl, openDiagram);
	}

	$(document).ready(function () {
		// Creare un'istanza di MutationObserver
		var observer = new MutationObserver(function (mutations) {
			mutations.forEach(function (mutation) {
				// Se la lista delle classi muta...
				if (mutation.attributeName === "class") {
					var classList = $(mutation.target).prop(mutation.attributeName);
					// ...e contiene la classe "swal2-height-auto", allora rimuovila
					if (classList.includes("swal2-height-auto")) {
						$(mutation.target).removeClass("swal2-height-auto");
					}
				}
			});
		});

		// Inizia ad osservare il target con le configurazioni specificate
		observer.observe(document.body, {
			attributes: true, // osserva solo le modifiche degli attributi
		});
		filter("standardbpmn");
	});
})();
