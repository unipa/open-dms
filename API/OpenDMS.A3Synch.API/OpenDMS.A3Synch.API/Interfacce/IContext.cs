using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IContext 
    {
        DbSet<Contacts> Contacts { get; set; }
        DbSet<Users> Users { get; set; }
        DbSet<UserGroups> UserGroups { get; set; }
        DbSet<UserGroupRoles> UserGroupRoles { get; set; }
        DbSet<Roles> Roles { get; set; }
        DbSet<OrganizationNodes> OrganizationNodes { get; set; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}
