using Together.Application.Features.FeatureRole.Responses;
using Together.Domain.Aggregates.RoleAggregate;

namespace Together.Application.Features.FeatureRole.Commands;

public sealed class CreateRoleCommand : IBaseRequest<CreateRoleResponse>
{
    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public List<string>? Claims { get; set; }
    
    public class Validator : AbstractValidator<CreateRoleCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) : 
        BaseRequestHandler<CreateRoleCommand, CreateRoleResponse>(httpContextAccessor)
    {
        protected override async Task<CreateRoleResponse> HandleAsync(CreateRoleCommand request, CancellationToken ct)
        {
            request.Claims = TogetherPolicies.RequiredPolicies()
                .Union(request.Claims!)
                .ToList();
            
            var role = request.MapTo<Role>();
            role.MarkUserCreated(CurrentUserClaims.Id);
            
            await context.Roles.AddAsync(role, ct);
            
            await context.SaveChangesAsync(ct);

            Message = "Created";

            return role.MapTo<CreateRoleResponse>();
        }
    }
}