using Microsoft.EntityFrameworkCore;
using RemoteSignInfocert.Context;
using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Models;
using RemoteSignInfocert.Utils;

namespace RemoteSignInfocert.DAO
{
    public class SignRoomDAO : ISignRoomDAO
    {
        private readonly UserDbContext _ctx;
        public SignRoomDAO(UserDbContext ctx)
        {
            _ctx = ctx;
        }

        public SignRoomModel GetSignRoom(string SignRoom, string username)
        {
            return _ctx.SignRooms.FirstOrDefault(p => p.SignRoom == SignRoom && p.UserName == username);
        }

        public SignRoomModel GetSignRoom(string SignRoom)
        {
            return _ctx.SignRooms.FirstOrDefault(p => p.SignRoom == SignRoom);
        }

        public List<SignRoomModel> GetSignRoomsToDelivery()
        {
            return _ctx.SignRooms.AsNoTracking().Where(p => p.Delivered == false && p.Status == SignRoomStatus.Signed).ToList();
        }

        public List<SignRoomModel> GetAllSignRooms()
        {
            return _ctx.SignRooms.AsNoTracking().ToList();
        }

        public int ConfirmDelivery(string SignRoom, string? esito = "")
        {
            var signRoom = _ctx.SignRooms.FirstOrDefault(p => p.SignRoom == SignRoom);
            signRoom.Delivered = true;
            signRoom.DeliveryDate = DateTime.UtcNow;
            signRoom.DeliveryResult = esito ?? "";
            signRoom.Status = SignRoomStatus.Completed;
            _ctx.SignRooms.Update(signRoom);
            return _ctx.SaveChanges();
        }

        public bool AddOrUpdateSignRoom(SignRoomModel model)
        {


            var sr = GetSignRoom(model.SignRoom);

            if (sr == null)
            {
                model.Delivered = false;
                model.DeliveryDate = null;
                _ctx.Add(model);
            }
            else
            {
                sr.Status = model.Status;
                sr.NumeroFile = model.NumeroFile;
                _ctx.Update(sr);
            }
            _ctx.SaveChanges();
            return true;
        }
        
        public bool UpdateSignRoomStatus(string signroom, SignRoomStatus status, string StatusComment = "")
        {
            var sr = GetSignRoom(signroom);

            if (sr == null)
                throw new NullReferenceException(signroom);
            else
                sr.Status = status;
            sr.StatusComment = StatusComment;
            sr.DeliveryResult = StatusComment;
            _ctx.Update(sr);

            _ctx.SaveChanges();
            return true;
        }

        public IEnumerable<FileParameter> GetSignRoomFiles(string SignRoom, string username)
        {
            return FileManagerUtils.GetAllFileParameter(SignRoom);
        }

    }
}
