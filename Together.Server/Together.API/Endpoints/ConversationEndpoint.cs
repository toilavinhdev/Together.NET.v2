using MediatR;
using Together.API.Extensions;
using Together.Application.Features.FeatureConversation.Commands;
using Together.Application.Features.FeatureConversation.Queries;
using Together.Application.Features.FeatureConversation.Responses;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class ConversationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/conversation").WithTags("Conversation");

        group.MapGet("/query", ListConversation);
        
        group.MapPost("/create", CreateConversation);
        
    }
    
    private static Task<BaseResponse<ConversationResponse>> ListConversation(ISender sender,
        [AsParameters] ConversationQuery query) => sender.Send(query);

    private static Task<BaseResponse<CreateConversationResponse>> CreateConversation(ISender sender,
        CreateConversationCommand command) => sender.Send(command);

    
}