using Serilog.Debugging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Task1.CQRS_MediatR_Using_gRPC.Services.Logger;

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
