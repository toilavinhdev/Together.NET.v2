using MediatR;
using Microsoft.AspNetCore.Authorization;
using Together.API.Extensions;
using Together.Application.Features.FeatureUser.Commands;
using Together.Application.Features.FeatureUser.Responses;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class AuthEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth").WithTags("Auth");
        
        group.MapPost("/sign-in", SignIn);
        
        group.MapPost("/external", External);
        
        group.MapPost("/sign-up", SignUp);
        
        group.MapPost("/logout", Logout);
        
        group.MapPost("/forgot-password", ForgotPassword);
        
        group.MapPost("/forgot-password/submit", SubmitForgotPasswordToken);
    }
    
    [AllowAnonymous]
    private static Task<BaseResponse<SignInResponse>> SignIn(ISender sender, SignInCommand command)
        => sender.Send(command);
    
    [AllowAnonymous]
    private static Task<BaseResponse<SignUpResponse>> SignUp(ISender sender, SignUpCommand command)
        => sender.Send(command);
    
    [AllowAnonymous]
    private static Task<BaseResponse<SignInResponse>> External(ISender sender, ExternalAuthCommand command)
        => sender.Send(command);
    
    [Authorize]
    private static Task<BaseResponse> Logout(ISender sender)
        => sender.Send(new LogoutCommand());
    
    [AllowAnonymous]
    private static Task<BaseResponse> ForgotPassword(ISender sender, ForgotPasswordCommand command)
        => sender.Send(command);
    
    [AllowAnonymous]
    private static Task<BaseResponse> SubmitForgotPasswordToken(ISender sender, SubmitForgotPasswordTokenCommand command)
        => sender.Send(command);
}