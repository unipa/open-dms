namespace OpenDMS.Domain.Enumerators
{
    public enum JobStatus
    {
        /// <summary>
        /// Attività pronta per essere eseguita
        /// </summary>
        Queued = 0,
        /// <summary>
        /// Attività in corso
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
        /// Attiivtà annullata
        /// </summary>
        Aborted = 4,
        /// <summary>
        /// Attività da eseguire ma ignorata
        /// </summary>
        Ignored = 5,
        /// <summary>
        /// Attività non nencessaria
        /// </summary>
        NotNeeded = 6
    }
}