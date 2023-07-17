namespace Task1.CQRS_MediatR_Using_gRPC.Exceptions;

public class AppException : Exception
{
    public ExceptionStatusCode StatusCode { get; set; }

    public AppException(ExceptionStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}