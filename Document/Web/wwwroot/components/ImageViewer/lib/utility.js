import { getImageInfo } from "/components/ImageViewer/lib/api/fetchData.js";
import { getPage } from "/components/ImageViewer/lib/api/fetchData.js";
export default class Utility {
  constructor(root) {
    this.root = root;
    this.api_get_url = null;
    this.api_get_page_url = null;
    this.ImageData = null;
  }

  //METODI UTILI

  //Questo metodo mi ritorna il numero di pagine da caricare (n_pages serve a settare il campionamento di caricamento)
  getNumImages(totalPages, loadedPages, n_pages) {
    var remainingPages = totalPages - loadedPages;
    var remainingImages = Math.min(remainingPages, n_pages);
    return remainingImages;
  }

  //CHIAMATE API
  async setEndpoint() {
    try {
//      const response = await fetch("/components/ImageViewer/endpoint.json");
//      const endpoints = await response.json();
        this.api_get_url = "/internalapi/preview/Get";//.env.DOCUMENTS_SERVICE;
        this.api_get_page_url = "/internalapi/preview/GetPage";// process.env.DOCUMENT_SERVICE;
        return {
            "api_get_url": "/internalapi/preview/Get",
            "api_get_page_url": "/internalapi/preview/GetPage"
        } //endpoints;
    } catch (error) {
      console.error(error);
    }
  }

  async setType(type) {
    var imageViewer = document.querySelector("image-viewer");
    imageViewer.setAttribute("type", type);
  }

  async checkProtected(password, documentId,  imageId) {
    await this.setEndpoint();
      await this.fetchImageData(password, documentId, imageId);
    if (this.ImageData.protected == true) {
      return true;
    } else {
      return false;
    }
  }

    async fetchImageData(password, documentId, imageId) {
    await this.setEndpoint();
    const URL = this.api_get_url;
        this.ImageData = await getImageInfo(URL, password, documentId, imageId);
    return this.ImageData;
  }

  async fetchErrorImage() {
    await this.setEndpoint();
    const imageURL = this.api_get_page_url;
      const imageId = 1;
      const documentId = 0;
    const contentId = 1;
    const pageIndex = 1;
    const password = "";
    const imageData = await this.fetchImageData();
    const width = imageData.defaultPageSize.width;
    const height = imageData.defaultPageSize.height;

    // Verifica se esiste già un'immagine con la classe "errorImage"
    const existingErrorImage = document.querySelector("img.errorImage");
    if (existingErrorImage) {
      return;
    }

    try {
      const imageBlob = await getPage(
          imageURL,
          documentId, 
        imageId,
        contentId,
        pageIndex,
        password
      );
      const imageUrlObject = URL.createObjectURL(imageBlob);
      // Usa l'URL dell'oggetto blob per visualizzare l'immagine in una pagina web
      const imgElement = document.createElement("img");
      imgElement.src = imageUrlObject;
      imgElement.classList.add("errorImage");
      imgElement.setAttribute("width", width);
      imgElement.setAttribute("height", height);
      imgElement.addEventListener("click", function () {
        $("#passwordModal").modal("show");
      });

        const div = document.getElementById("container_" + documentId + imageId);
      div.appendChild(imgElement);
      return imgElement;
    } catch (error) {
      console.error(error);
    }
  }

  //GESTIONE DELLA MODAL PER L'INSERIMENTO DELLA PASSWORD E LA CHIUSURA
  closeModal() {
    var modal = document.getElementById("passwordModal");
    var closeBtn = document.getElementById("closeModal");
    closeBtn.onclick = function () {
      modal.classList.remove("show");
      $(".modal").modal("hide");
      modal.style.display = "none";
      $(".modal-backdrop").remove();
    };
  }

  //CREAZIONE ELEMENTI HTML

  createToolbar(loadedPages, totalPages) {
  //  const div = document.createElement("div");
  //  div.classList.add("toolbar", "text-center");

  //  const firstPageButton = document.createElement("button");
  //  firstPageButton.classList.add("btn", "mr-2");
  //  firstPageButton.id = "first-page-button";

  //  const prevPageButton = document.createElement("button");
  //  prevPageButton.classList.add("btn", "mr-2");
  //  prevPageButton.id = "prev-page-button";

  //  const currentPageCountSpan = document.createElement("span");
  //  currentPageCountSpan.classList.add("page-count", "mr-2");
  //  currentPageCountSpan.id = "current-page-count";
  //  currentPageCountSpan.textContent = "1 /";

  //  const LoadedPagesCountSpan = document.createElement("span");
  //  LoadedPagesCountSpan.classList.add("page-count", "mr-2");
  //  LoadedPagesCountSpan.id = "loaded-pages-count";
  //  LoadedPagesCountSpan.textContent = loadedPages.toString();

  //  const TotalPagesCountSpan = document.createElement("span");
  //  TotalPagesCountSpan.classList.add("page-count", "mr-2");
      //  TotalPagesCountSpan.id = "total-pages-count";
      //TotalPagesCountSpan = document.getElementById("total-pages-count");
      //TotalPagesCountSpan.textContent = "di " + totalPages;

  //  const nextPageButton = document.createElement("button");
  //  nextPageButton.classList.add("btn", "mr-2");
  //  nextPageButton.id = "next-page-button";

  //  const lastPageButton = document.createElement("button");
  //  lastPageButton.classList.add("btn", "mr-2");
  //  lastPageButton.id = "last-page-button";

  //  const LoadMoreButton = document.createElement("input");
  //  LoadMoreButton.classList.add("btn", "mr-2");
  //  LoadMoreButton.type = "button";
  //  LoadMoreButton.id = "load-more-button";
  //  LoadMoreButton.value = "Carica altre immagini";

  //  // aggiungi tutti gli elementi creati all'elemento div
  //  div.appendChild(LoadMoreButton);
  //  div.appendChild(firstPageButton);
  //  div.appendChild(prevPageButton);
  //  div.appendChild(currentPageCountSpan);
  //  div.appendChild(LoadedPagesCountSpan);
  //  div.appendChild(TotalPagesCountSpan);
  //  div.appendChild(nextPageButton);
  //  div.appendChild(lastPageButton);

  //  this.root.appendChild(div);
  }
   
    onlyContainer(documentId, imageId) {
        const div = document.getElementById("container_" + documentId +imageId);
    div.classList.remove("extendedContainer");
    div.classList.add("onlyContainer");
  }

    extendedContainer(documentId, imageId) {
        const div = document.getElementById("container_" + documentId +imageId);
    //div.classList.add("extendedContainer");
    div.classList.remove("onlyContainer");
  }
}
