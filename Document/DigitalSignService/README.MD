# Introduzione
DigitalSignService è un applicativo per il sistema operativo Microsoft Windows che interfaccia i token di firma digitale USB.
Poichè, per motivi di sicurezza, non è possibile accedere in remoto a tali dispositivi, l'applicazione va attivata nel PC in cui è installato il token di firma.


## Installazione
E' sufficiente copiare l'intera cartella nel PC in cui deve essere eseguito e creare un collegamento dentro la sotto-cartella: 

C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup

oppure 

%AppData%\Roaming\Microsoft\Windows\Start Menu\Programs\Startup


## Configurazione
L'applicazione può essere configurata al primo tentativo di sottoscrizione dei documenti.
E' sufficiente indicare il tipo di provider utilizzato dal token affinchè il sistema proceda a  recuperare le informazioni relative allo slot e al dispositivo di firma. Successivamente si procederà a selezionare il certificato di firma e il relativo PIN.
Tutte queste informazioni, ad eccezione del PIN saranno memorizzate in un file di configurazione all'interno del PC e riproposte alla successiva richiesta di firma.



