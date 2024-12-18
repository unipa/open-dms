using System.Text;

namespace OpenDMS.Domain.Infrastructure.Providers;
public  class AbstractFileSystem : IVirtualFileSystem
{
    protected string rootFolder = "";
    public string Id { get; } = "";
    public virtual async Task<string> ReadAllText(string fileName)
    {
        // fileName = Path.Combine(rootFolder, fileName);
        byte[] data = await ReadAllBytes(fileName);
        string text = Encoding.UTF8.GetString(data);
        return text;
    }

    public virtual async Task<bool> WriteAllText(string fileName, string Text)
    {
//        fileName = Path.Combine(rootFolder, fileName);
        byte[] data = Encoding.UTF8.GetBytes(Text);
        return await WriteAllBytes(fileName, data);
    }

    public virtual async Task<Stream> ReadAsStream(string fileName)
    {
//        fileName = Path.Combine(rootFolder, fileName);
        return new MemoryStream(await ReadAllBytes(fileName));
    }

    public virtual async Task<bool> WriteFromStream(string toFileName, Stream fromStream)
    {
//        toFileName = Path.Combine(rootFolder, toFileName);
        //if (await Exists(toFileName)) return false;

        using (var M = new MemoryStream())
        {
            fromStream.Seek(0, SeekOrigin.Begin);
            fromStream.CopyTo(M);
            M.Seek(0, SeekOrigin.Begin);
            return await WriteAllBytes(toFileName, M.ToArray());
        }
    }

    public virtual async Task<bool> Move(string fromFileName, string toFileName)
    {
        //fromFileName = Path.Combine(rootFolder, fromFileName);
        //toFileName = Path.Combine(rootFolder, toFileName);
        //if (!File.Exists(fromFileName)) return false;
        //if (File.Exists(toFileName)) return false;
        bool ok = await WriteAllBytes(toFileName, await ReadAllBytes(fromFileName));
        if (ok) await Delete(fromFileName);
        return ok;
    }


    public virtual IEnumerable<string> GetFiles(string Directory, bool Recursive = false)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<string> GetDirectories(string BaseDirectory)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<bool> Exists(string fileName)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<byte[]> ReadAllBytes(string fileName)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<bool> WriteAllBytes(string fileName, byte[] image)
    {
        return true;
    }

    public virtual async Task<bool> Delete(string fileName, bool LogicalDelete = false)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<bool> Restore(string fileName)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<string> GetFiles(string BaseDirectory, string Pattern, bool Recursive = false)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<string> GetDirectories(string BaseDirectory, string Pattern)
    {
        throw new NotImplementedException();
    }
}
