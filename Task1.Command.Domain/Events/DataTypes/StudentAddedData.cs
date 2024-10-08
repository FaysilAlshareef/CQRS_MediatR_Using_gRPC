﻿using Task1.Command.Domain.Enums;

namespace Task1.Command.Domain.Events.DataTypes;

public record StudentAddedData : IEventData
{
    public StudentAddedData(string name, string address, string phone_Number)
    {
        Name = name;
        Address = address;
        Phone_Number = phone_Number;
    }

    private StudentAddedData()
    {

    }

    public string Name { get; private set; }
    public string Address { get; private set; }
    public string Phone_Number { get; private set; }
    public EventType Type => EventType.StudentAdded;
}
