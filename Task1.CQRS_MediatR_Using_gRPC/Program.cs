using Azure.Messaging.ServiceBus;
using Calzolari.Grpc.AspNetCore.Validation;
using Serilog;
using Task1.Command.Application;
using Task1.Command.Infra;
using Task1.Command.Infra.Services.Logger;
using Task1.CQRS_MediatR_Using_gRPC.ExceptionHandler;
using Task1.CQRS_MediatR_Using_gRPC.GrpcServices;
using Task1.CQRS_MediatR_Using_gRPC.Interceptors;
using Task1.CQRS_MediatR_Using_gRPC.Validators.Main;

namespace Task1.CQRS_MediatR_Using_gRPC;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = LoggerServiceBuilder.Build();

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddApplicationServices();

        builder.Services.AddInfraServices(builder.Configuration);
        builder.Services.AddGrpcValidation();
        builder.Services.AddStudentValidators();
        builder.Services.AddGrpc(option =>
        {
            option.Interceptors.Add<ThreadCultureInterceptor>();

            option.EnableMessageValidation();

            option.Interceptors.Add<ExceptionHandlingInterceptor>();
        });

        builder.Services.AddSingleton(s =>
        {
            return new ServiceBusClient(builder.Configuration["ServiceBus:ConnectionString"]);
        });

        builder.Host.UseSerilog();
        var app = builder.Build();


        // Configure the HTTP request pipeline.

        app.MapGrpcService<StudentService>();
        app.MapGrpcService<DemoEventService>();
        app.MapGrpcService<EventsHistoryService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}