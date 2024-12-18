
export function date (input, options) {

    var timeout = undefined;
    var hidden = true;
    var items = undefined;
    var tags = [];
    var lastLookup = "";
    var template = `
    <div id='{id}' class='lookup-container data-container' style='width:{width}'>
        <label class='{label-class}'>{label}</label>
        <div class='lookup-control'>
            <input type='date' {required} placeholder='{placeholder}' />
            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>
            <a href='#' class='lookup-search-button' tabindex=-1><i class='{search-icon}' title='{search-title}'></i></a>
        </div>
        <div class='lookup-search-box' style='display:none'>
            <ul class='lookup-values'>
            </ul>
        </div>
    </div>
    `;

   
    options = {} || options;  
    options.label = "" || input.getAttribute("label");
    options.required = input.getAttribute("required") == "true";
    options.placeholder = "" || input.getAttribute("placeholder");
    options.customProperties = "" || input.getAttribute("customProperties");
    options.width  = input.getAttribute("width") || "100%" ;
    var id = input.id+"_custominput";

    var control = template
    .replace('{id}',id)
    .replace('{label}',options.label)
    .replace('{required}',options.required)
    .replace('{width}',options.width)
    .replace('{label-class}',options.label ? "": "hidden")
    .replace('{placeholder}',options.placeholder)
    .replace('{lookup-loading-text}','')
    .replace('{clear-icon}','fa fa-times')
    .replace('{info-icon}','hidden fa fa-info-circle')
    .replace('{info-title}','')
    .replace('{search-icon}','fa fa-calendar')
    .replace('{search-title}','Cerca...');
    var e = input; //document.getElementById(input.id);
    var parent = e.parentNode;
    e.tabIndex = -1;
    e.style.display='none';
    e.insertAdjacentHTML('beforebegin',control);
    var cpanel = document.querySelector('#'+id);
    cpanel.addEventListener("focusout", (event => {
        if (!event.relatedTarget || event.relatedTarget.parentNode.className !='lookup-search-row')
            LostFocus();
    }))
    /* CREATE INPUT */
    var cinput = parent.querySelector('#'+id+' input');
    cinput.addEventListener('input', (event) => {
        var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
        btnclear.style.display = cinput.value ? "" :"none";  
        input.value=cinput.value;
    });
    // 4: observe changes on `hiddenInput`
    input.addEventListener('change', (event) => {
        cinput.value=input.value;
        Close();
    });
    /* CLEAR BUTTON */
    var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
    btnclear.style.display = "none";    
    btnclear.addEventListener('click', (event) => {
        Clear();
    });
    /* SEARCH */
    var btnsearch = parent.querySelector('#'+id+' a.lookup-search-button');
    btnsearch.addEventListener('click', (event) => {
        cinput.showPicker();
    });
   
    if (input.value) LookupData(input.value);

    var Close = () =>
    {
        var list = document.querySelector('#'+id+' .lookup-values');
        list.innerHTML = "";
        var box = document.querySelector('#'+id+' .lookup-search-box');
        box.style.display = 'none';       
        var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
        btnclear.style.display = cinput.value ? "" :"none";  
        hidden = true;
    }
  
    var Clear = () =>
    {
        input.value = "";
        input.dispatchEvent(new Event("change"));
    }
    var LostFocus = (i) =>
    {
        Close();
    }
}
