namespace Task1.Command.Domain.Commands;

public interface IStudentUpdateCommand
{
    Guid StudentId { get; }
    string Name { get; }
    string Phone_Number { get; }
}



