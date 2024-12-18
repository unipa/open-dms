using OpenDMS.TitulusIntegration.API.Models;

namespace OpenDMS.TitulusIntegration.API.BL.Interfacce
{
    public interface ITitulusBL
    {
        string GetCorrectHost(string url);
        string GetFolderMetadada(long id);
        string Search(string query, string? orderby);
        string LoadDocument(long physdoc);
        string GetDocumentUrl(long physdoc);
        string GetFileBase64(string fileID);
        string EncodeNumProt(string input);
        string GetDocumentFromProtocol(string numero_protocollo);
        string CreateNewDocument(NewDocument doc,bool draft);
    }
}
