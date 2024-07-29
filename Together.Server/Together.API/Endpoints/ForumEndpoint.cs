using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureForum.Commands;
using Together.Application.Features.FeatureForum.Queries;
using Together.Application.Features.FeatureForum.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class ForumEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/forum").WithTags("Forum");
        
        group.MapGet("/{forumId:guid}", GetForum);
        
        group.MapGet("/list", ListForum);

        group.MapPost("/create", CreateForum);
            
        group.MapPut("/update", UpdateForum);
        
        group.MapDelete("/{forumId:guid}", DeleteForum);
    }
    
    [TogetherPermission(TogetherPolicies.Forum.View)]
    private static Task<BaseResponse<GetForumResponse>> GetForum(ISender sender, Guid forumId)
        => sender.Send(new GetForumQuery(forumId));
        
    [TogetherPermission(TogetherPolicies.Forum.View)]
    private static Task<BaseResponse<List<ForumViewModel>>> ListForum(ISender sender, [AsParameters] ListForumQuery query)
        => sender.Send(query);
    
    [TogetherPermission(TogetherPolicies.Forum.Create)]
    private static Task<BaseResponse<CreateForumResponse>> CreateForum(ISender sender, CreateForumCommand command)
        => sender.Send(command);

    [TogetherPermission(TogetherPolicies.Forum.Update)]
    private static Task<BaseResponse> UpdateForum(ISender sender, UpdateForumCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Forum.Delete)]
    private static Task<BaseResponse> DeleteForum(ISender sender, Guid forumId)
        => sender.Send(new DeleteForumCommand(forumId));
}