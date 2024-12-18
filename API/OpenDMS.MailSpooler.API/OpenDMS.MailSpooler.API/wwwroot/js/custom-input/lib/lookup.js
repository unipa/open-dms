import { fetchLookupData } from "./api/fetchLookupData.js";
export default class Lookup extends HTMLElement {
  constructor(root, options, parent) {
    super();
    this.root = root;
    this.id = options.id;
    this.name = options.name;
    this.value = options.value;
    this.placeholder = options.placeholder;
    this.instanceId = options.instanceId;
    this.required = options.required;
    this.disabled = options.disabled;
    this.maxResults = options.maxResults;
    this.list = [];
    this.parent = parent;
    this.token = options.token;
  }

  deploy() {
    /* --- Template --- */
    this.root.querySelector(".input-type").innerHTML =
      this.getSearchBarTemplate();
    // Event Listener for the search button + query with debounce
    this.root.querySelector(".search-bar input").addEventListener(
      "input",
      this.debounce(async (e) => {
        const searchList = this.root.querySelector(".search-results");
        if (!e.target.value) {
          searchList.innerHTML = "";
          return;
        }
        const response = await fetchLookupData({
          searchTerm: e.target.value,
          instanceId: this.instanceId,
          token: this.token,
          maxResults: this.maxResults,
        });
        if (response.length > 0) {
          const filteredResponse = response.filter(
            (item) => item?.codice !== undefined && item?.name !== undefined
          );
          // If results are found render the list
          searchList.innerHTML = this.getListTemplate(filteredResponse);
          /* --- Event Listeners --- */

          // expandable list
          this.root.querySelectorAll(".toggle-button").forEach((item) => {
            item.addEventListener("click", (e) => {
              const toggleButton = e.currentTarget;
              const ulElement = toggleButton.parentElement.nextElementSibling;
              e.currentTarget.classList.toggle("chevron-active");
              ulElement.classList.toggle("is-visible");
            });
          });

          /* --- Item selection event listener --- */
          this.root
            .querySelectorAll(".search-results .selectable-item")
            .forEach((item) => {
              item.addEventListener("click", (e) => {
                const currentItem = this.findItemById(
                  filteredResponse,
                  e.currentTarget.id
                );
                this.root.querySelector(".search-bar input").value = "";

                //List output here
                if (this.parent.getAttribute("multivalue") === "true") {
                  if (
                    this.list.filter(
                      (item) => item.codice === currentItem.codice
                    ).length > 0
                  ) {
                    return;
                  }
                  this.list = [...this.list, currentItem];
                  this.getMultiSelectTemplate(this.list);
                  this.parent.setAttribute(
                    "value",
                    this.list.map((item) => item?.codice).join(",")
                  );
                } else {
                  this.parent.setAttribute("value", currentItem.codice);
                  this.root.querySelector("input").value = currentItem.name;
                }
                /* --- Clear Search Results on selection --- */
                this.root.querySelector(".search-results").innerHTML = "";
              });
            });
        } else {
          // If no results are found
          this.root.querySelector(".search-results").innerHTML =
            "<ul><li>Nessun risultato</li></ul>";
        }
      }, 500)
    );
    this.getStartValues(this.value);
  }

  getStartValues = async (value) => {
    let results = [];
    const values = value.split(",");

    for (let item of values) {
      const request = fetchLookupData(item, this.instanceId, true);
      const data = await request;
      if (data && data[0] && data[0].codice && data[0].name) {
        // Only add valid data to results
        results = [...results, data[0]];
      } else {
        const noResultItem = {
          alt: item,
          codice: item,
        };
        results = [...results, noResultItem];
      }
    }
    this.list = results;

    if (this.parent.getAttribute("multivalue") === "true") {
      if (this.list[0].codice === "") return (this.list = []);
      this.getMultiSelectTemplate(this.list);
      this.root.querySelector(".search-bar input").setAttribute("value", "");
    } else {
      const searchValue = this.list[0].name || this.list[0].alt;
      this.root
        .querySelector(".search-bar input")
        .setAttribute("value", searchValue);
    }
  };

  /* ---- Remove item from Multiselect array ---- */
  removeListItem = (id) => {
    this.list = this.list.filter((item) => item.codice !== id);
    if (this.list.length > 0) {
      this.parent.setAttribute(
        "value",
        this.list.map((item) => item.codice).join(",")
      );
    } else {
      this.parent.setAttribute("value", "");
      this.root.querySelector(".multiselect-array").innerHTML = "";
    }
  };

  /* ---- MultiSelect array Template ---- */
  getMultiSelectTemplate = (array) => {
    if (!array.length > 0) return;
    this.root.querySelector(".multiselect-array").innerHTML = `
    ${array.map((item) => this.createMultiSelectItem(item)).join("")}
    `;
    this.root.querySelectorAll(".multiselect-item").forEach((item) => {
      item.addEventListener("click", (e) => {
        this.removeListItem(e.currentTarget.id);
      });
    });
  };

  /* Debounce function */
  debounce = (func, wait, immediate) => {
    let timeout;

    return function () {
      const context = this,
        args = arguments;
      const later = function () {
        timeout = null;
        if (!immediate) func.apply(context, args);
      };

      const callNow = immediate && !timeout;
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
      if (callNow) func.apply(context, args);
    };
  };

  /* ---- Searchbar Template ---- */
  getSearchBarTemplate = () => {
    return `
    <div class="search-box">
    <div class="search-bar">
    <div class="multiselect-array">
    ${this.list?.map((item) => this.createMultiSelectItem(item)).join("")}
    </div>
    <input ${this.name ? `name="${this.name}" ` : ``}  ${
      this.codice ? `id="${this.codice}" ` : ``
    }  ${this.value ? `value="${this.value}" ` : ``} 
      ${this.placeholder ? `placeholder="${this.placeholder}" ` : ``} ${
      this.required === "true" ? `required ` : ``
    } ${this.disabled === "true" ? `disabled ` : ``} type="search">
    </div>
    <button>Search</button>
    </div>
    <div class="search-results"></div>
    `;
  };

  /* ---- MultiSelect Item Template ---- */
  createMultiSelectItem = (obj) => {
    const deleteIcon =
      '<svg class="delete-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512"><!--! Font Awesome Pro 6.3.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2023 Fonticons, Inc. --><path d="M310.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L160 210.7 54.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L114.7 256 9.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L160 301.3 265.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L205.3 256 310.6 150.6z"/></svg>';
    return `
    <div id="${obj.codice}" class="multiselect-item">
    <div style="white-space: nowrap;">${
      obj.name ? obj.name : '<span class="unavailable">' + obj.alt + "</span>"
    }</div>
    <div style="margin-left:5px;">${deleteIcon}</div>
    </div>
    `;
  };

  /* ---- Search List Template ---- */
  getListTemplate = (response) => {
    let list = "<ul>";
    response.map((item) => {
      list += this.createNestedList(item);
    });
    list += "</ul>";
    return list;
  };
  /* ---- Function for nested list generation ---- */
  createNestedList = (obj) => {
    let arrayPresent = null;
    for (let key in obj) {
      if (Array.isArray(obj[key])) {
        arrayPresent = obj[key];
        break; // stop the loop
      }
    }
    if (arrayPresent === null) {
      return this.createSingleValue(obj);
    }

    const chevronIcon =
      '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512"><path d="M201.4 406.6c12.5 12.5 32.8 12.5 45.3 0l192-192c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L224 338.7 54.6 169.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l192 192z"/></svg>';
    return `<li><div class="parent-item"><div id="${
      obj.codice
    }" class="selectable-item selectable-with-chevron"><span>${
      obj.name
    }</span><small>${
      obj.codice
    }</small></div><div class="toggle-button">${chevronIcon}</div></div><ul class="child-list">
      ${arrayPresent.map((item) => this.createNestedList(item)).join("")}
    </ul></li>`;
  };
  /* ---- If there are no arrays in obj ---- */
  createSingleValue = (obj) => {
    return `<li><div id="${obj.codice}" class="selectable-item selectable-without-chevron"><span>${obj.name}</span><small>${obj.codice}</small></div></li>`;
  };
  /* ----------------------- */

  // Recursive function to find the selected item, it's used to update the multiselect array through event listener
  findItemById = (array, id) => {
    for (const item of array) {
      if (item.codice == id) {
        return item;
      }
      for (let key in item) {
        if (Array.isArray(item[key])) {
          const result = this.findItemById(item[key], id);
          if (result) {
            return result;
          }
        }
      }
    }
  };
}
customElements.define("search-bar", Lookup);
