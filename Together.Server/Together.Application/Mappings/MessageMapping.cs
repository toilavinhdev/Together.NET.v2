using Together.Application.Features.FeatureMessage.Commands;
using Together.Application.Features.FeatureMessage.Responses;
using Together.Domain.Aggregates.MessageAggregate;

namespace Together.Application.Mappings;

public sealed class MessageMapping : Profile
{
    public MessageMapping()
    {
        CreateMap<SendMessageCommand, Message>();
        
        CreateMap<Message, SendMessageResponse>();
    }
}