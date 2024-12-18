# Introduzione 
Il servizio dms_api_signinfocert è un client che si interfaccia da un lato con l'app principale di OpenDMS e dall'altro con i servizi di firma remota di Infocert. 
Esse prendono in carico le richieste di firme remote per conto di OpenDMS, gestendo le fasi di autenticazione e richieste di firma di uno o più documenti per volta.

# Installazione:	

1. Modificare le opportune variabili di ambiente su file dms_api_signinfocert.yaml, descritte di seguito.
2. Deployare il file yaml dms_api_signinfocert.yaml nel proprio cluster.
 
# Parametri da configurare:

1. _baseuri : 
    Questo parametro indica il base uri dei servizi di Infocert, ad esempio "https://example_infocert_servizi.it".
	Attenzione: compreso di "https://".

2. PATH_BASE : 
    Questo parametro viene posto tra il domain e il path degli endpoint esposti dal servizio, 
	ad esempio: se un endpoint del servizio in questione è "https://dms_api_sign.com/Sign/ExampleEndpoint" ed il parametro PATH_BASE è "/path_base_example",
	Url finale per raggiungere l'endpoint sarà "https://dms_api_sign.com/path_base_example/Sign/ExampleEndpoint".

3. ReturnUrl : 
    Indica l'endpoint esposto dall'app principale di OpenDMS, 
	usata dal servizio di firma remota per avvisare il documentale che il file firmato è pronto per essere scaricato.
	Il path dell'endpoint è: "/RemoteSignHandler/ReceiveAckSignedFile", quindi nel parametro ReturnUrl va inserito "https://open_dms_host/RemoteSignHandler/ReceiveAckSignedFile" 
	sostituendo a "open_dms_host" l'host dell'app principale di OpenDMS.