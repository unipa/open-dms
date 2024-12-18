using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Encrypters;
using OpenDMS.Domain.Infrastructure.Providers;

namespace OpenDMS.Startup;

public static class StorageExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {


        services.AddTransient<IFileEncryptor, FastFileEncryptor>();
        services.AddTransient<IVirtualFileSystem, OSFileSystem>();
        services.AddTransient<IVirtualFileSystem, EncryptedOSFileSystem>();
        services.AddTransient<IVirtualFileSystemProvider, VirtualFileSystemProvider>();

        
        return services;
    }




}

