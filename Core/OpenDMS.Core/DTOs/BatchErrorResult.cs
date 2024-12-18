using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenDMS.Core.DTOs;

public class BatchErrorResult
{
    public bool Warning { get; set; }
    public int ItemId { get; set; }

    [JsonIgnore]
    public Exception Exception { get; set; } = null;
    public string Message { get; set; } = "";

    public BatchErrorResult(int itemid, Exception ex)
    {
        ItemId = itemid;
        Warning = false;
        Exception = ex;
        Message = ex.Message;
    }
    public BatchErrorResult(int itemid, string message)
    {
        ItemId = itemid;
        Warning = true;
        Exception = new Exception(message);
        Message = message;
    }
}
