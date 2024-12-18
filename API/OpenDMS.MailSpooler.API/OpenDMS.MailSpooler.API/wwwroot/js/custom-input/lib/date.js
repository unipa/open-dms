export default class DatePicker extends HTMLElement {
  constructor(root, { id, name, value, maxlength, required, disabled }) {
    super();
    this.root = root;
    this.id = id;
    this.name = name;
    this.value = this.value && this.formatDate(value);
    this.maxlength = maxlength;
    this.required = required;
    this.disabled = disabled;
  }

  deploy() {
    const dateInput = `<div class="date-field">
        <input ${this.name ? `name="${this.name}" ` : ``} ${
      this.id ? `id="${this.id}" ` : ``
    } ${this.required === "true" ? `required ` : ``} ${
      this.disabled === "true" ? `disabled ` : ``
    }
    ${this.value && `value="${this.value}"`} type="date"/>
        </div>`;
    this.root.querySelector(".input-type").innerHTML = dateInput;
  }
  formatDate(date) {
    return date.slice(0, 4) + "-" + date.slice(4, 6) + "-" + date.slice(6, 8);
  }
}
customElements.define("date-picker", DatePicker, {});
