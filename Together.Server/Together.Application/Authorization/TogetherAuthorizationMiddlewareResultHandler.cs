using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Together.Application.Authorization;

public sealed class TogetherAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
    
    public async Task HandleAsync(RequestDelegate next, 
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        var localizationService = LocalizationServiceFactory.GetInstance();
        
        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(
                new BaseResponse
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Errors = [
                        new BaseError(
                            TogetherErrorCodes.Server.Forbidden, 
                            localizationService.Get(TogetherErrorCodes.Server.Forbidden))
                    ]
                });
            return;
        }

        // Fall back to the default implementation.
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}