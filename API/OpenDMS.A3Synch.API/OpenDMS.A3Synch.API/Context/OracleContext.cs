using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;

namespace A3Synch.Context
{
    public class OracleContext : DbContext, IContext
    {
        private readonly IConfiguration _config;

        public OracleContext(DbContextOptions<OracleContext> options, IConfiguration config)
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
            optionsBuilder.UseOracle(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Modifica il nome della tabella in maiuscolo
                entity.SetTableName(entity.GetTableName().ToUpper());

                // Modifica il nome delle colonne in maiuscolo
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToUpper());
                }
            }
        }
    }
}
