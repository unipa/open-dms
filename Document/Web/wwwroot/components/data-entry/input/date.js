var template = `
            <div id='{id}' class='lookup-container data-container' style='min-width:160px;max-width:{width}'>
                <label class='{label-class}'>{label}</label>
                <div class='lookup-control'>
                    <input type='date' autocomplete='off' class='lookup-input' {required} {disabled}  id='{input_id}_description' name='{input_name}_description' oninvalid="Data non valida" placeholder='{placeholder}' />
                    <input type='hidden' id='{input_id}_formatted' name='{input_name}_formatted' />
                </div>
            </div>
            `;

var _template = `
            <div id='{id}' class='lookup-container data-container' style='min-width:160px;max-width:{width}'>
                <label class='{label-class}'>{label}</label>
                <div class='lookup-control'>
                    <input type='date' autocomplete='off' class='lookup-input' {required} {disabled}  id='{input_id}_description' name='{input_name}_description' oninvalid="Data non valida" placeholder='{placeholder}' />
                    <a href='#' class='lookup-clear-button' {disabled} tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>
                    <a href='#' class='lookup-search-button' {disabled} tabindex=-1><i class='{search-icon}' title='{search-title}'></i></a>
                </div>
            </div>
            `;


export function date(input) {

    var hidden = false;
    var _id = input.id;
    var _name = input.name;
    if (_name == "") _name = _id;
    if (_id == "") _id = _name.replace(/\./ig, "_");
    var id = _id + "_custominput";
    var parent = input.parentNode;
    var formatted = _id + "_formatted";

    var options = {};
    options.label = input.getAttribute("label") || "";
    options.required = input.hasAttribute("required");
    options.disabled = input.hasAttribute("disabled");
    options.placeholder = input.getAttribute("placeholder") || "";
    options.customProperties = "" || input.getAttribute("customProperties");
    options.width = input.getAttribute("width") || "100%";
    options.minDate = input.getAttribute("minDate") || "";
    options.maxDate = input.getAttribute("maxDate") || "";
    options.minDateControl = input.getAttribute("minDateControl") || "";
    options.maxDateControl = input.getAttribute("maxDateControl") || "";
    options.minDate = parseDate(options.minDate);
    options.maxDate = parseDate(options.maxDate);

    addControl();

    var cpanel = document.querySelector('#' + id);
    var cinput = parent.querySelector('#' + id + ' input');
    //var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    //var btnsearch = parent.querySelector('#' + id + ' a.lookup-search-button');

    var Close = () => {
        if (cinput.value)
            $("#"+formatted).val(new Date(cinput.value).toLocaleDateString("it-IT"));
        else
            $("#" + formatted).val("__/__/____");

        //btnclear.style.visibility = cinput.value ? "visible" : "hidden";
        hidden = true;
    }
    var Clear = () => {
        input.value = "";
        if (options.minDate)
            input.value = options.minDate;
        input.dispatchEvent(new Event("change"));
    }
    var LostFocus = (i) => {
        Close();
    }


    addEvents();
    initialize();

    function initialize() {

        if (input.value) {
            //btnclear.style.visibility = "hidden";
            var a = input.value;//.getAttribute("value");
            if (a && a.length > 5) {
                if (a[4] != '-')
                    a = a.substr(0, 4) + "-" + a.substr(4)
                if (input.value[7] != '-')
                    a = a.substr(0, 7) + "-" + a.substr(7)
                input.value = a;
                input.dispatchEvent(new Event("change"));
            }
        }

    }
    function addEvents() {
        /* CLEAR BUTTON */
        //btnclear.addEventListener('click', (event) => {
        //    Clear();
        //});

        /* SEARCH */
        //btnsearch.addEventListener('click', (event) => {
        //    try {
        //        cinput.showPicker();
        //    } catch (ex ){
        //        //try {
        //        //    cinput.click();
        //        //} catch {
        //            alert("Questa funzionalità non è supportata dal tuo browser.\nPer aprire il calendario clicca direttamente sulla data\n\nErrore: "+ex)
        //        //}
        //    }
        //});

        cpanel.addEventListener("focusout", (event => {
            if (event.relatedTarget != null && event.relatedTarget != undefined && event.relatedTarget.parentNode.className != 'lookup-search-row')
                LostFocus();
        }))

        cinput.addEventListener('input', (event) => {
            //btnclear.style.visibility = cinput.value ? "visible" : "hidden";
        });
        cinput.addEventListener('change', (event) => {
            if (options.minDate && cinput.value && new Date(cinput.value) < new Date(options.minDate))
                cinput.value = options.minDate;
            if (options.maxDate && cinput.value && new Date(cinput.value) > new Date(options.maxDate))
                cinput.value = options.maxDate;

            if (options.minDateControl && cinput.value && new Date(cinput.value) < new Date($("[name='" + options.minDateControl + "']").val()))
                cinput.value = $("[name='" + options.minDateControl + "']").val();
            if (options.maxDateControl && cinput.value && new Date(cinput.value) > new Date($("[name='" + options.maxDateControl + "']").val()))
                cinput.value = $("[name='" + options.maxDateControl + "']").val();

            var ischanged = cinput.value != input.value;
            input.value = cinput.value;
            if (ischanged) input.dispatchEvent(new Event("change"));
            Close();
        });

        input.addEventListener('change', (event) => {
            if (input.value.length > 5) {
                if (input.value[4] != '-')
                    input.value = input.value.substr(0, 4) + "-" + input.value.substr(4)
                if (input.value[7] != '-')
                    input.value = input.value.substr(0, 7) + "-" + input.value.substr(7)
            }

            if (options.minDateControl && input.value && new Date(input.value) < new Date($("[name='" + options.minDateControl + "']").val()))
                input.value = $("[name='" + options.minDateControl + "']").val();
            if (options.maxDateControl && input.value && new Date(input.value) > new Date($("[name='" + options.maxDateControl + "']").val()))
                input.value = $("[name='" + options.maxDateControl + "']").val();

            if (options.minDate && input.value && new Date(input.value) < new Date(options.minDate))
                input.value = options.minDate;
            if (options.maxDate && input.value && new Date(input.value) > new Date(options.maxDate))
                input.value = options.maxDate;

            var ischanged = cinput.value != input.value;
            cinput.value = input.value;
            if (ischanged) cinput.dispatchEvent(new Event("change"));
            Close();
        });

    }
    function addControl() {

        if ($("#" + id).length > 0) return;

        var control = template
            .replace(/{id}/ig, id)
            .replace(/{input_name}/ig, _name)
            .replace(/{input_id}/ig, _id)
            .replace('{label}', options.label)
            .replace('{label-class}', options.label ? "" : "hidden")
            .replace('{required}', (!options.tag && options.required) ? "required" : "")
            .replace(/{disabled}/ig, (options.disabled) ? "disabled" : "")
            .replace('{width}', options.width)
            .replace('{placeholder}', options.placeholder)
            .replace('{lookup-loading-text}', '')
            .replace('{clear-icon}', 'fa fa-times')
            .replace('{info-icon}', 'hidden fa fa-info-circle')
            .replace('{info-title}', '')
            .replace('{search-icon}', 'fa fa-calendar')
            .replace('{search-title}', 'Cerca...');

        var e = input;
        e.tabIndex = -1;
        e.removeAttribute("required");
        e.style.display = 'none';
        e.insertAdjacentHTML('beforebegin', control);
    }
    function parseDate(value) {
        var stringDate = value
            .replace("YYYY", new Date().getFullYear())
            .replace("MM", new Date().getMonth() + 1)
            .replace("DD", new Date().getDate());
        let i = value.indexOf(":");
        // se c'è almeno un indicatore di somma o sottrazione, isolo la parte iniziale di data ed elaboro gli indicatori
        if (i >= 0) {
            stringDate = value.substr(0, i);
            value = value.substr(i);
        }
        // +1y
        // 0123
        // last = 3
        // size=2
        while (i > 0) {
            let multiplier = value[0] == "+" ? 1 : -1;
            value = value.substr(1);
            let d = new Date(stringDate);
            let last = value.indexOf("+");
            if (last < 0)
                last = value.indexOf("-");
            if (last < 0)
                last = value.length;

            var size = value.substr(last-1, 1).toLowerCase();
            if (size == "y") {
                var unit = parseInt(value.substr(0, last - 1));
                d = new Date(d.setFullYear(d.getFullYear() + unit * multiplier));
            }
            else {
                if (size == "m") {
                    var unit = parseInt(value.substr(0, last - 1));
                    d = new Date(d.setMonth(d.getMonth() + unit * multiplier));
                }
                else {
                    if (size == "d") {
                        var unit = parseInt(value.substr(0, last - 1));
                        d = new Date(d.setMonth(d.getMonth() + unit * multiplier));
                    }
                    else {
                        var unit = parseInt(value);
                        d = new Date(d.setDate(d.getDate() + unit * multiplier));
                    }
                }
            }
            stringDate = d.toISOString();
            if (last >= value.length)
                i = -1;
            else {
                value = value.substr(last);
                i = 1;
            }
        }
 
        return stringDate;
    }

}


//if (input.value) {
//    cinput.value = input.value;
//    cinput.dispatchEvent(new Event("change"));
//}

