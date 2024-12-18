const https = require("https");
const fs = require("fs");
const path = require("path");
const sirv = require("sirv");
const express = require("express");
const dotenv = require("dotenv");
const crypto = require("crypto");
const jwt = require("express-jwt");
const Keycloak = require("keycloak-connect");
const session = require("express-session");
const cors = require("cors");
const axios = require("axios");
const bodyParser = require("body-parser");

// Carica le variabili d'ambiente dal file .env
dotenv.config();

const certificatePATH = process.env.certificatePATH;
const ENCRYPTION_KEY = process.env.ENCRYPTION_KEY;

function decrypt(text) {
  let textParts = JSON.parse(text); // Converti la stringa JSON in un oggetto
  let iv = Buffer.from(textParts.iv, "hex");
  let encryptedText = Buffer.from(textParts.encryptedData, "hex");
  let decipher = crypto.createDecipheriv(
    "aes-256-cbc",
    Buffer.from(ENCRYPTION_KEY),
    iv
  );
  let decrypted = decipher.update(encryptedText);

  decrypted = Buffer.concat([decrypted, decipher.final()]);

  return decrypted.toString();
}

const passphraseCertEncrypted = process.env.passphraseCert;
const passphraseCert = decrypt(passphraseCertEncrypted);

const options = {
  pfx: fs.readFileSync(path.join(__dirname, certificatePATH)),
  passphrase: passphraseCert,
};

const app = express();

// Middleware per impostare l'intestazione X-Frame-Options su SAMEORIGIN
app.use((req, res, next) => {
  res.setHeader("X-Frame-Options", "SAMEORIGIN");
  next();
});

const basePath = process.env.BASEPATH;

const host = process.env.HOST;

const port = process.env.PORT;

const formjs_host = process.env.FORM_JS_HOST;

const formjs_PORT = process.env.FORM_JS_PORT;

const formjs_basepath = process.env.FORM_JS_BASEPATH;

const clientId = process.env.clientId;

const serverUrl = process.env.serverUrl;

const realm = process.env.realm;

const secret = process.env.secret;

const documentAPI = process.env.documentAPI;

const checkoutAPI = process.env.checkoutAPI;

const checkinAPI = process.env.checkinAPI;

const appContextApi = process.env.appContextApi;

const iframepath = process.env.iframepath;

// Configura Keycloak
let keycloakConfig = {
  clientId: clientId,
  bearerOnly: false,
  serverUrl: serverUrl,
  realm: realm,
  credentials: {
    secret: secret,
  },
};

//ignora i certificati self signed
process.env["NODE_TLS_REJECT_UNAUTHORIZED"] = 0;

let keycloak = new Keycloak(
  {
    store: require("memory-cache"),
  },
  keycloakConfig
);

app.use(
  cors({
    origin: "*", // o qualsiasi altro dominio che stai utilizzando
    credentials: true, // permette di condividere i cookies tra diversi domini
  })
);

app.use(
  session({
    secret: "efwefwe444444asdgasgvsd",
    resave: false, // non salva nuovamente la sessione se non viene modificata
    saveUninitialized: false, // non salva una nuova sessione che non è stata modificata
    cookie: { secure: true }, // attiva solo se il tuo sito è servito su HTTPS
  })
);

// Utilizza Keycloak come middleware, tutte le richieste passeranno per questo middleware
// che si occuperà di verificare la validità del token JWT
app.use(
  keycloak.middleware({
    logout: "/logout",
  })
);

// Per le rotte che vuoi proteggere, utilizza il metodo keycloak.protect()
app.use(
  basePath,
  keycloak.protect(),
  sirv("public/bpm/camunda", { dev: true })
);

//chiama api per il get dei diagrammi
app.get(
  basePath + "/api/getDiagram/:id",
  keycloak.protect(),
  async function (req, res) {
    var token =
      req.kauth && req.kauth.grant && req.kauth.grant.access_token.token;
    var id = req.params.id; // Prendi l'ID dal parametro della rotta

    if (token) {
      try {
        const response = await axios.get(documentAPI + id + checkoutAPI, {
          headers: {
            accept: "text/plain",
            Authorization: `Bearer ${token}`,
          },
        });

        res.json(response.data);
      } catch (error) {
        console.log(error);
        res.status(error.response.status).send(error.response.statusText);
      }
    } else {
      res.status(401).send("Non autorizzato");
    }
  }
);

app.get(
  basePath + "/api/getPalette",
  keycloak.protect(),
  async function (req, res) {
    var token =
      req.kauth && req.kauth.grant && req.kauth.grant.access_token.token;

    if (token) {
      try {
        const response = await axios.get(appContextApi, {
          headers: {
            accept: "text/plain",
            Authorization: `Bearer ${token}`,
          },
        });

        // legge un altro file JSON
        const secondFilePath = path.join(
          __dirname,
          "/app/descriptors/customelements.json"
        );
        let secondRawdata = fs.readFileSync(secondFilePath);
        let secondFile = JSON.parse(secondRawdata);

        console.log(response.data);

        // sostituisce interamente il contenuto del secondo file con response.data.customElementItems
        if (
          response.data.taskGroups !== undefined &&
          response.data.taskGroups !== "undefined"
        ) {
          secondFile = response.data.taskGroups;
        } else {
          secondFile = [];
        }

        fs.writeFileSync(secondFilePath, JSON.stringify(secondFile, null, 2));

        res.json(response.data);
      } catch (error) {
        console.log(error);
        res.status(error.response.status).send(error.response.statusText);
      }
    } else {
      res.status(401).send("Non autorizzato");
    }
  }
);

// Configura Express per utilizzare il body-parser come middleware.
app.use(bodyParser.json({ limit: "900mb" }));
app.use(bodyParser.urlencoded({ limit: "900mb", extended: true }));

app.post(
  basePath + "/api/postDiagram/:id",
  keycloak.protect(),
  async function (req, res) {
    var token =
      req.kauth && req.kauth.grant && req.kauth.grant.access_token.token;
    var id = req.params.id; // Prendi l'ID dal parametro della rotta

    //console.log(req.body);

    if (token) {
      try {
        const response = await axios.post(
          documentAPI + id + checkinAPI,
          {
            content: req.body.content, // ricevi il contenuto dal corpo della richiesta
            previewInBase64: req.body.previewInBase64, // ricevi la previewInBase64 dal corpo della richiesta
            previewExtension: req.body.previewExtension,
            toBePublished: req.body.toBePublished,
          },
          {
            headers: {
              accept: "*/*",
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        //console.log(response.status);
        res.json(response.status);
      } catch (error) {
        console.log(error);
        res.status(error.response.status).send(error.response.statusText);
      }
    } else {
      res.status(401).send("Non autorizzato");
    }
  }
);

const server = https.createServer(options, app);

// Leggi il file JSON
const jsonFilePath = path.join(__dirname, "/app/descriptors/tenantConfig.json");
let jsonData = fs.readFileSync(jsonFilePath);
let jsonObject = JSON.parse(jsonData);

if (port == 443 || port == 80) {
  // Modifica i valori nel file JSON
  jsonObject.urls.forEach((url) => {
    url.domain = `${host}:`;
    url.iframepath = iframepath;
  });
} else {
  // Modifica i valori nel file JSON
  jsonObject.urls.forEach((url) => {
    url.domain = `${host}:${port}`;
    url.iframepath = iframepath;
  });
}

if (formjs_PORT == 443 || formjs_PORT == 80) {
  // Modifica i valori nel file JSON
  jsonObject.urls.forEach((url) => {
    url.form_js_url = `https://${formjs_host}${formjs_basepath}`;
    url.iframepath = iframepath;
  });
} else {
  // Modifica i valori nel file JSON
  jsonObject.urls.forEach((url) => {
    url.form_js_url = `https://${formjs_host}:${formjs_PORT}${formjs_basepath}`;
    url.iframepath = iframepath;
  });
}

// Riscrivi il file JSON con i nuovi valori
fs.writeFileSync(jsonFilePath, JSON.stringify(jsonObject, null, 2));

// Leggi il file JSON
const jsonFilePath2 = path.join(
  __dirname,
  "/public/bpm/camunda/descriptors/tenantConfig.json"
);
let jsonData2 = fs.readFileSync(jsonFilePath2);
let jsonObject2 = JSON.parse(jsonData2);

if (port == 443 || port == 80) {
  // Modifica i valori nel file JSON
  jsonObject2.urls.forEach((url) => {
    url.domain = `${host}:`;
    url.iframepath = iframepath;
  });
} else {
  // Modifica i valori nel file JSON
  jsonObject2.urls.forEach((url) => {
    url.domain = `${host}:${port}`;
    url.iframepath = iframepath;
  });
}

if (formjs_PORT == 443 || formjs_PORT == 80) {
  // Modifica i valori nel file JSON
  jsonObject2.urls.forEach((url) => {
    url.form_js_url = `https://${formjs_host}${formjs_basepath}`;
    url.iframepath = iframepath;
  });
} else {
  // Modifica i valori nel file JSON
  jsonObject2.urls.forEach((url) => {
    url.form_js_url = `https://${formjs_host}:${formjs_PORT}${formjs_basepath}`;
    url.iframepath = iframepath;
  });
}

// Riscrivi il file JSON con i nuovi valori
fs.writeFileSync(jsonFilePath2, JSON.stringify(jsonObject2, null, 2));

// Leggi il file HTML
const htmlFilePath = path.join(__dirname, "/app/index.html");
let htmlData = fs.readFileSync(htmlFilePath, "utf-8");

// Sostituisci i riferimenti a "/bpm/camunda" con basePath negli import di file CSS e JS
htmlData = htmlData.replace(
  /href="\/[^"]*?((\/vendor|\/css)[^"]*)/g,
  `href="${basePath}$1`
);
htmlData = htmlData.replace(
  /src="\/[^"]*?(\/app\.js[^"]*)/g,
  `src="${basePath}$1`
);

// Riscrivi il file HTML con i nuovi valori
fs.writeFileSync(htmlFilePath, htmlData);

// Leggi il file HTML
const htmlFilePath2 = path.join(__dirname, "/public/bpm/camunda/index.html");
let htmlData2 = fs.readFileSync(htmlFilePath2, "utf-8");

// Sostituisci i riferimenti a "/bpm/camunda" con basePath negli import di file CSS e JS
htmlData2 = htmlData2.replace(
  /href="\/[^"]*?((\/vendor|\/css)[^"]*)/g,
  `href="${basePath}$1`
);
htmlData2 = htmlData2.replace(
  /src="\/[^"]*?(\/app\.js[^"]*)/g,
  `src="${basePath}$1`
);

// Riscrivi il file HTML con i nuovi valori
fs.writeFileSync(htmlFilePath2, htmlData2);

server.listen(port, () => {
  console.log(`Server running at https://${host}:${port}${basePath}`);
});
