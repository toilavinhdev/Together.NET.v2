using MediatR;
using Microsoft.AspNetCore.Authorization;
using Together.API.Extensions;
using Together.Application.Features.FeatureMessage.Commands;
using Together.Application.Features.FeatureMessage.Queries;
using Together.Application.Features.FeatureMessage.Responses;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class MessageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/message").WithTags("Message");
        
        group.MapGet("/list", ListMessage);
        
        group.MapPost("/send", SendMessage);
    }
    
    [Authorize]
    private static Task<BaseResponse<ListMessageResponse>> ListMessage(ISender sender, [AsParameters] ListMessageQuery query)
        => sender.Send(query);
    
    [Authorize]
    private static Task<BaseResponse<SendMessageResponse>> SendMessage(ISender sender, SendMessageCommand command)
        => sender.Send(command);
}