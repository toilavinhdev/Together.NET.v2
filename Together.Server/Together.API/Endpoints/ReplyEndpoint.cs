using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureReply.Commands;
using Together.Application.Features.FeatureReply.Queries;
using Together.Application.Features.FeatureReply.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class ReplyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/reply").WithTags("Reply");

        group.MapGet("/list", ListReply);
        
        group.MapPost("/create", CreateReply);
        
        group.MapPost("/vote", VoteReply);
        
        group.MapPut("/update", UpdateReply);
        
        group.MapDelete("/{replyId:guid}", DeleteReply);
    }
    
    [TogetherPermission(TogetherPolicies.Reply.View)]
    private static Task<BaseResponse<ListReplyResponse>> ListReply(ISender sender, [AsParameters] ListReplyQuery query)
        => sender.Send(query);
    
    [TogetherPermission(TogetherPolicies.Reply.Create)]
    private static Task<BaseResponse<CreateReplyResponse>> CreateReply(ISender sender, CreateReplyCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Reply.Vote)]
    private static Task<BaseResponse<VoteReplyResponse>> VoteReply(ISender sender, VoteReplyCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Reply.Update)]
    private static Task<BaseResponse> UpdateReply(ISender sender, UpdateReplyCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Reply.Delete)]
    private static Task<BaseResponse> DeleteReply(ISender sender, Guid replyId)
        => sender.Send(new DeleteReplyCommand(replyId));
}