using Together.Application.Features.FeatureReply.Commands;
using Together.Application.Features.FeatureReply.Responses;
using Together.Domain.Aggregates.ReplyAggregate;

namespace Together.Application.Mappings;

public sealed class ReplyMapping : Profile
{
    public ReplyMapping()
    {
        CreateMap<CreateReplyCommand, Reply>();
        
        CreateMap<Reply, CreateReplyResponse>();
    }
}