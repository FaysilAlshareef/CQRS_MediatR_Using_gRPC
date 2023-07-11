using Calzolari.Grpc.AspNetCore.Validation;
using Task1.CQRS_MediatR_Using_gRPC.Validators;

namespace Task1.CQRS_MediatR_Using_gRPC.Services;

public static class GrpcRegisterExtension
{

    public static IServiceCollection AddStudentValidators(this IServiceCollection services)
    {
        services.AddValidator<CreateStudentRequestValidator>();
        services.AddValidator<UpdateStudentRequestValidator>();

        return services;
    }
}
