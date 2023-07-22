using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Application.Contracts.Services.BaseService;
using Task1.Command.Application.Contracts.Services.ServiceBus;
using Task1.Command.Domain.Entities;
using Task1.Command.Domain.Models;

namespace Task1.Command.Infra.Services.BaseService;
public class CommitEventService : ICommitEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBusPublisher _serviceBusPublisher;

    public CommitEventService(IUnitOfWork unitOfWork, IServiceBusPublisher serviceBusPublisher)
    {
        _unitOfWork = unitOfWork;
        _serviceBusPublisher = serviceBusPublisher;
    }

    public async Task CommitNewEventsAsync(Student student)
    {
        var newEvents = student.GetUncommittedEvents();

        await _unitOfWork.Events.AddRangeAsync(newEvents);

        var messages = OutboxMessage.ToManyMessages(newEvents);

        await _unitOfWork.OutboxMessages.AddRangeAsync(messages);

        await _unitOfWork.CompleteAsync();

        student.MarkChangesAsCommitted();

        _serviceBusPublisher.PublishAsync();
    }
}