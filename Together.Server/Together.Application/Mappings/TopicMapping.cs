using Together.Application.Features.FeatureForum.Responses;
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
        
        CreateMap<Topic, GetTopicResponse>();

        CreateMap<Topic, TopicViewModel>()
            .ForMember(viewModel => viewModel.PostCount, cfg => cfg
                .MapFrom(topic => topic.Posts!.LongCount()))
            .ForMember(viewModel => viewModel.ReplyCount, cfg => cfg
                .MapFrom(topic => topic.Posts!.SelectMany(p => p.Replies!).LongCount()));
    }
}