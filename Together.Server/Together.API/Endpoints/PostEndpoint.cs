using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeaturePost.Commands;
using Together.Application.Features.FeaturePost.Queries;
using Together.Application.Features.FeaturePost.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class PostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/post").WithTags("Post");
        
        group.MapGet("/list", ListPost);
        
        group.MapGet("/{postId:guid}", GetPost);
        
        group.MapPost("/create", CreatePost);
        
        group.MapPost("/vote", VotePost);
    }
    
    [TogetherPermission(TogetherPolicies.Post.View)]
    private static Task<BaseResponse<ListPostResponse>> ListPost(ISender sender, [AsParameters] ListPostQuery query)
        => sender.Send(query);
    
    [TogetherPermission(TogetherPolicies.Post.View)]
    private static Task<BaseResponse<GetPostResponse>> GetPost(ISender sender, Guid postId)
        => sender.Send(new GetPostQuery(postId));
    
    [TogetherPermission(TogetherPolicies.Post.Create)]
    private static Task<BaseResponse<CreatePostResponse>> CreatePost(ISender sender, CreatePostCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Post.Vote)]
    private static Task<BaseResponse<VotePostResponse>> VotePost(ISender sender, VotePostCommand command)
        => sender.Send(command);
}