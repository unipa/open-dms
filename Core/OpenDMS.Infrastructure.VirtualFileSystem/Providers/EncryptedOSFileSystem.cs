using Microsoft.Extensions.Configuration;

namespace OpenDMS.Domain.Infrastructure.Providers;

public class EncryptedOSFileSystem : OSFileSystem, IVirtualFileSystem
{
    private readonly IFileEncryptor encryptor;
    public string Id { get; } = "C";

    public EncryptedOSFileSystem(IConfiguration config, IFileEncryptor Encryptor) : base (config)
    {
        encryptor = Encryptor;
    }


    public override async Task<byte[]> ReadAllBytes(String fileName)
    {
        return await encryptor.Decrypt(await base.ReadAllBytes(fileName+ ".encrypted"));
    }

    public override async Task<bool> WriteAllBytes(String fileName, byte[] image)
    {
        return await base.WriteAllBytes(fileName+".encrypted", await encryptor.Encrypt(image));
    }

    public override async Task<bool> Delete(String fileName, Boolean LogicalDelete)
    {
        return await base.Delete(fileName + ".encrypted", LogicalDelete);
    }

    public override async Task<bool> Exists(string fileName)
    {
        return File.Exists(fileName + ".encrypted");
    }

    public override async Task<bool> Restore(string fileName)
    { 
        bool retVal = false;
        if (File.Exists(fileName + ".encrypted")) return false;
        if (File.Exists(fileName + ".encrypted" + ".Deleted"))
        {
            File.Move(fileName + ".encrypted" + ".Deleted", fileName + ".encrypted");
            retVal = true;
        }
        return retVal;        
    }
}
