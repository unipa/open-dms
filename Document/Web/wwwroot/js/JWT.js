

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