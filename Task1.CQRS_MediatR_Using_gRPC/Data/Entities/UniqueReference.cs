using Task1.CQRS_MediatR_Using_gRPC.Models;

namespace Task1.CQRS_MediatR_Using_gRPC.Data.Entities;

public class UniqueReference
{
    private UniqueReference() { }

    public UniqueReference(Student student)
    {
        Id = student.Id;
        Name = student.Name;
    }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
}
