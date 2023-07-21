using FluentValidation;
using Task1.CQRS_MediatR_Using_gRPC.Protos;
using Task1.CQRS_MediatR_Using_gRPC.Resources;

namespace Task1.CQRS_MediatR_Using_gRPC.Validators;

public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator()
    {
        RuleFor(u => u.StudentId)
            .NotEmpty()
            .WithName("StudentId")
            .WithMessage(Phrases.StudentIdNotSend)
            .Must(studentId => Guid.TryParse(studentId, out _));


        RuleFor(c => c.Name)
          .NotEmpty()
          .WithName("Name")
          .WithMessage(Phrases.StudentNameNotSend);


        RuleFor(c => c.PhoneNumber)
          .NotEmpty()
          .WithName("PhoneNumber")
          .WithMessage(Phrases.StudentAddressNotSend)
          .Length(9, 13);
        ;
    }
}
