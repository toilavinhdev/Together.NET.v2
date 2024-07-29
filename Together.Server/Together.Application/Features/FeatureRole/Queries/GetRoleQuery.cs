using Together.Application.Features.FeatureRole.Responses;

namespace Together.Application.Features.FeatureRole.Queries;

public sealed class GetRoleQuery(Guid id) : IBaseRequest<GetRoleResponse>
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<GetRoleQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<GetRoleQuery, GetRoleResponse>(httpContextAccessor)
    {
        protected override async Task<GetRoleResponse> HandleAsync(GetRoleQuery request, CancellationToken ct)
        {
            var role = await context.Roles
                .Where(r => r.Id == request.Id)
                .Select(r => r.MapTo<GetRoleResponse>())
                .FirstOrDefaultAsync(ct);

            if (role is null) throw new DomainException(TogetherErrorCodes.Role.RoleNotFound);

            return role;
        }
    }
}