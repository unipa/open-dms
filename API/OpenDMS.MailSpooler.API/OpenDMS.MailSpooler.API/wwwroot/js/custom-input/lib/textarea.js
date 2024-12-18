export default class Textarea extends HTMLElement {
  constructor(
    root,
    { id, name, value, placeholder, maxlength, required, disabled }
  ) {
    super();
    this.root = root;
    this.id = id;
    this.name = name;
    this.value = value;
    this.placeholder = placeholder;
    this.maxlength = maxlength;
    this.required = required; 
    this.disabled = disabled;
  }

  setValue(value) {
    this.root.querySelector("textarea").value = value;
  }
  
  deploy() {
    const multiLineInput = `<div class="textarea-field">
    <textarea ${this.name ? `name="${this.name}" ` : ``} ${
      this.id ? `id="${this.id}" ` : ``
    } ${this.value ? `value="${this.value}" ` : ``} ${
      this.maxlength ? `maxlength="${this.maxlength}" ` : ``
    } ${this.placeholder ? `placeholder="${this.placeholder}" ` : ``} ${this.required === "true" ? `required ` : ``} ${this.disabled === "true" ?  `disabled ` : ``}></textarea></div>`;
    this.root.querySelector(".input-type").innerHTML = multiLineInput;
    this.root.querySelector("textarea").addEventListener("input", (event) => {
      this.value = event.target.value;
      this.dispatchEvent(
        new Event("getValue", { detail: { value: event.target.value } })
      );
    });
  }
}
customElements.define("text-area", Textarea, {});
