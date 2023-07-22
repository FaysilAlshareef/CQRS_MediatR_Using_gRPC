using Calzolari.Grpc.AspNetCore.Validation;

namespace Task1.CQRS_MediatR_Using_gRPC.Validators.Main;

public static class GrpcRegisterExtension
{

    public static IServiceCollection AddStudentValidators(this IServiceCollection services)
    {
        services.AddValidator<CreateStudentRequestValidator>();
        services.AddValidator<UpdateStudentRequestValidator>();

        return services;
    }
}
