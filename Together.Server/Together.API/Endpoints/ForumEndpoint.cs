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
        
        group.MapGet("/list", ListTopic);

        group.MapPost("/create", CreateForum);
    }
    
    [TogetherPermission(TogetherPolicies.Forum.Create)]
    private static Task<BaseResponse<CreateForumResponse>> CreateForum(ISender sender, CreateForumCommand command)
        => sender.Send(command);
    
        
    [TogetherPermission(TogetherPolicies.Forum.View)]
    private static Task<BaseResponse<List<ForumViewModel>>> ListTopic(ISender sender)
        => sender.Send(new ListForumQuery());
}