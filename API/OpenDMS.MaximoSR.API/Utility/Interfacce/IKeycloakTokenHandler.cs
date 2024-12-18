namespace OpenDMS.MaximoSR.API.Utility.Interfacce
{
    public interface IKeycloakTokenHandler
    {
        Task<string?> IsLoginStillValidAsync();
        Task<string> RefreshTokenAsync(string refreshToken);
    }
}