using iText.Layout.Renderer;
using Web.DTOs;

namespace Web.BL
{
    public interface IRemoteSignService
    {
        // Elimina le Signroom dell'utente nello stato <= 2 (ready)
        // Ritorna l'elenco delle SignRoom eliminate 
        Task<List<SignRoom_DTO>> RemoveUnusedSignRooms(string UserName);

        // Recupera l'elenco delle SignRoom attive (Signing, signed, ...)
        Task<List<SignRoom_DTO>> GetActiveSignRooms(string UserName);

        // Ritorna l'ID della SignRoom creata oppure una Eccezione 
        Task<SignRoom_DTO> CreateSignRoom(string UserName, SignTypes SignType, int Documents);

        // Rimuove una SignRoom di un utente.
        Task DeleteSignRoom(string SignRoomId, string UserName);

        // Ritorna informazioni sulla SignRoom 
        Task<SignRoom_DTO> GetSignRoom(string SignRoomId);

        // Ritorna una Eccezione in caso di errore
        Task AddDocument (string SignRoomId, string FileName, string SignFields, byte[] FileData, string ReturnData = "");

        // Genera un nuovo OTP e lo invia all'utente
        Task GenerateOTP (string SignRoomId, string UserName);

        // Avvia il processo di firma e torna una Eccezione in caso di errori
        Task Sign(string SignRoomId, string OTP);

    }
}
