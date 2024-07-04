using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeaturePost.Commands;
using Together.Application.Features.FeaturePost.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class PostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/post").WithTags("Post");
        
        group.MapPost("/create", CreatePost);
    }
    
    [TogetherPermission(TogetherPolicies.Post.Create)]
    private static Task<BaseResponse<CreatePostResponse>> CreatePost(ISender sender, CreatePostCommand command)
        => sender.Send(command);
}