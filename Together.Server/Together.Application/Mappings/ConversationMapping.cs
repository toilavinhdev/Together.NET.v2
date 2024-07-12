using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Mappings;

public sealed class ConversationMapping : Profile
{
    public ConversationMapping()
    {
        CreateMap<Conversation, CreateConversationResponse>();
    }
}