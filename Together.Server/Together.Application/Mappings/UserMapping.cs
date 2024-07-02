using Together.Application.Features.FeatureUser.Responses;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Application.Mappings;

public sealed class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, IdentityPrivilege>()
            .ForMember(dest => dest.RoleClaims, cfg =>
                cfg.MapFrom(src => src.UserRoles!.SelectMany(ur => ur.Role.Claims).ToList()));
        
        CreateMap<User, MeResponse>();

        CreateMap<User, SignUpResponse>();
    }
}