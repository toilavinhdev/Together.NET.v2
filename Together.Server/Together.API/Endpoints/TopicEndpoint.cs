using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureTopic.Commands;
using Together.Application.Features.FeatureTopic.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class TopicEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/topic").WithTags("Topic");
        
        group.MapPost("/create", CreateTopic);
    }
    
    [TogetherPermission(TogetherPolicies.Topic.Create)]
    private static Task<BaseResponse<CreateTopicResponse>> CreateTopic(ISender sender, CreateTopicCommand command)
        => sender.Send(command);
}