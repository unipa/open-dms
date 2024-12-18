namespace OpenDMS.MaximoSR.API.Utility.Interfacce
{
    public interface IHttpCallHandlerV2
    {
        Task<T> DeleteAsyncCall<T>(string URL, StringContent content, string? token);
        Task<T> GetAsyncCall<T>(string URL, string? token, List<Tuple<string, string>>? headers);
        Task<string> GetStringAsyncCall(string URL, string? token, List<Tuple<string, string>>? headers);
        Task<T> PostAsyncCall<T>(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers);
        Task<string> PostStringAsyncCall(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers);
        Task<T> PutAsyncCall<T>(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers);
    }
}