function Carica(codice) {
    // [autore: Bosco]
    //Scritto per la lista di bootstrap italia
    //Ricordardi di inserire negli elementi <li> l'attrivuto codice con un id.
    // gestiste le classi che modificano la grafica degli elementi selezionati a sinistra
    if (!codice) codice = "";
    $(".link-list li").removeClass("active");
    $(".link-list li[codice='" + codice + "'] a").addClass("active");
}

function ShowSuccess() {
    // [autore: Bosco]
    //funzione usata per mostrare i messaggi di successo nelle pagine che utilizzano knockoutJS
    setTimeout(function () {
        $("#successMessage").fadeIn(250).delay(3000);
    }, 100);
}

function cambiaClasse() { //funzioni che animano il menu laterale [autore: Martinez]
    var elemento = document.getElementById("leftMenuAnimationShow");
    var elemento2 = document.getElementById("leftMenuAnimationHide");


    if (elemento.classList.contains("xcol-2")) {
        elemento.classList.toggle("xcol-2", false);
        elemento.classList.toggle("xcol-1", true);
        //elemento.classList.toggle("ms-5", false);
        $(".sidebarText").show().fadeOut(300);

    } else {
        elemento.classList.toggle("xcol-1", false);
        elemento.classList.toggle("xcol-2", true);
        //elemento.classList.toggle("ms-4", true);
        $(".sidebarText").hide().fadeIn(300);
    }

    if (elemento2.classList.contains("xcol-10")) {
        elemento2.classList.toggle("xcol-10", false);
        elemento2.classList.toggle("xcol-11", true);
        //elemento.classList.toggle("ms-5", false);
        //      $(".sidebarText").show().fadeOut(300);
    } else {
        elemento2.classList.toggle("xcol-11", false);
        elemento2.classList.toggle("xcol-10", true);
        //elemento.classList.toggle("ms-4", true);
        //      $(".sidebarText").hide().fadeIn(300);
    }
}

function SelezionaMenu(menu) {
    $(".menu__box .list-item a ." + menu).parent().parent().parent().addClass("active");

}
function FadeOutSuccessMessagge(time = 10000) {
    // [autore: Bosco]
    var alertNode = $('.alert-success')
    alertNode.fadeOut(time);
}

function AttivazionePopover() {
    // funzione per attivare i popover di bootstrap italia,  richiamata al document.ready()
    // oppure alla fine del Select() in caso di knockout js
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    })
}


function ShowMessage(type, message, Message_Target = 'Message_Target', RefreshPath = "/Home/Index") {

    var AlertTemplate = ` <div class="alert alert-danger mt-2 alert-dismissible fade show mx-4 bg-white" role="alert" style="position: fixed;right: 0;left: 0;z-index: 101;width: -webkit-fill-available;">
                        <h6 class="alert-heading">Qualcosa è andato storto durente il caricamento dei dati: </h6>
                        <p>{{data.message}}<a href="{{data.refreshPath}}"><button type="button" class="btn btn-link ">Ricarica</button></a></p>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Chiudi avviso">
                            <svg class="icon">
                                <use href="@($"{Configuration["PATH_BASE"]}/bootstrap-italia/svg/sprites.svg#it-close")"></use>
                            </svg>
                        </button>
                    </div>`;

    var SuccessTemplate = `<div class="alert alert-success alert-dismissible fade show bg-white mt-2 mx-4" role="alert" style="position: fixed;right: 0;left: 0;z-index: 101;width: -webkit-fill-available;">
                        <p>{{data.message}}</p>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Chiudi avviso">
                            <svg class="icon">
                                <use href="@($"{Configuration["PATH_BASE"]}/bootstrap-italia/svg/sprites.svg#it-close")"></use>
                            </svg>
                        </button>
                    </div>`;

    const template = (type == "success") ? SuccessTemplate : AlertTemplate;
    const rendered = Mustache.render(template, { data: { message: message, refreshPath: RefreshPath } });
    document.getElementById(Message_Target).innerHTML = rendered;

}

function isTokenExpired(token) {
    // [autore: Bosco]
    // funzione per verificare se il token è scaduto. Ritorna true in caso di token scaduto.

    // Leggo la scadenza
    let decodedToken = null;
    try {
        decodedToken = parseJwt(token);
    } catch (err) { return true; }
    const tokenExpiration = decodedToken.exp;
    const expiration = new Date(tokenExpiration * 1000);

    // Ottengo il datetime in questo momento
    const now = new Date();

    // Controllo
    return expiration < now;

    function parseJwt(token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    }
}

function RefreshToken(token) {
    // [autore: Bosco]
    //funzione per refreshare il token, in caso di token scaduto, usa una chiamata ajax verso una api nell HomeController che restituisce l'access_token rigenerato.
    // Restituisce un token valido in caso successo (che sia uno nuovo o quello vecchio ancora in validità); Esegue il Reject della promise in caso di fallimento nel refresh.

    return new Promise((resolve, reject) => {

        if (isTokenExpired(token)) {
            //$.ajax({
            //    url: "/Home/RefreshToken",
            //    type: "GET",
            //    headers: { "Content-Type": "application/json", 'accept': 'text/plain', "Authorization": "Bearer " + token },
            //    beforeSend: function (xhr) {
            //        xhr.excludeBeforeSend = true; // Imposta la flag per escludere la beforeSend
            //    }
            //})
            //    .done(function (newToken) {
            //        resolve(newToken);
            //    })
            //    .fail(function (err) {
            //        reject();
            //    });

            reject();

        } else resolve(token);
    });
}

function AddRefreshTokenFilter() {
    // [autore: Bosco]
    //funzione per intercettare le call ajax ed eseguire il refresh del token quando necessario 
    //condizioni: deve esistere una variabile globale "token" che viene usate per eseguire le chiamate ajax
    $.ajaxSetup({
        beforeSend: function (xhr) {
            RefreshToken(token) //esegue il controllo sul token[variabile globale]
                .then((newToken) => {
                    /*[variabile globale]*/
                    token = newToken;
                })
                .catch(function (err) {
                    //in caso di fallimento del refresh, esegue l'abort della chiamata.
                    xhr.responseText = "Autenticazione scaduta.";
                    xhr.abort(); // Annulla la chiamata AJAX
                });
        }
    });
}

function ShowRequiredPopup(formId) {
    //funzione per far comparire i popup manualmente in caso di campi non validi

    var formHTML = document.getElementById(formId);

    // Seleziona tutti gli elementi input richiesti all'interno del div temporaneo
    var requiredInputs = formHTML.querySelectorAll('input[required]');

    // Itera sui campi di input richiesti
    requiredInputs.forEach(function (inputElement) {
        // Verifica la validità del campo di input
        if (!inputElement.checkValidity()) {
            // Triggera l'evento "invalid" sull'elemento input non valido
            inputElement.dispatchEvent(new Event('invalid'));

            // Imposta il valore vuoto per l'elemento input
            inputElement.value = '';
        }
    });
}