namespace Task1.CQRS_MediatR_Using_gRPC.Data.Entities;

public class UniqueReference
{
    private UniqueReference() { }

    public UniqueReference(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
}
