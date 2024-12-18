***SETUP AMBIENTE DI TEST***
Per preparare un ambiente di test è necessario installare i seguenti container:
- Zeebe (https://hub.docker.com/r/camunda/zeebe)
- MySql / MariaDB
- Seq


***SETUP KEYCLOAK***
1. Aprire Keycloak su http://localhost:18080/auth
2. accedere con "admin", "admin" 
3. recarsi sul realm "camunda-platform"
4. recarsi su "Clients"
4. creare un nuovo client con le seguenti impostazioni:
        nome = dms
        valid redirect URI = https://localhost:7001/*, https://localhost:5001/*, https://localhost:6001/*
        valid post logout URI = https://localhost:7001/*, https://localhost:5001/*, https://localhost:6001/*
        Web Origins = *, +
        Authetication flow = Standard, Direct Accesss,Implicit, OAuth 2.0
5. dopo aver confermato cliccare su Client Scopes 
6. cliccare su dms-dedicated
7. cliccare su "Add Mapper" -> "From predefined..." -> User Realm Role
8. impostare:
        mutivalued = On
        token Claim Name = roles
        clain JSON Type = String
        Add to ID token = On
        Add to access token = On
        Add to UserInfo = On
9. caricare almeno un utente con ruolo "admin"


***AVVIO AMBIENTE TEST MINIMALE***
1. avviare i container MySql e Zeebe
2. lanciare https://localhost:5001/ (api workflow)
3. lanciare https://localhost:7001/internalapi/swagger (api frontend)
4. eseguire l'api "Database" -> "Update"
5. lanciare https://localhost:7001/ (UI frontend)
6. lanciare https://localhost:6001/ (api firma digitale remota)
7. accedere cpm l'utente di amministrazione creato su keycloak
