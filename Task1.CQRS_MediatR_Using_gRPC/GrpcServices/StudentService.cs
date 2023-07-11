using Grpc.Core;
using MediatR;
using Task1.CQRS_MediatR_Using_gRPC.Extensions;

using Task1.CQRS_MediatR_Using_gRPC.Protos;


namespace Task1.CQRS_MediatR_Using_gRPC.GrpcServices;

public class StudentService : Student.StudentBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IMediator mediator, ILogger<StudentService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<Responce> Create(CreateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand();
        var student = await _mediator.Send(command);

        return new Responce()
        {
            Message = "Success Added Student",
            Output = student.ToOutput()
        };
    }

    public override async Task<Responce> Update(UpdateStudentRequest request, ServerCallContext context)
    {
        var command = request.ToCommand();
        var student = await _mediator.Send(command);

        return new Responce()
        {
            Message = "",
            Output = student.ToOutput()

        };
    }
}
