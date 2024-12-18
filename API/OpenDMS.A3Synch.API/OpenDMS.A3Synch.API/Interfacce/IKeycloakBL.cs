using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IKeycloakBL
    {
        Task<int> SynchAllInKC();

        Task<(string AccessToken, string RefreshToken)> GetTokens();
        void ResetStatus();
    }
}
