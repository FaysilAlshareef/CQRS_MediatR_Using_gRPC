
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Repositories;
using Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

namespace Task1.CQRS_MediatR_Using_gRPC.Services
{
    public class ServiceBusPublisher
    {
        private readonly IServiceProvider _provider;
        private readonly ServiceBusSender _sender;
        private readonly ILogger<ServiceBusPublisher> _logger;
        private static readonly object _lockObject = new();

        public ServiceBusPublisher(
            IServiceProvider provider,
            ServiceBusClient busClient,
            IConfiguration configuration,
            ILogger<ServiceBusPublisher> logger
        )
        {
            _provider = provider;
            _sender = busClient.CreateSender(configuration["ServiceBus:Topic"]);
            _logger = logger;

        }


        public void PublishAsync()
        {
            //using var scope = _provider.CreateScope();

            //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //context.AttachRange(messages);

            //await PublishAndRemoveMessages(messages, context);

            // Don't wait
            Task.Run(PublishNonPublishedMessages);
        }

        private void PublishNonPublishedMessages()
        {
            lock (_lockObject)
            {
                using var scope = _provider.CreateScope();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var messages = unitOfWork.OutboxMessages.GetAllAsync().GetAwaiter().GetResult();

                PublishAndRemoveMessages(messages, unitOfWork).GetAwaiter().GetResult();
            }
        }


        private async Task PublishAndRemoveMessages(IEnumerable<OutboxMessage> messages, IUnitOfWork unitOfWork)
        {
            foreach (var message in messages)
            {
                await SendMessageAsync(message.Event);

                unitOfWork.OutboxMessages.Remove(message);

                await unitOfWork.CompleteAsync();
            }
        }

        private async Task SendMessageAsync(Event @event)
        {
            var body = new MessageBody()
            {
                AggregateId = @event.AggregateId,
                DateTime = @event.DateTime,
                Sequence = @event.Sequence,
                Type = @event.Type.ToString(),
                UserId = @event.UserId,
                Version = @event.Version,
                Data = ((dynamic)@event).Data
            };

            var json = JsonConvert.SerializeObject(body);

            await _sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes(json))
            {
                CorrelationId = @event.Id.ToString(),
                MessageId = @event.Id.ToString(),
                PartitionKey = @event.AggregateId.ToString(),
                Subject = @event.Type.ToString(),
                ApplicationProperties = {
                    { nameof(@event.AggregateId), @event.AggregateId },
                    { nameof(@event.Sequence), @event.Sequence },
                    { nameof(@event.Version), @event.Version },
                }
            });
        }
    }
}
