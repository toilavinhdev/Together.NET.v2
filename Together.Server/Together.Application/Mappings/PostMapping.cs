using Together.Application.Features.FeaturePost.Responses;
using Together.Domain.Aggregates.PostAggregate;

namespace Together.Application.Mappings;

public sealed class PostMapping : Profile
{
    public PostMapping()
    {
        CreateMap<Post, CreatePostResponse>();
    }
}