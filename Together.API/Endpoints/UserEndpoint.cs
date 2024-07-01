using MediatR;
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
        
        group.MapGet("/{userId:guid}", GetUser);
        
        group.MapPut("/me/update-profile", UpdateProfile);
        
        group.MapPut("/me/update-password", UpdatePassword);
    }
    
    [TogetherPermission(TogetherPolicies.User.Me)]
    private static Task<BaseResponse<MeResponse>> Me(ISender sender)
        => sender.Send(new MeQuery());
    
    [TogetherPermission(TogetherPolicies.User.Get)]
    private static Task<BaseResponse<GetUserResponse>> GetUser(ISender sender, Guid userId)
        => sender.Send(new GetUserQuery(userId));
    
    [TogetherPermission(TogetherPolicies.User.UpdateProfile)]
    private static Task<BaseResponse> UpdateProfile(ISender sender, UpdateProfileCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.User.UpdatePassword)]
    private static Task<BaseResponse> UpdatePassword(ISender sender, UpdatePasswordCommand command)
        => sender.Send(command);
}