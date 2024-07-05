using Together.Application.Features.FeatureForum.Commands;
using Together.Application.Features.FeatureForum.Responses;
using Together.Domain.Aggregates.ForumAggregate;

namespace Together.Application.Mappings;

public sealed class ForumMapping : Profile
{
    public ForumMapping()
    {
        CreateMap<CreateForumCommand, Forum>();
        
        CreateMap<Forum, CreateForumResponse>();

        CreateMap<Forum, ForumViewModel>();
    }
}