namespace Together.Application.Features.FeatureRole.Commands;

public sealed class UpdateRoleCommand : IBaseRequest
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }

    public List<string> Claims { get; set; } = default!;
    
    public class Validator : AbstractValidator<UpdateRoleCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Claims).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) :
        BaseRequestHandler<UpdateRoleCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdateRoleCommand request, CancellationToken ct)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
            if (role is null) throw new DomainException(TogetherErrorCodes.Role.RoleNotFound);

            role.Name = request.Name;
            role.Description = request.Description;
            role.Claims = request.Claims;
            role.MarkUserModified(CurrentUserClaims.Id);
            
            context.Roles.Update(role);
            
            await context.SaveChangesAsync(ct);

            Message = "Updated";
        }
    }
}