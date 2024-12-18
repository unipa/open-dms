// modules are defined as an array
// [ module function, map of requires ]
//
// map of requires is short require name -> numeric require
//
// anything defined in a previous bundle is accessed via the
// orig method which is the require for previous bundles
parcelRequire = (function (modules, cache, entry, globalName) {
  // Save the require from previous bundle to this closure if any
  var previousRequire = typeof parcelRequire === 'function' && parcelRequire;
  var nodeRequire = typeof require === 'function' && require;

  function newRequire(name, jumped) {
    if (!cache[name]) {
      if (!modules[name]) {
        // if we cannot find the module within our internal map or
        // cache jump to the current global require ie. the last bundle
        // that was added to the page.
        var currentRequire = typeof parcelRequire === 'function' && parcelRequire;
        if (!jumped && currentRequire) {
          return currentRequire(name, true);
        }

        // If there are other bundles on this page the require from the
        // previous one is saved to 'previousRequire'. Repeat this as
        // many times as there are bundles until the module is found or
        // we exhaust the require chain.
        if (previousRequire) {
          return previousRequire(name, true);
        }

        // Try the node require function if it exists.
        if (nodeRequire && typeof name === 'string') {
          return nodeRequire(name);
        }

        var err = new Error('Cannot find module \'' + name + '\'');
        err.code = 'MODULE_NOT_FOUND';
        throw err;
      }

      localRequire.resolve = resolve;
      localRequire.cache = {};

      var module = cache[name] = new newRequire.Module(name);

      modules[name][0].call(module.exports, localRequire, module, module.exports, this);
    }

    return cache[name].exports;

    function localRequire(x){
      return newRequire(localRequire.resolve(x));
    }

    function resolve(x){
      return modules[name][1][x] || x;
    }
  }

  function Module(moduleName) {
    this.id = moduleName;
    this.bundle = newRequire;
    this.exports = {};
  }

  newRequire.isParcelRequire = true;
  newRequire.Module = Module;
  newRequire.modules = modules;
  newRequire.cache = cache;
  newRequire.parent = previousRequire;
  newRequire.register = function (id, exports) {
    modules[id] = [function (require, module) {
      module.exports = exports;
    }, {}];
  };

  var error;
  for (var i = 0; i < entry.length; i++) {
    try {
      newRequire(entry[i]);
    } catch (e) {
      // Save first error but execute all entries
      if (!error) {
        error = e;
      }
    }
  }

  if (entry.length) {
    // Expose entry point to Node, AMD or browser globals
    // Based on https://github.com/ForbesLindesay/umd/blob/master/template.js
    var mainExports = newRequire(entry[entry.length - 1]);

    // CommonJS
    if (typeof exports === "object" && typeof module !== "undefined") {
      module.exports = mainExports;

    // RequireJS
    } else if (typeof define === "function" && define.amd) {
     define(function () {
       return mainExports;
     });

    // <script>
    } else if (globalName) {
      this[globalName] = mainExports;
    }
  }

  // Override the current require with this new one
  parcelRequire = newRequire;

  if (error) {
    // throw error from earlier, _after updating parcelRequire_
    throw error;
  }

  return newRequire;
})({"bpm/form/components/input/lookup.js":[function(require,module,exports) {
"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.lookup = lookup;
var observer = new MutationObserver(function (mutations) {
  // 2: iterate over `MutationRecord` array
  mutations.forEach(function (mutation) {
    // 3.1: check if the mutation type and the attribute name match
    // 3.2: check if value changed
    if (mutation.type === 'attributes' && mutation.attributeName === 'value') {
      // 3.4: trigger `change` event
      mutation.target.dispatchEvent(new Event('change'));
    }
  });
});
function lookup(input, options) {
  var url_search = "/api/ui/DataType/Search/{1}/{2}/{0}";
  var url_lookup = "/api/ui/DataType/Lookup/{1}/{2}/{0}";
  var timeout = undefined;
  var hidden = true;
  var items = undefined;
  var tags = [];
  var lastLookup = "";
  var template = "\n    <div id='{id}' class='lookup-container' style='width:{width}'>\n        <label class='{label-class}'>{label}</label>\n        <div class='lookup-control'>\n            <div class=\"lookup-tags\"></div>\n            <input type='text'  {required} placeholder='{placeholder}' />\n            <span class='lookup-loading'>{lookup-loading-text}</span>\n            <a href='#' class='lookup-info-button' tabindex=-1><i class='{info-icon}' title='{info-title}'></i></a>\n            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>\n            <a href='#' class='lookup-search-button' tabindex=-1><i class='{search-icon}' title='{search-title}'></i></a>\n        </div>\n        <div class='lookup-search-box' style='display:none'>\n            <ul class='lookup-values'>\n            </ul>\n        </div>\n    </div>\n    ";
  var row_template = "\n    <li id='{id}' class='lookup-search-row'>\n        <a href='#' item={i}>\n            <i class='{icon}'></i>\n            {lookupvalue}\n            <small>{value}</small>\n        </a>\n    </li>\n    ";
  var tag_template = "\n    <div class='lookup-tag'>\n        <span item='{value}' class='lookup-tag-value'>\n            <i class='{icon}'></i>\n            {lookupvalue}\n        </span>\n        <a href='#' item='{value}' class='lookup-tag-close'>\n            <i class='fa fa-times'></i>\n        </a>\n    </div>\n    ";
  options = {} || options;
  options.label = "" || input.getAttribute("label");
  options.placeholder = "" || input.getAttribute("placeholder");
  options.width = input.getAttribute("width") || "100%";
  options.tag = input.getAttribute("tag") == "true";
  options.maxResults = 8 || input.getAttribute("maxResults");
  options.type = "lookup" || input.getAttribute("type");
  options.tableId = "" || input.getAttribute("tableId");
  options.required = input.getAttribute("required") == "true";
  options.customProperties = "" || input.getAttribute("customProperties");
  var id = input.id + "_custominput";
  var control = template.replace('{id}', id).replace('{label}', options.label).replace('{label-class}', options.label ? "" : "hidden").replace('{required}', options.required).replace('{width}', options.width).replace('{placeholder}', options.placeholder).replace('{lookup-loading-text}', '').replace('{clear-icon}', 'fa fa-times').replace('{info-icon}', 'hidden fa fa-info-circle').replace('{info-title}', '').replace('{search-icon}', 'fa fa-search').replace('{search-title}', 'Cerca...');
  var e = input; //document.getElementById(input.id);
  var parent = e.parentNode;
  e.tabIndex = -1;
  //e.style.position='absolute';
  //e.style.left = '-1000px'; 
  e.style.display = 'none';
  //e.style.fontSize=0;
  e.insertAdjacentHTML('beforebegin', control);
  var cpanel = document.querySelector('#' + id);
  cpanel.addEventListener("focusout", function (event) {
    if (!event.relatedTarget || event.relatedTarget.parentNode.className != 'lookup-search-row') LostFocus();
  });
  /* CREATE INPUT */
  var cinput = parent.querySelector('#' + id + ' input');
  cinput.addEventListener('input', function (event) {
    if (timeout) clearTimeout(timeout);
    timeout = setTimeout(function () {
      if (cinput.value) {
        var url = url_search.replace("{0}", cinput.value).replace("{1}", options.tableId).replace("{2}", options.maxResults);
        fetch(url).then(function (response) {
          return response.json();
        }).then(function (data) {
          items = data;
          Render(items);
        });
      } else Clear();
    }, 125);
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
    btninfo.style.display = "none";
  });
  // 4: observe changes on `hiddenInput`
  input.addEventListener('change', function (event) {
    LookupData(input.value);
    //        cinput.dispatchEvent(new Event("input"));
  });
  /* CLEAR BUTTON */
  var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
  var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
  btnclear.style.display = "none";
  btninfo.style.display = "none";
  btnclear.addEventListener('click', function (event) {
    input.value = "";
    input.dispatchEvent(new Event("change"));
    Close();
  });
  /* SEARCH */
  var btnsearch = parent.querySelector('#' + id + ' a.lookup-search-button');
  btnsearch.addEventListener('click', function (event) {
    console.log("CERCA");
  });
  var LookupData = function LookupData(data) {
    tags = [];
    var url = url_lookup.replace("{0}", data).replace("{1}", options.tableId).replace("{2}", options.maxResults);
    fetch(url).then(function (response) {
      return response.json();
    }).then(function (data) {
      //items = data;
      data.forEach(function (e, i) {
        Select(e);
      });
    });
  };
  if (input.value) LookupData(input.value);
  var Render = function Render(data) {
    var ul = "<ul>";
    var i = 0;
    if (data) data.forEach(function (element) {
      ul += row_template.replace(/{id}/g, id + "_" + i).replace("{i}", i).replace("{icon}", element.icon).replace(/{value}/g, element.formattedValue).replace("{lookupvalue}", element.lookupValue);
      i++;
    });
    var list = document.querySelector('#' + id + ' .lookup-values');
    list.innerHTML = ul;
    if (hidden) {
      var box = document.querySelector('#' + id + ' .lookup-search-box');
      box.style.display = '';
      hidden = false;
    }
    document.querySelectorAll('#' + id + ' .lookup-search-row a').forEach(function (element) {
      var i = element.getAttribute("item");
      element.addEventListener("click", function () {
        Select(items[i]);
      });
    });
  };
  var Close = function Close() {
    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
    btninfo.style.display = input.value && !options.tag ? "" : "none";
    var list = document.querySelector('#' + id + ' .lookup-values');
    list.innerHTML = "";
    var box = document.querySelector('#' + id + ' .lookup-search-box');
    box.style.display = 'none';
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    hidden = true;
  };
  var Select = function Select(e) {
    hidden = true;
    if (options.tag) {
      if (e) {
        tags.push(e);
        input.value = tags.map(function (e) {
          return e.formattedValue;
        }).join(",");
        cinput.value = "";
        lastLookup = "";
        ShowTags();
      }
    } else {
      if (e) {
        input.value = e.formattedValue;
        cinput.value = e.lookupValue;
        lastLookup = e.lookupValue;
      } else {
        input.value = "";
        cinput.value = "";
        lastLookup = "";
      }
    }
    Close();
  };
  var Clear = function Clear() {
    Select();
  };
  var LostFocus = function LostFocus(i) {
    if (!hidden) if (items.length == 1) Select(items[0]);
    if (!input.value) cinput.value = "";else cinput.value = lastLookup;
    Close();
  };
  var ShowTags = function ShowTags() {
    var tagcontrols = "";
    tags.forEach(function (element) {
      var tagc = tag_template
      // .replace(/{id}/g,element.id)
      .replace(/{value}/g, element.formattedValue).replace('{icon}', element.icon).replace('{lookupvalue}', element.lookupValue);
      tagcontrols += tagc;
    });
    var e = document.querySelector("#" + id + " .lookup-tags");
    e.innerHTML = tagcontrols;
    document.querySelectorAll(".lookup-tag-close").forEach(function (element) {
      var el = element.getAttribute("item");
      element.addEventListener("click", function () {
        var i = tags.findIndex(function (f) {
          return f.formattedValue == el;
        });
        tags.splice(i, 1);
        input.value = tags.map(function (e) {
          return e.formattedValue;
        }).join(",");
        ShowTags();
      });
    });
  };
}
},{}],"bpm/form/components/input/select.js":[function(require,module,exports) {
"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.select = select;
function select(input, options) {
  var timeout = undefined;
  var hidden = true;
  var items = undefined;
  var tags = [];
  var lastLookup = "";
  var template = "\n    <div id='{id}' class='lookup-container' style='width:{width}'>\n        <label class='{label-class}'>{label}</label>\n        <div class='lookup-control'>\n            <div class=\"lookup-tags\"></div>\n            <input type='text'  {required} placeholder='{placeholder}' />\n            <span class='lookup-loading'>{lookup-loading-text}</span>\n            <a href='#' class='lookup-info-button' tabindex=-1><i class='{info-icon}' title='{info-title}'></i></a>\n            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>\n            <a href='#' class='lookup-search-button' tabindex=-1><i class='{search-icon}' title='{search-title}'></i></a>\n        </div>\n        <div class='lookup-search-box' style='display:none'>\n            <ul class='lookup-values'>\n            </ul>\n        </div>\n    </div>\n    ";
  var row_template = "\n    <li id='{id}' class='lookup-search-row'>\n        <a href='#' item={i}>\n            <i class='{icon}'></i>\n            {lookupvalue}\n            <small>{value}</small>\n        </a>\n    </li>\n    ";
  var tag_template = "\n    <div class='lookup-tag'>\n        <span item='{value}' class='lookup-tag-value'>\n            <i class='{icon}'></i>\n            {lookupvalue}\n        </span>\n        <a href='#' item='{value}' class='lookup-tag-close'>\n            <i class='fa fa-times'></i>\n        </a>\n    </div>\n    ";
  options = {} || options;
  options.label = "" || input.getAttribute("label");
  options.placeholder = "" || input.getAttribute("placeholder");
  options.width = input.getAttribute("width") || "100%";
  options.tag = "" || input.getAttribute("tag");
  options.maxResults = 8 || input.getAttribute("maxResults");
  options.type = "lookup" || input.getAttribute("type");
  options.required = input.getAttribute("required") == "true";
  options.tableId = "" || input.getAttribute("tableId");
  options.customProperties = "" || input.getAttribute("customProperties");
  options.items = eval(input.getAttribute("items") || "[]");
  var id = input.id + "_custominput";
  var control = template.replace('{id}', id).replace('{width}', options.width).replace('{label}', options.label).replace('{label-class}', options.label ? "" : "hidden").replace('{required}', options.required).replace('{placeholder}', options.placeholder).replace('{lookup-loading-text}', '').replace('{clear-icon}', 'fa fa-times').replace('{info-icon}', 'hidden fa fa-info-circle').replace('{info-title}', '').replace('{search-icon}', 'fa fa-chevron-down').replace('{search-title}', 'Cerca...');
  var e = input; //document.getElementById(input.id);
  var parent = e.parentNode;
  e.tabIndex = -1;
  //e.style.position='absolute';
  //e.style.left = '-1000px'; 
  e.style.display = 'none';
  //e.style.fontSize=0;
  e.insertAdjacentHTML('beforebegin', control);
  var cpanel = document.querySelector('#' + id);
  cpanel.addEventListener("focusout", function (event) {
    if (!event.relatedTarget || event.relatedTarget.parentNode.className != 'lookup-search-row') LostFocus();
  });
  /* CREATE INPUT */
  var cinput = parent.querySelector('#' + id + ' input');
  cinput.addEventListener('input', function (event) {
    if (cinput.value) {
      var data = options.items;
      if (cinput.value) {
        data = data.filter(function (e) {
          return e.lookupValue.indexOf(cinput.value) >= 0;
        });
      }
      Render(data);
    } else Clear();
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
    btninfo.style.display = "none";
  });
  // 4: observe changes on `hiddenInput`
  input.addEventListener('change', function (event) {
    LookupData(input.value);
    //        cinput.dispatchEvent(new Event("input"));
  });
  /* CLEAR BUTTON */
  var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
  var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
  btnclear.style.display = "none";
  btninfo.style.display = "none";
  btnclear.addEventListener('click', function (event) {
    input.value = "";
    input.dispatchEvent(new Event("change"));
    Close();
  });
  /* SEARCH */
  var btnsearch = parent.querySelector('#' + id + ' a.lookup-search-button');
  btnsearch.addEventListener('click', function (event) {
    if (hidden) Render(options.items);else Close();
  });
  var LookupData = function LookupData(data) {
    tags = [];
    if (data) data.split(",").forEach(function (item, i) {
      values = options.items.filter(function (e) {
        return e.value === item;
      });
      if (values.length === 1) Select(values[0]);
    });
  };
  if (input.value) LookupData(input.value);
  var Render = function Render(data) {
    var ul = "<ul>";
    items = data;
    var i = 0;
    if (data) data.forEach(function (element) {
      ul += row_template.replace(/{id}/g, id + "_" + i).replace("{i}", i).replace("{icon}", element.icon).replace(/{value}/g, element.value).replace("{lookupvalue}", element.lookupValue);
      i++;
    });
    var list = document.querySelector('#' + id + ' .lookup-values');
    list.innerHTML = ul;
    if (hidden) {
      var box = document.querySelector('#' + id + ' .lookup-search-box');
      box.style.display = '';
      hidden = false;
    }
    document.querySelectorAll('#' + id + ' .lookup-search-row a').forEach(function (element) {
      var i = element.getAttribute("item");
      element.addEventListener("click", function () {
        Select(items[i]);
      });
    });
  };
  var Close = function Close() {
    var btninfo = parent.querySelector('#' + id + ' a.lookup-info-button');
    btninfo.style.display = input.value && !options.tag ? "" : "none";
    var list = document.querySelector('#' + id + ' .lookup-values');
    list.innerHTML = "";
    var box = document.querySelector('#' + id + ' .lookup-search-box');
    box.style.display = 'none';
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    hidden = true;
  };
  var Select = function Select(e) {
    hidden = true;
    if (options.tag) {
      if (e) {
        tags.push(e);
        input.value = tags.map(function (e) {
          return e.value;
        }).join(",");
        cinput.value = "";
        lastLookup = "";
        ShowTags();
      }
    } else {
      if (e) {
        input.value = e.value;
        cinput.value = e.lookupValue;
        lastLookup = e.lookupValue;
      } else {
        input.value = "";
        cinput.value = "";
        lastLookup = "";
      }
    }
    Close();
  };
  var Clear = function Clear() {
    Select();
  };
  var LostFocus = function LostFocus(i) {
    if (!hidden) if (items.length == 1) Select(items[0]);
    if (!input.value) cinput.value = "";else cinput.value = lastLookup;
    Close();
  };
  var ShowTags = function ShowTags() {
    var tagcontrols = "";
    tags.forEach(function (element) {
      var tagc = tag_template
      // .replace(/{id}/g,element.id)
      .replace(/{value}/g, element.value).replace('{icon}', element.icon).replace('{lookupvalue}', element.lookupValue);
      tagcontrols += tagc;
    });
    var e = document.querySelector("#" + id + " .lookup-tags");
    e.innerHTML = tagcontrols;
    document.querySelectorAll(".lookup-tag-close").forEach(function (element) {
      var el = element.getAttribute("item");
      element.addEventListener("click", function () {
        var i = tags.findIndex(function (f) {
          return f.value == el;
        });
        tags.splice(i, 1);
        input.value = tags.map(function (e) {
          return e.value;
        }).join(",");
        ShowTags();
      });
    });
  };
}
},{}],"bpm/form/components/input/date.js":[function(require,module,exports) {
"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.date = date;
function date(input, options) {
  var timeout = undefined;
  var hidden = true;
  var items = undefined;
  var tags = [];
  var lastLookup = "";
  var template = "\n    <div id='{id}' class='lookup-container data-container' style='width:{width}'>\n        <label class='{label-class}'>{label}</label>\n        <div class='lookup-control'>\n            <input type='date' {required} placeholder='{placeholder}' />\n            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>\n            <a href='#' class='lookup-search-button' tabindex=-1><i class='{search-icon}' title='{search-title}'></i></a>\n        </div>\n        <div class='lookup-search-box' style='display:none'>\n            <ul class='lookup-values'>\n            </ul>\n        </div>\n    </div>\n    ";
  options = {} || options;
  options.label = "" || input.getAttribute("label");
  options.required = input.getAttribute("required") == "true";
  options.placeholder = "" || input.getAttribute("placeholder");
  options.customProperties = "" || input.getAttribute("customProperties");
  options.width = input.getAttribute("width") || "100%";
  var id = input.id + "_custominput";
  var control = template.replace('{id}', id).replace('{label}', options.label).replace('{required}', options.required).replace('{width}', options.width).replace('{label-class}', options.label ? "" : "hidden").replace('{placeholder}', options.placeholder).replace('{lookup-loading-text}', '').replace('{clear-icon}', 'fa fa-times').replace('{info-icon}', 'hidden fa fa-info-circle').replace('{info-title}', '').replace('{search-icon}', 'fa fa-calendar').replace('{search-title}', 'Cerca...');
  var e = input; //document.getElementById(input.id);
  var parent = e.parentNode;
  e.tabIndex = -1;
  e.style.display = 'none';
  e.insertAdjacentHTML('beforebegin', control);
  var cpanel = document.querySelector('#' + id);
  cpanel.addEventListener("focusout", function (event) {
    if (!event.relatedTarget || event.relatedTarget.parentNode.className != 'lookup-search-row') LostFocus();
  });
  /* CREATE INPUT */
  var cinput = parent.querySelector('#' + id + ' input');
  cinput.addEventListener('input', function (event) {
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    input.value = cinput.value;
  });
  // 4: observe changes on `hiddenInput`
  input.addEventListener('change', function (event) {
    cinput.value = input.value;
    Close();
  });
  /* CLEAR BUTTON */
  var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
  btnclear.style.display = "none";
  btnclear.addEventListener('click', function (event) {
    Clear();
  });
  /* SEARCH */
  var btnsearch = parent.querySelector('#' + id + ' a.lookup-search-button');
  btnsearch.addEventListener('click', function (event) {
    cinput.showPicker();
  });
  if (input.value) LookupData(input.value);
  var Close = function Close() {
    var list = document.querySelector('#' + id + ' .lookup-values');
    list.innerHTML = "";
    var box = document.querySelector('#' + id + ' .lookup-search-box');
    box.style.display = 'none';
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    hidden = true;
  };
  var Clear = function Clear() {
    input.value = "";
    input.dispatchEvent(new Event("change"));
  };
  var LostFocus = function LostFocus(i) {
    Close();
  };
}
},{}],"bpm/form/components/input/text.js":[function(require,module,exports) {
"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.text = text;
function text(input, options) {
  var items = undefined;
  var tags = [];
  var templateSingleLine = "\n    <div id='{id}' class='lookup-container' style='width:{width}'>\n        <label class='{label-class}'>{label}</label>\n        <div class='lookup-control'>\n            <div class=\"lookup-prefix\">{prefix}</div>\n            <div class=\"lookup-tags\"></div>\n            <input type='text' {required} '{pattern}' maxlength={len} placeholder='{placeholder}' />\n            <div class=\"lookup-suffix\">{suffix}</div>\n            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>\n        </div>\n        <div class='lookup-search-box' style='display:none'>\n            <ul class='lookup-values'>\n            </ul>\n        </div>\n    </div>\n    ";
  var templateMultiLine = "\n    <div id='{id}' class='lookup-container' style='width:{width}'>\n        <label class='{label-class}'>{label}</label>\n        <div class='lookup-control'>\n            <div class=\"lookup-prefix\">{prefix}</div>\n            <div class=\"lookup-tags\"></div>\n            <textarea rows={rows} {required} '{pattern}' maxlength={len} placeholder='{placeholder}'></textarea>\n            <div class=\"lookup-suffix\">{suffix}</div>\n            <a href='#' class='lookup-clear-button' tabindex=-1><i class='{clear-icon}' title='Cancella'></i></a>\n        </div>\n        <div class='lookup-search-box' style='display:none'>\n            <ul class='lookup-values'>\n            </ul>\n        </div>\n    </div>\n    ";
  var tag_template = "\n    <div class='lookup-tag'>\n        <span item='{value}' class='lookup-tag-value'>\n            {lookupvalue}\n        </span>\n        <a href='#' item='{value}' class='lookup-tag-close'>\n            <i class='fa fa-times'></i>\n        </a>\n    </div>\n    ";
  options = {} || options;
  options.label = "" || input.getAttribute("label");
  options.placeholder = "" || input.getAttribute("placeholder");
  options.prefix = input.getAttribute("prefix") || "";
  options.suffix = input.getAttribute("suffix") || "";
  options.width = input.getAttribute("width") || "100%";
  options.pattern = input.getAttribute("pattern") || "";
  options.maxLength = input.getAttribute("maxLength") || "255";
  options.rows = input.getAttribute("rows") || "1";
  options.formatted = input.getAttribute("formatted") || "false";
  options.required = input.getAttribute("required") == "true";
  options.tag = input.getAttribute("tag") == "true";
  options.customProperties = "" || input.getAttribute("customProperties");
  var id = input.id + "_custominput";
  var control = (options.rows > 1 ? templateMultiLine : templateSingleLine).replace('{id}', id).replace('{prefix}', options.prefix).replace('{suffix}', options.suffix).replace('{label}', options.label).replace('{label-class}', options.label ? "" : "hidden").replace('{placeholder}', options.placeholder).replace('{required}', options.required).replace('{width}', options.width).replace('{pattern}', options.pattern).replace('{len}', options.maxLength).replace('{rows}', options.rows).replace('{lookup-loading-text}', '').replace('{clear-icon}', 'fa fa-times').replace('{info-icon}', 'hidden fa fa-info-circle').replace('{info-title}', '').replace('{search-icon}', 'fa fa-clipboard').replace('{search-title}', 'Cerca...');
  var e = input;
  var parent = e.parentNode;
  e.tabIndex = -1;
  e.style.display = 'none';
  e.insertAdjacentHTML('beforebegin', control);
  var cpanel = document.querySelector('#' + id);
  /* CREATE INPUT */
  var cinput = parent.querySelector('#' + id + (options.rows > 1 ? ' textarea' : ' input'));
  cinput.addEventListener('input', function (event) {
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
    input.value = cinput.value;
  });
  cinput.addEventListener('keyup', function (event) {
    if (event.keyCode == 13 && options.tag) {
      Select(cinput.value);
    }
  });
  // 4: observe changes on `hiddenInput`
  input.addEventListener('change', function (event) {
    LookupData(input.value);
  });
  /* CLEAR BUTTON */
  var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
  btnclear.addEventListener('click', function (event) {
    Clear();
  });
  var LookupData = function LookupData(data) {
    tags = [];
    if (options.tag) {
      if (data) data.split(",").forEach(function (item, i) {
        Select(item);
      });
    } else cinput.value = data;
    Close();
  };
  var Close = function Close() {
    var list = document.querySelector('#' + id + ' .lookup-values');
    list.innerHTML = "";
    var btnclear = parent.querySelector('#' + id + ' a.lookup-clear-button');
    btnclear.style.display = cinput.value ? "" : "none";
  };
  var Select = function Select(e) {
    if (e) {
      tags.push(e);
      input.value = tags.map(function (e) {
        return e.value;
      }).join(",");
      cinput.value = "";
      ShowTags();
    }
    Close();
  };
  var Clear = function Clear() {
    tags = [];
    cinput.value = "";
    input.value = "";
    ShowTags();
    Close();
  };
  var LostFocus = function LostFocus(i) {
    if (options.tag && cinput.value) Select(cinput.value);
  };
  var ShowTags = function ShowTags() {
    var tagcontrols = "";
    tags.forEach(function (element) {
      var tagc = tag_template
      // .replace(/{id}/g,element.id)
      .replace(/{value}/g, element).replace('{icon}', "").replace('{lookupvalue}', element);
      tagcontrols += tagc;
    });
    var e = document.querySelector("#" + id + " .lookup-tags");
    e.innerHTML = tagcontrols;
    document.querySelectorAll(".lookup-tag-close").forEach(function (element) {
      var el = element.getAttribute("item");
      element.addEventListener("click", function () {
        var i = tags.findIndex(function (f) {
          return f == el;
        });
        tags.splice(i, 1);
        input.value = tags.map(function (e) {
          return e;
        }).join(",");
        ShowTags();
      });
    });
  };
  cpanel.addEventListener("focusout", function (event) {
    if (!event.relatedTarget || event.relatedTarget.parentNode.className != 'lookup-search-row') LostFocus();
  });
  LookupData(input.value + "");
}
},{}],"bpm/form/components/data-entry.js":[function(require,module,exports) {
"use strict";

var _lookup = require("./input/lookup.js");
var _select = require("./input/select.js");
var _date = require("./input/date.js");
var _text = require("./input/text.js");
var observer = new MutationObserver(function (mutations) {
  // 2: iterate over `MutationRecord` array
  mutations.forEach(function (mutation) {
    // 3.1: check if the mutation type and the attribute name match
    // 3.2: check if value changed
    mutation.addedNodes.forEach(function (e, i) {
      //debugger;
      if (e.nodeName == "INPUT") try {
        //debugger;      
        var type = e.getAttribute("type");
        if (type == "select") (0, _select.select)(e);
        if (type == "lookup") (0, _lookup.lookup)(e);
        if (type == "date") (0, _date.date)(e);
        if (type == "textbox") (0, _text.text)(e);
      } catch (_unused) {
        console.log("non entro");
      }
      ;
      if (mutation.target.nodeName == "INPUT") try {
        var type = e.getAttribute("type");
        if (type == "select") (0, _select.select)(e);
        if (type == "lookup") (0, _lookup.lookup)(e);
        if (type == "date") (0, _date.date)(e);
        if (type == "textbox") (0, _text.text)(e);
      } catch (_unused2) {
        console.log("non entro");
      }
      ;
    });
  });
  // if (
  //     mutation.type === 'attributes'
  //     && mutation.attributeName === 'value'
  // ) {
  //     // 3.4: trigger `change` event
  //     mutation.target.dispatchEvent(new Event('change'));
  // }
});

observer.observe(document, {
  subtree: true,
  attributes: true,
  childList: true
});
document.querySelectorAll("input[type=lookup]").forEach(function (element) {
  (0, _lookup.lookup)(element);
});
document.querySelectorAll("input[type=select]").forEach(function (element) {
  (0, _select.select)(element);
});
document.querySelectorAll("input[type=date]").forEach(function (element) {
  (0, _date.date)(element);
});
document.querySelectorAll("input[type=textbox]").forEach(function (element) {
  (0, _text.text)(element);
});

// document.querySelectorAll("input[type=12]").forEach(element => {
//     lookup(element);
// });
// document.querySelectorAll("input[type=10]").forEach(element => {
//     lookup(element);
// });
// document.querySelectorAll("input[type=11]").forEach(element => {
//     lookup(element);
// });
// document.querySelectorAll("input[type=9]").forEach(element => {
//     lookup(element);
// });
},{"./input/lookup.js":"bpm/form/components/input/lookup.js","./input/select.js":"bpm/form/components/input/select.js","./input/date.js":"bpm/form/components/input/date.js","./input/text.js":"bpm/form/components/input/text.js"}],"node_modules/parcel-bundler/src/builtins/hmr-runtime.js":[function(require,module,exports) {
var global = arguments[3];
var OVERLAY_ID = '__parcel__error__overlay__';
var OldModule = module.bundle.Module;
function Module(moduleName) {
  OldModule.call(this, moduleName);
  this.hot = {
    data: module.bundle.hotData,
    _acceptCallbacks: [],
    _disposeCallbacks: [],
    accept: function (fn) {
      this._acceptCallbacks.push(fn || function () {});
    },
    dispose: function (fn) {
      this._disposeCallbacks.push(fn);
    }
  };
  module.bundle.hotData = null;
}
module.bundle.Module = Module;
var checkedAssets, assetsToAccept;
var parent = module.bundle.parent;
if ((!parent || !parent.isParcelRequire) && typeof WebSocket !== 'undefined') {
  var hostname = "" || location.hostname;
  var protocol = location.protocol === 'https:' ? 'wss' : 'ws';
  var ws = new WebSocket(protocol + '://' + hostname + ':' + "51055" + '/');
  ws.onmessage = function (event) {
    checkedAssets = {};
    assetsToAccept = [];
    var data = JSON.parse(event.data);
    if (data.type === 'update') {
      var handled = false;
      data.assets.forEach(function (asset) {
        if (!asset.isNew) {
          var didAccept = hmrAcceptCheck(global.parcelRequire, asset.id);
          if (didAccept) {
            handled = true;
          }
        }
      });

      // Enable HMR for CSS by default.
      handled = handled || data.assets.every(function (asset) {
        return asset.type === 'css' && asset.generated.js;
      });
      if (handled) {
        console.clear();
        data.assets.forEach(function (asset) {
          hmrApply(global.parcelRequire, asset);
        });
        assetsToAccept.forEach(function (v) {
          hmrAcceptRun(v[0], v[1]);
        });
      } else if (location.reload) {
        // `location` global exists in a web worker context but lacks `.reload()` function.
        location.reload();
      }
    }
    if (data.type === 'reload') {
      ws.close();
      ws.onclose = function () {
        location.reload();
      };
    }
    if (data.type === 'error-resolved') {
      console.log('[parcel] ✨ Error resolved');
      removeErrorOverlay();
    }
    if (data.type === 'error') {
      console.error('[parcel] 🚨  ' + data.error.message + '\n' + data.error.stack);
      removeErrorOverlay();
      var overlay = createErrorOverlay(data);
      document.body.appendChild(overlay);
    }
  };
}
function removeErrorOverlay() {
  var overlay = document.getElementById(OVERLAY_ID);
  if (overlay) {
    overlay.remove();
  }
}
function createErrorOverlay(data) {
  var overlay = document.createElement('div');
  overlay.id = OVERLAY_ID;

  // html encode message and stack trace
  var message = document.createElement('div');
  var stackTrace = document.createElement('pre');
  message.innerText = data.error.message;
  stackTrace.innerText = data.error.stack;
  overlay.innerHTML = '<div style="background: black; font-size: 16px; color: white; position: fixed; height: 100%; width: 100%; top: 0px; left: 0px; padding: 30px; opacity: 0.85; font-family: Menlo, Consolas, monospace; z-index: 9999;">' + '<span style="background: red; padding: 2px 4px; border-radius: 2px;">ERROR</span>' + '<span style="top: 2px; margin-left: 5px; position: relative;">🚨</span>' + '<div style="font-size: 18px; font-weight: bold; margin-top: 20px;">' + message.innerHTML + '</div>' + '<pre>' + stackTrace.innerHTML + '</pre>' + '</div>';
  return overlay;
}
function getParents(bundle, id) {
  var modules = bundle.modules;
  if (!modules) {
    return [];
  }
  var parents = [];
  var k, d, dep;
  for (k in modules) {
    for (d in modules[k][1]) {
      dep = modules[k][1][d];
      if (dep === id || Array.isArray(dep) && dep[dep.length - 1] === id) {
        parents.push(k);
      }
    }
  }
  if (bundle.parent) {
    parents = parents.concat(getParents(bundle.parent, id));
  }
  return parents;
}
function hmrApply(bundle, asset) {
  var modules = bundle.modules;
  if (!modules) {
    return;
  }
  if (modules[asset.id] || !bundle.parent) {
    var fn = new Function('require', 'module', 'exports', asset.generated.js);
    asset.isNew = !modules[asset.id];
    modules[asset.id] = [fn, asset.deps];
  } else if (bundle.parent) {
    hmrApply(bundle.parent, asset);
  }
}
function hmrAcceptCheck(bundle, id) {
  var modules = bundle.modules;
  if (!modules) {
    return;
  }
  if (!modules[id] && bundle.parent) {
    return hmrAcceptCheck(bundle.parent, id);
  }
  if (checkedAssets[id]) {
    return;
  }
  checkedAssets[id] = true;
  var cached = bundle.cache[id];
  assetsToAccept.push([bundle, id]);
  if (cached && cached.hot && cached.hot._acceptCallbacks.length) {
    return true;
  }
  return getParents(global.parcelRequire, id).some(function (id) {
    return hmrAcceptCheck(global.parcelRequire, id);
  });
}
function hmrAcceptRun(bundle, id) {
  var cached = bundle.cache[id];
  bundle.hotData = {};
  if (cached) {
    cached.hot.data = bundle.hotData;
  }
  if (cached && cached.hot && cached.hot._disposeCallbacks.length) {
    cached.hot._disposeCallbacks.forEach(function (cb) {
      cb(bundle.hotData);
    });
  }
  delete bundle.cache[id];
  bundle(id);
  cached = bundle.cache[id];
  if (cached && cached.hot && cached.hot._acceptCallbacks.length) {
    cached.hot._acceptCallbacks.forEach(function (cb) {
      cb();
    });
    return true;
  }
}
},{}]},{},["node_modules/parcel-bundler/src/builtins/hmr-runtime.js","bpm/form/components/data-entry.js"], null)
//# sourceMappingURL=/data-entry.32febdfc.js.map