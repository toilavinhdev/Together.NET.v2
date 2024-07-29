using Together.Application.Features.FeatureRole.Commands;
using Together.Application.Features.FeatureRole.Responses;
using Together.Domain.Aggregates.RoleAggregate;

namespace Together.Application.Mappings;

public sealed class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<CreateRoleCommand, Role>()
            .ForMember(role => role.IsDefault, cfg => cfg
                .MapFrom(_ => false));
        
        CreateMap<Role, CreateRoleResponse>();
        
        CreateMap<Role, RoleViewModel>();
        
        CreateMap<Role, GetRoleResponse>();
    }
}