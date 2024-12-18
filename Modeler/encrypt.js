const crypto = require('crypto');
const fs = require('fs');
const path = require('path');
const dotenv = require('dotenv');

// Carica le variabili d'ambiente dal file .env
dotenv.config();

const ENCRYPTION_KEY = process.env.ENCRYPTION_KEY;
const IV_LENGTH = 16;

const pathToEnv = path.join(__dirname, '.env'); // Percorso al tuo file .env

function encrypt(text) {
    let iv = crypto.randomBytes(IV_LENGTH);
    let cipher = crypto.createCipheriv('aes-256-cbc', Buffer.from(ENCRYPTION_KEY), iv);
    let encrypted = cipher.update(text);

    encrypted = Buffer.concat([encrypted, cipher.final()]);

    // Crea un oggetto con i valori iv e encryptedData
    let output = {
        iv: iv.toString('hex'),
        encryptedData: encrypted.toString('hex')
    };

    // Restituisci una stringa JSON
    return JSON.stringify(output);
}


const data = process.argv[2]; // Prendi l'input da linea di comando
const encrypted = encrypt(data);

// Leggi il file .env
let env = fs.readFileSync(pathToEnv, 'utf-8');

// Sostituisci la passphrase con la versione criptata
env = env.replace(/(passphraseCert=).*/, `$1${encrypted}`);

// Riscrivi il file .env con il nuovo valore
fs.writeFileSync(pathToEnv, env);

console.log('Password criptata e scritta nel file .env con successo.');
