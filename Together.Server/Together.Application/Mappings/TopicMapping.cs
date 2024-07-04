using Together.Application.Features.FeatureTopic.Commands;
using Together.Application.Features.FeatureTopic.Responses;
using Together.Domain.Aggregates.TopicAggregate;

namespace Together.Application.Mappings;

public sealed class TopicMapping : Profile
{
    public TopicMapping()
    {
        CreateMap<CreateTopicCommand, Topic>();
        
        CreateMap<Topic, CreateTopicResponse>();
    }
}