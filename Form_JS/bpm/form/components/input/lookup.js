const observer = new MutationObserver((mutations) => {
    // 2: iterate over `MutationRecord` array
    mutations.forEach(mutation => {
        // 3.1: check if the mutation type and the attribute name match
        // 3.2: check if value changed
        if (
            mutation.type === 'attributes'
            && mutation.attributeName === 'value'
        ) {
            // 3.4: trigger `change` event
            mutation.target.dispatchEvent(new Event('change'));
        }
    });
});



export function lookup (input, options) {
    var url_search = "/api/ui/DataType/Search/{1}/{2}/{0}";
    var url_lookup = "/api/ui/DataType/Lookup/{1}/{2}/{0}";

    var timeout = undefined;
    var hidden = true;
    var items = undefined;
    var tags = [];
    var lastLookup = "";
    var template = `
    <div id='{id}' class='lookup-container' style='width:{width}'>
        <label class='{label-class}'>{label}</label>
        <div class='lookup-control'>
            <div class="lookup-tags"></div>
            <input type='text'  {required} placeholder='{placeholder}' />
            <span class='lookup-loading'>{lookup-loading-text}</span>
            <a href='#' class='lookup-info-button' tabindex=-1><i class='{info-icon}' title='{info-title}'></i></a>
            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>
            <a href='#' class='lookup-search-button' tabindex=-1><i class='{search-icon}' title='{search-title}'></i></a>
        </div>
        <div class='lookup-search-box' style='display:none'>
            <ul class='lookup-values'>
            </ul>
        </div>
    </div>
    `;

    var row_template = `
    <li id='{id}' class='lookup-search-row'>
        <a href='#' item={i}>
            <i class='{icon}'></i>
            {lookupvalue}
            <small>{value}</small>
        </a>
    </li>
    `;

    var tag_template = `
    <div class='lookup-tag'>
        <span item='{value}' class='lookup-tag-value'>
            <i class='{icon}'></i>
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
    options.width  = input.getAttribute("width") || "100%" ;
    options.tag = input.getAttribute("tag") == "true";
    options.maxResults = 8 || input.getAttribute("maxResults");
    options.type = "lookup" || input.getAttribute("type");
    options.tableId = "" || input.getAttribute("tableId");
    options.required = input.getAttribute("required") == "true";
    options.customProperties = "" || input.getAttribute("customProperties");
   var id = input.id+"_custominput";

    var control = template
    .replace('{id}',id)
    .replace('{label}',options.label)
    .replace('{label-class}',options.label ? "": "hidden")
    .replace('{required}',options.required)
    .replace('{width}',options.width)
    .replace('{placeholder}',options.placeholder)
    .replace('{lookup-loading-text}','')
    .replace('{clear-icon}','fa fa-times')
    .replace('{info-icon}','hidden fa fa-info-circle')
    .replace('{info-title}','')
    .replace('{search-icon}','fa fa-search')
    .replace('{search-title}','Cerca...');
    var e = input; //document.getElementById(input.id);
    var parent = e.parentNode;
    e.tabIndex = -1;
    //e.style.position='absolute';
    //e.style.left = '-1000px'; 
    e.style.display='none';
    //e.style.fontSize=0;
    e.insertAdjacentHTML('beforebegin',control);
    var cpanel = document.querySelector('#'+id);
    cpanel.addEventListener("focusout", (event => {
        if (!event.relatedTarget || event.relatedTarget.parentNode.className !='lookup-search-row')
            LostFocus();
    }))
    /* CREATE INPUT */
    var cinput = parent.querySelector('#'+id+' input');
    cinput.addEventListener('input', (event) => {
         if (timeout) clearTimeout(timeout);
        timeout = setTimeout(()=> {
            if (cinput.value)
            {
                let url =url_search.replace("{0}", cinput.value).replace("{1}", options.tableId).replace("{2}", options.maxResults);
                fetch(url)
                .then((response) => {
                        return response.json()
                }).then((data)=> {
                    items = data;
                    Render (items);
                });
            }
            else
            Clear();
    }, 125);

        var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
        btnclear.style.display = cinput.value ? "" :"none";  
        var btninfo = parent.querySelector('#'+id+' a.lookup-info-button');
        btninfo.style.display = "none";    
    });
    // 4: observe changes on `hiddenInput`
    input.addEventListener('change', (event) => {
        LookupData(input.value);
//        cinput.dispatchEvent(new Event("input"));
    });
    /* CLEAR BUTTON */
    var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
    var btninfo = parent.querySelector('#'+id+' a.lookup-info-button');
    btnclear.style.display = "none";    
    btninfo.style.display = "none";    
    btnclear.addEventListener('click', (event) => {
        input.value = "";
        input.dispatchEvent(new Event("change"));
        Close();
    });
    /* SEARCH */
    var btnsearch = parent.querySelector('#'+id+' a.lookup-search-button');
    btnsearch.addEventListener('click', (event) => {
        console.log("CERCA");
    });
   
    var LookupData = (data) => {
        tags = [];
        let url =url_lookup.replace("{0}", data).replace("{1}", options.tableId).replace("{2}", options.maxResults);
        fetch(url)
        .then((response) => {
                return response.json()
        }).then((data)=> {
        //items = data;
        data.forEach((e,i)=> {
            Select (e);
        });
    });
    }
    if (input.value) LookupData(input.value);

    var Render = (data) => {
        var ul = "<ul>";
        var i=0;
        if (data)
        (data).forEach(element => {
            ul+= row_template
            .replace(/{id}/g, id+"_"+i)
            .replace("{i}", i)
            .replace("{icon}", element.icon)
            .replace(/{value}/g, element.formattedValue)
            .replace("{lookupvalue}", element.lookupValue);
            i++;
        }
        );

        var list = document.querySelector('#'+id+' .lookup-values');
        list.innerHTML = ul;
        if (hidden)
        {
            var box = document.querySelector('#'+id+' .lookup-search-box');
            box.style.display = '';
            hidden = false;
        }
        document.querySelectorAll('#'+id+' .lookup-search-row a').forEach(element => {
            var i = element.getAttribute("item");
            element.addEventListener("click", ()=>{
                 Select(items[i]);
                 });
        });
    }
    var Close = () =>
    {
        var btninfo = parent.querySelector('#'+id+' a.lookup-info-button');
        btninfo.style.display = input.value && !options.tag ? "" : "none"; 

        var list = document.querySelector('#'+id+' .lookup-values');
        list.innerHTML = "";
        var box = document.querySelector('#'+id+' .lookup-search-box');
        box.style.display = 'none';       
        var btnclear = parent.querySelector('#'+id+' a.lookup-clear-button');
        btnclear.style.display = cinput.value ? "" :"none";  
        hidden = true;
    }
    var Select = (e) =>
    {
        hidden = true;
  
            if (options.tag)
            {
                if (e)
                {
                tags.push (e);
                input.value = tags.map(e=>e.formattedValue).join(",");
                cinput.value ="";
                lastLookup = "";
                ShowTags();
                }
            } else
            {
                if (e) {
                    input.value = e.formattedValue;
                cinput.value =e.lookupValue;
                lastLookup = e.lookupValue;
                } else
                {
                    input.value =  "";
                    cinput.value ="";
                    lastLookup = "";
                        
                }

            }
        
        Close();
    }
    var Clear = () =>
    {
        Select();
    }
    var LostFocus = (i) =>
    {
        if (!hidden)
            if (items.length == 1)
                Select(items[0]);
        if (!input.value)
            cinput.value = "";
        else
        cinput.value = lastLookup;
        Close();
    }
    var ShowTags = () => {
  var tagcontrols = "";
        (tags).forEach(element => {
            var tagc = tag_template
            // .replace(/{id}/g,element.id)
                .replace(/{value}/g, element.formattedValue)
            .replace('{icon}',element.icon)
            .replace('{lookupvalue}',element.lookupValue)
            tagcontrols+= tagc;
        });
        var e = document.querySelector("#"+id+" .lookup-tags");
            e.innerHTML = tagcontrols;
        document.querySelectorAll(".lookup-tag-close").forEach(element => {
            var el = element.getAttribute("item");
            element.addEventListener("click", ()=>{
                let i = tags.findIndex(f => f.formattedValue == el);
                tags.splice(i,1);
                input.value = tags.map(e => e.formattedValue).join(",");
                ShowTags(); 
            });
        });      
    }
}
