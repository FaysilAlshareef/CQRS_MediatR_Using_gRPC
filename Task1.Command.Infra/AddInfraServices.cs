using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Application.Contracts.Services.BaseService;
using Task1.Command.Application.Contracts.Services.ServiceBus;
using Task1.Command.Infra.Persistence;
using Task1.Command.Infra.Persistence.Repositories;
using Task1.Command.Infra.Services.BaseService;
using Task1.Command.Infra.Services.ServiceBus;

namespace Task1.Command.Infra;
public static class InfraContainer
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();

        services.AddSingleton(s =>
        {
            return new ServiceBusClient(configuration["ServiceBus:ConnectionString"]);
        });

        services.AddSingleton<IServiceBusEventSender, ServiceBusEventSender>();



        services.AddScoped<ICommitEventService, CommitEventService>();



        return services;
    }

}