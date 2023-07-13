using FluentValidation;
using Task1.CQRS_MediatR_Using_gRPC.Protos;

namespace Task1.CQRS_MediatR_Using_gRPC.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateStudentRequestValidator()
    {
        //RoleFor(c => c.Name)

        RuleFor(c => c.Name)
               .NotEmpty()
               .WithName("Name")
               ;
        RuleFor(c => c.Address)
               .NotEmpty()
               .WithName("Address");

        RuleFor(c => c.PhoneNumber)
               .NotEmpty()
               .Length(9, 13)
               .WithName("PhoneNumber")
               ;
    }
}
