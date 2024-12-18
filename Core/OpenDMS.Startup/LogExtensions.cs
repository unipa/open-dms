using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace OpenDMS.Startup
{
    public static class LogExtensions
    {
        public static ILoggingBuilder AddLogging(this ILoggingBuilder builder, string LogFileName, string SeqUrl = "")
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Environment.CurrentDirectory,  "Log" , $"OpenDMS.{LogFileName}.log"), rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"
                );
            Console.WriteLine("File di Log: " + LogFileName);
            if (!String.IsNullOrEmpty(SeqUrl))
            {
                Console.WriteLine("Seq URL: " + SeqUrl);
                logger = logger.WriteTo.Seq(SeqUrl);
            }
            return builder.AddSerilog(logger.CreateLogger());
        }

        public static ILoggingBuilder AddLogging2(this ILoggingBuilder builder, string LogFileName, IConfiguration config, string SeqUrl = "")
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration (config)
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Environment.CurrentDirectory, "Log", $"OpenDMS.{LogFileName}.log"), rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"
                );
            Console.WriteLine("File di Log: " + LogFileName);
            if (!String.IsNullOrEmpty(SeqUrl))
            {
                Console.WriteLine("Seq URL: " + SeqUrl);
                logger = logger.WriteTo.Seq(SeqUrl);
            }
            return builder.AddSerilog(logger.CreateLogger());
        }
    }
}