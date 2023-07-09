namespace Task1.CQRS_MediatR_Using_gRPC.Extensions;
using Task1.CQRS_MediatR_Using_gRPC.Protos;
using Student = Task1.CQRS_MediatR_Using_gRPC.Models.Student;

public static class ModelExtensions
{
    public static StudentOutput ToOutput(this Student student)
        => new()
        {
            Id = student.Id.ToString(),
            Name = student.Name,
            Address = student.Address,
            PhoneNumber = student.PhoneNumber
        };
}
