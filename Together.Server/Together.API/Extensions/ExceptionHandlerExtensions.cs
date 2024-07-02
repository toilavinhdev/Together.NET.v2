using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Together.Shared.Constants;
using Together.Shared.Exceptions;
using Together.Shared.Localization;
using Together.Shared.ValueObjects;

namespace Together.API.Extensions;

public static class ExceptionHandlerExtensions
{
    public static void UseDefaultExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errApp =>
        {
            errApp.Run(async ctx =>
            {
                var feature = ctx.Features.Get<IExceptionHandlerFeature>();

                if (feature is not null)
                {
                    var exception = feature.Error;
                    await WriteResponseAsync(ctx, exception);
                }
            });
        });
    }
    
    private static async Task WriteResponseAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)GetStatusCode(exception);
        await context.Response.WriteAsJsonAsync(
            new BaseResponse()
            {
                StatusCode = GetStatusCode(exception),
                Errors = GetResponseErrors(exception)
            });
    }

    private static HttpStatusCode GetStatusCode(Exception ex) => ex switch
    {
        UnauthorizedAccessException => HttpStatusCode.Unauthorized,
        DomainException dEx => dEx.StatusCode,
        ValidationException => HttpStatusCode.BadRequest,
        _ => HttpStatusCode.InternalServerError
    };
    
    private static List<BaseError> GetResponseErrors(Exception ex)
    {
        switch (ex)
        {
            case UnauthorizedAccessException:
                return [
                    new BaseError(
                        TogetherErrorCodes.Server.Unauthorized, 
                        TogetherErrorCodes.Server.Unauthorized)
                ];
            case ValidationException vEx:
                return vEx.Errors
                    .Select(failure =>
                        new BaseError(
                            failure.ErrorCode, 
                            failure.ErrorMessage, 
                            failure.PropertyName))
                    .ToList();
            default:
            {
                var errorCount = 5;
                var errors = new List<BaseError> { ex.ToResponseError() };

                var inner = ex.InnerException;
        
                while (inner is not null && errorCount-- > 0)
                {
                    errors.Add(inner.ToResponseError());
                    inner = inner.InnerException;
                }

                return errors.ToList();
            }
        }
    }

    private static BaseError ToResponseError(this Exception ex)
    {
        var localizationService = LocalizationServiceFactory.GetInstance();
        
        return ex is DomainException dEx 
            ? new BaseError(dEx.Code, localizationService.Get(dEx.Code), dEx.Parameter)
            : new BaseError(TogetherErrorCodes.Server.InternalServer, ex.Message);
    }
}