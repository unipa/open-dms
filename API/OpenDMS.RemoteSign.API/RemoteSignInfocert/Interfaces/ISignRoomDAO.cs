using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.Interfaces
{
    public interface ISignRoomDAO
    {
        int ConfirmDelivery(string SignRoom, string? esito = "");
        List<SignRoomModel> GetAllSignRooms();
        SignRoomModel GetSignRoom(string SignRoom,string username);
        SignRoomModel GetSignRoom(string SignRoom);
        List<SignRoomModel> GetSignRoomsToDelivery();
        bool AddOrUpdateSignRoom(SignRoomModel model);
        bool UpdateSignRoomStatus(string signroom, SignRoomStatus status, string StatusComment = "");
        IEnumerable<FileParameter> GetSignRoomFiles(string SignRoom, string username);
    }
}