import TextInput from "../lib/text-input.js";
import Textarea from "../lib/textarea.js";
import Lookup from "../lib/lookup.js";
import DatePicker from "../lib/date.js";
import MixinStyle from "../lib/mixinstyle.js";
import NumberInput from "../lib/number-input.js";
const template = document.createElement("template");
//If imported via script tag, use the following line
//const cssPath = './css/custom-input.css';
//If imported as node module, use the following line

export default class CustomInput extends HTMLElement {
  static get formAssociated() {
    return true;
  }
  static set formAssociated(value) {
    return value;
  }

  constructor() {
    super();
    const shadow = this.attachShadow({ mode: "open" });
    this.internals = this.attachInternals();
    this.width = this.getAttribute("width");
    this.maxlength = this.getAttribute("maxlength");
    this.label = this.getAttribute("label") || "";
    this.name = this.getAttribute("name");

    template.innerHTML = `
    ${`<label for="${this.name}">${this.label}</label>`}
<div class="input-type"></div>`;

    shadow.append(template.content.cloneNode(true));
  }

  connectedCallback() {
    const styleElement = new MixinStyle(this.shadowRoot);
    this.shadowRoot.insertBefore(styleElement, this.shadowRoot.firstChild);
    const el = this.shadowRoot,
      obj = {
        id: this.getAttribute("id"),
        name: this.getAttribute("name"),
        value: this.getAttribute("value") || "",
        placeholder: this.getAttribute("placeholder"),
        instanceId: this.getAttribute("instanceId"),
        maxlength: this.getAttribute("maxlength"),
        required: this.getAttribute("required"),
        disabled: this.getAttribute("disabled"),
        multivalue: this.getAttribute("multivalue"),
        maxResults: this.getAttribute("maxResults"),
        token: this.getAttribute("token"),
      },
      type = this.getAttribute("type");

    /* --- Type configuration --- */

    switch (type) {
      /* TEXT */
      case "text":
        const textInput = new TextInput(el, obj);
        this.textInstance = textInput;
        textInput.deploy();
        this.createEvent(
          el.querySelector('input[type="text"]'),
          "input",
          "getValue"
        );
        textInput.addEventListener("change", (event) => {
          this.value = event.detail.value;
        });

        break;
      /* TEXT */
      case "number":
        const numberInput = new NumberInput(el, obj);
        this.numberInstance = numberInput;
        numberInput.deploy();
        this.createEvent(
          el.querySelector('input[type="number"]'),
          "input",
          "getValue"
        );
        numberInput.addEventListener("change", (event) => {
          this.value = event.detail.value;
        });

        break;
      /* TEXTAREA */
      case "textarea":
        const textarea = new Textarea(el, obj);
        this.textareaInstance = textarea;
        textarea.deploy();
        this.createEvent(el.querySelector("textarea"), "input", "getValue");
        break;
      /* SEARCH BAR */
      case "lookup":
        const lookup = new Lookup(el, obj, this);
        this.lookupInstance = lookup;
        lookup.deploy();
        lookup.addEventListener("fetch", (event) => {
          this.value = event.detail.value.codice;
        });

        el.querySelector("input").addEventListener("input", (event) => {
          if (event.target.value === "") {
            this.value = "";
          }
        });
        break;
      case "date":
        const date = new DatePicker(el, obj);
        this.dateInstance = date;
        date.deploy();
        date.root.querySelector("input").addEventListener("change", (e) => {
          this.setAttribute("value", this.removeDashesFromDate(e.target.value));
        });
        break;
      default:
        el.querySelector(".input-type").innerHTML = "No type selected";
        break;
    }

    // Remove the input if visible is false
    this.getAttribute("visible") === "false" &&
      (el.querySelector(".input-type").style.display = "none");
    // Set the width of the input
    this.width && (el.querySelector(".input-type").style.width = this.width);
  }

  /* --- Functions --- */

  /* Events Dispatching */
  createEvent(selector, listener, eventName) {
    if (selector instanceof Lookup) {
      selector.addEventListener(listener, (event) => {
        this.dispatchEvent(
          new Event(eventName, {
            detail: { value: event.detail.value },
          })
        );
      });
    } else {
      selector.addEventListener(listener, (event) => {
        this.value = event.target.value;
        this.dispatchEvent(
          new Event(eventName, {
            detail: { value: event.target.value },
            bubbles: true,
          })
        );
      });
    }
  }

  /* Dashes removing from date */
  removeDashesFromDate = (dateString) => {
    return dateString.replace(/-/g, "");
  };

  /* --- Attributes Watching --- */
  static get observedAttributes() {
    return [
      "type",
      "placeholder",
      "id",
      "width",
      "visible",
      "value",
      "name",
      "maxlength",
      "required",
      "disabled",
      "label",
      "instanceId",
      "multivalue",
    ];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    const el = this.shadowRoot;
    const input = el.querySelector("input");
    const textarea = el.querySelector("textarea");
    const date = el.querySelector("input[type='date']");
    const lookup = el.querySelector(".search-bar");
    switch (name) {
      case "width":
        if (input) {
          input.style.width = newValue;
        }
        if (textarea) {
          textarea.style.width = newValue;
        }
        break;
      case "visible":
        this.style.display = newValue === "true" ? "block" : "none";
        break;
      case "placeholder":
        if (input) {
          input.setAttribute("placeholder", newValue);
        }
        if (textarea) {
          textarea.setAttribute("placeholder", newValue);
        }
        break;
      case "value":
        this.textInstance && this.textInstance.setValue(newValue);
        this.numberInstance && this.numberInstance.setValue(newValue);
        this.textareaInstance && this.textareaInstance.setValue(newValue);
        this.dateInstance &&
          date.setAttribute(
            "value",
            newValue.slice(0, 4) +
              "-" +
              newValue.slice(4, 6) +
              "-" +
              newValue.slice(6, 8)
          );
        this.lookupInstance &&
          newValue !== "" &&
          this.lookupInstance.getStartValues(newValue);

        break;
      case "maxlength":
        if (input) {
          input.setAttribute("maxlength", newValue);
        }
        if (textarea) {
          textarea.setAttribute("maxlength", newValue);
        }
        break;
      case "required":
        if (input) {
          input.setAttribute("required", newValue);
        }
        if (textarea) {
          textarea.setAttribute("required", newValue);
        }
        break;
      case "disabled":
        if (input) {
          newValue == "true"
            ? input.setAttribute("disabled", true)
            : input.removeAttribute("disabled");
        }
        if (textarea) {
          newValue == "true"
            ? textarea.setAttribute("disabled", true)
            : textarea.removeAttribute("disabled");
        }
        break;
      case "label":
        el.querySelector("label").innerHTML = newValue;
        break;
      case "name":
        if (input) {
          input.setAttribute("name", newValue);
        }
        if (textarea) {
          textarea.setAttribute("name", newValue);
        }
        break;
      case "id":
        if (input) {
          input.setAttribute("id", newValue);
          el.querySelector("label").setAttribute("for", newValue);
        }
        if (textarea) {
          textarea.setAttribute("id", newValue);
          el.querySelector("label").setAttribute("for", newValue);
        }
        break;
      case "instanceId":
        if (input) {
          input.setAttribute("instanceId", newValue);
        }
        break;

      case "multivalue":
        if (this.lookupInstance) {
          if (newValue === "true") {
            this.lookupInstance.getStartValues(this.getAttribute("value"));
            lookup.querySelector("input").setAttribute("value", "");
          }
          if (newValue === "false") {
            el.querySelector(".multiselect-array").innerHTML = "";
            this.lookupInstance.list = [];
          }
        }

        break;

      default:
        break;
    }
  }

  get value() {
    if (!this.shadowRoot.querySelector("textarea")) {
      return this.shadowRoot.querySelector("input").value;
    }
    return this.shadowRoot.querySelector("textarea").innerText;
  }

  set value(newValue) {
    this.setAttribute("value", newValue);
    if (this.shadowRoot.querySelector("textarea")) {
      this.shadowRoot.querySelector("textarea").textContent = newValue;
      this.shadowRoot.querySelector("textarea").value = newValue;
    }
  }
}
/* --- Registering the element --- */
window.customElements.define("custom-input", CustomInput);
