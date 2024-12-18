import Utility from './utility.js';
//import $ from 'jquery';
export default class Decrypt extends HTMLElement {

  constructor(root) {
    super();
    this.root = root;
  }

  deploy(initialType) {
    const utility = new Utility(this.root);
    // utility.onlyContainer();
    this.createModal();
    utility.fetchErrorImage();

    switch (initialType) {
      case "esteso":
        break;

      case "compatto":
        break;
    }
  }


  createModal() {
    const utility = new Utility(this.root);
    // Creazione del div principale con le sue classi e id
    const modalDiv = document.createElement('div');
    modalDiv.classList.add('modal', 'fade');
    modalDiv.id = 'passwordModal';
    modalDiv.setAttribute('tabindex', '-1');
    modalDiv.setAttribute('aria-labelledby', 'passwordModalLabel');
    modalDiv.setAttribute('aria-hidden', 'true');

    // Creazione del div della finestra di dialogo
    const dialogDiv = document.createElement('div');
    dialogDiv.classList.add('modal-dialog');

    // Creazione del div del contenuto
    const contentDiv = document.createElement('div');
    contentDiv.classList.add('modal-content');

    // Creazione dell'intestazione
    const headerDiv = document.createElement('div');
    headerDiv.classList.add('modal-header');

    const title = document.createElement('h5');
    title.classList.add('modal-title');
    title.id = 'passwordModalLabel';
    title.textContent = 'Inserisci la password';

    headerDiv.appendChild(title);

    // Creazione del corpo
    const bodyDiv = document.createElement('div');
    bodyDiv.classList.add('modal-body');

    const form = document.createElement('form');

    const formGroup = document.createElement('div');
    formGroup.classList.add('mb-3');

    const label = document.createElement('label');
    label.classList.add('form-label');
    label.setAttribute('for', 'passwordInput');
    label.textContent = 'Password';

    const input = document.createElement('input');
    input.classList.add('form-control');
    input.setAttribute('type', 'password');
    input.id = 'passwordInput';
    input.setAttribute('placeholder', 'Inserisci la password');
    input.required = true;

    formGroup.appendChild(label);
    formGroup.appendChild(input);

    const errorDiv = document.createElement('div');
    errorDiv.id = 'passwordError';
    errorDiv.style.display = 'none'; // Setto lo stile di default a display none
    errorDiv.classList.add('text-danger');
    errorDiv.textContent = 'Password errata. Riprovare.';

    form.appendChild(formGroup);
    form.appendChild(errorDiv);
    bodyDiv.appendChild(form);

    // Creazione del piè di pagina
    const footerDiv = document.createElement('div');
    footerDiv.classList.add('modal-footer');

    const closeButton = document.createElement('button');
    closeButton.classList.add('btn', 'btn-secondary');
    closeButton.setAttribute('type', 'button');
    closeButton.id = 'closeModal';
    closeButton.setAttribute('data-bs-dismiss', 'modal');
    closeButton.textContent = 'Chiudi';

    const submitButton = document.createElement('button');
    submitButton.classList.add('btn', 'btn-primary');
    submitButton.setAttribute('type', 'button');
    submitButton.id = 'submitPasswordButton';
    submitButton.textContent = 'Submit';

    footerDiv.appendChild(closeButton);
    footerDiv.appendChild(submitButton);

    // Aggiunta degli elementi creati al DOM
    contentDiv.appendChild(headerDiv);
    contentDiv.appendChild(bodyDiv);
    contentDiv.appendChild(footerDiv);

    dialogDiv.appendChild(contentDiv);
    modalDiv.appendChild(dialogDiv);

    // Aggiunta del Modal al body della pagina
    document.body.appendChild(modalDiv);

    utility.closeModal();
    this.getPasswordFromModal();
  }



  // Nella funzione getPasswordFromModal() del tuo componente Decrypt, restituisci la password inserita dall'utente nella Promise risolta
  async getPasswordFromModal() {
    return new Promise(async (resolve, reject) => {
      const utility = new Utility(this.root);
      var self = this;
      var submitBtn = document.getElementById("submitPasswordButton");
      var passwordInput = document.getElementById("passwordInput");
      var passwordModal = document.getElementById("passwordModal");
      var passwordError = document.getElementById("passwordError");
      submitBtn.onclick = async function () {
        if (passwordInput.checkValidity()) {
          let password = passwordInput.value;
          let success = await utility.checkProtected(password);
          if (!success) {
            passwordModal.classList.remove("show");
            $('.modal').modal('hide');
            passwordModal.style.display = "none";
            $('.modal-backdrop').remove();
            resolve(password);
          } else {
            passwordError.style.removeProperty("display");
            reject(new Error("Password errata"));
          }
        } else {
          passwordInput.reportValidity();
        }
      }
    });
  }


}
customElements.define('viewer-decrypt', Decrypt);