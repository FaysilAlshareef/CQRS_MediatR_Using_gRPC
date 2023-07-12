using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Students.Command.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Services;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = TestBase.UseSqlDatabase)]
namespace Students.Command.Test;
public abstract class TestBase
{
    public const bool UseSqlDatabase = true;
    private GrpcChannel _channel;
    private TestWebApplicationFactory<Program> _factory;
    public ITestOutputHelper Outout { get; }

    public TestBase(ITestOutputHelper output)
    {
        Outout = output;
    }

    public GrpcChannel Channel
    {
        get
        {
            if (_channel == null)
                return _channel;

            Initialize();
            return _channel ?? throw new Exception("return _channel");
        }
        private set => _channel = value;
    }
    public TestWebApplicationFactory<Program> Factory
    {
        get
        {
            if (_factory == null)
                return _factory;

            Initialize();
            return _factory ?? throw new Exception("return _channel");
        }
        private set => _factory = value;
    }

    public void Initialize(
        Action<IServiceCollection> configureTopic = null,
        Action<IServiceCollection> configureOther = null
        )
    {
        if (configureTopic == null)
            configureTopic = services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ServiceBusPublisher));
                services.Remove(descriptor);

                var mock = new Mock<ServiceBusPublisher>();
                mock.Setup(t => t.PublishAsync());
                services.AddSingleton(mock.Object);

            };
        void Configure(IServiceCollection services)
        {
            configureTopic?.Invoke(services);
            configureOther?.Invoke(services);
        }

        var factory = new TestWebApplicationFactory<Program>(Outout, Configure);
        var client = factory.CreateClient();

        Channel = GrpcChannel.ForAddress(client.BaseAddress ?? throw new Exception(), new GrpcChannelOptions()
        {
            HttpClient = client,
        });

        Factory = factory;


        ResetDb();
    }

    private void ResetDb()
    {
        if (UseSqlDatabase is false)
            return;

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        context.EventStore.RemoveRange(context.EventStore);
        context.UniqueReferences.RemoveRange(context.UniqueReferences);
        context.OutboxMessages.RemoveRange(context.OutboxMessages);
        context.SaveChanges();
    }
}
