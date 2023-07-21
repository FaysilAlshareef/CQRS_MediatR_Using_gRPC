using FluentValidation;
using Task1.CQRS_MediatR_Using_gRPC.Protos;
using Task1.CQRS_MediatR_Using_gRPC.Resources;

namespace Task1.CQRS_MediatR_Using_gRPC.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateStudentRequestValidator()
    {
        //RoleFor(c => c.Name)

        RuleFor(c => c.Name)
               .NotEmpty()
               .WithName("Name")
               .WithMessage(Phrases.StudentNameNotSend)
               ;
        RuleFor(c => c.Address)
               .NotEmpty()
               .WithName("Address")
               .WithMessage(Phrases.StudentAddressNotSend)
               ;

        RuleFor(c => c.PhoneNumber)
               .NotEmpty()
               .Length(9, 13)
               .WithName("PhoneNumber")
               .WithMessage(Phrases.PleaseEnterPhoneNumber);
    }
}
