using Microsoft.EntityFrameworkCore;

namespace Elmi.Core.DataAccess.Interfaces
{
    public interface IDatabaseProviderConfigurator
    {
        void Configure(DbContextOptionsBuilder optionsBuilder);
    }
}
