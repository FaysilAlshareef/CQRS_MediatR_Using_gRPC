
using Task1.Command.Domain.Models;

namespace Task1.Command.Domain.Entities;

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


    public void Update(string name)
    {
        Name = name;
    }
}
