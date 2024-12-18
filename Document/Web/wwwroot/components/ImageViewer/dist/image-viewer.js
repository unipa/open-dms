import Esteso from "../lib/esteso.js";
import Compatto from "../lib/compatto.js";
import Decrypt from "../lib/decrypt.js";
const template = document.createElement("template");
//const shadowRootcssCustomPath = "/components/ImageViewer/css/shadow-root.css";
/* const cssBootstrapPath =
  "https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css"; */
//const cssCustomTagPath = "/components/ImageViewer/css/image-viewer.css";

export default class ImageViewer extends HTMLElement {
  constructor() {
    super();
    // this.insertJQueryScript();
      this.imageId = this.getAttribute("imageid");
      this.documentId = this.getAttribute("documentId");
      this.type = this.getAttribute("type");
      this.n_pages = parseInt(this.getAttribute("pages"));
      if (this.n_pages === undefined || isNaN(this.n_pages)) {
          this.n_pages = 10;
      }
      this.onrender = this.getAttribute("onRender") ?  () => { eval(this.getAttribute("onRender")) } : undefined;

      template.innerHTML = template.innerHTML = "";
          //`<style> @import "` + shadowRootcssCustomPath + `</style>`;

      $(this.parentNode).append($("<div id='container_" + this.documentId + this.imageId + "' class='ImageViewerContainer' />"));
    const shadow = this.attachShadow({ mode: "open" });
      shadow.append(template.content.cloneNode(true));
    //Settaggio parametri del tag custom

    this.initialType = null;
//    this.insertDivContainer();
    this.insertCss(); 
    this.typeChanged = false;
      this.password = "";

//      this.connectedCallback();
    // this.insertJQueryScript(this.insertBootstrapScript);
  }


   // insertDivContainer() {
   //  const imageViewer = document.getElementById('image-viewer');
   //  // Creo un div che serve da Container
   //  const container = document.createElement('div');
   //  container.setAttribute('id', 'container');
   //  container.setAttribute('class', 'text-center extendedContainer');
   //  // Inserisco il container come padre del mio elemento custom "Image Viewer"
   //  imageViewer.parentNode.insertBefore(container, imageViewer);
   //  container.appendChild(imageViewer);
   //} 

  insertCss() {
    // Crea l'elemento link per cssCustom
    //var cssCustomTag = document.createElement("link");
    // Imposta gli attributi dell'elemento link
    //cssCustomTag.setAttribute("rel", "stylesheet");
    //cssCustomTag.setAttribute("type", "text/css");
    //cssCustomTag.setAttribute("href", cssCustomTagPath);

    // Crea l'elemento link per cssBootstrap

    //var cssBootstrap = document.createElement("link");
    // Imposta gli attributi dell'elemento link
    //cssBootstrap.setAttribute("rel", "stylesheet");
    //cssBootstrap.setAttribute("type", "text/css");
    // Aggiunge l'elemento link alla sezione head del documento

    // Aggiunge gli elementi link alla sezione head del documento
    //document.head.appendChild(cssCustomTag);
    //document.head.appendChild(cssBootstrap);
  }

  // insertJQueryScript(callback) {
  //   var jqueryScript = document.createElement("script");
  //   jqueryScript.setAttribute("src", jqueryScriptPath);
  //   document.head.appendChild(jqueryScript);
  //   jqueryScript.onload = callback; // chiamiamo la callback quando lo script è stato caricato
  // }

  // insertBootstrapScript() {
  //   var bootstrapScript = document.createElement("script");
  //   bootstrapScript.setAttribute("src", bootstrapScriptPath);
  //   document.head.appendChild(bootstrapScript);
  // }

  //Funzione per selezionare il componente da deployare in base agli attributi del componente principale
  async connectedCallback() {
    switch (this.type) {
      //Comportamento per l'attributo esteso sul custom-tag
      case "compatto":
        this.initialType = "compatto";
            const compatto = new Compatto(
                this.documentId,
          this.imageId
        );
        compatto.deploy(this.password);
        break;

      //Comportamento per l'attributo compatto sul custom-tag
      case "esteso":
        this.initialType = "esteso";
        const esteso = new Esteso(
            this.documentId,
          this.imageId,
            this.n_pages,
            this.onrender
        );
        esteso.deploy(this.password);
        break;

      //Comportamento per l'attributo decrypt sul custom-tag nel caso in cui il documento sia protetto da password
      case "decrypt":
        let passwordCorrect = false;
        while (!passwordCorrect) {
          const decrypt = new Decrypt();
          decrypt.deploy(this.initialType);
          const self = this;
          // Chiama il metodo getPasswordFromModal() per ottenere la password dall'utente
          const password = await decrypt
            .getPasswordFromModal()
            .then((password) => {
              self.password = password;
              passwordCorrect = true;
              // Imposta l'attributo "type" del componente <image-viewer> a "esteso"
              const imageViewer = document.getElementById("image-viewer");
              imageViewer.setAttribute("type", this.initialType);
            })
            .catch((error) => {
              console.error(error);
            });
        }
        break;

      default:
        console.error("No valid type in image-viewer");
        break;
    }
  }

  static get observedAttributes() {
    return ["type"];
  }

  attributeChangedCallback(name) {
    if (!this.typeChanged) {
      // controlla se è il primo cambiamento di attributo
      this.typeChanged = true;
      return; // esce dalla funzione senza eseguire alcuna azione
    }
    switch (name) {
      case "type":
        this.type = this.getAttribute("type");
        break;

      default:
        break;
    }
    // Salvataggio del nuovo valore dell'attributo type
    this.connectedCallback();
  }
}

customElements.define("image-viewer", ImageViewer);
