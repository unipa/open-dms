import Utility from "./utility.js";
import { LoadImage } from "../../../js/pdf/loadimage.js";

export default class Esteso extends HTMLElement {
    constructor(documentId, imageId, n_pages, onrender) {
        super();
        this.totalPages = 1; //Numero di pagine totali del documento
        this.loadedPages = 0; //Pagine caricate
        this.pagesToLoad = 0;
        this.isLoading = false;
        this.imageId = imageId;
        this.documentId = documentId;
        this.currentContentId = 0;
        this.currentPage = 1;
        this.imgWidth = null;
        this.imgHeight = null;
        this.blur = false;
        this.Scrolling = false;
        this.password = null;
        this.onrender = onrender;
        this.root = document.getElementById("container_" + this.documentId + this.imageId);

        //Variabili di configurazione
        this.n_pages = n_pages;
    }
 
    async deploy(password) {
        this.password = password;

        const utility = new Utility(this.root);
        utility.extendedContainer(this.documentId, this.imageId);
        let isProtected = await utility.checkProtected(password, this.documentId, this.imageId);
        if (isProtected != true) {
            this.start();
        } else {
            utility.setType("decrypt");
            return;
        }
    }

    async start() {
        this.removeErrorImages();
        this.removeModal();
        const utility = new Utility(this.root);
        const ImageData = await utility.fetchImageData(
            this.password,
            this.documentId,
            this.imageId
        );
        this.totalPages = ImageData.pages;
        this.imgWidth = ImageData.defaultPageSize.width;
        this.imgHeight = ImageData.defaultPageSize.height;
        this.blur = ImageData.obscured;
        //utility.createToolbar(this.loadedPages, this.totalPages, this.imageId);

        //setInterval(this.loadSequentiallyImages.bind(this), 250);

        const TotalPagesCountSpan = document.getElementById("total-pages-count");
        if (TotalPagesCountSpan)
            TotalPagesCountSpan.textContent = this.totalPages;

        const nextPageButton = document.getElementById("next-page-button");
        if (nextPageButton)
            nextPageButton.addEventListener("click", () => {
                this.scrollToImage(this.calculateNextPage());
            });

        const prevPageButton = document.getElementById("prev-page-button");
        if (prevPageButton)
            prevPageButton.addEventListener("click", () => {
            this.scrollToImage(this.calculatePrevPage());
        });

        // Aggiungiamo l'evento di scroll al div con id "container"
        const cont = document.getElementById("container_" + this.documentId + this.imageId);
        cont.addEventListener("scroll", () => {
            const container = $("#container_" + this.documentId + this.imageId);
            // Scrollo appena raggiungo il 33% finale dell'elenco...
            if (container.scrollTop() >= container[0].scrollHeight - container.height() * 1.33 && this.totalPages > this.loadedPages && !this.isLoading && !this.Scrolling) {
                console.debug(Date.now()+" - scrolling è false")
                this.Scrolling = true;
                this.loadMoreImages();
            }
        });

        //const LoadMoreButton = document.getElementById("load-more-button");
        //LoadMoreButton.addEventListener("click", () => this.loadMoreImages());

        const LoadaingMessage= document.createElement("div");
        LoadaingMessage.id = "loading-message";
        LoadaingMessage.innerHTML = "Caricamento in corso..." 
        cont.after (LoadaingMessage);

        this.loadMoreImages();
        //this.loadSequentiallyImages();
    }


    // Funzione per il caricamento di altre immagini
    async loadMoreImages() {
        const utility = new Utility(this.root);
        const numImages = utility.getNumImages(
            this.totalPages,
            this.loadedPages,
            this.n_pages
        );
//        const LoadMoreButton = document.getElementById("load-more-button");
//        LoadMoreButton.style.display = "none"; // Nascondo il bottone di caricamento
        this.pagesToLoad = numImages;
        this.loadSequentiallyImages();
    }
  
    async loadSequentiallyImages() {
        if (
            this.isLoading ||
            this.pagesToLoad == 0 ||
            this.totalPages == this.currentContentId
        ) return;

        if (this.isLoading == false) {
            this.isLoading = true;
            this.updateLoadedPagesCount();
            const LoadingMessage = document.getElementById("loading-message");
            LoadingMessage.style.display = "flex"; 

            this.currentContentId++; // Incremento il numero della prossima pagina da caricare
            const newImg = await this.loadPage(this.currentContentId, () => {
                this.loadedPages++;
                if (this.loadedPages == 1 && this.onrender) {
                    this.onrender();
                }
                this.pagesToLoad--;
                LoadingMessage.style.display = "none";
                this.isLoading = false;
                if (this.pagesToLoad > 0) {
                    setTimeout(this.loadSequentiallyImages.bind(this), 10);
                } else { 
                    this.Scrolling = false;
                    var cont = $("#container_" + this.documentId + this.imageId)[0];
                    cont.dispatchEvent(new Event("scroll"));
                    //cont.trigger("scroll");
                }
            });
            const cont = document.getElementById("container_" + this.documentId + this.imageId);
            cont.appendChild(newImg);
        }
    }
    updateCurrentPagesCount(id) {
        const currentPagesCount = document.getElementById("current-page-count");
        if (currentPagesCount)
            currentPagesCount.textContent = id.toString();

        document.querySelectorAll(".ImagePage").forEach((e, i) => {
            e.classList.remove("SelectedImage");
        });
        document.getElementById("page_" + this.documentId + this.imageId + "-" + id).classList.add("SelectedImage");

    }

    async loadPage(currentContentId, callback) {
        const utility = new Utility(this.root);
        const endpoints = await utility.setEndpoint();
        const url =
            endpoints.api_get_page_url +
            `?documentId=${this.documentId}&imageId=${this.imageId}&PageIndex=${currentContentId}&Password=${this.password}`;
        const img = document.createElement("img");
        img.id = "page_" + this.documentId + this.imageId + "-" + currentContentId;
        img.pageId = currentContentId;
        img.classList.add("ImagePage");
        img.classList.add("large");
        img.classList.add("loading");
       var cw = this.root.offsetWidth;
        //img.loading = "lazy"; // Aggiungiamo l'attributo loading=lazy all'elemento img
        //img.title = currentContentId;
        img.addEventListener("load", (data) => {
            img.classList.remove("loading");
            img.original_Width = img.clientWidth;
            img.original_Height = img.clientHeight;
            var w = cw - 38;
            if (w > this.imgWidth)
                w = this.imgWidth;
//            img.style.width = w+"px";
            LoadImage(img);
            callback(img);
        });
        if (this.blur == true) {
            img.classList.add("ObscuredImages");
        }
        img.src = url;// imgUrl;
        var self = this;
//        const obscured = this.blur;
        img.addEventListener("mouseover", function (e) {
            self.updateCurrentPagesCount(e.target.pageId)
        });


        return img;
    }

    //Funzioni per gli elementi visuali
    updateLoadedPagesCount() {
        //const loadedPagesCount = document.getElementById("loaded-pages-count");
        //if (this.loadedPages + 1 <= this.totalPages)
        //  loadedPagesCount.textContent = (this.loadedPages + 1).toString();
    }




    scrollToImage(id) {
        var image = document.getElementById(id);
        if (image) {
            var fixedElement = document.getElementById("container_" + this.documentId + this.imageId);
            fixedElement.scrollTo({
                top: image.offsetTop,
                behavior: "smooth",
            });
            //image.classList.add("SelectedImage");
            this.updateCurrentPagesCount(image.pageId);
        }
    }

    calculateNextPage() {
        let ret = 1;
        const currentPagesCount = document.getElementById("current-page-count");
        ret = parseInt(currentPagesCount.textContent) + 1;
        if (ret > this.loadedPages)
            ret = this.loadedPages;
        return "page_" + this.documentId + this.imageId + "-" + (ret);
    }

    calculatePrevPage() {
        let ret = 1;
        const currentPagesCount = document.getElementById("current-page-count");
        ret = parseInt(currentPagesCount.textContent) - 1;
        if (ret < 1)
            ret = 1;
        return "page_" + this.documentId + this.imageId + "-" + (ret);

    }


    removeErrorImages() {
        const images = document.getElementsByTagName("img");
        for (let i = 0; i < images.length; i++) {
            const img = images[i];
            if (img.classList.contains("errorImage")) {
                img.parentNode.removeChild(img);
            }
        }
    }

    removeModal() {
        try {
            const modal = document.getElementById("passwordModal");
            modal.remove();
        } finally {
            return;
        }
    }
}
customElements.define("viewer-esteso", Esteso);
