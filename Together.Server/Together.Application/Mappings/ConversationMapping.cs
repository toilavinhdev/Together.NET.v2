using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Mappings;

public sealed class ConversationMapping : Profile
{
    public ConversationMapping()
    {
        CreateMap<Conversation, CreateConversationResponse>()
            .ForMember(response => response.ParticipantIds, cfg => cfg
                .MapFrom(c => c.ConversationParticipants.Select(cp => cp.UserId).ToList()));
    }
}