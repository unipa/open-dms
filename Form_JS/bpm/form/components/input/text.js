
export function text (input, options) {

    var items = undefined;
    var tags = [];
    var templateSingleLine = `
    <div id='{id}' class='lookup-container' style='width:{width}'>
        <label class='{label-class}'>{label}</label>
        <div class='lookup-control'>
            <div class="lookup-prefix">{prefix}</div>
            <div class="lookup-tags"></div>
            <input type='text' {required} '{pattern}' maxlength={len} placeholder='{placeholder}' />
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
    options = {} || options;  
    options.label = "" || input.getAttribute("label");
    options.placeholder = "" || input.getAttribute("placeholder");
    options.prefix = input.getAttribute("prefix") || "";
    options.suffix = input.getAttribute("suffix") || "";
    options.width  = input.getAttribute("width") || "100%" ;
    options.pattern = input.getAttribute("pattern") || "";
    options.maxLength = input.getAttribute("maxLength") || "255";
    options.rows = input.getAttribute("rows") || "1";
    options.formatted = input.getAttribute("formatted") || "false";
    options.required = input.getAttribute("required") == "true";
    options.tag = input.getAttribute("tag") == "true";
    options.customProperties = "" || input.getAttribute("customProperties");
    var id = input.id+"_custominput";

    var control = (options.rows > 1 ? templateMultiLine : templateSingleLine)
    .replace('{id}',id)
    .replace('{prefix}',options.prefix)
    .replace('{suffix}',options.suffix)
    .replace('{label}',options.label)
    .replace('{label-class}',options.label ? "": "hidden")
    .replace('{placeholder}',options.placeholder)
    .replace('{required}',options.required)
    .replace('{width}',options.width)
    .replace('{pattern}',options.pattern)
    .replace('{len}',options.maxLength)
    .replace('{rows}',options.rows)
    .replace('{lookup-loading-text}','')
    .replace('{clear-icon}','fa fa-times')
    .replace('{info-icon}','hidden fa fa-info-circle')
    .replace('{info-title}','')
    .replace('{search-icon}','fa fa-clipboard')
    .replace('{search-title}','Cerca...');
    var e = input; 
    var parent = e.parentNode;
    e.tabIndex = -1;
    e.style.display='none';
    e.insertAdjacentHTML('beforebegin',control);
    var cpanel = document.querySelector('#'+id);
    /* CREATE INPUT */
    var cinput = parent.querySelector('#'+id+ (options.rows > 1 ? ' textarea' : ' input'));

    cinput.addEventListener('input', (event) => {
        var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
        btnclear.style.display = cinput.value ? "" :"none";  
        input.value=cinput.value;
    });
    cinput.addEventListener('keyup', (event) => {
        if (event.keyCode == 13 && options.tag)
        {
            Select (cinput.value)
        }
    });
    // 4: observe changes on `hiddenInput`
    input.addEventListener('change', (event) => {
        LookupData(input.value);
    });
    /* CLEAR BUTTON */
    var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
    btnclear.addEventListener('click', (event) => {
        Clear();
    });
   
    var LookupData = (data) => {
        tags = [];
        if (options.tag)
        {
            if (data)
            data.split(",").forEach((item,i)=>{
                    Select (item);
                });
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
    var Select = (e) =>
    {
        if (e)
        {
            tags.push (e);
            input.value = tags.map(e=>e.value).join(",");
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
    cpanel.addEventListener("focusout", (event => {
        if (!event.relatedTarget || event.relatedTarget.parentNode.className !='lookup-search-row')
            LostFocus();
    }))

    LookupData(input.value+"");
}
