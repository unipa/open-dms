using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using OpenDMS.Domain.Constants;

namespace OpenDMS.Domain.Infrastructure.Providers;

public class OSFileSystem : AbstractFileSystem
{
    public string Id { get; } = "";
    /// <summary>
    /// RootFolder deve essere un percorso di rete o un filesystem montato (es. smb, nfs) nel formato linux/mac
    /// /volumes/archivio/
    /// /share/archivio/
    /// 
    /// la classe provvederà a trasformarlo nel formato windows se necessario
    /// \\127.0.0.1\Volumes\archivio\
    /// \\127.0.0.1\share\archivio\
    /// 
    /// </summary>
    /// <param name="RootFolder"></param>
    public OSFileSystem(IConfiguration configuration)
    {
        rootFolder = configuration[StaticConfiguration.CONST_DOCUMENTS_ROOLFOLDER];
        if (string.IsNullOrEmpty(rootFolder))
        {
            rootFolder = OperatingSystem.IsWindows()
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage")
                : Path.Combine("var","opt","dms");
        }
        rootFolder = Path.Combine(rootFolder, "files");
        if (!Directory.Exists(rootFolder))
        {
            Directory.CreateDirectory(rootFolder);
        }
    }
    public override async Task<Stream> ReadAsStream(string fileName)
    {
        var OSFileName = GetOSFilePath(Path.Combine(rootFolder, fileName));
        if (File.Exists(OSFileName))
            return File.OpenRead(OSFileName);
        return null;
    }

    public override async Task<byte[]> ReadAllBytes(String fileName)
    {
        var OSFileName = GetOSFilePath(Path.Combine(rootFolder, fileName));
        if (File.Exists(OSFileName))
            return File.ReadAllBytes(OSFileName);
        return null;
    }

    public override async Task<bool> WriteAllBytes(String fileName, byte[] image)
    {
        var OSFileName = GetOSFilePath(Path.Combine(rootFolder, fileName));
        bool retVal = false;
        String dir = Path.GetDirectoryName(OSFileName);
        if (dir != null)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
         //   if (!File.Exists(OSFileName))
            {
                File.WriteAllBytes(OSFileName, image);
                retVal = true;
            }
        
        return retVal;
    }

    public override async Task<bool> Delete(String fileName, Boolean LogicalDelete)
    {
        var OSFileName = GetOSFilePath(Path.Combine(rootFolder, fileName));
        bool retVal = false;
        if (File.Exists(OSFileName))
            if (LogicalDelete)
            {
                if (!File.Exists(OSFileName + ".Deleted"))
                    File.Move(fileName, OSFileName + ".Deleted");
            }
            else
                File.Delete(OSFileName);
        retVal = true;
        return retVal;
    }

    public override async Task<bool> Exists(string fileName)
    {
        var OSFileName = GetOSFilePath(Path.Combine(rootFolder, fileName));
        return File.Exists(OSFileName);
    }

    public override async Task<bool> Restore(string fileName)
    {
        var OSFileName = GetOSFilePath(Path.Combine(rootFolder, fileName));
        bool retVal = false;
        if (File.Exists(OSFileName)) return false;
        if (File.Exists(OSFileName + ".Deleted"))
        {
            File.Move(OSFileName + ".Deleted", OSFileName);
            retVal = true;
        }
        return retVal;        
    }


    public override IEnumerable<string> GetFiles(string BaseDirectory, string Pattern, bool Recursive = false)
    {
        BaseDirectory = Path.Combine(rootFolder, BaseDirectory);

        return Directory.EnumerateFiles(BaseDirectory, Pattern, Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }

    public override IEnumerable<string> GetDirectories(string BaseDirectory, string Pattern)
    {
        BaseDirectory = Path.Combine(rootFolder, BaseDirectory);
        return Directory.EnumerateDirectories(BaseDirectory, Pattern, SearchOption.AllDirectories );
    }


    private string GetOSFilePath(string path)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            var d = path.Replace('/', Path.DirectorySeparatorChar);
            //if (d.StartsWith(Path.DirectorySeparatorChar))
            //    d = "\\localhost" + d;
            return  d;
        }
        else
        {
            var d = path.Replace('\\', '/');
            return  d;
        }
    }
}
