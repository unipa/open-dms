function Carica(codice) {
    //Scritto per la lista di bootstrap italia
    //Ricordardi di inserire negli elementi <li> l'attrivuto codice con un id.
    // gestiste le classi che modificano la grafica degli elementi selezionati a sinistra
    if (!codice) { 
        codice = "";
    }
    //$(".link-list li").removeClass("active");
    $(".link-list li[codice='" + codice + "'] a").addClass("active");
}

function ShowSuccess() {
    //funzione usata per mostrare i messaggi di successo nelle pagine che utilizzano knockoutJS
    setTimeout(function () {
        $("#successMessage").fadeIn(250).delay(3000);
    }, 100);
}

//funzioni che animano il menu laterale  ------------------//
function cambiaClasse() {
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
///---------------------------------------------------//

function FadeOutSuccessMessagge(time = 10000) {
    //funzione per far svanire i messaggi di successo
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



function ShowMessage(type, message, Message_Target = 'Message_Target', RefreshPath) {
    // funzione per far comparire i messaggi di errore o di successo.
    // Adoperando i template Mustache i messaggi vengono inserire nel div con id uguale all'input 'Message_Target'
    // l'input RefreshPath può essere nullo

    var AlertTemplate = ` <div class="alert alert-danger mt-2 alert-dismissible fade show mx-4 bg-white" role="alert" style="position:fixed;right:0;bottom:0;z-index: 101;width: 400px;">
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Chiudi avviso">
                            <svg class="icon">
                                <use href="@($"{Configuration["PATH_BASE"]}/bootstrap-italia/svg/sprites.svg#it-close")"></use>
                            </svg>
                        </button>
                        <h4 class="alert-heading">Si è verificato un errore</h4>
                        <p style="height: 100%;width: 100%;padding:10px 0;line-break: auto;max-height: 200px;overflow: hidden;text-overflow: ellipsis;">
                            {{data.message}}
                        </p>
                    </div>`;

    var SuccessTemplate = `<div class="alert alert-success alert-dismissible fade show bg-white mt-2 mx-4" role="alert" style="position: fixed;right:0;bottom:0;z-index: 101;width: 400px">
                        <h4 class="alert-heading">{{data.message}}</h4>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Chiudi avviso">
                            <svg class="icon">
                                <use href="@($"{Configuration["PATH_BASE"]}/bootstrap-italia/svg/sprites.svg#it-close")"></use>
                            </svg>
                        </button>
                    </div>`;

    const template = (type == "success") ? SuccessTemplate : AlertTemplate;
    const rendered = Mustache.render(template, { data: { message: message, refreshPath: RefreshPath } });

    var div = document.createElement('div');
    div.innerHTML = rendered;

    //document.getElementById(Message_Target).innerHTML = rendered;
    document.body.appendChild(div);

}




function Confirm(text, Message_Target = 'Message_Target', yesCallback, noCallback = function () { return; }) {

    var Modal_Template = "ConfermaModal_Template";
    var Modal_Content = "ConfermaModal_Content";
    var Modal_Id = "ConfermaModal";
    var Modal_Form_Button = "ConfermaFormButton";
    var Modal_Form_Button_Cancella = "CancellaFormButton";

    //elaboro il template mustache dentro la modale ancora nascosta
    const template = `
                            <div class="modal " tabindex="-1" role="dialog" id="ConfermaModal" aria-labelledby="modal4Title">
                                <div class="modal-dialog modal-dialog-centered" role="document">
                                    <div class="modal-content" id="ConfermaModal_Content">
                                        <div class="modal-header">
                                            <h2 class="modal-title h5 " id="modalCenterTitle">Conferma azione</h2>
                                        </div>
                                        <div class="modal-body">
                                            <p>{{data}}</p>
                                        </div>
                                        <div class="modal-footer">
                                            <button class="btn btn-primary btn-sm" type="button" id="ConfermaFormButton">Conferma</button>
                                            <button class="btn btn-outline-secondary btn-sm" type="button" id="CancellaFormButton">Annulla</button>
                                        </div>
                                    </div>
                                </div>
                            </div>`;

    const rendered = Mustache.render(template, { data: text });

    var div = document.createElement('div');
    div.innerHTML = rendered;

    //document.getElementById(Message_Target).innerHTML = rendered;
    document.body.appendChild(div);

    //mostro la modale
    var ConfermaModal = new bootstrap.Modal(document.getElementById(Modal_Id));
    ConfermaModal.show()

    var ConfirmButton = document.getElementById(Modal_Form_Button);
    ConfirmButton.onclick = async (e) => {
        if (yesCallback)
            yesCallback();
        ConfermaModal.hide();
    };
    var CancelButton = document.getElementById(Modal_Form_Button_Cancella);
    CancelButton.onclick = async (e) => {
        if (noCallback)
            noCallback();
        ConfermaModal.hide();
    };
}

function isTokenExpired(token) {
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
    //funzione per refreshare il token, in caso di token scaduto, usa una chiamata ajax verso una api nell HomeController che restituisce l'access_token rigenerato.
    // Restituisce un token valido in caso successo (che sia uno nuovo o quello vecchio ancora in validità); Esegue il Reject della promise in caso di fallimento nel refresh.

    return new Promise((resolve, reject) => {

        if (isTokenExpired(token)) {
            $.ajax({
                url: "/Home/RefreshToken",
                type: "GET",
                headers: { "Content-Type": "application/json", 'accept': 'text/plain', "Authorization": "Bearer " + token },
                beforeSend: function (xhr) {
                    xhr.excludeBeforeSend = true; // Imposta la flag per escludere la beforeSend
                }
            })
                .done(function (newToken) {
                    resolve(newToken);
                })
                .fail(function (err) {
                    reject();
                });
            
        } else resolve(token);
    });
}

function AddRefreshTokenFilter() {
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

function ShowRequiredPopupJS(formId) {
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


// funzione per aggiungere giorni ana variabile Date. 
//es: new Date().addDays(1) restituirà la data di oggi avanti di un giorno
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function convertIntToDate(input) {
    var inputString = input.toString();
    if (inputString.length !== 8) {
        throw new Error("L'input deve essere un intero a 8 cifre nel formato 'yyyyMMdd'.");
    }

    var year = parseInt(inputString.substring(0, 4));
    var month = parseInt(inputString.substring(4, 6));
    var day = parseInt(inputString.substring(6, 8));

    var date = new Date(year, month-1, day+1);
    return date.toISOString().split('T')[0];
}






