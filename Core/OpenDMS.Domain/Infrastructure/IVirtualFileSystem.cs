namespace OpenDMS.Domain.Infrastructure;

public interface IVirtualFileSystem
{
    string Id { get;  }
    Task<bool> Exists(string fileName);

    Task<byte[]> ReadAllBytes(string fileName);
    Task<bool> WriteAllBytes(string fileName, byte[] image);

    Task<string> ReadAllText(string fileName);
    Task<bool> WriteAllText(string fileName, string Text);
    
    Task<Stream> ReadAsStream(string fileName);
    Task<bool> WriteFromStream(string toFileName, Stream fromStream);

    Task<bool> Delete(string fileName, bool LogicalDelete = false);
    Task<bool> Restore (string fileName);

    Task<bool> Move(string fromFileName, string toFileName);


//    byte[] Export(string fromFileName, string toFileName);
//    bool Import(string fromFileName, string toFileName, bool move = false);


    IEnumerable<string> GetFiles(string BaseDirectory, string Pattern, bool Recursive = false);
    IEnumerable<string> GetDirectories(string BaseDirectory, string Pattern);
}

