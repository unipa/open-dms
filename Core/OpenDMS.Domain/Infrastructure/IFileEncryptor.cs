
namespace OpenDMS.Domain.Infrastructure;

public interface IFileEncryptor
{

    Task<byte[]> Encrypt(byte[] data);
    Task<byte[]> Decrypt(byte[] data);
}
