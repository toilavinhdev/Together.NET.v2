using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Together.Shared.ValueObjects;

namespace Together.Shared.Mediator;

public abstract class BaseRequestHandler(IHttpContextAccessor httpContextAccessor)
{
    protected readonly CurrentUserClaims CurrentUserClaims = CurrentUserClaims.GetValue(httpContextAccessor);
    
    protected HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    
    protected string? Message { get; set; }
}

public abstract class BaseRequestHandler<TRequest>(IHttpContextAccessor httpContextAccessor) 
    : BaseRequestHandler(httpContextAccessor), IRequestHandler<TRequest, BaseResponse> 
    where TRequest : IBaseRequest
{
    protected abstract Task HandleAsync(TRequest request, CancellationToken ct);
    
    public async Task<BaseResponse> Handle(TRequest request, CancellationToken ct)
    {
        await HandleAsync(request, ct);

        return new BaseResponse
        {
            StatusCode = StatusCode,
            Message = Message
        };
    }
}

public abstract class BaseRequestHandler<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) 
    : BaseRequestHandler(httpContextAccessor), IRequestHandler<TRequest, BaseResponse<TResponse>> 
    where TRequest : IBaseRequest<TResponse>
{
    protected abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken ct);
        
    public async Task<BaseResponse<TResponse>> Handle(TRequest request, CancellationToken ct)
    {
        var response = await HandleAsync(request, ct);
        
        return new BaseResponse<TResponse>
        {
            StatusCode = StatusCode,
            Message = Message,
            Data = response
        };
    }
}