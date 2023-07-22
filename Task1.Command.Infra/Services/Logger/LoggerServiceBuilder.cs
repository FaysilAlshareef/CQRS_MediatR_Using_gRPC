using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace Task1.Command.Infra.Services.Logger;
public class LoggerServiceBuilder
{
    public static ILogger Build()
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

        var serilogConfiguration = configuration.GetSection("Serilog");
        var seqUrl = serilogConfiguration["SeqUrl"];
        var appName = serilogConfiguration["AppName"];

        var logger = new LoggerConfiguration()
                        .Enrich.WithProperty("name", appName)
                        .ReadFrom.Configuration(configuration);

        if (!string.IsNullOrWhiteSpace(seqUrl))
        {
            logger.WriteTo.Seq(
                serverUrl: seqUrl
            );
            SelfLog.Enable(Console.Error);
        }

        return logger.CreateLogger();
    }
}
