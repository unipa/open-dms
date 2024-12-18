namespace OpenDMS.Domain.Enumerators
{
    public enum JobStatus
    {
        /// <summary>
        /// Attivit� pronta per essere eseguita
        /// </summary>
        Queued = 0,
        /// <summary>
        /// Attivit� in corso
        /// </summary>
        Running = 1,
        /// <summary>
        /// Completata con successo
        /// </summary>
        Completed = 2,
        /// <summary>
        /// Completata in errore
        /// </summary>
        Failed = 3,
        /// <summary>
        /// Attiivt� annullata
        /// </summary>
        Aborted = 4,
        /// <summary>
        /// Attivit� da eseguire ma ignorata
        /// </summary>
        Ignored = 5,
        /// <summary>
        /// Attivit� non nencessaria
        /// </summary>
        NotNeeded = 6
    }
}