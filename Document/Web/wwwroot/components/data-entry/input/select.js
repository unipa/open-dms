var template = `
        <div id='{id}' class='lookup-container' style='width:{width}'>
            <label class='{label-class}'>{label}</label>
            <div class='lookup-control'>
                <select autocomplete='off' class='lookup-input' id='{input_id}_lookup' {required}></select>
                <input type='hidden' name='{input_name}_description' id='{input_id}_description' />
                <span class='hidden lookup-loading'>{lookup-loading-text}</span>
                <a href='#' class='lookup-info-button' tabindex=-1><i class='{info-icon}' title='{info-title}'></i></a>
                <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella' style="margin-left:-56px;"></i></a>

            </div>
            <div class='lookup-search-box' style='display:none'>
                <ul class='lookup-values'>
                </ul>
            </div>
        </div>
        `;
//<input type='text' autocomplete='off' class='lookup-input' id='{input_id}_lookup2' {required} placeholder='{placeholder}' />
//<a href='#' class='lookup-search-button' tabindex=- 1 > <i class='select-caret' title='{search-title}'></i></a >
var row_template = `
        <option id='{id}' value='{value}' class='lookup-search-row' item='{value}'>
                {lookupvalue}
        </option>
        `;

var xrow_template = `
        <li id='{id}' class='lookup-search-row'>
            <a href='#' item='{value}' tabindex='-1'>
                {icon}
                {lookupvalue}
                <small>{value}</small>
            </a>
        </li>
        `;

var tag_template = `
        <div class='lookup-tag'>
            <span item='{value}' class='lookup-tag-value'>
                {icon}
                {lookupvalue}
            </span>
            <a href='#' item='{value}' class='lookup-tag-close'>
                <i class='fa fa-times'></i>
            </a>
        </div>
        `;

var waitingItems = false;

export function select (input) {

    var timeout = undefined;
    var hidden = true;
    var items = undefined;
    var tags = [];
    var lastLookup = "";
    var parent = input.parentNode;

    var options = {};  
    options.label =  input.getAttribute("label") || "";
    options.placeholder =  input.getAttribute("placeholder") || "";
    options.width  = input.getAttribute("width") || "100%" ;
    options.tag = input.getAttribute("tag") || "";
    options.maxResults =  input.getAttribute("maxResults") || 100;
    options.type = input.getAttribute("type") || "select";
    options.required = input.hasAttribute("required");
    options.tableId = input.getAttribute("tableId") || "";
    options.customProperties = JSON.parse(input.getAttribute("customProperties") || "{}");
    options.items = eval(input.getAttribute("items") || "[]");
    if (options.customProperties) {
        if (options.customProperties.items)
            options.items = options.customProperties.items;
        if (options.customProperties.maxResults)
            options.maxResults = options.customProperties.maxResults;
    }
  

    var _id = input.id;
    var _name = input.name;
    if (!_name) _name = _id;
    if (!_id) _id = _name.replace(/\./ig, "_");
    var id = _id + "_custominput";


    addControl();

    var $linput = $("#" + _id + "_description");
    var cpanel = document.querySelector('#' + id);
    var cinput = parent.querySelector('#' + id + ' select');
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    var btnsearch = parent.querySelector('#' + id + ' a.lookup-search-button');
    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');

    if (options.items.length == 0) {
        waitingItems = true;
        var url_search = "/internalapi/ui/DataType/Search/{1}/{2}/{0}";
        let url = url_search.replace("{0}", escape("")).replace("{1}", escape(options.tableId)).replace("{2}", options.maxResults);
        fetch(url)
            .then((response) => {
                return response.json()
            }).then((data) => {
                options.items = data;
                //waitingItems = false;
                RenderListItems(data);
                addEvents();
                initialize();
            });
    }

    var RenderListItems = (data) => {
        items = data;
        $(cinput).empty();
        if (options.placeholder) {
            $(cinput).append("<option>" + options.placeholder +"</option>");
        } else {
            if (options.tag) {
                $(cinput).append("<option></option>");
            }

        }


        var ul = "";
        var i = 0;
        if (data) {
            if (options.tag) {
                data = data.filter(d => !tags.find(t => t.value.toLowerCase() == d.value.toLowerCase()));
            }
            (data).forEach(element => {
//                var icon = (element.icon && element.icon.length > 0) ? (element.icon.substring(0, 1) == "/" ? "<img src='" + element.icon + "' class='smallavatar'/>" : "<i class='" + element.icon + "'></i>") : "";
                ul = row_template
                    .replace(/{id}/g, id + "_" + i)
                    .replace("{i}", i)
                    .replace(/{value}/g, element.value)
                    .replace("{lookupvalue}", element.lookupValue);
                i++;
                $(cinput).append(ul);
            });
        }
    }

    //var RenderListItems = (data) => {
    //    items = data;
    //    var ul = "";
    //    var i = 0;
    //    if (data) {
    //        if (options.tag) {
    //            data = data.filter(d => !tags.find(t => t.value == d.value));
    //        }
    //        (data).forEach(element => {
    //            var icon = (element.icon && element.icon.length > 0) ? (element.icon.substring(0, 1) == "/" ? "<img src='" + element.icon + "' class='smallavatar'/>" : "<i class='" + element.icon + "'></i>") : "";
    //            ul += row_template
    //                .replace(/{id}/g, id + "_" + i)
    //                .replace("{i}", i)
    //                .replace("{icon}", icon)
    //                .replace(/{value}/g, element.value)
    //                .replace("{lookupvalue}", element.lookupValue);
    //            i++;
    //        });
    //    }
    //    var list = document.querySelector('#' + id + ' .lookup-values');
    //    list.innerHTML = ul;
    //    if (hidden) {
    //        var box = document.querySelector('#' + id + ' .lookup-search-box');
    //        box.style.display = '';
    //        hidden = false;
    //    }
    //    document.querySelectorAll('#' + id + ' .lookup-search-row a').forEach(element => {
    //        var i = element.getAttribute("item");

    //        element.addEventListener('keydown', (event) => {
    //            if (event.code == "ArrowUp") {
    //                var el = $(element).parent().prev();
    //                if (el.length == 0)
    //                    el = $(element).parent().siblings().last();
    //                $(el).find("a")[0].focus();
    //            } else
    //                if (event.code == "ArrowDown") {
    //                    var el = $(element).parent().next();
    //                    if (el.length == 0)
    //                        el = $(element).parent().siblings().first();
    //                    $(el).find("a")[0].focus();
    //                } else
    //                    if (event.code == "Tab" || event.code == "Escape") {
    //                        Close();
    //                    }

    //        });

    //        element.addEventListener("click", (e) => {
    //            Select(items.find(t => t.value == $(e.target).attr("item")));
    //        });
    //    });

    //}
    //var Close = () => {
    //    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
    //    if (btninfo) btninfo.style.display = input.value && !options.tag ? "" : "none";

    //    var list = document.querySelector('#' + id + ' .lookup-values');
    //    if (list) list.innerHTML = "";
    //    var box = document.querySelector('#' + id + ' .lookup-search-box');
    //    if (box) box.style.display = 'none';
    //    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    //    if (btnclear) btnclear.style.display = input.value ? "" : "none";
    //    hidden = true;
    //}
    var RenderTags = () => {
        var oldTags = document.querySelectorAll("#" + id + " .lookup-tag");
        var e = document.querySelector("#" + id + " .lookup-control");
        oldTags.forEach(e => { e.remove(); });
        (tags).forEach(element => {
            var icon = (element.icon && element.icon.length > 0) ? (element.icon.substring(0, 1) == "/" ? "<img src='" + element.icon + "' class='smallavatar'/>" : "<i class='" + element.icon + "'></i>") : "";
            var tagc = tag_template
                // .replace(/{id}/g,element.id)
                .replace(/{value}/g, element.formattedValue)
                .replace('{icon}', icon)
                .replace('{lookupvalue}', element.lookupValue.replace("<", "&lt;").replace(">", "&gt;"))
            var el = document.createElement("div");
            e.prepend(el);
            el.outerHTML = tagc;
        });
        input.value = tags.map(e => e.formattedValue).join(",");
        $linput.val(tags.map(e => e.lookupValue).join(","));
        if (tags.length > 0)
            $(cinput).removeAttr("required")
        else
            if (options.required)
                $(cinput).attr("required", "required")

        document.querySelectorAll(".lookup-tag-close").forEach(element => {
            var el = element.getAttribute("item");
            element.addEventListener("click", () => {
                let i = tags.findIndex(f => f.formattedValue.toLowerCase() == el.toLowerCase());
                tags.splice(i, 1);
                input.value = tags.map(e => e.formattedValue).join(",");
                input.dispatchEvent(new Event("change"));
                RenderTags();
            });
        });
        btnclear.style.display = input.value ? "" : "none";
        btninfo.style.display = "none";
    }
    var Select = (e, trigger = true) => {
        //hidden = true;
        if (options.tag) {
            if (e) {
                if (!tags.find(i => i.formattedValue.toLowerCase() == e.formattedValue.toLowerCase())) {
                    tags.push(e);
                }
            }
            else {
                tags = [];
            }
            $(cinput).val("");
            //lastLookup = "";
            RenderTags();
            //if (tags.length == options.items.length)
            //    Close()
            //else {
                cinput.focus();
            //    RenderListItems(options.items);
            //}
        } else {
            if (e) {
                input.value = e.formattedValue;
                $(cinput).val(e.formattedValue);
                //lastLookup = e.lookupValue;
                $linput.val(e.lookupValue);
            } else {
                input.value = "";
                $(cinput).val("");
                //lastLookup = "";
                $linput.val("");

            }
            //Close();
        }
    }
    var Clear = (show) => {
        input.value = "";
        $(cinput).val("");
        //lastLookup = "";
        tags = [];
        input.dispatchEvent(new Event("change"));
        RenderTags();
        //Close();
    }
    //var LostFocus = (i) => {
    //    if (!hidden)
    //        if (items.length == 1)
    //            Select(items[0]);
    //    cinput.value = lastLookup;
    //    Close();
    //}
    var LookupData = (data) => {
        var old = input.value;
        if (data)
            data.split(",").forEach((item, i) => {
                var values = options.items.filter(e => e.formattedValue.toLowerCase() == item.toLowerCase());
                if (values.length === 1)
                    Select(values[0],false);
            });
        if (old != input.value)
            input.dispatchEvent(new Event("change"));
        //lastLookup = cinput.value;
    }

    if (!waitingItems) {
        addEvents();
        initialize();
    }

    function initialize() {
        btnclear.style.display = "none";
        btninfo.style.display = "none";
        if (input.value) LookupData(input.value);
        //Close();
    }

    function addControl() {
        if ($("#" + id).length > 0) return;
        var control = template
            .replace(/{id}/ig, id)
            .replace(/{input_id}/ig, _id)
            .replace(/{input_name}/ig, _name)
            .replace('{width}', options.width)
            .replace('{label}', options.label)
            .replace('{label-class}', options.label ? "" : "hidden")
            .replace('{required}', (options.required) ? "required" : "")
            //.replace('{placeholder}', options.placeholder)
            .replace('{lookup-loading-text}', '')
            .replace('{clear-icon}', 'fa fa-times')
            .replace('{info-icon}', 'hidden fa fa-info-circle')
            .replace('{info-title}', '')
            .replace('{search-icon}', 'fa fa-caret-down')
            .replace('{search-title}', 'Cerca...');


        var e = input;
        e.tabIndex = -1;
        e.style.display = 'none';
        e.type = "hidden";
        e.removeAttribute("required");
        e.insertAdjacentHTML('beforebegin', control);

    }
    function addEvents() {
        /* CLEAR BUTTON */
        btnclear.addEventListener('click', (event) => {
            Clear();
        });
        /* SEARCH */
        //btnsearch.addEventListener('click', (event) => {
        //    if (hidden)
        //        RenderListItems(options.items);
        //    else
        //        Close();
        //});
        /* PANNELLO */
        //cpanel.addEventListener("focusout", (event => {
        //   if (event.relatedTarget != null && event.relatedTarget != undefined && event.relatedTarget.parentNode.className != 'lookup-search-row')
        //        LostFocus();
        //}))
        /* INPUT A VIDEO */
        //cinput.addEventListener('keyup', (event) => {
        //    if (!hidden && event.code == "ArrowUp") {
        //        var list = document.querySelectorAll('#' + id + ' .lookup-search-row a[item]');
        //        var el = list[list.length - 1];
        //        event = event || window.event;
        //        event.stopImmediatePropagation();
        //        event.cancelBubble = true;
        //        event.bubbles = false;
        //        el.focus();
        //    }
        //    if (!hidden && event.code == "ArrowDown") {
        //        var el = document.querySelectorAll('#' + id + ' .lookup-search-row a[item]')[0];
        //        el.focus();
        //        event = event || window.event;
        //        event.stopImmediatePropagation();
        //        event.cancelBubble = true;
        //        event.bubbles = false;
        //    }
        //});
        cinput.addEventListener('change', (event) => {
            //if (cinput.value) {
                //var data = options.items;
                //if (cinput.value) {
                //    var val = cinput.value.toLowerCase();
                //    data = data.filter(e => e.lookupValue.toLowerCase().indexOf(val) >= 0);
            //}
            cinput.selectedOptions.forEach((e, i) => {
                var val = e.value.toLowerCase();
                var el = options.items.find(d => d.value.toLowerCase() == val);
                Select(el, true);
            })
                //RenderListItems(data);
            //}
            btnclear.style.display = (tags.length > 0 || input.value) ? "" : "none";
            btninfo.style.display = "none";
        });

        //cinput.addEventListener('input', (event) => {
        //    if (cinput.value) {
        //        var data = options.items;
        //        if (cinput.value) {
        //            data = data.filter(e => e.lookupValue.indexOf(cinput.value) >= 0);
        //        }
        //        RenderListItems(data);
        //    }
        //    btnclear.style.display = (tags.length > 0 || cinput.value) ? "" : "none";
        //    btninfo.style.display = "none";
        //});
//        cinput.addEventListener("focusin", (event => {
//            if (event.relatedTarget != null && event.relatedTarget != undefined && event.relatedTarget.parentNode.className != 'lookup-search-row')
//                RenderListItems(options.items);
//        }))
//        cinput.addEventListener("focusout", (event => {
//           if (event.relatedTarget != null && event.relatedTarget != undefined && event.relatedTarget.parentNode.className != 'lookup-search-row')
//                LostFocus();
//        }))

        /* INPUT NASCOSTO */
        $(input).off("change").on("change", (event) => {
            tags = [];
            LookupData(input.value);

        });

    }
}
