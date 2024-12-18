/// <reference path="../tinymce/tinymce.min.js" />


var qs = (function (a) {
    if (a == "") return {};
    var b = {};
    for (var i = 0; i < a.length; ++i) {
        var p = a[i].split('=', 2);
        if (p.length == 1)
            b[p[0]] = "";
        else
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
    }
    return b;
})(window.location.search.substr(1).split('&'));

function getCaretCoordinates() {
    let x = 0,
        y = 0;
    const isSupported = typeof window.getSelection !== "undefined";
    if (isSupported) {
        const selection = window.getSelection();
        if (selection.rangeCount !== 0) {
            const range = selection.getRangeAt(0).cloneRange();
            range.collapse(true);
            const rect = range.getClientRects()[0];
            if (rect) {
                x = rect.left;
                y = rect.top;
            }
        }
    }
    return { x, y };
}

function getCaretPosition(editableDiv) {
    var caretPos = 0,
        sel, range;
    if (window.getSelection) {
        sel = window.getSelection();
        if (sel.rangeCount >= 0) {
            range = sel.getRangeAt(0);
            if (range.commonAncestorContainer.parentNode == editableDiv) {
                caretPos = range.endOffset;
            }
        }
    } else if (document.selection && document.selection.createRange) {
        range = document.selection.createRange();
        if (range.parentElement() == editableDiv) {
            var tempEl = document.createElement("span");
            editableDiv.insertBefore(tempEl, editableDiv.firstChild);
            var tempRange = range.duplicate();
            tempRange.moveToElementText(tempEl);
            tempRange.setEndPoint("EndToEnd", range);
            caretPos = tempRange.text.length;
        }
    }
    return caretPos;
}
var variable_container_template = 
`<div class='lookup-search-box' id="variable_template">
    <ul class='lookup-values'>
    </ul>
</div>`;
var variable_row_template = `
    <li class='lookup-search-row'>
        <a href='#'>
            {lookupvalue}
        </a>
    </li>
    `;
// Componente che inietta un listener in un campo di testo per gestire l'imputazione di variabili custom
// Il componente si attiva alla pressione del tasto "{" o alla pressione dei task "CTRL + SPACE"
function AddVariable(inputControl, variableStrings) {
    if (typeof inputControl === "string")
        var $input = $(inputControl);
    else
        var $input = $(inputControl);
    $input.on("keyup", (e) => {
        if (e.keyCode === 186) {
            // ho premuto "{";
            e.target.focus();
            var ed = tinyMCE.get(inputControl);
            if (ed)
                var position = ed.selection.getRng().startOffset; // getCaretPosition(e.target);
            else
                var position = getCaretPosition(e.target);
            var valore = (e.target.tagType == "INPUT" || e.target.tagType == "TEXTAREA") ? $(e.target).val() : $(e.target).html();
            let start = position;
            while (start >= 0 && valore[start] != "{") start--;
            var filtro = valore.substring(start, position);
            var variables = variableStrings.filter((v, i, a) => { v.indexOf(filtro) == 0 }).slice(0, 8);
            var parsedVariables = variables.map(v => { return v.substring(filtro.length) });
            var pos = getCaretCoordinates();
            var $elenco = $("#variable_template");
            if ($elenco.length == 0) {
                $(variable_container_template).appendTo(document);
                $elenco = $("#variable_template");
            }
            $elenco.css("left", pos.x + e.target.clientLeft);
            $elenco.css("top", pos.y) + e.target.clientTop;
            $elenco.empty();
            parsedVariables.forEach(e => {
                $row = $(variable_row_template.replace("{lookupvalue}", e));
                $row.find("a").on("click", (e) => {
                    var text = e.target.innerHTML;
                    var valore = (e.target.tagType == "INPUT" || e.target.tagType == "TEXTAREA") ? $(e.target).val() : $(e.target).html();
                    valore = valore.substring(0, start) + text + valore.substring(position);
                    if ((e.target.tagType == "INPUT" || e.target.tagType == "TEXTAREA"))
                        $(e.target).val(valore);
                    else
                        $(e.target).html(valore);
                });
            });

        }
    });
}

var variable_open = false;

function AddVariable2Tiny(inputControl,  variableStrings) {
    var $input = $( inputControl.getBody());
    $input.on("keyup", (e) => {
        if (e.keyCode === 186 || variable_open) {
            // ho premuto "{";
            e.target.focus();
            variable_open = true;
            var ed = inputControl;
            var position = ed.selection.getRng().startOffset; // getCaretPosition(e.target);
            var valore = $(e.target).text();
            let start = position-1;
            while (start > 0 && valore[start] != '{') start--;
            var filtro = valore.substring(start, position);
            var variables = variableStrings.filter((v, i, a) => { return v.indexOf(filtro) == 0 }).slice(0, 8);
            var parsedVariables = variables.map(v => { return v.substring(filtro.length) });
            var pos = getCaretCoordinates();
            var $elenco = $("#variable_template");
            if ($elenco.length == 0) {
                $("form").after (variable_container_template);
                $elenco = $("#variable_template");
            }
            $elenco.css("left", pos.x + parseFloat( e.target.clientLeft));
            $elenco.css("top", pos.y + parseFloat(e.target.clientTop));
            $elenco.empty();
            var $ul = $("<ul class='nav nav-list'></ul>");
            $elenco.append($ul);
            parsedVariables.forEach(e => {
                $row = $(variable_row_template.replace("{lookupvalue}", e));
                $ul.append($row);
                $row.find("a").on("click", (a) => {
                    var text = $(a.target).text();
                    var valore = inputControl.getBody().innerText;
                    valore = valore.substring(0, start) + text + valore.substring(position+1);
                    inputControl.getBody().innerText = valore;
                    variable_open = false;
                    $elenco.remove();
                });
            });

        }
    });
}



function InizializzaTextEditor(selector, content, variables = null, height = null) {
    return new Promise((resolve, reject) => {
        tinymce.init({
            selector: selector,
            height: height,
            menubar: false,
            relative_urls: false,
            language: 'it',
            plugins: [
                'emoticons', 'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
                'anchor', 'searchreplace', 'visualblocks', 'fullscreen', 'mention',
                'insertdatetime', 'media', 'table', 'code'
            ],
            toolbar: 'undo redo | insert | bold italic undeline strikethrough | styles | alignleft aligncenter alignright alignjustify | bullist numlist | emoticons | link image code',
            // callback per selezionare l'immagine
            /* enable title field in the Image dialog*/
            image_title: true,
            /* enable automatic uploads of images represented by blob or data URIs*/
            automatic_uploads: true,
            /*
              URL of our upload handler (for more details check: https://www.tiny.cloud/docs/configure/file-image-upload/#images_upload_url)
              images_upload_url: 'postAcceptor.php',
              here we add custom filepicker only to Image dialog
            */
            file_picker_types: 'image',
            /* and here's our custom image picker*/
            file_picker_callback: (cb, value, meta) => {
                const input = document.createElement('input');
                input.setAttribute('type', 'file');
                input.setAttribute('accept', 'image/*');

                input.addEventListener('change', (e) => {
                    const file = e.target.files[0];

                    const reader = new FileReader();
                    reader.addEventListener('load', () => {
                        /*
                          Note: Now we need to register the blob in TinyMCEs image blob
                          registry. In the next release this part hopefully won't be
                          necessary, as we are looking to handle it internally.
                        */
                        const id = 'blobid' + (new Date()).getTime();
                        const blobCache = tinymce.activeEditor.editorUpload.blobCache;
                        const base64 = reader.result.split(',')[1];
                        const blobInfo = blobCache.create(id, file, base64);
                        blobCache.add(blobInfo);

                        /* call the callback and populate the Title field with the file name */
                        cb(blobInfo.blobUri(), { title: file.name });
                    });
                    reader.readAsDataURL(file);
                });
                input.click();
            },
            mentions: {
                queryBy: 'Id',
                delimiter: '{',
                items: 256,
                source: variables
            //    function(query, process, delimiter) {
            //        // query.term is the text the user typed after the '@'
            //        if (delimiter === '{') {
            //            var tags = variables;
            //            tags = tags.filter(function (tag) {
            //                return tag.indexOf(query.toLowerCase()) !== -1;
            //            });

            //            tags = tags.slice(0, 10);

            //            // Where the user object must contain the properties `id` and `name`
            //            // but you could additionally include anything else you deem useful.
            //            process(tags);
            //        }
            //    }
            },
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }',
            setup: function (editor) {
                editor.on('init', function (e) {
                    $(".tox-promotion").remove();
                    if (selector[0] == ("#")) selector = selector.substr(1);
                        
                    //var elements = tinymce.get(selector);
                    //if (elements)
                    //    $(elements).each((i,editor) => {
                            var el = editor.getElement();
                            if (el != undefined && el.value != undefined)
                                editor.getBody().innerHTML = el.value;
                            else
                                editor.getBody().innerHTML = content;
                            //if (variables)
                            //    AddVariable2Tiny(editor, variables)
                            if (el.required) {
                                el.style = "display:block;position:absolute;left:50%;top:50%;width:0;height:0;z-index:-1";
                                el.setAttribute("tab-index", "-1");
                            }
                        //})
                    resolve();
                });
                editor.on('input', function (e) {
                    var el = tinymce.activeEditor.getElement();
                    el.value = tinymce.activeEditor.getContent();
                    resolve();
                });
                editor.on('submit', (e) => {
                    var el = tinymce.activeEditor.getElement();
                    if (el.required) {
                        var content = tinymce.activeEditor.getContent({ format: 'text' });
                        if ($.trim(content) == '') {
                            var evt = event | e;
                            evt.preventDefault();
                        }
                    }
                });
            }
        });
    });


}

function ShowMessage(type, message, RefreshPath) {
    ShowMessage(type, message, 'Message_Target', RefreshPath);
}

function ShowMessage(type, message, Message_Target = 'Message_Target', RefreshPath) {
    // funzione per far comparire i messaggi di errore o di successo.
    // Adoperando i template Mustache i messaggi vengono inserire nel div con id uguale all'input 'Message_Target'
    // l'input RefreshPath può essere nullo

    var AlertTemplate = ` <div class="alert alert-danger mt-2 alert-dismissible fade show mx-4 bg-white" role="alert" style="position:fixed;right:0;bottom:0;z-index: 101;width: 400px;">
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Chiudi avviso">
                            <svg class="icon">
                                <use href="@($"{Configuration["PATH_BASE"]}/bootstrap-italia/svg/sprites.svg#it-close")"></use>
                            </svg>
                        </button>
                        <h4 class="alert-heading">Si è verificato un errore</h4>
                        <p style="height: 100%;width: 100%;padding:10px 0;line-break: auto;max-height: 200px;overflow: hidden;text-overflow: ellipsis;">
                            {{data.message}}
                        </p>
                    </div>`;

    var SuccessTemplate = `<div class="alert alert-success alert-dismissible fade show bg-white mt-2 mx-4" role="alert" style="position: fixed;right:0;bottom:0;z-index: 101;width: 400px">
                        <h4 class="alert-heading">{{data.message}}</h4>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Chiudi avviso">
                            <svg class="icon">
                                <use href="@($"{Configuration["PATH_BASE"]}/bootstrap-italia/svg/sprites.svg#it-close")"></use>
                            </svg>
                        </button>
                    </div>`;

    const template = (type == "success") ? SuccessTemplate : AlertTemplate;
    const rendered = Mustache.render(template, { data: { message: message, refreshPath: RefreshPath } });

    var div = document.createElement('div');
    div.innerHTML = rendered;

    //document.getElementById(Message_Target).innerHTML = rendered;
    document.body.appendChild(div);

}

function Confirm(text, Message_Target = 'Message_Target', yesCallback, noCallback = function () { return; }) {

    var Modal_Template = "ConfermaModal_Template";
    var Modal_Content = "ConfermaModal_Content";
    var Modal_Id = "ConfermaModal";
    var Modal_Form_Button = "ConfermaFormButton";
    var Modal_Form_Button_Cancella = "CancellaFormButton";

    //elaboro il template mustache dentro la modale ancora nascosta
    const template = `
                            <div class="modal " tabindex="-1" role="dialog" id="ConfermaModal" aria-labelledby="modal4Title">
                                <div class="modal-dialog modal-dialog-centered" role="document">
                                    <div class="modal-content" id="ConfermaModal_Content">
                                        <div class="modal-header">
                                            <h2 class="modal-title h5 " id="modalCenterTitle">Conferma azione</h2>
                                        </div>
                                        <div class="modal-body">
                                            <p>{{data}}</p>
                                        </div>
                                        <div class="modal-footer">
                                            <button class="btn btn-primary btn-sm" type="button" id="ConfermaFormButton">Conferma</button>
                                            <button class="btn btn-outline-secondary btn-sm" type="button" id="CancellaFormButton">Annulla</button>
                                        </div>
                                    </div>
                                </div>
                            </div>`;

    const rendered = Mustache.render(template, { data: text });

    var div = document.createElement('div');
    div.innerHTML = rendered;

    //document.getElementById(Message_Target).innerHTML = rendered;
    document.body.appendChild(div);

    //mostro la modale
    var ConfermaModal = new bootstrap.Modal(document.getElementById(Modal_Id));
    ConfermaModal.show()

    var ConfirmButton = document.getElementById(Modal_Form_Button);
    ConfirmButton.onclick = async (e) => {
        if (yesCallback)
            yesCallback();
        ConfermaModal.hide();
    };
    var CancelButton = document.getElementById(Modal_Form_Button_Cancella);
    CancelButton.onclick = async (e) => {
        if (noCallback)
            noCallback();
        ConfermaModal.hide();
    };
}


function ShowRequiredPopupJS(formId) {
    //funzione per far comparire i popup manualmente in caso di campi non validi
    if (typeof formId === 'string')
    {
        var formHTML = document.getElementById(formId);
    } else
    {
        var formHTML = formId;
    }
    if (!formHTML) return true;
    // Seleziona tutti gli elementi input richiesti all'interno del div temporaneo, senza quelli nascosti
    // Itera sui campi di input richiesti

    var requiredhiddenInputs = $('[required]:hidden');
    requiredhiddenInputs.each((i, e) => { $(e).removeAttr("required") });

    var requiredhiddenInputs = $('[required]:disabled');
    requiredhiddenInputs.each((i, e) => { $(e).removeAttr("required") });

    var requiredInputs = formHTML.querySelectorAll('[required]');
    // Itera sui campi di input richiesti
    var notOk = 0;
    requiredInputs.forEach(function (inputElement) {
        var $el = $(inputElement);
        // Verifica la validità del campo di input o select
        var ok = $el.attr("disabled") || $el.attr("hidden") || (inputElement.tagName == "SELECT" ? inputElement.value : inputElement.checkValidity());
        if (!ok) {
            // Triggera l'evento "invalid" sull'elemento input non valido
            notOk++;
            inputElement.dispatchEvent(new Event('invalid'));
            try {
                inputElement.focus();
            } catch (e) {
            }
            // Imposta il valore vuoto per l'elemento input
            //if (inputElement.type != "radio" && inputElement.type != "checkbox")
            //    inputElement.value = '';
        }
    });
    return notOk == 0;
}

function getFormValues(formId) {
    if (typeof formId === 'string')
        var form = document.getElementById(formId);
    else
        var form = formId;
    if (!form) return;

    var inputs = form.querySelectorAll('input, select, textarea');
    var formData = {};

    for (var i = 0; i < inputs.length; i++) {
        var input = inputs[i];
        var name = input.getAttribute('name');
//        if (!name) {
//            name = input.Id;
//        }
        var value = input.value;
        // Trasforma la prima lettera della chiave in minuscolo
        if (name) {  // se l'elemento non ha l'attributo name non viene inserito --------!!!!!!!!!!!!!
            var lowercaseKey = name.charAt(0).toLowerCase() + name.slice(1);
            var type = input.getAttribute('type');
            if (type == "checkbox" || type == "radio") {
                if (input.checked)
                    formData[lowercaseKey] = value;
                else
                    if (formData[lowercaseKey] == undefined)
                        formData[lowercaseKey] = "";

            }
            else {
                formData[lowercaseKey] = value;
                if (input.tagName == "SELECT") {
                    formData[lowercaseKey + "_description"] = input.selectedIndex >= 0 ? input.item(input.selectedIndex).text : "";

                }
            }
        }
    }

    return formData;
}

function setFormTokens(formschema, variables) {

    Object.keys(variables).forEach((variable, i) => {
        formschema = formschema.replace(/variable/ig, variables[variable]);
    });
    return formschema; 
}

function setField(e, value, force) {

    if (e.getAttribute("type") == "checkbox" || e.getAttribute("type") == "radio") {
        var c = e.checked;
        e.checked = value.split(",").includes(e.value);
        if (e.checked)
            $(e).attr("checked", "checked");
        else
            $(e).removeAttr("checked");
        if (c != e.checked || force)
            e.dispatchEvent(new Event("change"));
    }
    else
        if (e.tagName == "INPUT") {
            if (e.value != value || force) { 
                e.value = value;
                e.setAttribute("value", value)
                $(e).attr("value", value);
                if (e.value != value) e.dispatchEvent(new Event("change"));
            }
        }
        else if (e.tagName == "TEXTAREA") {
            if (e.value != value || force) {
                e.value = value;
                e.innerHTML = value;
                if (e.value != value) e.dispatchEvent(new Event("change"));
            }
        }
        else if (e.tagName == "SELECT") {
            if (e.value != value || force) {
                var $opt = $(e).find("option[value='" + value + "']");
                if ($opt.length > 0) { 
                    e.value = value;
                    $opt.attr("selected", "selected");
                    if (e.value != value) e.dispatchEvent(new Event("change"));
                }
            }
        }
        else {
            e.innerHTML = value;
        }
}

function setFormValues(variables) {
    if (!variables || variables == {}) return;
    try {
        Object.keys(variables).forEach((variable, i) => {
            var el = $("#" + variable);
            if (el.length == 0) {
                el = $("[name='" + variable + "']");
            }
            if (el.length > 0) {
                el.forEach((e, i) => {
                    setField(e, variables[variable]);
                })
            }
        });
    } catch { };
}

function CheckRequiredPopupJS(formId) {
    //funzione per far comparire i popup manualmente in caso di campi non validi
    var formHTML = document.getElementById(formId);

    // Seleziona tutti gli elementi input richiesti all'interno del div temporaneo
    var requiredInputs = formHTML.querySelectorAll('input[required]');
    // Itera sui campi di input richiesti
    requiredInputs.forEach(function (inputElement) {
        // Verifica la validità del campo di input
        if (!inputElement.checkValidity()) {
            // Triggera l'evento "invalid" sull'elemento input non valido
            inputElement.dispatchEvent(new Event('invalid'));
            // Imposta il valore vuoto per l'elemento input
            inputElement.value = '';
        }
    });
}



// funzione per aggiungere giorni ana variabile Date. 
//es: new Date().addDays(1) restituirà la data di oggi avanti di un giorno
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function formatSize(value) {
    value = parseInt(value);
    let symbol = "B";
    if (value >= 1000) {
        symbol = "KB";
        value = Math.round(value / 10) / 100;
    }
    if (value >= 1000) {
        symbol = "MB";
        value = Math.round(value / 10) / 100;
    }
    if (value >= 1000) {
        symbol = "GB";
        value = Math.round(value / 10) / 100;
    }
    if (value >= 1000) {
        symbol = "TB";
        value = Math.round(value / 10) / 100;
    }
    let $value = value.toString().replace(".", ",")
    if (value >= 1000)
        $value = $value.substr(0, 1) + "." + $value.substr(1);
    return $value + " " + symbol;
}

function convertIntToDate(input) {
    var inputString = input.toString();
    if (inputString.length !== 8) {
        throw new Error("L'input deve essere un intero a 8 cifre nel formato 'yyyyMMdd'.");
    }

    var year = parseInt(inputString.substring(0, 4));
    var month = parseInt(inputString.substring(4, 6));
    var day = parseInt(inputString.substring(6, 8));

    var date = new Date(year, month - 1, day + 1);
    return date.toISOString().split('T')[0];
}

function ShowSuccess() {
    // [autore: Bosco]
    //funzione usata per mostrare i messaggi di successo nelle pagine che utilizzano knockoutJS
    setTimeout(function () {
        $("#successMessage").fadeIn(250).delay(3000);
    }, 100);
}
