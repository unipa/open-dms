using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IUtils
    {
        string GetApiTimestamp();
        string GetApiNonce();
        string GetApiToken(string apiTimestamp, string apiNonce);
        DateTime GetData();
        bool CheckClosed(string date);
        int ISOnumericDate(DateTime? date);
        DateTime ISOnumericToDate(string isoDate);
        bool IsJSON(string jsonString);
        Task<StrutturaInput> GetOrganizationPage(string url);
//        Task<string> GetAllOrganizationsJson();
        Task<List<Struttura>> GetAllOrganizationNodes();
        int MaxRightBound(List<OrganizationNodes> list);
        int ParentLeftBound(List<OrganizationNodes> list, string id_struttura);
        Task<List<Members>> GetMembersInStructure(string UserGroupId, string ExternalId);
        Task<List<Members>> GetAllMembersInStructures();
        Task ClearFile();
        Task SendErrorNotification(string errorMessage);

    }
}
