using OpenDMS.Domain.Infrastructure;

namespace OpenDMS.Domain.Infrastructure.Encrypters;

public  class FastFileEncryptor : IFileEncryptor
{
    public async Task<byte[]> Encrypt(byte[] data)
    {
        for (var i = 0; i < data.Length; i++)
        {
            byte uno = (byte)(data[i] & 1);
            data[i] = (byte)((data[i] >> 1) | (uno * 128));
        }
        return data;
    }
    public async Task<byte[]> Decrypt(byte[] data)
    {
        for (var i = 0; i < data.Length; i++)
        {
            byte uno = (byte)((data[i] & 128) > 0 ? 1 : 0);
            data[i] = (byte)((data[i] << 1) | uno);
        }
        return data;
    }
}
