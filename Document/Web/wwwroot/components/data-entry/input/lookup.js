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


var template = `
    <div id='{id}' class='lookup-container' style='width:{width}'  {disabled} >
        <label class='{label-class}'>{label}</label>
        <div class='lookup-control'>
            <i class='icon'></i>
            <input type='text' autocomplete='off' class='lookup-input' id='{input_id}_lookup' {disabled} {required} placeholder='{placeholder}' />
            <input type='hidden' name='{input_name}_description' id='{input_id}_description'  />
            <span class='hidden lookup-loading'>{lookup-loading-text}</span>
            <a href='#' class='lookup-info-button' tabindex=-1 {disabled} ><i class='{info-icon}' title='{info-title}'></i></a>
            <a href='#' class='lookup-clear-button' tabindex=-1 {disabled} ><i class='{clear-icon}' title='Cancella'></i></a>
            <a href='#' class='lookup-search-button' tabindex=-1 {disabled} ><i class='{search-icon}' title='{search-title}'></i></a>
            <a href='#' class='hidden lookup-wait-button' tabindex=-1 {disabled} ><i class='fa fa-refresh fa-spin' title='Attendere prego...'></i></a>
        </div>
        <div class='lookup-search-box' style='display:none'>
            <ul class='lookup-values'>
            </ul>
        </div>
    </div>
    `;

var row_template = `
    <li id='{id}' class='lookup-search-row' >
        <a href='#' item='{i}'  tabindex="-1">
            {icon}
            {lookupvalue}
            <small>{value}</small>
        </a>
    </li>
    `;

var submenu_template = `
    <li id='{id}' class='lookup-search-row'>
        <a href='#' item='{i}' tabindex="-1">
            {icon}
            {lookupvalue}
            <small>{value}</small>
        </a>
        <a class="lookup-switch" href="#" onclick="$(this).find('i').toggleClass('hidden'); $(this).next().toggleClass('hidden'); return false">
            <i class="fa fa-chevron-down"></i>
            <i class="fa fa-chevron-up hidden"></i>
        </a>
        <ul class="lookup-values lookup-subitems hidden">{children}</ul>
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


export function lookup (input) {
    var url_search = "/internalapi/ui/DataType/Search/{1}/{2}/{0}";
    var url_lookup = "/internalapi/ui/DataType/Lookup/{1}/{2}/{0}";

    var timeout = undefined;
    var hidden = true;
    var items = undefined;
    var tags = [];
    var lastLookup = "";
    var options = {};
  
    options.label = input.getAttribute("label") || "";
    options.placeholder = input.getAttribute("placeholder") || "";
    options.width  = input.getAttribute("width") || "100%" ;
    options.tag = input.getAttribute("tag") == "true";
    options.maxResults = input.getAttribute("maxResults") || 8;
    options.type = input.getAttribute("type") || "lookup";
    options.tableId = input.getAttribute("tableId");
    options.required = input.hasAttribute("required");
    options.disabled = input.hasAttribute("disabled");
    options.customProperties = input.getAttribute("customProperties") || "";



    var _id = input.id;
    var _name = input.name;
    if (!_name) _name = _id;
    if (!_id) _id = _name.replace(/\./ig, "_");
    var id = _id+ "_custominput";
    var parent = input.parentNode;

    addControl();

    var $linput = $("#" + _id + "_description");
    var $icon = $(parent).find(".icon");
    var cpanel = document.querySelector('#' + id);
    var cinput = parent.querySelector('#' + id + ' input');

    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
    var btnsearch = parent.querySelector('#' + id + ' a.lookup-search-button');
    var btnwait = parent.querySelector('#' + id + ' a.lookup-wait-button');


    var InternalSearch = () => {
        if (timeout) clearTimeout(timeout);

        timeout = setTimeout(() => {
            $(btnsearch).addClass("hidden");
            $(btnwait).removeClass("hidden");
            $(cinput).attr("readonly", "readonly");
            timeout = null;
            let url = url_search.replace("{0}", escape(cinput.value)).replace("{1}", escape(options.tableId)).replace("{2}", options.maxResults);
            fetch(url)
                .then((response) => {
                    return response.json()
                }).then((data) => {
                    items = data;
                    Render(items);
                    $(btnsearch).removeClass("hidden");
                    $(btnwait).addClass("hidden");
                    $(cinput).removeAttr("readonly");
                }).catch(() => {
                    $(btnsearch).removeClass("hidden");
                    $(btnwait).addClass("hidden");
                    $(cinput).removeAttr("readonly");
                })
        }, 250);
    }
    var LookupData = (data) => {
        if (!data) {
            input.dispatchEvent(new Event("change"));
            lastLookup = cinput.value;
        }
        let url = url_lookup.replace("{0}", escape(data)).replace("{1}", escape(options.tableId)).replace("{2}", options.maxResults);
        fetch(url)
            .then((response) => {
                return response.json()
            }).then((data) => {
                //items = data;
                var old = input.value;
                data.forEach((e, i) => {
                    Select(e, false);
                });
                if (old != input.value)
                    input.dispatchEvent(new Event("change"));
                lastLookup = cinput.value;

            });
    }
    var Render = (data) => {
        var ul = CreateList("", data);

        var list = document.querySelector('#' + id + ' .lookup-values');
        list.innerHTML = ul;
        if (hidden) {
            var box = document.querySelector('#' + id + ' .lookup-search-box');
            box.style.display = '';
            hidden = false;
        }
        document.querySelectorAll('#' + id + ' .lookup-search-row a[item]').forEach(element => {
            var index = element.getAttribute("item");
            var indexes = index.split(".");
            var item = items[indexes[0]];
            indexes.slice(1).forEach(i => {
                item = item.children[i];
            });

            $(element).off('keydown').on('keydown', (event) => {
                if (event.key == "ArrowUp") {
                    var el = $(element).parent().prev();
                    if (el.length == 0)
                        el = $(element).parent().siblings().last();
                    $(el).find("a")[0].focus();
                } else
                    if (event.key == "ArrowDown") {
                        var el = $(element).parent().next();
                        if (el.length == 0)
                            el = $(element).parent().siblings().first();
                        $(el).find("a")[0].focus();
                    } else
                        if (event.code == "Tab" || event.code == "Escape") {
                            Close();
                        }

            });

            $(element).off("click").on("click", () => {
                Select(item);
            });
        });
    }
    var Close = () => {
        btninfo.style.display = input.value && !options.tag ? "" : "none";
        btnclear.style.display = (tags.length > 0 || cinput.value) ? "" : "none";

        var list = document.querySelector('#' + id + ' .lookup-values');
        if (list) list.innerHTML = "";
        var box = document.querySelector('#' + id + ' .lookup-search-box');
        if (box) box.style.display = 'none';
        hidden = true;
    }
    var Select = (e, trigger = true) => {
        hidden = true;

        var old = input.value;
        if (options.tag) {
            if (e) {
                if (tags.findIndex(i => i.value == e.value) < 0) {
                    tags.push(e);
                    input.value = tags.map(e => e.formattedValue).join(",");
                }
            } else {
                tags = [];
                input.value = "";
            }
            cinput.value = "";
            lastLookup = "";
            RenderTags();
            cinput.focus();
        } else {
            if (e) {
                input.value = e.formattedValue;
                cinput.value = e.lookupValue;
                lastLookup = e.lookupValue;
                $linput.val(e.lookupValue);
                $icon.empty();
                var icon = (e.icon && e.icon.length > 0) ? (e.icon.substring(0, 1) == "/" ? "<img src='" + e.icon + "' class='smallavatar'/>" : "<i class='" + e.icon + "'></i>") : "";
                $icon.append(icon);
            } else {
                $icon.empty();
                input.value = "";
                cinput.value = "";
                lastLookup = "";
                $linput.val("");

            }

        }
        if (old != input.value && trigger)
            input.dispatchEvent(new Event("change"));

        Close();
    }
    var Clear = () => {
        Select();
    }
    var LostFocus = (i) => {
        if (!hidden) {
            if (cinput.value) {
                if (items.length == 1) {
                    Select(items[0]);
                }
                else {
                    cinput.value = lastLookup;
                    Close();
                }
            }
            else {
                Clear();
            }
        }
    }
    var RenderTags = () => {
        var oldTags = document.querySelectorAll("#" + id + " .lookup-tag");
        var e = document.querySelector("#" + id + " .lookup-control");
        oldTags.forEach(e => { e.remove(); });

        (tags).forEach(element => {
            var icon = (element.icon && element.icon.length > 0) ? (element.icon.substring(0, 1) == "/" ? "<img src='" + element.icon + "' class='smallavatar'/>" : "<i class='" + element.icon + "'></i>") : "";
            var tagc = tag_template
                .replace(/{value}/g, element.formattedValue)
                .replace('{icon}', icon)
                .replace('{lookupvalue}', element.lookupValue.replace("<", "&lt;").replace(">", "&gt;"))
            var el = document.createElement("div");
            e.prepend(el);
            el.outerHTML = tagc;
        });
        $linput.val(tags.map(e => e.lookupValue).join(","));
        input.value = (tags.map(e => e.formattedValue).join(","));
        if (tags.length > 0)
            $(cinput).removeAttr("required")
        else
            if (options.required)
                $(cinput).attr("required", null)

        //e.prepend(tagcontrols);
        //e.innerHTML = tagcontrols;
        document.querySelectorAll(".lookup-tag-close").forEach(element => {
            var el = element.getAttribute("item");
            $(element).off("click").on("click", () => {
                let i = tags.findIndex(f => f.formattedValue == el);
                tags.splice(i, 1);
                input.value = tags.map(e => e.formattedValue).join(",");
                input.dispatchEvent(new Event("change"));
                RenderTags();
            });
        });
    }

    addEvents();
    initialize();

    function initialize() {
        btnclear.style.display = "none";
        btninfo.style.display = "none";
        if (options.disabled) {
            btnsearch.style.display = "none";
            btnclear.classList.add("hidden");
            btninfo.classList.add("hidden");
        }

        if (input.value) LookupData(input.value);
    }
    function addControl() {
       if ($("#" + id).length > 0) return;
       var control = template
            .replace(/{id}/ig, id)
            .replace(/{input_id}/ig, _id)
            .replace(/{input_name}/ig, _name)
            .replace('{label}', options.label)
            .replace('{label-class}', options.label ? "" : "hidden")
            .replace('{required}', (options.required) ? "required" : "")
            .replace(/{disabled}/ig, (options.disabled) ? "disabled" : "")
            .replace('{width}', options.width)
            .replace('{placeholder}', options.placeholder)
            .replace('{lookup-loading-text}', '')
            .replace('{clear-icon}', 'fa fa-times')
            .replace('{info-icon}', 'hidden fa fa-info-circle')
            .replace('{info-title}', '')
            .replace('{search-icon}', 'fa fa-search')
            .replace('{search-title}', 'Cerca...');
        var e = input; //document.getElementById(input.id);
        e.tabIndex = -1;
        if (!options.required)
            e.removeAttribute("required");
        //e.style.position='absolute';
        //e.style.left = '-1000px'; 
        e.style.display = 'none';
        //e.style.fontSize=0;
        e.insertAdjacentHTML('beforebegin', control);

    }
    function addEvents() {
        if (!options.disabled) {
            btnclear.addEventListener('click', (event) => {
                Clear();
            });
            /* SEARCH */
            btnsearch.addEventListener('click', (event) => {
                InternalSearch();
            });
        }
        $(cpanel).off("focusout").on("focusout", (event => {
            if (event.relatedTarget != null && event.relatedTarget != undefined && event.relatedTarget.parentNode.className != 'lookup-search-row')
                LostFocus();
        }))

        /* INPUT CREATO */
        $(cinput).off('keyup').on('keyup', (event) => {
            if (!hidden && event.key == "ArrowUp") {
                var list = document.querySelectorAll('#' + id + ' .lookup-search-row a[item]');
                var el = list[list.length - 1];
                el.focus();
            }
            if (!hidden && event.key == "ArrowDown") {
                var el = document.querySelectorAll('#' + id + ' .lookup-search-row a[item]')[0];
                el.focus();
            }
        });
        $(cinput).off("focusout").on("focusout", (event => {
            if (event.relatedTarget != null && event.relatedTarget != undefined && event.relatedTarget.parentNode.className != 'lookup-search-row')
                LostFocus();
        }))
        $(cinput).off('input').on('input', (event) => {
            if (cinput.value) 
                InternalSearch();
            var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
            btnclear.style.display = input.value ? "" : "none";
            var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
            btninfo.style.display = "none";
        });

        /* INPUT ORIGINALE */
        $(input).off("change").on("change", (event) => {
            tags = [];
            LookupData(input.value);

        });
    }
    function CreateList(prefix, data) {
        var ul = "";
        var i = 0;
        if (data)
            (data).forEach(element => {
                var childs = "";
                if (element.children.length > 0)
                    childs = CreateList(prefix+i+".", element.children);

                var icon = (element.icon && element.icon.length > 0) ? (element.icon.substring(0, 1) == "/" ? "<img src='" + element.icon + "' class='smallavatar'/>" : "<i class='" + element.icon + "'></i>") : "";
                var lookupvalue = element.lookupValue ?? "";
                ul += (childs ? submenu_template : row_template)
                    .replace(/{id}/g, id + "_" + prefix + i)
                    .replace(/{i}/g, prefix+i)
                    .replace("{children}", childs)
                    .replace("{icon}", icon)
                    .replace(/{value}/g, element.formattedValue)
                    .replace("{lookupvalue}", lookupvalue.replace("<", "&lt;").replace(">", "&gt;"));
                i++;
            }
            );
        //ul += "</ul>";
        return ul;
    }

}
