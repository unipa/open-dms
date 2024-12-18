namespace MessageBus.Interface
{

    public delegate Task MessageReceived<T>(T message);

    public interface IMessaging<T>: IDisposable
    {
        event MessageReceived<T> OnMessageReceived;

        /// <summary>
        ///  Pubblica un messaggio sulla coda. 
        /// </summary>
        /// <typeparam name="T"> Tipo di oggetto che costituirà il messaggio. </typeparam>
        /// <param name="message"> Oggetto che costituirà il messaggio. </param>
        /// <param name="queue"> Nome della coda.</param>
        void PushMessage(T message, string queue);

        /// <summary>
        /// Funzione che estrae dalla coda un singolo messaggio e manda immediatamente il comando di eliminazione del messaggio alla coda
        /// </summary>
        /// <typeparam name="T"> Tipo di oggetto in cui deserializzare il messaggio </typeparam>
        /// <param name="queue"> nome della coda </param>
        /// <returns> Oggetto di tipo T trovato nel primo messaggio disponibile sulla coda</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        T BasicGetSingleMessage(string queue);

        /// <summary>
        /// Funzione che estrae dalla coda un singolo messaggio ed esegue la funzione "beforeAck(messaggio estratto)" passata in input  prima di mandare l'ack di ricezione al server
        /// </summary>
        /// <typeparam name="T"> Tipo di oggetto in cui deserializzare il messaggio </typeparam>
        /// <param name="queue"> nome della coda </param>
        /// <returns> Oggetto di tipo T trovato nel primo messaggio disponibile sulla coda</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        T GetSingleMessage(string queue, Action<T> beforeAck);

        /// <summary>
        /// Mette in attesa di messaggi disponibili l'oggetto 
        /// </summary>
        /// <param name="queue"> nome della coda </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task Listening(string queue);

        void StopListening();

        void Dispose();
    }

}