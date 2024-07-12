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

        group.MapGet("/list", ListConversation);
        
        group.MapPost("/create", CreateConversation);
        
    }
    
    private static Task<BaseResponse<ListConversationResponse>> ListConversation(ISender sender,
        [AsParameters] ListConversationQuery query) => sender.Send(query);

    private static Task<BaseResponse<CreateConversationResponse>> CreateConversation(ISender sender,
        CreateConversationCommand command) => sender.Send(command);

    
}