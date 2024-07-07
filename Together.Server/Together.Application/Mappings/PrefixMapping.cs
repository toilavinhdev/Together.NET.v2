using Together.Application.Features.FeaturePrefix.Commands;
using Together.Application.Features.FeaturePrefix.Responses;
using Together.Domain.Aggregates.PostAggregate;

namespace Together.Application.Mappings;

public sealed class PrefixMapping : Profile
{
    public PrefixMapping()
    {
        CreateMap<CreatePrefixCommand, Prefix>();
        
        CreateMap<Prefix, CreatePrefixResponse>();
        
        CreateMap<Prefix, PrefixViewModel>();
    }
}