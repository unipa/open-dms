using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Infrastructure.Database.Builder;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;
using System.Text;

namespace Web.Controllers.Admin
{
    [Authorization(":admin")]
    [Route("Admin/sql/[action]")]
    public class AdminSQLController : Controller
    {
        private readonly ILogger<AdminSQLController> _logger;
        private readonly ISqlBuilder sqlBuilder;
        private readonly IApplicationDbContextFactory contextFactory;

        public AdminSQLController(ILogger<AdminSQLController> logger, ISqlBuilder sqlBuilder, IApplicationDbContextFactory contextFactory)
        {
            _logger = logger;
            this.sqlBuilder = sqlBuilder;
            this.contextFactory = contextFactory;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
//        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public class ParseRequest
        {
            public string sql { get; set; }
            public string rows { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Parse([FromBody] ParseRequest request)
        {
            try
            {
                StringBuilder sb = new();
                var DS = contextFactory.GetDbContext();

                IDbConnection connection = DS.Database.GetDbConnection();
                IDbCommand command = connection.CreateCommand();
                command.CommandTimeout = 60000;
                command.CommandType = CommandType.Text;
                connection.Open();
                command.CommandText = request.sql.Trim();
                bool isselect = request.sql.Trim().ToLower().StartsWith("select");
                if (isselect)
                {
                    if (!String.IsNullOrEmpty(request.rows)) command.CommandText +=  " LIMIT " + request.rows;
                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader != null)
                        {
                            sb.AppendLine("<table class='table zebra-cross'>");
                            sb.AppendLine("<thead>");
                            sb.AppendLine("<tr>");
                            for (var i = 0; i < reader.FieldCount; i++)
                                sb.AppendLine("<th>" + reader.GetName(i) + "</th>");
                            sb.AppendLine("</tr>");
                            sb.AppendLine("</thead>");
                            sb.AppendLine("<tbody>");
                        }

                        while (reader != null && reader.Read())
                        {
                            sb.AppendLine("<tr>");
                            for (var i = 0; i < reader.FieldCount; i++)
                                sb.AppendLine("<td>" + reader[i].ToString() + "</td>");
                            sb.AppendLine("</tr>");
                        }
                        if (reader != null)
                        {
                            sb.AppendLine("</tbody>");
                            sb.AppendLine("</table>");
                        }
                    }
                }
                else
                {
                    int r = command.ExecuteNonQuery();
                    sb.AppendLine("<table>");
                    sb.AppendLine("<thead>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<th>Risultato</th>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</thead>");
                    sb.AppendLine("<tbody>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td>" + r.ToString() + "</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</tbody>");
                    sb.AppendLine("</table>");
                }

                return Ok(sb.ToString());
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}