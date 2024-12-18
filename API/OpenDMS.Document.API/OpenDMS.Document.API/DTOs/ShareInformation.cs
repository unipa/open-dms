using OpenDMS.Domain.Enumerators;

namespace OpenDMS.DocumentManager.API.DTOs
{
    public class ShareInformation
    {
        /// <summary>
        /// Titolo (opzionale)
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Messaggio (opzionale, solo per RequestType generiche)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Elenco di <profileType><profileId> a cui inviare il documento
        /// </summary>
        public List<string> To { get; set; }
        /// <summary>
        /// Elenco di <profileType><profileId> a cui inviare il documento in CC
        /// </summary>
        public List<string> Cc { get; set; }
        /// <summary>
        /// Tipo di richiesta (nessuna, generica, ...)
        /// </summary>
        public ActionRequestType RequestType { get; set; }
        public bool AssignToAllUsers { get; set; }
    }
}
