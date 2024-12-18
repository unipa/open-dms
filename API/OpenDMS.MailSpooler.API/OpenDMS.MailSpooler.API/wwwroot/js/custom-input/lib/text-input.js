export default class TextInput extends HTMLElement {
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
    this.root.querySelector("input").value = value;
  }


  deploy() {
    const singleLineInput = `<div class="text-field">
        <input ${this.name ? `name="${this.name}" ` : ``}  ${
      this.id ? `id="${this.id}" ` : ``
    } ${this.maxlength ? `maxlength="${this.maxlength}" ` : ``}
          type="text" ${
            this.placeholder ? `placeholder="${this.placeholder}" ` : ``
          } ${this.required === "true" ? `required ` : ``} ${
      this.disabled === "true" ? `disabled ` : ``
    } ${this.value ? `value="${this.value}"` : ``}/>
        </div>`;
    this.root.querySelector(".input-type").innerHTML = singleLineInput;
  }
}
customElements.define("text-input", TextInput);
