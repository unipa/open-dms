using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenDMS.Preservation.Core.Interfaces.Data;
using OpenDMS.Preservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Implementations.Data.DbContexts
{
    public class SQLServerCSDbContext : DbContext, ICSContext
    {
        private readonly IConfiguration _config;
        public DbSet<Documenti> documenti { get; set; }
        public DbSet<IDC> idc { get; set; }
        public DbSet<PDD> pdd { get; set; }
        public DbSet<Metadata> metadata { get; set; }

        public SQLServerCSDbContext(IConfiguration configuration, DbContextOptions<SQLServerCSDbContext> options) : base(options)
        {
            _config = configuration;
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn_string = _config.GetConnectionString("SQLServer");
            optionsBuilder
                .EnableSensitiveDataLogging()
               .UseSqlServer(conn_string);
        }
    }
}
