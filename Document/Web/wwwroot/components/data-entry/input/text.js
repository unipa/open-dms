var templateSingleLine = `
    <div id='{id}' class='lookup-container' style='min-width:100px;max-width:{width}'>
        <label class='{label-class}'>{label}</label>
        <div class='lookup-control'>
            <div class="lookup-prefix">{prefix}</div>
            <div class="lookup-tags"></div>
            <input type='text' class='lookup-input' {required} '{pattern}' maxlength={len} placeholder='{placeholder}' />
            <div class="lookup-suffix">{suffix}</div>
            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>
        </div>
        <div class='lookup-search-box' style='display:none'>
            <ul class='lookup-values'>
            </ul>
        </div>
    </div>
    `;
var templateMultiLine = `
    <div id='{id}' class='lookup-container' style='width:{width}'>
        <label class='{label-class}'>{label}</label>
        <div class='lookup-control'>
            <div class="lookup-prefix">{prefix}</div>
            <div class="lookup-tags"></div>
            <textarea rows={rows} {required} '{pattern}' maxlength={len} placeholder='{placeholder}'></textarea>
            <div class="lookup-suffix">{suffix}</div>
            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>
        </div>
        <div class='lookup-search-box' style='display:none'>
            <ul class='lookup-values'>
            </ul>
        </div>
    </div>
    `;
var tag_template = `
    <div class='lookup-tag'>
        <span item='{value}' class='lookup-tag-value'>
            {lookupvalue}
        </span>
        <a href='#' item='{value}' class='lookup-tag-close'>
            <i class='fa fa-times'></i>
        </a>
    </div>
    `;


export function text (input) {

    var tags = [];

    var options = {};  
    options.label = input.getAttribute("label") || "";
    options.placeholder = input.getAttribute("placeholder") || "";
    options.prefix = input.getAttribute("prefix") || "";
    options.suffix = input.getAttribute("suffix") || "";
    options.width  = input.getAttribute("width") || "100%" ;
    options.pattern = input.getAttribute("pattern") || "";
    options.maxLength = input.getAttribute("maxLength") || "255";
    options.rows = input.getAttribute("rows") || "1";
    options.formatted = input.getAttribute("formatted") || "false";
    options.required = input.hasAttribute("required");
    options.tag = input.getAttribute("tag") == "true";
    try {
        options.customProperties = eval(input.getAttribute("customProperties") || "{}");
    } catch (e) {
        options.customProperties = {};
    }

    if (options.customProperties) {
        if (options.customProperties.rows)
            options.rows = options.customProperties.rows;
        if (options.customProperties.formatted)
            options.formatted = options.customProperties.formatted;
        if (options.customProperties.prefix)
            options.prefix = options.customProperties.prefix;
        if (options.customProperties.suffix)
            options.suffix = options.customProperties.suffix;
        if (options.customProperties.pattern)
            options.pattern = options.customProperties.pattern;
    }
    var _id = input.id;
    if (!_id) _id = input.name.replace(/\./ig, "_");
    var id = _id + "_custominput";
    var parent = input.parentNode;

    addControl();

    var cpanel = document.querySelector('#' + id);
    var cinput = parent.querySelector('#' + id + (options.rows > 1 ? ' textarea' : ' input'));
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
   
    var LookupData = (data) => {
        tags = [];
        if (options.tag)
        {
            var old = input.text;
            if (data)
                data.split(",").forEach((item,i)=>{
                    Select (item, false);
                });
            if (old != input.value )
                input.dispatchEvent(new Event("change"));
       }
        else
        cinput.value = data;
        Close();
    }
    var Close = () =>
    {
        var list = document.querySelector('#'+id+' .lookup-values');
        list.innerHTML = "";
        var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
        btnclear.style.display = cinput.value ? "" :"none";  
    }
    var Select = (e, trigger=true) =>
    {
        if (e)
        {
            var old = input.value;
            tags.push(e);
            input.value = tags.map(e => e.value).join(",");
            if (old != input.value && trigger)
                input.dispatchEvent(new Event("change"));

            cinput.value ="";
            ShowTags();
        }
        Close();
    }
    var Clear = () =>
    {
        tags = [];
        cinput.value = "";
        input.value = "";
        input.dispatchEvent(new Event("change"));
        ShowTags();
        Close();
    }
    var LostFocus = (i) =>
    {
        if (options.tag && cinput.value)
                Select(cinput.value);
    }
    var ShowTags = () => {
        var tagcontrols = "";
        (tags).forEach(element => {
            var tagc = tag_template
            // .replace(/{id}/g,element.id)
            .replace(/{value}/g,element)
            .replace('{icon}',"")
            .replace('{lookupvalue}',element)
            tagcontrols+= tagc;
        });
        var e = document.querySelector("#"+id+" .lookup-tags");
            e.innerHTML = tagcontrols;
        document.querySelectorAll(".lookup-tag-close").forEach(element => {
            var el = element.getAttribute("item");
            element.addEventListener("click", ()=>{
                let i = tags.findIndex(f=>f == el);
                tags.splice(i,1);
                input.value = tags.map(e=>e).join(",");
                ShowTags(); 
            });
        });      
    }

    addEvents();
    initialize();



    function initialize() {
        LookupData(input.value + "");
    }
    function addControl() {
        if ($("#" + id).length > 0) return;

        var control = (options.rows > 1 ? templateMultiLine : templateSingleLine)
            .replace('{id}', id)
            .replace('{prefix}', options.prefix)
            .replace('{suffix}', options.suffix)
            .replace('{label}', options.label)
            .replace('{label-class}', options.label ? "" : "hidden")
            .replace('{placeholder}', options.placeholder)
            .replace('{required}', options.required ? "required" : "")
            .replace('{width}', options.width)
            .replace('{pattern}', options.pattern)
            .replace('{len}', options.maxLength)
            .replace('{rows}', options.rows)
            .replace('{lookup-loading-text}', '')
            .replace('{clear-icon}', 'fa fa-times')
            .replace('{info-icon}', 'hidden fa fa-info-circle')
            .replace('{info-title}', '')
            .replace('{search-icon}', 'fa fa-clipboard')
            .replace('{search-title}', 'Cerca...');
        var e = input;
        e.tabIndex = -1;
        e.removeAttribute("required");
        e.style.display = 'none';
        e.insertAdjacentHTML('beforebegin', control);

    }
    function addEvents() {
        if (cpanel != null)
            cpanel.addEventListener("focusout", (event => {
                if (!event.relatedTarget || event.relatedTarget.parentNode.className != 'lookup-search-row')
                    LostFocus();
            }))
        /* CREATE INPUT */
        cinput.addEventListener('input', (event) => {
            btnclear.style.display = cinput.value ? "" : "none";
        });
        cinput.addEventListener('keyup', (event) => {
            if (event.keyCode == 13 && options.tag) {
                Select(cinput.value)
            }
        });
        cinput.addEventListener('change', (event) => {
            input.value = cinput.value;
            input.dispatchEvent(new Event("change"));
        });
        /* ORIGINAL INPUT */
        input.addEventListener('change', (event) => {
            LookupData(input.value);
        });
        /* CLEAR BUTTON */
        btnclear.addEventListener('click', (event) => {
            Clear();
        });
    }
}


            