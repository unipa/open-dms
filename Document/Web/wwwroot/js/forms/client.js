// FORM 

import { dataEntry } from "/components/data-entry/data-entry.js";

var Model = {};
var queries = new Object();
var names = new Object();
var dataTransfer = new Object();

export function renderFileUploads() {
    var attachments = $("INPUT[type='file']");
    attachments.each((i, el) => {
        _showFileUpload(el);
    });
    _renderSelectFiles();
}

function _showFileUpload(attachment) {

    var variableName = "#" + attachment.id;
    $(variableName).addClass("hidden");
    var containerId = $(variableName).attr("container-id");
    if (containerId) {
        var $container = $(containerId);
        var $files = $container.find("[fid]");
        $files.remove();
        $container.addClass("hidden");
        $container.find(".file").addClass("hidden");
    }
    attachment.onchange = () => {
        attachment.files.forEach((e, i) => {
            e.status = "";
        });
        _renderSelectFiles();
    }

    $(variableName + "_addButton").off("click").on("click", () => {
        attachment.click();
    });
}
// Crea il componente che permette di selezionare file dal pc

function UpdateFileControl(attachment) {
    var variableName = "#" + attachment.id;
    var attachmentId = "#" + attachment.id + "_id";
    var containerId = $(variableName).attr("container-id");

    if ($(attachmentId).val() || $(variableName)[0].files.length > 0) {
        // file presente
        $(variableName).addClass("hidden");
    }
    else {
        // file assente
        $(variableName).removeClass("hidden");
    }

    if (containerId) {
        var $container = $(containerId);
        var $files = $container.find("[fid]");
        $files.remove();
        $container.addClass("hidden");
        $container.find(".file").addClass("hidden");
    }
    attachment.onchange = () => {
        attachment.files.forEach((e, i) => {
            e.status = "";
        });
        _renderSelectFiles();
    }

    $(variableName + "_addButton").off("click").on("click", () => {
        attachment.click();
    });
}

// Renderizza un form su un container nel quale applica un model
export function RenderForm(form, container, model) {
    console.log("Start Rendering: " + new Date());
    if (!form)
        form = {
            html: "", schema: "", data: "", template: "", renderer: undefined
        };
    if (!model)
        model = form;

    Model.Form = form;
    Model.Attachments = [];
    Model.Data = model;
    Model.Container = container;
    // oggetto usato per renderizzare l'intero form (es. nel caso dei task è l'oggetto che contiene taskiteminfo

    try {
        if (Model.Form) {
            if (Model.Form.data)
                Model.Form.data = JSON.parse(Model.Form.data)
            else
                Model.Form.data = {};
        }
    } catch (ex) {
        Model.Form.html = ex.message;
        Model.Form.formType == "HTML"
    }

    if (Model.Form.formType == "FORMJS") {
        _renderFormJS();
    };
    if (Model.Form.formType == "HTML") {
        _renderHTMLForm();
    }
    console.log("Stop Rendering: " + new Date());
}
export function DisableFormComponents() {
    var form = $("#htmlform");
    var formhtml = form.length > 0 ? form[0] : undefined;
    if (!formhtml) return;

    $(formhtml).addClass("FormPreview")
    $(formhtml).find("input,select,textarea,a,button,radio,checkbox,iframe").attr("disabled", true);
    $(formhtml).find("input,select,textarea,a,button,radio,checkbox").off("change");
    tinymce.get().forEach(e => { e.mode.set("readonly") });
}

export function EnableFormComponents() {
    var form = $("#htmlform");
    var formhtml = form.length > 0 ? form[0] : undefined;
    if (!formhtml) return;
    $(formhtml).removeClass("FormPreview")
    $(formhtml).find("input,select,textarea,a,button,radio,checkbox").removeAttr("disabled");
}
export function Save(ApiCallBack, Draft) {

    //01. Validate Controls
    Model.Errors = "";
    var ok = Draft || _internalValidateForm();
    if (Model.Errors) {
        alert("Si sono verificati i seguenti errori e per proseguire e' necessario correggerli:\n\n" + Model.Errors);
        return false;
    } else
        if (!ok) return false;

    // 1. Cerco campi associati ad allegati
    // 2. Leggo i campi "input[file]" non associati ad un Id definito
    // 3. Eseguo upload e recuper Id
    // 4. Aggiorno Id e FileHash degli allegati
    // 5. Recupero l'elenco degli Id da utilizzare come allegati nel documento master
    Model.Attachments = [];
    PrepareForUpload();
    ShowUploadPanel();
    CheckFilesToUpload(ApiCallBack);
    return false;
}


// Torna il numero di allegati ancora da caricare
// Se tutti i campi "FILE" sono associati ad un Id allora ho caricato tutto
// I campi senza Id sono in Upload o in Errore
function CountFilesToUpload() {
    var n = 0;
    var files = $("INPUT[type='file']");
    if (files.length > 0) {
        files.each((i, attachment) => {
            let attachmentId = $("#" + attachment.id + "_id").val();
            if (!attachmentId) {
                n += 1;
            }
        });
    }
    return n;
}
function GetAttachmentsId() {
    var attachments = [];
    var files = $("INPUT[type='file']");
    if (files.length > 0) {
        files.each((i, attachment) => {
            let attachmentId = $("#" + attachment.id + "_id").val();
            if (attachmentId) {
                attachments.push(parseInt(attachmentId));
            }
        });
    }
    return attachments;
}
function GetUploadError() {
    var errors = "";
    var files = $("INPUT[type='file']");
    if (files.length > 0) {

        files.each((i, attachment) => {
            let attachmentId = $("#" + attachment.id + "_id").val();
            if (!attachmentId) {
                attachment.files.forEach((e, i) => {
                    if (e.status == "error") {
                        errors += e.error + "\n";
                    }
                });
            }
        });
    }
    return errors;
}
function PrepareForUpload() {
    var errors = "";
    var files = $("INPUT[type='file']");
    if (files.length > 0) {

        files.each((i, attachment) => {
            var attachmentId = $("#" + attachment.id + "_id").val();
            attachment.files.forEach((e, i) => {
                if (!attachmentId) {
                    e.status = undefined;
                }
                else {
                    e.status = "done";
                }
            });

        });
    }
}
function ShowUploadPanel() {
    var panel = $("<div class='loader' style='background-color:rgba(5,65,185,.85);position:absolute;top:0;left:0;right:0;bottom:0;z-index:1;padding:30% 10%;text-align:center;color:#fff' />")
    var spinner = "<div style='font-size:15em'><i class='fa fa-spinner fa-spin'></i></div>";
    panel.append(spinner);
    var msg = $("<h1>Salvataggio dati in corso...</h1>")
    panel.append(msg);
    $(document.body).append(panel);
    NumberOfUploads = 0;
}
function ShowPDFPanel() {
    var panel = $("<div class='loader' style='background-color:rgba(5,65,185,.85);position:absolute;top:0;left:0;right:0;bottom:0;z-index:1;padding:30% 10%;text-align:center;color:#fff' />")
    var spinner = "<div style='font-size:15em'><i class='fa fa-spinner fa-spin'></i></div>";
    panel.append(spinner);
    var msg = $("<h1>Generaizone PDF in corso...</h1>")
    panel.append(msg);
    $(document.body).append(panel);
    NumberOfUploads = 0;
}


function HideUploadPanel() {
    $(".loader").remove();
}

var NumberOfUploads = 0;
var NumberOfParallelsUploads = 3;
// Avvio l'upload parallelo di più files
function CheckFilesToUpload(ApiCallBack) {
    var files = $("INPUT[type='file']");
    if (files.length > 0) {
        files.each((i, attachment) => {
            let attachmentId = $("#" + attachment.id + "_id").val();
            if (!attachmentId) {
                attachment.files.forEach((e, i) => {
                    if (!e.status) {
                        //alert("carico: " + attachment.id);
                        if (NumberOfUploads < NumberOfParallelsUploads) {
                            e.status = "starting";
                            var variableName = attachment.id;
                            var file = e;
                            NumberOfUploads++;
                            console.log("START:" + variableName + " - " + NumberOfUploads);
                            UploadSingleFile(variableName, file);
                        }
                    }
                });
            }
        });
    }
    var ErrorMessage = GetUploadError();
    if (ErrorMessage) {
        HideUploadPanel();
        alert("Si sono verificati i seguenti errori durante il caricamento degli allegati:\n" + ErrorMessage + "\n\nRipetere il caricamento o rimuovere i file in errore");
    }
    else {
        if (NumberOfUploads > 0)
            window.setTimeout(() => { CheckFilesToUpload(ApiCallBack); }, 250);
        else {
            _callFormSave(ApiCallBack);
        }
    }
    return false;
}
function UploadSingleFile(variableName, file, ApiCallBack) {
    file.status = "reading";
    _updateStatus(variableName, file, "reading");
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
    }
    // masterId è l'id del documento di riferimento
    var masterId = 0;
    // Procedo solo se non sto già scaricando il file
    const reader = new FileReader();
    reader.onload = (data) => {
        _updateStatus(variableName, file, "preparing");
        var filedata = reader.result;
        var b64 = filedata.substr(filedata.indexOf(',') + 1);
        var amodel = $("#" + variableName).attr("attachment");
        if (!amodel)
            amodel = variableName + "-model";
        const attachmentModel = amodel ? GetModel(amodel) : {};
        _updateStatus(variableName, file, "uploading");
        try {
            fetch("/internalapi/action/UploadFile",
                {
                    method: 'post',
                    headers: {
                        RequestVerificationToken: VerificationCode,
                        'Content-Type': 'application/json;',
                        Accept: 'application/json;charset=ISO-8859-1'
                    },
                    body: JSON.stringify({
                        documentId: 0,
                        folderId: masterId,
                        filename: file.name,
                        data: b64,
                        document: attachmentModel
                    })
                })
                .then(response => {
                    if (response.ok)
                        return response.json();
                    else
                        return response.text().then(text => { throw new Error(text) });
                })
                .then(data => {
                    if (data.error) {
                        _updateStatus(variableName, file, "error", data.error);
                        NumberOfUploads = 0;
                    }
                    else {
                        if (data.documentId) {
                            // Memorizzo l'id del documento su una variabile specifica
                            var afield = $("#" + variableName + "_id");

                            if (afield.length > 0) {
                                setField(afield[0], data.documentId);
                            }
                            var afield = $("#" + variableName + "_hash");
                            if (afield.length > 0)
                                setField(afield[0], data.image.hash);
                            _updateStatus(variableName, file, "done");
                            NumberOfUploads--;
                        } else {
                            _updateStatus(variableName, file, "error", "Errore durante l'upload");
                            NumberOfUploads = 0;
                        }
                    }
                })
                .catch(err => {
                    _updateStatus(variableName, file, "error", err);
                    NumberOfUploads = 0;
                });
        } catch (ex) {
            _updateStatus(variableName, file, "error", "EXCEPTION: " + ex);
            NumberOfUploads = 0;
        }
    };
    var raised = false;
    reader.onerror = (data) => {
        NumberOfUploads = 0;
        raised = true;
        _updateStatus(variableName, file, "error", data);
    };
    try {
        reader.readAsDataURL(file);
    } catch (err) {
        if (raised) return;
        NumberOfUploads = 0;
        _updateStatus(variableName, file, "error", err);

    }
}

export function GetFormPreview() {

    var form = $("#htmlform");
    var formhtml = form.length > 0 ? form[0] : undefined;
    if (!formhtml) return;

    var $print = $(".print");
    $print.addClass("printRemoved").removeClass("print");

    var $btn = $(".btn:visible");
    $btn.addClass("removedForPreview hidden");



    var $form = $(formhtml);
    $form.find("select").each(function (i, e) {
        var v = $(e).val();
        $(e).attr('value', v);
        $(e).find("option[value='" + v + "']").attr('selected', "selected");
    });
    $form.find("textarea").each(function () {
        $(this).html($(this).val());
    });
    $form.find("input[type='text']").each(function () {
        $(this).attr('value', $(this).val());
    });
    $form.find("input[type='date']").each(function () {
        $(this).attr('value', $(this).val());
    });
    $form.find("input[type='number']").each(function () {
        $(this).attr('value', $(this).val());
    });
    $form.find("input[type='time']").each(function () {
        $(this).attr('value', $(this).val());
    });
    $form.find("input[type='tel']").each(function () {
        $(this).attr('value', $(this).val());
    });
    $form.find("input[type='email']").each(function () {
        $(this).attr('value', $(this).val());
    });
    $form.find("input[type='file']").each(function () {
        $(this).hide();
    });
    $form.find("input[type='checkbox']").each(function (i, e) {
        if ($(e)[0].checked)
            $(e).attr('checked', 'checked');
        else
            $(e).removeAttr('checked');
    });
    $form.find("input[type='radio']").each(function (i, e) {
        if ($(e)[0].checked)
            $(e).attr('checked', 'checked');
        else
            $(e).removeAttr('checked');
    });
    var inlinestyles = "";
    var styles = $("link[rel='stylesheet']");
    $(styles).each(function (i, e) {
        var shet = e.sheet;
        try {
            if (shet && shet.cssRules) {
                var content = Array.from(shet.cssRules).map(rule => rule.cssText).join(' ');
                inlinestyles += "\n" + content + "\n";
            }

        } catch (e) {
        }
    });
    $form.find("img").each(function (i, e) {
        var canvas = document.createElement('canvas');
        canvas.width = this.naturalWidth; // or 'width' if you want a special/scaled size
        canvas.height = this.naturalHeight; // or 'height' if you want a special/scaled size
        if (canvas.height > 0) {
            try {
                canvas.getContext('2d').drawImage(e, 0, 0);
                e.src = canvas.toDataURL('image/png');
            } catch { };
        }
    });
    var html = $form.html();
    return "<!DOCTYPE html>\n<html><head>\n<meta charset='UTF-16' /><style>table { border: none; }" + inlinestyles + "</style>\n\n</head><body style='background-color:#fff;'>" + html + "\n</body>\n</html>";


}

// Aggiorna un modello recuperando le informazioni da i controlli con un TagName
export function GetModel(tagName, previousModel, container) {
    if (!tagName) tagName = "name";

    var data = previousModel ? JSON.parse(JSON.stringify(previousModel)) : new Object();
    var originalData = previousModel ? previousModel : data;

    if (!container) container = Model.Container;
    var $container = $(container);
    var arrays = [];
    var filters = "[" + tagName + "]";
    var properties = $container.find(filters);
    dataTransfer = new Object();


    if (tagName == "name") {
        $("[template]").each((i, e) => {
            var s = e.getAttribute("source");
            if (s)
                data[s] = [];
        });
    }
    var found = [];
    $(properties).each((i, e) => {
        var $e = $(e);
        var tag = e.getAttribute("name");
        if (tag) {
            if (found.indexOf(tag) >= 0) return;
            found.push(tag);
        }
        var FieldName = e.getAttribute(tagName);
        var key = e.getAttribute(tagName + "-key");
        var keyValue = e.getAttribute(tagName + "-key-value");
        var kfield = e.getAttribute(tagName + "-field");
        if (!kfield)
            kfield = e.getAttribute(tagName + "-value");
        var fieldIndex = e.getAttribute(tagName + "-index");
        if (!fieldIndex)
            fieldIndex = -1;
        //sintassi per le chiavi
        if (FieldName && FieldName.startsWith("#")) {
            keyValue = FieldName.slice(1);
            FieldName = "fieldList";
            key = "fieldName";
            kfield = "value";
        }



        // Non imposto un campo già impostato
        var ok = FieldName != undefined;
        if (ok) {

            var names = FieldName.split(".");
            var currentFieldObject = data;
            var originalFieldOject = originalData;

            var previousFieldObject = data;
            var originaPreviousFieldObject = originalData;
            names.slice(0, -1).forEach((n, index) => {
                var i = parseInt(n);
                // se la proprietà è un indice numerico di array
                // esempio: names = test.5
                // last[test] = []
                if (!isNaN(i)) {
                    n = i;
                    // creo l'array se non esiste
                    if (!Array.isArray(currentFieldObject)) {
                        if (index > 0) {
                            previousFieldObject[names[index - 1]] = [];
                            originaPreviousFieldObject[names[index - 1]] = [];

                            currentFieldObject = previousFieldObject[names[index - 1]];
                            originalFieldOject = originaPreviousFieldObject[names[index - 1]];
                        }
                        else {
                            data = [];

                            currentFieldObject = data;
                            originalFieldOject = originalData;
                        }
                    }
                    arrays.push(currentFieldObject);
                }

                // creo il riferimento padre della proprietà da inserire se non è definito
                if (!currentFieldObject[n] || (Array.isArray(currentFieldObject[n]) && currentFieldObject[n].length == 0))
                    currentFieldObject[n] = {};

                previousFieldObject = currentFieldObject;
                originaPreviousFieldObject = originalFieldOject;
                // Aggiorno il puntamento dell'oggetto corrente
                currentFieldObject = currentFieldObject[n];
                originalFieldOject = originalFieldOject[n];
            });
            var n = names[names.length - 1];
            var i = parseInt(n);
            if (!isNaN(i)) n = i;
            if (currentFieldObject[n] == undefined)
                currentFieldObject[n] = "";

            var index = 0;
            var value = undefined;
            var template = e.getAttribute("template");
            var source = e.getAttribute("source");
            if (template && tagName != "name" && source) {
                var model = GetModel(undefined, undefined, e);
                value = JSON.stringify(model[source]);
            } else {
                $("[name='" + tag + "']").each((i, e) => {
                    var v = "";
                    {
                        v = (e.tagName == "INPUT" || e.tagName == "SELECT" || e.tagName == "TEXTAREA") ? $(e).val() : e.innerHTML;
                        if ((e.getAttribute("type") == "checkbox" || e.getAttribute("type") == "radio") && (!e.checked)) {
                            v = undefined;
                        }
                        if (v != undefined) {
                            if (value != undefined)
                                value += ",";
                            else
                                value = "";
                            value += v;
                        }
                        //    if ((e.getAttribute("type") == "checkbox" || e.getAttribute("type") == "radio")) {
                        //        if (e.checked) value += v;
                        //    }
                        //    else
                        //        if (v)
                        //            value = v;
                    }
                });
            }

            if (key) {
                if (!currentFieldObject[n])
                    currentFieldObject[n] = []
                index = currentFieldObject[n].findIndex(f => f[key] == keyValue)
                if (index == -1) {
                    index = Object.keys(currentFieldObject[n]).length;
                    currentFieldObject[n][index] = {};
                    currentFieldObject[n][index][key] = keyValue;
                }
            } else {
                index = fieldIndex;
            }

            if (index >= 0) {
                if (kfield) {
                    if (!currentFieldObject[n] || (Array.isArray(currentFieldObject[n]) && currentFieldObject[n].length == 0))
                        currentFieldObject[n] = {};
                    currentFieldObject = currentFieldObject[n][index]; // = value;
                    originalFieldOject = originalFieldOject[n][index]; // = value;
                    n = kfield;
                }
                else {
                    if (!currentFieldObject[n])
                        currentFieldObject[n] = [];
                    currentFieldObject = currentFieldObject[n]; // = value;
                    originalFieldOject = originalFieldOject[n]; // = value;
                    n = index;
                }
            }
            // se sto aggiornando un modello precedente, che aveva già un valore,
            // svuoto la variabile.
            // Per evitare che venga svuotata ad ogni ingresso, svuoto anche il
            // modello precedente, così entrerà in questa condizione solo una volta
            try {
                if (originalFieldOject[n] && previousModel) {
                    currentFieldObject[n] = ""; // value;
                    originalFieldOject[n] = "";
                }
            } catch { };
            if (currentFieldObject[n] && currentFieldObject[n].length > 0) {
                if (value) {
                    currentFieldObject[n] = currentFieldObject[n] + "," + value;
                    if (e.tagName == "SELECT")
                        currentFieldObject[n + "_description"] += "," + e.selectedIndex >= 0 ? e.item(e.selectedIndex).text : "";
                }
            }
            else {
                currentFieldObject[n] = value;
                if (e.tagName == "SELECT")
                    currentFieldObject[n + "_description"] = e.selectedIndex >= 0 ? e.item(e.selectedIndex).text : "";
            }
        }
    });
    arrays.forEach((item, index) => {
        for (var i = item.length - 1; i >= 0; i--) {
            if (!item[i])
                item.splice(index, 1);
        }
    });

    var $files = $container.find("INPUT[type=file]");
    $files.each((i, el) => {
        var id = el.id;
        el.files.forEach((file) => {
            if (!dataTransfer[id])
                dataTransfer[id] = new DataTransfer();
            dataTransfer[id].items.add(file);
        });
    });

    if (data["documentDate"] == "") data["documentDate"] = null;
    return data;
}

function _setTemplateCount(templateId, count) {
    let $container = $("#" + templateId);
    let $count = $container.find(".count");
    if ($count.length == 0) {
        $count = $("<input type='hidden' name='" + templateId + ".length' class='count' value='0' />");
        $count.appendTo($container);
    }
    if (!count) count = 0;
    $count.val(count);
    return count;
}

export function addRow(templateId, hidden) {
    let $container = $("#" + templateId);
    let index = $container.attr("index");
    if (index) index = parseInt(index);
    if (!index) index = 0;

    let source = $container.attr("source");
    var min = parseInt(($container.attr("min")));
    if (!min) min = 0;
    var max = parseInt(($container.attr("max")));
    if (!max) max = 999;

    if (index < max) {
        var t = Model.Templates[$container.attr("template")];
        if (!t) t = document.getElementById($container.attr("template"));

        var count = _setTemplateCount(templateId);
        _setTemplateCount(templateId, count + 1);

        var template = t.innerHTML;
        template = template.replace(/{index}/ig, index).replace(/{row}/ig, 1 + index).replace(/{count}/ig, count);
        template = Mustache.to_html(template, Model.Form.data);

        // sostituisco le espressioni {{}} legate all'oggetto source sul template
        //if (source) {
        //    var model = Model.Form.data[source][index];
        //    var i = template.indexOf("{" + source + "." + index + ".");
        //    while (i >= 0) {
        //        if (i >= 0) {
        //            var j = template.indexOf("}", i);
        //            var tag = template.substring(i + 1, j);
        //            var value = _getValue(Model.Form.data, tag);
        //            template = template.replace("{" + tag + "}", value);
        //        }
        //        i = template.indexOf("{" + source + "." + index + ".");
        //    }
        //}

        // Recupero il DOM
        var $template = $(template);
        $template.attr("index", index);
        // Mostro il pulsante Rimuovi
        var btn = $template.find(".removeRow");
        if (btn.length > 0) {
            if (index >= min) {
                btn.removeClass("hidden");
                btn.off("click").on("click", () => {
                    if (source) {
                        var i = $template.attr("index");
                        var model = GetModel(undefined, undefined, $container[0]);
                        model[source].splice(i, 1);
                        //if (Model.Form.data[source])
                        Model.Form.data[source] = model[source];
                        $container.empty();
                        $container.attr("index", 0);
                        _createList(model, $container[0]);
                    //    _setModel(model, "name", $container[0]);
                    //    var $attachments = $container.find("INPUT[type=file]");
                    //    $attachments.each((i, el) => {
                    //        _showFileUpload(el);
                    //    });
                    //    _renderSelectFiles();
                    //    // Ripeto l'assegnazione perchè _setModel ripristina il valore precedente
                    //    _setTemplateCount(templateId, 0);
                    }
                    else {
                        $template.remove();
                        let c = _setTemplateCount(templateId);
                        _setTemplateCount(templateId, c - 1);
                    }
                    var u = $template.attr("update");
                    if (u) {
                        $u = $("[name='" + u + "']"); if ($u.length > 0) _updateFormulaTags(u[0]);
                    }
                    _updateFormulaTags(undefined, "{" + templateId + ".length}");
                    var removeCallback = $container.attr("onRemove");
                    if (window[removeCallback])
                        window[removeCallback]();
                });
            } else {
                btn.addClass("hidden");
            }

        }
        $container.append($template);
        $template.show();
        var $attachments = $template.find("INPUT[type=file]");
        $attachments.each((i, el) => {
            _showFileUpload(el);
        });
        _renderSelectFiles();

        // Imposto il prossimo indice
        index++;
        $container.attr("index", index);
        if (!hidden) dataEntry();

        _updateFormulaTags(undefined, "{" + templateId + ".length}");
        _enableFormulaTags($template[0]);


        var addCallback = $container.attr("onAdd");
        if (window[addCallback])
            window[addCallback]($template);
    }


}



export function UpdateModel(modelName, newModel) {
    //oldModel = GetModel(undefined, undefined);
    let model = Model.Form.data || {};
    if (modelName) {

        if (newModel) {
            model[modelName] = newModel;
            if (Model.Form.data)
                if (!Model.Form.data[modelName])
                    Model.Form.data[modelName] = newModel;
        }
        let $templates = $("[source='" + modelName + "']");
        if ($templates.length > 0) {
            $templates.each((i, e) => {
                let $container = $(e);

                let index = $container.attr("index");
                if (index) index = parseInt(index);
                if (!index) index = 0;

                var t = document.getElementById($container.attr("template"));
                var template = t.innerHTML;
                template = template.replace(/{index}/ig, index).replace(/{row}/ig, 1 + index).replace(/{count}/ig, 0);
                // Recupero il DOM
                var $template = $(template);
                $template.attr("index", index);

                $container.empty();
                $container.attr("index", 0);
                _createList(model, $container[0]);
                //_setModel(model, "name", $container[0]);
                //var $attachments = $container.find("INPUT[type=file]");
                //$attachments.each((i, el) => {
                //    _showFileUpload(el);
                //});
                //_renderSelectFiles();
                //// Ripeto l'assegnazione perchè _setModel ripristina il valore precedente
                //_setTemplateCount(e.id, 0);

            });
        } else {
            var tag = $("[name='" + modelName + "']");
            if (tag.length > 0) {
                setField(tag[0], _getValue(Model.Form.data, modelName), true);
                _updateFormulaTags(tag[0]);
            }
        }
    }

}

export function Query(control) {
    let $c = $(control);
    let Value = $c.val();
    let dataSource = $c.attr("db");
    let tag = $c.attr("record");
    let sql = $c.attr("query");
    let pageSize = $c.attr("pageSize");
    let pageIndex = $c.attr("pageIndex");
    let placeholder = $c.attr("placeholder");

    let callback = $c.attr("callback");

    let ValueName = $c.attr("option-value");
    let TextName = $c.attr("option-text");

    if (!pageIndex) pageIndex = 0;
    if (!pageSize) pageSize = 50;
    if (dataSource && sql) {
        fetch("/internalapi/ExternalDatasource/Query?Id=" + escape(dataSource) + "&Query=" + escape(sql) + "&pageSize=" + pageSize + "&pageIndex=" + pageIndex)
            .then(response => {
                if (response.ok)
                    return response.json();
                else
                    return response.text().then(text => { throw new Error(text) });
            })
            .then(data => {
                names[tag] = data[0];
                queries[tag] = data.slice(1);
                let vindex = names[tag].indexOf(ValueName);
                let tindex = names[tag].indexOf(TextName);
                let o = "";
                if (placeholder)
                    o += "<option value=''>" + placeholder + "</option>";

                queries[tag].forEach((e, i) => {
                    o += "<option value='" + e[vindex] + "'>" + e[tindex] + "</option>";
                });
                $c.html(o);
                if (tag) {
                    $c.off("change").on("change", (e) => {
                        let selectedIndex = $c.prop('selectedIndex');
                        if (placeholder) selectedIndex--;
                        if (control.name)
                            Model.Form.data[control.name] = $c.val();
                        var record = selectedIndex >= 0 ? queries[tag][selectedIndex] : undefined;
                        // Aggiorna i tag html relativi alla query
                        $("[record-" + tag + "]").each((i, e) => {
                            let $e = $(e);
                            var fieldName = $e.attr("record-" + tag);
                            let nameIndex = names[tag].indexOf(fieldName);
                            if (nameIndex >= 0) {
                                let valore = record ? record[nameIndex] : "";
                                var isDate = $e.hasClass("date");
                                var isCurrency = $e.hasClass("money");
                                var isNumber = $e.hasClass("number");
                                if (isDate)
                                    try {
                                        valore = new Date(valore).toLocaleDateString();
                                    } catch (e) {

                                    }
                                if (isNumber)
                                    try {
                                        valore = valore.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                                    } catch (e) {

                                    }
                                setField(e, valore);
                                _updateFormulaTags(e);

                            }
                        });
                        if (callback) {
                            var obj = {};
                            var rec = names[tag];
                            if (rec) {
                                rec.forEach((e, i) => {
                                    let valore = record ? record[i] : "";
                                    obj[e] = valore;
                                })
                            }
                            window[callback](obj);
                        }
                        _updateFormulaTags(control);
                        if (Model.Form.data)
                            _setModel(Model.Form.data);


                    })
                }
                if (Value) {

                    $c.val(Value);
                    control.dispatchEvent(new Event("change"))
                    dataEntry();
                }
                if (!placeholder) control.dispatchEvent(new Event("change"));
            });



    }
}



function _removeFile(variableName) {

    var vname = "#" + variableName;
    $(vname + "_addButton").removeClass("hidden");
    var $removeBtn = $(vname + "_removeButton");
    $removeBtn.addClass("hidden").off("click");

    $("#" + variableName + "_id").val("");
    $("#" + variableName + "hash").val("");
    $("#" + variableName).val("");


    var containerId = $(vname).attr("container-id");
    if (containerId) {
        var $container = $("#" + containerId);
        var $files = $container.find("[fid]");
        $files.each((i, e) => { $(e).remove() });
        var $name = $container.find(".name");
        if ($name.length > 0)
            $name.text("");
        var $size = $container.find(".size");
        if ($size.length > 0)
            $size.text("");
        var $date = $container.find(".date");
        if ($date.length > 0)
            $date.text("");
        //$container.addClass("hidden");
    }
}

function _renderSelectFiles() {

    var $filecontrols = $("INPUT[type='file']");
    $filecontrols.each((i, filecontrol) => {
        var variableName = "#" + filecontrol.id;
        var attachmentId = "#" + filecontrol.id + "_id";
        var $attachment = $(filecontrol);
        var $attachmentId = $(attachmentId);

        // Creo il contenitore con i file selezionati
        var $addBtn = $(variableName + "_addButton");
        var $removeBtn = $(variableName + "_removeButton");
        var containerId = $attachment.attr("container-id");

        if (containerId) {
            var $acontainer = $("#" + containerId);
            $acontainer.removeClass("hidden");
            // mostro le informazioni di ciascun file associato alla variabile v
            if ($removeBtn.length == 0) {
                $removeBtn = $("<a href='#' id='" + filecontrol.name + "_removeButton' class='btn btn_xs btn-link removeButton'><i class='fa fa-trash-o'></i></a>");
                $acontainer.append($removeBtn);
            }
            // elimino tutte le visualizzazioni precedenti
            var $filedivs = $acontainer.find("[fid]");
            if ($filedivs.length > 0)
                $filedivs.remove();

            // se ho selezionato dei files o avevo già degli Id...
            var ok1 = ($attachmentId.length > 0) && ($attachmentId.val().length > 0);
            var ok2 = (filecontrol.files.length > 0);
            if (ok1 || ok2) {
                // se il file è già stato archiviato
                if (ok1) {
                    var $filediv = $acontainer.find("[fid='" + filecontrol.id + "']");
                    if ($filediv.length == 0) {
                        var $html = $acontainer.find(".file");
                        if ($html.length > 0) {
                            $filediv = $html.clone();
                            $filediv.removeClass("file");
                            $filediv.removeClass("hidden");
                            $filediv.attr("fid", filecontrol.id);
                            $acontainer.prepend($filediv);
                        }
                    }
                    if ($filediv.length == 0) {
                        $filediv = $acontainer;
                    }
                    var $name = $filediv.find(".name");
                    if ($name.length > 0)
                        $name.text("Documento acquisito in precedenza");
                    var $size = $filediv.find(".size");
                    if ($size.length > 0)
                        $size.text("");
                    var $date = $filediv.find(".date");
                    if ($date.length > 0)
                        $date.text("");
                }
                else {
                    filecontrol.files.forEach((e, i) => {
                        var $filediv = $acontainer.find("[fid='" + filecontrol.id + "_" + e.name + "']");
                        if ($filediv.length == 0) {
                            // recupero il template che visualizza il file selezionato
                            var $html = $acontainer.find(".file");
                            if ($html.length > 0) {
                                $filediv = $html.clone();
                                $filediv.removeClass("file");
                                $filediv.removeClass("hidden");
                                $filediv.attr("fid", filecontrol.id + "_" + e.name);
                                $acontainer.prepend($filediv);
                            }
                        }
                        if ($filediv.length == 0) {
                            $filediv = $acontainer;
                        }

                        var $name = $filediv.find(".name");
                        if ($name.length > 0)
                            $name.text(e.name);
                        var $size = $filediv.find(".size");
                        if ($size.length > 0)
                            $size.text(formatSize(e.size));
                        var $date = $filediv.find(".date");
                        if ($date.length > 0)
                            $date.text(e.lastModifiedDate);
                        var $mime = $filediv.find(".mimeType");
                        if ($mime.length > 0)
                            $mime.text(e.type);
                    });
                }
            }
            if (ok1 || ok2) {
                $addBtn.addClass("hidden");
                $removeBtn.removeClass("hidden").off("click").on("click", () => { if (confirm("Rimuovere questo file ?")) _removeFile(filecontrol.id) });
            } else {
                $addBtn.removeClass("hidden");
                $removeBtn.addClass("hidden").off("click");
            }
        }
    });
}

function CacheTemplates(htmlform) {
    Model.Templates = [];

    var $container = $(htmlform);
    var templates = $container.find("[template]");
    $(templates).each((i, e) => {
        var name = e.getAttribute("template");
        var html = document.getElementById(name);
        Model.Templates[name] = html;
    });
}


function _renderHTMLForm() {
    var $container = $(Model.Container);

    //2. Converto Tag Mustache
    Model.Form.onCreate = undefined;
    Model.Form.onStart = undefined;
    Model.Form.onEdit = undefined;
    Model.Form.onRender = undefined;
    Model.Form.onValidator = undefined;
    Model.Form.onClosing = undefined;
    Model.Form.onPostSave = undefined;
    try {
        // recupero l'html del template esterno (tutto il form)
        var html = $(Model.Form.template).html();
        if (Model.Data) {
            Model.Data.formatDate = function () {
                return function (val, render) {
                    var d = new Date(val);
                    return render(d.toLocaleString());
                };
            }
            Model.Data.formatMoney = function () {
                return function (val, render) {
                    var n = parseFloat(val);
                    return render(n.toFixed(2) + " €");
                };
            }
            Model.Data.formatNumber = function () {
                return function (val, render) {
                    var n = parseFloat(val);
                    return render(n.toFixed(2));
                };
            }
            // Elaboro le variabili del form generico principale (es. NewForm o Task) senza
            // i dati del form specifico.
            // Questo perchè Mustache andrebbe a svuotare le variabili non riconosciute perchè
            // presenti all'interno di Model.Form.data
            html = Mustache.render(html, Model.Data);
            console.log("Mustache Model.Data: " + new Date());
        }
        // Renderizzo il form generico principale (es. NewForm o Task), senza il form specifico
        $container.show();
        $container.html(html);
        $(Model.Container + "_loading").hide();

        var $form = $container.find("#htmlform");
        var htmlform = $form.length > 0 ? $form[0] : undefined;
        if (htmlform) {
            // Elaboro le variabili del form specifico
            Model.Form.data.formatDate = function () {
                return function (val, render) {
                    try {
                        var d = val ? (new Date(render(val))).toLocaleString().substring(0, 10) : "";
                        return (d);
                    }
                    catch {
                        return "";
                    }
                };
            }
            Model.Form.data.formatMoney = function () {
                return function (val, render) {
                    var n = parseFloat(render(val));
                    return (n.toLocaleString(undefined, { minimumFractionDigits: 2 }) + " EUR");
                };
            }
            Model.Form.data.formatNumber = function () {
                return function (val, render) {
                    var n = parseFloat(render(val));
                    return (n.toLocaleString(undefined, { minimumFractionDigits: 2 }));
                };
            }


            // Renderizzo il form specifico senza elaborare le variabili al suo interno
            // in modo tale da catturare le funzioni di start, validate e
            // generare contenuti dinamici relativi ad array
            htmlform.innerHTML = Model.Form.schema;
            CacheTemplates(htmlform);

            // Renderizzo il form e sostituisco il modello nelle espressioni {{}}
            Model.Form.html =  Mustache.to_html(htmlform.innerHTML, Model.Form.data);

            htmlform.innerHTML = "";
            $(htmlform).append(Model.Form.html);

            if (window["Create"])
                Model.Form.onCreate = window["Create"];
            if (window["Start"])
                Model.Form.onStart = window["Start"];
            if (window["Render"])
                Model.Form.onRender = window["Render"];
            if (window["Edit"])
                Model.Form.onEdit = window["Edit"];
            if (window["Validate"])
                Model.Form.onValidator = window["Validate"];
            if (window["PostSave"])
                Model.Form.onPostSave = window["PostSave"];
            if (window["Closing"])
                Model.Form.onClosing = window["Closing"];

            if (Model.Form.onCreate)
                Model.Form.onCreate();

            var $print = $(".printRemoved");
            $print.addClass("print").removeClass("printRemoved");

            var $btn = $(".removedForPreview");
            $btn.removeClass("hidden removedForPreview");

            //dataEntry();
            //console.log("Enable DataEntry: " + new Date());

            //_enableFormulaTags($container[0]);
            //console.log("Enable Formulas: " + new Date());

            //_loadUserProperties();
            //console.log("Enabled UserProperties: " + new Date());
            //renderFileUploads();

            _setModel(Model.Form.data);

            if (Model.Form.onStart)
                Model.Form.onStart(Model.Form.data);

            // Imposto gli attachment globali
            var $formattachments = $container.find("INPUT[type=file]");
            $formattachments.each((i, el) => {
                _showFileUpload(el);
            });

            // Imposto gli array
            var templates = $container.find("[template]");
            $(templates).each((i, e) => {
                let $tcontainer = $(e);
                $tcontainer.empty();
                $tcontainer.attr("index", 0);

                _createList(Model.Form.data, e);
                //_setModel(Model.Form.data, "name", e);

                //var $attachments = $tcontainer.find("INPUT[type=file]");
                //$attachments.each((i, el) => {
                //    _showFileUpload(el);
                //});
                //_renderSelectFiles();
                //// Ripeto l'assegnazione perchè _setModel ripristina il valore precedente
                //_setTemplateCount(e.id, 0);

            });

            _renderSelectFiles();

            if (Model.Form.onRender)
                Model.Form.onRender(Model.Form.data);


            dataEntry();
            console.log("Enable DataEntry: " + new Date());

            _enableFormulaTags($container[0]);
            console.log("Enable Formulas: " + new Date());
        }

        //12. gestione eventi
        $container.find("[name='ExitCode']").on("click", () => {
            var choise = $container.find("[name='ExitCode']:checked").val();
            var $note = $container.find("[name='Justification']");
            if ($note.length > 0) {
                if (choise == "1") {
                    if (!$note.val())
                        $note.text("Autorizzato");
                }
                else
                    $note.text("");
            }
        });

        //12. attività post render
        if (Model.Form.renderer)
            Model.Form.renderer();

    }
    catch (ex) {

        document.write("RENDERFORM: " + ex.message + " <br/> <br/> STACK: " + ex.stack); // window.location.href = "/Error?ErrorMessage=" + ex.message;
    };
}
function _renderFormJS() {
    var $container = $(Model.Container);
    try {
        // Template del Task Form
        Model.Form.html = "";
        var html = $(Model.Form.template).html();
        if (Model.Data) {
            html = Mustache.to_html(html, Model.Data);
        }
        $(Model.Container + "_loading").hide();
        $container.show();
        $container.html(html);

        FormViewer.createForm({
            container: "#htmlform",
            schema: JSON.parse(Model.Form.schema),
            data: Model.Form.data
        }).then(f => {
            Model.Form.formjs = f;
            setTimeout(() => {


            }, 50)
        })

        _enableFormulaTags($container[0]);
        //        _showTemplates();
        dataEntry();
        _setModel(Model.Form.data);
        _loadUserProperties();

        $container.find("[name='ExitCode']").on("click", () => {
            var choise = $container.find("[name='ExitCode']:checked").val();
            var $note = $container.find("[name='Justification']");
            if ($note.length > 0) {
                if (choise == "1") {
                    if (!$note.val())
                        $note.text("Autorizzato");
                }
                else
                    $note.text("");
            }
        });

        //12. attività post render
        if (Model.Form.renderer)
            Model.Form.renderer();
    }
    catch (ex) {
        document.write("REDERFORMJS: " + ex.message); // window.location.href = "/Error?ErrorMessage=" + ex.message;
    };
}

function _updateStatus(variableName, file, status, error) {
    if (!error) error = "";
    file.status = status;
    file.error = error;
    if (status == "reading") status = "Lettura file...";
    if (status == "preparing") status = "Classificazione...";
    if (status == "uploading") status = "Archiviazione...";
    if (status == "done") status = "File Acquisito";
    if (status == "error") status = "Errore";
    var $attachment = $("#" + variableName);
    var containerId = $attachment.attr("container-id");
    if (containerId) {
        var $acontainer = $("#" + containerId);

        var $filediv = $acontainer.find("[fid='" + variableName + "_" + file.name + "']");
        if ($filediv.length > 0) {
            $filediv.attr("status", status);
            var $status = $filediv.find(".status");
            if ($status.length > 0)
                $status.text(status);
            var $error = $filediv.find(".error");
            if ($error.length > 0)
                $error.text(error);
        }
    }
}
function _uploadSingleFile(variableName, file, ApiCallBack) {
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
    }
    // masterId è l'id del documento di riferimento
    var masterId = 0;
    // Procedo solo se non sto già scaricando il file
    if (!file.status) {
        file.status = "reading";
        _updateStatus(variableName, file, "reading");
        const reader = new FileReader();
        reader.onload = (data) => {
            _updateStatus(variableName, file, "preparing");
            var filedata = reader.result;
            var b64 = filedata.substr(filedata.indexOf(',') + 1);
            // V1: DEPRECATO: Lasciato per compatibilità
            var amodel = $("#" + variableName).attr("attachment");
            // V2: Il modello è il nome della variabile + "-model"
            if (!amodel)
                amodel = variableName + "-model";
            const attachmentModel = amodel ? GetModel(amodel) : {};
            _updateStatus(variableName, file, "uploading");

            fetch("/internalapi/action/UploadFile",
                {
                    method: 'post',
                    headers: {
                        RequestVerificationToken: VerificationCode,
                        'Content-Type': 'application/json;',
                        Accept: 'application/json;charset=ISO-8859-1'
                    },
                    body: JSON.stringify({
                        documentId: 0,
                        folderId: masterId,
                        filename: file.name,
                        data: b64,
                        document: attachmentModel
                    })
                })
                .then(response => {
                    if (response.ok)
                        return response.json();
                    else
                        return response.text().then(text => { throw new Error(text) });
                })
                .then(data => {
                    if (data.error) {
                        _updateStatus(variableName, file, "error", data.error);
                    }
                    else {
                        if (data.documentId) {

                            Model.Attachments.push(data);
                            // Memorizzo l'id del documento su una variabile specifica
                            var afield = $("#" + variableName + "_id");
                            if (afield.length > 0) {
                                setField(afield[0], data.documentId);
                            }
                            var afield = $("#" + variableName + "_hash");
                            if (afield.length > 0)
                                setField(afield[0], data.image.hash);
                             //attachment.documentIds.push(data.documentId)
                            _updateStatus(variableName, file, "done");
                        } else {
                            _updateStatus(variableName, file, "error", "Errore durante l'upload");
                        }
                    }
                    _uploadFiles(ApiCallBack);
                })
                .catch(err => {
                    _updateStatus(variableName, file, "error", err);
                    _uploadFiles(ApiCallBack);

                });
        };
        reader.onerror = (data) => {
            _updateStatus(variableName, file, "error", data);
            _uploadFiles(ApiCallBack);
        };
        reader.readAsDataURL(file);
    }
    else {
        _uploadFiles(ApiCallBack);
    }
}
function _uploadFiles(ApiCallBack) {
    var variableName = "";
    var file = undefined;
    var error = "";
    var files = $("INPUT[type='file']");
    if (files.length > 0) {
        var wait = false;
        files.each((i, attachment) => {
            // ToLoad += attachment.files.length;
            attachment.files.forEach((e, i) => {
                // se non ha uno stato è da caricare
                if (!e.status) {
                    variableName = attachment.id;
                    file = e;
                    return false;
                } else {
                    if (e.error) {
                        error = e.error;
                        return false;
                    }
                    else
                        // se lo sto caricando attendo
                        if (e.status != "done")
                            wait = true;
                }
            });
            if (file || error) return false;
        });
        if (file) {
            _uploadSingleFile(variableName, file, ApiCallBack);
        }
        else
            if (error) {
                $(".loader").remove();
                alert("Si sono verificati i seguenti errori durante il caricamento degli allegati e per proseguire e' necessario correggerli:\n\n" + error);
            }
            else
                // se è in corso un upload attendo
                if (wait)
                    window.setTimeout(() => { _uploadFiles(ApiCallBack); }, 500);
                else
                    _callFormSave(ApiCallBack);
    }
    else _callFormSave(ApiCallBack);

    return false;
}
function _callFormSave(ApiCallBack) {

    var attachments = GetAttachmentsId();
    Model.Data = new Object();
    if (Model.Form) {
        var oldData = Model.Form.data;
        if (Model.Form.formType == "FORMJS") {
            const {
                data,
                errors
            } = Model.Form.formjs.submit();
            Model.Data = (data);
            Model.Errors = errors;
        }
        if (Model.Form.formType == "HTML") {
            Model.Data = GetModel("name", oldData);
            Model.Document = GetModel("document");
        }
    }
    Model.Attachments = attachments;
    try {
        ApiCallBack(Model, () => { HideUploadPanel(); });
    } catch (e) {
    }
}



function _getValue(model, variable) {
    if (!variable) return "";
    var fieldObj = model;
    var FieldName = variable;
    var names = FieldName.split(".");
    names.forEach(n => {
        try {
            if (fieldObj[n] == undefined) {
                fieldObj = undefined;
                return undefined
            }
            fieldObj = fieldObj[n];
        } catch {
            return undefined
        };
    });
    return fieldObj;
}


function _parseModel(model, variable) {
    if (!variable) return variable;
    if (variable.startsWith("{{") & variable.endsWith("}}")) {
        variable = variable.slice(2, -2);
        variable = _getValue(model, variable);
    }
    return variable;
}

function _createList(model, control, hidden) {
    var template = control.getAttribute("template");
    if (template) {
        var FieldName = control.getAttribute("source");
        if (!FieldName) FieldName = template;
        var fieldObj = _getValue(model, FieldName);
        var minimo = _parseModel(model, control.getAttribute("min"));
        var tmin = 0 + parseInt(minimo);
        if (isNaN(tmin)) tmin = 0;
        var omin = fieldObj ? Object.keys(fieldObj).length : 0;
        var min = Math.max(tmin, omin);
        _setTemplateCount(control.id, 0);
        while (min > 0) {
            addRow(control.id, hidden);
            min--;
        }


        _setModel(model, "name", control);

        var $attachments = $(control).find("INPUT[type=file]");
        $attachments.each((i, el) => {
            _showFileUpload(el);
        });
        _renderSelectFiles();
        // Ripeto l'assegnazione perchè _setModel ripristina il valore precedente
        _setTemplateCount(control.id, 0);


    }
}

// es. model='documentDate'
// es. model='image.Description',
// es. model='FieldList',  model-key='FieldName' model-key-value='protocollo' model-field = 'Value'
// es. model='FieldList',  model-index='5' model-field='Value' 
function _setModel(model, tagName, container) {
    if (!model || model == {}) return;
    if (!container) container = Model.Container;
    var $container = $(container);
    try {

        // Creo l'html dei campi di tipo array
        // verifico il minimo previsto nel tag e prendo il maggiore
        // tra la proprietù "min" e fieldObj.Length
        //var templates = $container.find("[template]");
        //$(templates).each((i, e) => {
        //    _createList(model, e);
        //});

        //if (!model || model == {}) return;
        // Riempio i tag relativi al modello
        // Prendo come Properties tutti i cmpi con un name
        if (!tagName) tagName = "name";
        var filters = "[" + tagName + "]";
        var properties = $container.find(filters);

        $(properties).each((i, variable) => {
            var FieldName = variable.getAttribute(tagName);
            var key = variable.getAttribute(tagName + "-key");
            var keyValue = variable.getAttribute(tagName + "-key-value");
            var field = variable.getAttribute(tagName + "-field");
            if (!field)
                field = variable.getAttribute(tagName + "-value");
            var fieldIndex = variable.getAttribute(tagName + "-index");
            if (!fieldIndex)
                fieldIndex = -1;
            //sintassi per le chiavi
            if (FieldName && FieldName.startsWith("#")) {
                keyValue = FieldName.slice(1);
                FieldName = "fieldList";
                key = "fieldName";
                field = "value";
            }
            else {
                if (FieldName) FieldName = FieldName[0].toLowerCase() + FieldName.substring(1);
                if (key) key = key[0].toLowerCase() + key.substring(1);
                if (field) field = field[0].toLowerCase() + field.substring(1);
                if (keyValue) keyValue = keyValue[0].toLowerCase() + keyValue.substring(1);
            }
            var ok = FieldName != undefined;
            if (ok) {
                // recupero l'ultimo nome di proprietà all'interno di una sintassi tipo: <var.field...field>
                var fieldObj = _getValue(model, FieldName);

                var index = 0;
                var value = "";
                if (fieldObj != undefined) {
                    if (key) {
                        index = fieldObj.findIndex(f => f[key] == keyValue)
                    } else {
                        index = fieldIndex;
                    }
                    if (index >= 0) {
                        if (field)
                            value = fieldObj[index][field];
                        else
                            value = fieldObj[index];
                    }
                    else {
                        value = fieldObj;
                    }
                    //if (variable.tagName == "SELECT") {
                    //    var $opt = $(variable).find("option[value='" + value + "']");
                    //    if ($opt.length == 0) {
                    //        var opt = "<option value='" + value + "'>" + _getValue(model, variable.name + "_description") + "</option>";
                    //        $(variable).append(opt);
                    //    }
                    //}
                    setField(variable, value, true);
                    _updateFormulaTags(variable);
                }
            }
        });

        var $files = $container.find("INPUT[type=file]");
        $files.each((i, el) => {
            var id = el.id;
            var dt = dataTransfer[id];
            if (dt)
                el.files = dt.files;
        });

    } catch { };
}


function _internalValidateForm() {
    var ok = true;
    Model.Errors = "";
    if (Model.Form && Model.Form.onValidator) {
        var ok = true;
        try {
            ok = Model.Form.onValidator();
            if (ok == undefined) ok = true;
        } catch (e) {
            Model.Errors = e.message;
            ok = false;
        }
    }
    if (ok) {
        var files = $("INPUT[type='file']");
        files.each((i, e) => {
            var variableName = e.id;
            var variableId = $("#" + e.id + "_id").val();
            var required = $(e).attr("required");
            var disabled = $(e).attr("disabled");
            var hidden = $(e).attr("hidden");
            //TODO: gestire il caso di controlli required nascosti
            if (required && !variableId && !disabled && !hidden && e.files.length == 0) {
                var button = $("#" + variableName + "_addButton");
                button[0].scrollIntoView();
                button[0].dispatchEvent(new Event('invalid'));
                button.focus();
                Model.Errors = "Non hai selezionato un file";
                ok = false;
                return false;
            }
        });

        //var selectes = $("SELECT:required");
        //selectes.each((i, e) => {
        //    var variableName = e.id;
        //    var disabled = $(e).attr("disabled");
        //    var hidden = $(e).attr("hidden");
        //    if (!disabled && !hidden && !e.value) {
        //        e.scrollIntoView();
        //        e.checkValidity()
        //        e.dispatchEvent(new Event('invalid'));
        //        e.focus();
        //        var name = e.name;
        //        if (!name) name = e.id;
        //        //Model.Errors = "Non hai indicato un valore per il campo '"+name+"'";
        //        ok = false;
        //        return false;
        //    }
        //});
    }
    if (ok) {
        var form = $("#htmlform");
        var htmlform = form.length > 0 ? form[0] : undefined;
        if (htmlform) {
            if (ok) {
                ok = ShowRequiredPopupJS(htmlform);//faccio comparire i popup manualmente in caso di campi non validi
                if (!ok && !Model.Errors) {
                    ok = htmlform.reportValidity();
                    if (ok)
                        Model.Errors = "Uno o piu' campi obbligatori non sono stati indicati";
                    ok = false;
                }
            }
            if (ok) ok = htmlform.reportValidity();
        }
    }

    if (ok == undefined) ok = false;
    Model.Validated = ok;
    return ok;
}
function _loadUserProperties() {
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
    }

    $("[userSetting]").each((i, e) => {
        var $e = $(e);
        var category = $e.attr("userSetting");
        if (category) category += ".";
        var name = $e.attr("name");
        if (!name) name = e.id;
        $e.on("change", () => {

            fetch("/internalapi/userSetting/Setting",
                {
                    method: 'post',
                    headers: {
                        RequestVerificationToken: VerificationCode,
                        'Content-Type': 'application/json;',
                        Accept: 'application/json;charset=ISO-8859-1'
                    },
                    body: JSON.stringify({
                        attributeId: category + name,
                        contactId: "-",
                        companyId: 0,
                        value: $e.val()
                    })
                });
        });
        if (!$e.val()) {
            var isdisabled = $e.attr("disabled");
            $e.attr("disabled", true);
            fetch("/internalapi/userSetting/Setting/" + escape(category + name))
                .then(response => {
                    if (response.ok)
                        return response.text();
                    else
                        return response.text().then(text => { throw new Error(text) });
                })
                .then(data => {
                    if ($e.val() != data) {
                        $e.val(data);
                        $e[0].dispatchEvent(new Event("change"));
                    }
                    if (!isdisabled) $e.removeAttr("disabled");
                });
        }
    });
}

function updateAttachment(name, value) {

    var name = $("#" + name);
    if (name.length > 0) {
        var c = $("#" + name).attr("container");
        var acontainer = undefined;
        if (!c) {
            acontainer = $("#" + name + "_container");
            if (acontainer.length == 0) {
                var parent = $("#" + name).parent();
                acontainer = $("<div id='" + name + "_container'><div class='file'></div></div>");
                parent.prepend(acontainer);
            }
        }
        else
            acontainer = $("#" + c);

        acontainer.show().removeClass("hidden");
        value.forEach((e, i) => {
            fetch("/internalapi/Content/" + e)
                .then(response => {
                    if (response.ok)
                        return response.json();
                    else
                        return response.text().then(text => { throw new Error(text) });
                })
                .then(data => {
                    var file = acontainer.find("[fname='" + data.imageId + "']");
                    if (file.length == 0) {
                        file = acontainer.find(".file");
                        file.attr("fname", data.imageId);
                        acontainer.append(file);
                    }
                    var name = file.find(".name");
                    if (name.length == 0) {
                        name = $("<div class='name'></div>");
                        file.append(name);
                    }
                    name.text(data.fileName);
                    var size = file.find(".size");
                    if (size.length > 0) {
                        size.text(formatSize(data.fileSize));
                    }
                    var btn = file.find(".remove");
                    if (btn.length > 0) {
                        file.remove(btn);
                    }
                })
                .catch(err => {
                    alert(err);
                });
        });
    }
}


// FORMULE PER I FORM

function _showTemplates() {
    $(document).find("[template]").each((index, e2) => {
        var min = parseInt($(e2).attr("min"));
        if (min > 0) {
            while (min > 0) {
                addRow(e2.id);
                min = min - 1;
            }
        }
    });

}



function _setClass(e, tagName, className, reverse) {
    var formula = e.getAttribute(tagName);
    var val = _updateSingleFormula(e, formula);
    if ((val && !reverse) || (!val && reverse))
        $(e).removeClass(className);
    else
        $(e).addClass(className);
}
function _setAttr(e, tagName, attribute, reverse) {
    var formula = e.getAttribute(tagName);
    var val = _updateSingleFormula(e, formula);
    if ((val && !reverse) || (!val && reverse))
        $(e).attr(attribute, attribute);
    else
        $(e).removeAttr(attribute);
}

function _updateFormulaTags(element, name) {
    if (!name) {
        if (!element.name) return;
        name = "{" + element.name + "}";
    }

    $(document).find("[visibleFor*='" + name + "']").each((index, e2) => {
        _setClass(e2, "visibleFor", "hidden");
    });
    $(document).find("[hiddenFor*='" + name + "']").each((index, e2) => {
        _setClass(e2, "hiddenFor", "hidden", true);
    });

    $(document).find("[showIf*='" + name + "']").each((index, e2) => {
        _setClass(e2, "showIf", "hidden");
    });
    $(document).find("[hideIf*='" + name + "']").each((index, e2) => {
        _setClass(e2, "hideIf", "hidden", true);
    });

    $(document).find("[requiredFor*='" + name + "']").each((index, e2) => {
        _setAttr(e2, "requiredFor", "required");
    });
    $(document).find("[checkedFor*='" + name + "']").each((index, e2) => {
        _setAttr(e2, "checkedFor", "checked");
        $(e2).attr("disabled", true);
    });
    $(document).find("[uncheckedFor*='" + name + "']").each((index, e2) => {
        _setAttr(e2, "uncheckedFor", "checked", true);
        $(e2).attr("disabled", true);
    });

    $(document).find("[enabledFor*='" + name + "']").each((index, e2) => {
        _setAttr(e2, "enabledFor", "disabled", true);
    });
    $(document).find("[disabledFor*='" + name + "']").each((index, e2) => {
        _setAttr(e2, "disabledFor", "disabled");
    });

    if (element) {
        var category = element.getAttribute("userSetting");
        if (category) {
            var VerificationCode = "";
            var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
            if (RequestVerificationCode.length > 0) {
                VerificationCode = RequestVerificationCode[0].value;
            }
            fetch("/internalapi/userSetting/Setting",
                {
                    method: 'post',
                    headers: {
                        RequestVerificationToken: VerificationCode,
                        'Content-Type': 'application/json;',
                        Accept: 'application/json;charset=ISO-8859-1'
                    },
                    body: JSON.stringify({
                        attributeId: category + "." + element.name,
                        contactId: "-",
                        companyId: 0,
                        value: $(element).val()
                    })
                });
        }
    }

    $(document).find("[formula*='" + name + "']").each((index, e) => {
        // estraggo i campi dalla formula

        var formula = e.getAttribute("formula");
        var valore = "" + _updateSingleFormula(e, formula);
        try {
            var lookup = e.getAttribute("lookup");
            let currentValue = $(e).val();
            if (currentValue != valore) {
                if (lookup) {
                    if ((e.type == "checkbox" || e.type == "radio" || e.type == "input" || e.type == "select" || e.type == "textarea"))
                        $(e).val("...");
                    else
                        $(e).text("...");

                    fetch("/internalapi/ui/DataType/Lookup/" + escape(lookup) + "/1/" + escape(valore))
                        .then(r => {
                            if (r.ok)
                                return r.json();
                            else
                                return r.text().then(text => { throw new Error(text) });
                        })
                        .then(valore => {

                            if ((e.type == "checkbox" || e.type == "radio"))
                                e.checked = valore;
                            else


                                var lookups = $("[lookup=" + lookup + "]");
                            if (lookups.length > 0) {
                                lookups.each(el => {
                                    var l = $(el);
                                    var html = l.html();
                                    if (html.indexOf("{") >= 0) {
                                        html = html
                                            .replace(/{icon}/g, valore[0].icon)
                                            .replace(/{formattedValue}/g, valore[0].formattedValue)
                                            .replace(/{lookupValue}/g, valore[0].lookupValue)
                                            .replace(/{value}/g, valore[0].value);
                                        valore[0].fields.forEach(e => {
                                            html = html
                                                .replace("{fields." + e.id + "}", e.value)
                                                .replace("{" + e.id + "}", e.value);
                                        })
                                        l.html(html);
                                    }
                                    else {
                                        if ((e.tagName == "INPUT" || e.tagName == "SELECT" || e.tagName == "TEXTAREA"))
                                            l.val(valore[0].lookupValue);
                                        else
                                            l.html(valore[0].lookupValue);
                                    }
                                })
                            }
                            $(e).trigger("change");
                        })
                }
                else {
                    if ((e.type == "checkbox" || e.type == "radio")) {
                        var checked = (valore.split(",").includes(valore));
                        if (e.checked != checked) {
                            e.checked = checked;
                            $(e).trigger("change");
                        }
                    }
                    else {
                        if ((e.tagName == "INPUT" || e.tagName == "SELECT" || e.tagName == "TEXTAREA")) {
                            if ($(e).val() != valore) {
                                $(e).val(valore);
                                $(e).trigger("change");
                            }
                        }
                        else {
                            if ($(e).html() != valore) {
                                $(e).html(valore);
                                $(e).trigger("change");

                            }
                        }
                    }

                }
            }
        }
        catch (ex) {
            console.debug(ex)
        };

    });
}

function _updateSingleFormula(control, formula) {
    if (!formula) return "";
    var _formula = formula;
    var decimal = control ? control.getAttribute("decimal") : 0;
    if (!decimal)
        decimal = control ? control.getAttribute("digits") : 0;
    var emptyvalue = control ? control.getAttribute("emptyValue") : "";
    if (!emptyvalue) emptyvalue = "";

    var i = formula.indexOf("{");
    while (i >= 0) {
        var j = formula.indexOf("}");
        if (j >= 0) {
            var nomecampo = formula.substring(i + 1, j);
            var padre = control ? $(control).parent() : $(document);
            var campo = padre.find("[name='" + nomecampo + "']");
            while (campo && campo.length == 0) {
                padre = padre.parent();
                if (padre != padre.parent() && padre.length > 0)
                    campo = padre.find("[name='" + nomecampo + "']");
                else
                    break;
            }
            var val = "";// IsNumber ? 0 : "";
            if (campo && campo.length > 0) {
                campo.each((i, e) => {

                    var v = "";
                    if ((e.type == "checkbox" || e.type == "radio"))
                        if (e.checked) v = e.value; else v = v;
                    else
                        if ((e.tagName == "INPUT" || e.tagName == "SELECT" || e.tagName == "TEXTAREA"))
                            v = e.value
                        else
                            v = e.innerText;
                    if (!v) v = emptyvalue;
                    let IsDate = v.length > 8 && v[4] == '-' && v[7] == '-'; // && (e.tagName == "INPUT" && e.type == "date");
                    if (IsDate) {
                        //IsNumber = false;
                        val = v;
                    } else {
                        let vi = parseFloat(v);
                        let IsNumber = !isNaN(vi);// (e.tagName == "INPUT" && e.type == "number");
                        if (IsNumber) {
                            v = vi; // parseFloat(v);
                            //if (isNaN(v)) v = 0;
                            if (!val) val = 0;
                            val += v;
                            if (decimal)
                                val = parseFloat(val).toFixed(decimal);
                            else
                                val = parseFloat(val);
                        }
                        else {
                            if (val && v) val += ",";
                            val += v;
                        }
                    }
                });
                //				var val = campo.val();
                if (!val && emptyvalue) {
                    val = emptyvalue;
                }
            }
            formula = formula.substring(0, i) + val + formula.substring(j + 1);
            i = formula.indexOf("{");
        } else i = -1;
    }
    var valore = "";
    try {
        valore = eval(formula);
        var vi = parseFloat(valore);
        if (!isNaN(vi) && decimal) valore = vi.toFixed(decimal);
    }
    catch (ex) {
        console.log("formula:" + _formula + " => " + formula + " => " + ex);
    };
    return valore;
}

function _enableFormulaTags(container) {
    $(container).find("[addRow]").each((index, e) => {
        $(e).off("click").on("click", () => {
            addRow(e.getAttribute("addRow"));
        });
    });
    $(container).find("input,select,textarea,checkbox,option").each((index, e) => {
        $(e).off("change").on("change", () => {
            _updateFormulaTags(e);
        });
        _updateFormulaTags(e);
    });


    $(container).find("[visibleFor]").each((index, e2) => {
        var formula = $(e2).attr("visibleFor");
        var val = _updateSingleFormula(e2, formula);
        if (val)
            $(e2).removeClass("hidden");
        else
            $(e2).addClass("hidden");
    });
    $(document).find("[requiredFor*='" + name + "']").each((index, e2) => {
        var formula = $(e2).attr("requiredFor");
        var val = _updateSingleFormula(e2, formula);
        if (val)
            $(e2).attr("required", "required");
        else
            $(e2).removeAttr("required");
    });
    $(container).find("[hiddenFor]").each((index, e2) => {
        var formula = $(e2).attr("hiddenFor");
        var val = _updateSingleFormula(e2, formula);
        if (!val)
            $(e2).removeClass("hidden");
        else
            $(e2).addClass("hidden");
    });
    $(container).find("[enabledFor]").each((index, e2) => {
        var formula = $(e2).attr("enabledFor");
        var val = _updateSingleFormula(e2, formula);
        if (val)
            $(e2).removeAttr("disabled");
        else
            $(e2).attr("disabled", true);
    });
    $(container).find("[disabledFor]").each((index, e2) => {
        var formula = $(e2).attr("disabledFor");
        var val = _updateSingleFormula(e2, formula);
        if (!val)
            $(e2).removeAttr("disabled");
        else
            $(e2).attr("disabled", true);
    });
    $(container).find("[visibleFor],[enabledFor],[requiredFor],[hiddenFor],[disabledFor]").each((index, e) => {
        _updateFormulaTags(e);
    });


    $(container).find("[query]").each((index, e) => {
        Query(e);
    });
}


function GUID() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}
