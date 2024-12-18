using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("internalapi/database")]
    public class DatabaseController : ControllerBase
    {
        private readonly IApplicationDbContextFactory dbContextFactory;

        public DatabaseController(IApplicationDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public async Task<string> Migrate()
        {
            try
            {
                using var dbContext = dbContextFactory.GetDbContext();
                dbContext.Database.Migrate();
                return "OK";
            } catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}