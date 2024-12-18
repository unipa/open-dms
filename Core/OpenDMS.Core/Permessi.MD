*** Gestione dei Permessi ***

La piattaforma distingue 2 tipologie di permessi:
1. Permessi applicativi
3. Permessi documentali

*** Permessi applicativi ***
I permessi applicativi definiscono le funzionalità a cui un utente può essere abilitato.
Sono gestiti all'interno della speciale ACL "Permessi Globali" e possono essere definiti a livello di utente, ruolo, ruolo in un gruppo o di gruppo

*** Permessi documentali ***
Definiscono i permessi sui documenti di un utente, ruolo, ruolo in un gruppo o di un gruppo e sono gestiti a 3 livelli:
1. Sui ruoli 
2. Sulle ACL associate ai documenti
3. Sui documenti

I permessi sui ruoli definiscono le operazioni consentite su tutti i documenti
I permessi sulle ACL definiscono le operazioni consentite sui documenti associati, e possono essere indicati per singolo utente, ruolo, gruppo o ruolo in un gruppo
I permessi sui documenti autorizzano o negano operazioni su singole istanze documentali e possono essere indicati per singolo utente, ruolo, gruppo o ruolo in un gruppo

** Calcolo dei permessi **
Per la determinazione dei permessi su un documento, il sistema adotta il seguente algoritmo:
1. L'utente è sicuramente autorizzato se esiste un permesso non revocato sul documento
2. L'utente è autorizzato se esiste almeno un ruolo o un gruppo che abbia un permesso non revocato nelle ACL o nel documento

I permessi possono essere revocati, per utente, ruolo o gruppo, solo sulle singole istanze documentali.
Per verificare se esiste almeno un ruolo con un permesso non revocato, il sistema estrae tutti i ruoli  dell'utente (non associati a gruppi) che possiedono il permesso richiesto, tutti i ruoli dell'utente che sono stati autorizzati nelle acl, tutti i ruoli dell'utente indicati nelle istanze documentali e verifica che ne esista almeno uno non revocato. Lo stesso procedimento avviene per i gruppi.



