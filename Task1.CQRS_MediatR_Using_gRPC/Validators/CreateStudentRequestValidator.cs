using FluentValidation;
using Task1.CQRS_MediatR_Using_gRPC.Protos;

namespace Task1.CQRS_MediatR_Using_gRPC.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateStudentRequestValidator()
    {
        //RoleFor(c => c.Name)

        RuleFor(c => c.Name)
               .NotEmpty();
        RuleFor(c => c.Address)
               .NotEmpty();

        RuleFor(c => c.PhoneNumber)
               .NotEmpty()
               .MaximumLength(10);
    }
}
