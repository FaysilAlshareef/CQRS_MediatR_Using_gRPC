﻿using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Extensions;

namespace Task1.CQRS_MediatR_Using_gRPC.Models;

public class Student : Aggregate<Student>
{
    private Student() { }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }


    #region Create

    public static Student Create(StudentAddCommand command)
    {
        var @event = command.ToEvent();
        var student = new Student();
        student.ApplyChange(@event);

        return student;
    }

    public void Apply(StudentAddedEvent @event)
    {
        Name = @event.Data.Name;
        Address = @event.Data.Address;
        PhoneNumber = @event.Data.Phone_Number;
    }

    #endregion


    #region Update

    public void Update(StudentUpdateCommand command)
    {
        if (Name == command.Name && PhoneNumber == command.Phone_Number)
            return;

        var @event = command.ToEvent(Sequence + 1);
        ApplyChange(@event);
    }

    public void Apply(StudentUpdatedEvent @event)
    {
        Name = @event.Data.Name;
        PhoneNumber = @event.Data.Phone_Number;
    }
    #endregion
}
