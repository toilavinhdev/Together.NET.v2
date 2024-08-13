using MediatR;
using Microsoft.AspNetCore.Authorization;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureUser.Commands;
using Together.Application.Features.FeatureUser.Queries;
using Together.Application.Features.FeatureUser.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class UserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/user").WithTags("User");
        
        group.MapGet("/me", Me);
        
        group.MapGet("/me/permissions", Permissions);
        
        group.MapGet("/{userId:guid}", GetUser);
        
        group.MapGet("/list", ListUser);
        
        group.MapPost("/me/upload-avatar", UploadAvatar).DisableAntiforgery();
        
        group.MapPut("/me/update-profile", UpdateProfile);
        
        group.MapPut("/me/update-password", UpdatePassword);
    }
    
    [Authorize]
    private static Task<BaseResponse<MeResponse>> Me(ISender sender)
        => sender.Send(new MeQuery());
    
    [Authorize]
    private static Task<BaseResponse<List<string>>> Permissions(ISender sender)
        => sender.Send(new PermissionsQuery());
    
    [TogetherPermission(TogetherPolicies.User.Get)]
    private static Task<BaseResponse<GetUserResponse>> GetUser(ISender sender, Guid userId)
        => sender.Send(new GetUserQuery(userId));
    
    [TogetherPermission(TogetherPolicies.User.List)]
    private static Task<BaseResponse<ListUserResponse>> ListUser(ISender sender, [AsParameters] ListUserQuery query)
        => sender.Send(query);
    
    [Authorize]
    private static Task<BaseResponse> UpdateProfile(ISender sender, UpdateProfileCommand command)
        => sender.Send(command);
    
    [Authorize]
    private static Task<BaseResponse> UpdatePassword(ISender sender, UpdatePasswordCommand command)
        => sender.Send(command);
    
    [Authorize]
    private static Task<BaseResponse<string>> UploadAvatar(ISender sender, IFormFile file)
        => sender.Send(new UploadAvatarCommand(file));
}