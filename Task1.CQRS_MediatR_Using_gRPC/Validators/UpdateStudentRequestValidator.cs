using FluentValidation;
using Task1.CQRS_MediatR_Using_gRPC.Protos;

namespace Task1.CQRS_MediatR_Using_gRPC.Validators;

public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator()
    {
        RuleFor(u => u.StudentId)
            .NotEmpty()
            .WithName("StudentId")
            .Must(studentId => Guid.TryParse(studentId, out _));


        RuleFor(c => c.Name)
          .NotEmpty()
          .WithName("Name");


        RuleFor(c => c.PhoneNumber)
          .NotEmpty()
          .WithName("PhoneNumber")
                         .Length(9, 13);
        ;
    }
}
