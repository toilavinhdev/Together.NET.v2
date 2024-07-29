using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureTopic.Commands;
using Together.Application.Features.FeatureTopic.Queries;
using Together.Application.Features.FeatureTopic.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class TopicEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/topic").WithTags("Topic");
        
        group.MapGet("/{topicId:guid}", GetTopic);
        
        group.MapPost("/create", CreateTopic);
        
        group.MapPut("/update", UpdateTopic);
        
        group.MapDelete("/{topicId:guid}", DeleteTopic);
    }
        
    [TogetherPermission(TogetherPolicies.Topic.View)]
    private static Task<BaseResponse<GetTopicResponse>> GetTopic(ISender sender, Guid topicId)
        => sender.Send(new GetTopicQuery(topicId));
    
    [TogetherPermission(TogetherPolicies.Topic.Create)]
    private static Task<BaseResponse<CreateTopicResponse>> CreateTopic(ISender sender, CreateTopicCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Topic.Update)]
    private static Task<BaseResponse> UpdateTopic(ISender sender, UpdateTopicCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Topic.Delete)]
    private static Task<BaseResponse> DeleteTopic(ISender sender, Guid topicId)
        => sender.Send(new DeleteTopicCommand(topicId));
}