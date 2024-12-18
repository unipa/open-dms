import Utility from "./utility.js";
export default class Compatto extends HTMLElement {
  constructor(documentId, imageId) {
    super();
    this.imageId = imageId;
    this.documentId = documentId ;
    this.currentContentId = 1;
    this.pageIndex = 1;
    this.width = null;
    this.height = null;
    this.password = null;
    this.root = document.getElementById("container_"+this.documentId + this.imageId);
  }

  async deploy(password) {
    this.password = password;
    const utility = new Utility(this.root);
    // utility.onlyContainer();
    await this.start();
  }

  async start() {
    await this.loadFirstPage(1);
  }

  // funzione per caricare i contenuti della pagina specificata
  async loadFirstPage(page) {
    // chiamo l'API per ottenere i contenuti della pagina specificata
    try {
        const utility = new Utility(this.root);
        //const utility = new Utility();
        const endpoints = await utility.setEndpoint();
        const url =
            endpoints.api_get_page_url +
            `?documentId=${this.documentId}&imageId=${this.imageId}&PageIndex=${page}&Password=${this.password}&small=true`;

        const img = document.createElement("img");
        img.id = "page_" + this.documentId + this.imageId + "-" + page;
        img.pageId = page;
        img.classList.add("ImagePage");
        img.classList.add("loading");
        img.loading = "lazy"; // Aggiungiamo l'attributo loading=lazy all'elemento img
        img.title = " ";
        img.src = "data:image/png;base64,";
        img.addEventListener("load", (data) => {
            if (this.blur == true) {
                img.classList.add("ObscuredImages");

            }
            img.classList.remove("loading");
            //img.setAttribute("width", "100%");
            //img.setAttribute("heigth", "auto");
        });
        this.root.appendChild(img);
        //let imageData = await utility.fetchImageData(this.password, this.documentId, this.imageId);
        //this.width = imageData?.defaultPageSize?.width;
        //this.height = imageData?.defaultPageSize?.height;

        img.src = url;// imgUrl;
    } catch (error) {
      let div = document.createElement("div");
      div.setAttribute("class", "no-image-service");
      div.innerHTML = "Servizio preview non disponibile: "+error;
      this.root.appendChild(div);
    }
  }
}
customElements.define("viewer-compatto", Compatto);
