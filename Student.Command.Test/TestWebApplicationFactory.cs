using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Xunit.Abstractions;

namespace Students.Command.Test;
public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly ITestOutputHelper _output;
    private readonly Action<IServiceCollection> _configure;
    private readonly string _dbName;

    public TestWebApplicationFactory(ITestOutputHelper output, Action<IServiceCollection> configure)
    {
        _dbName = Guid.NewGuid().ToString();
        _output = output;
        _configure = configure;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(loggingBuilder =>
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.TestOutput(_output, LogEventLevel.Information)
                        .CreateLogger();
        });

        builder.ConfigureServices(services =>
        {
            var descripter = services.Single(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(descripter);
            _configure(services);
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (TestBase.UseSqlDataBase)
                    options.UseSqlServer("Server =.; Database=StudentDb_Command.Test; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServerCertificate =True;");
                else
                    options.UseInMemoryDatabase(_dbName);


            });
        });

    }
}
