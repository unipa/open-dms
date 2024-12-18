using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;

namespace A3Synch.Context
{
    public class SqlServerContext : DbContext, IContext
    {
        private readonly IConfiguration _config;

        public SqlServerContext(DbContextOptions<SqlServerContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<UserGroupRoles> UserGroupRoles { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<OrganizationNodes> OrganizationNodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = (string)_config.GetValue(typeof(string), "ConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
