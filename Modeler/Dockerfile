# Utilizza l'immagine ufficiale di Node.js come base
FROM node:16

# Crea la directory dell'applicazione
WORKDIR /app

# Copia il package.json e package-lock.json nella cartella /app
COPY package*.json ./

# Copia la cartella node_modules esistente
COPY node_modules/ node_modules/

# Copia il resto dei file dell'applicazione nella cartella /app
COPY . .

RUN npm i npm-run-all
RUN npm i webpack

# Esegui la build dell'applicazione
#RUN npm run build

# Espone la porta 5000 per l'applicazione
#EXPOSE 5000

# Avvia l'applicazione
CMD ["npm", "run", "start"]
