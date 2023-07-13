using System.Security.Cryptography;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

namespace Task1.CQRS_MediatR_Using_gRPC.Services;

public interface IServiceBusPublisher
{
    void PublishAsync();

}