using System.Net;

namespace Together.Shared.Exceptions;

public class DomainException : Exception
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
    
    public string Code { get; }
    
    public string? Parameter { get; }

    public DomainException(string code) : base(code)
    {
        Code = code;
    }

    public DomainException(string code, 
        string? parameter = null, 
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(code)
    {
        Code = code;
        Parameter = parameter;
        StatusCode = statusCode;
    }
}