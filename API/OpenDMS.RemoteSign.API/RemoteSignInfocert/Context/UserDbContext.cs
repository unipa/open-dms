using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.Context
{
    public class UserDbContext : DbContext
    {

        private readonly IConfiguration configuration;
        public UserDbContext(IConfiguration config)
        {
            configuration = config;
        }

        public DbSet<UserModel> Utenti { get; set; }
        public DbSet<SignRoomModel> SignRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserModel>().ToTable("Utenti");  
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var provider = (configuration["Provider"] ?? "mysql").ToLower();
            if (provider=="mysql")
                optionsBuilder.UseMySql(configuration.GetConnectionString(provider), ServerVersion.AutoDetect(configuration.GetConnectionString(provider)));
            if (provider == "sqlite")
                optionsBuilder.UseSqlite(configuration.GetConnectionString(provider));
        }

    }
}
