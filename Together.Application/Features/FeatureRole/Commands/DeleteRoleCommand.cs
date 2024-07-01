namespace Together.Application.Features.FeatureRole.Commands;

public sealed class DeleteRoleCommand(Guid roleId) : IBaseRequest
{
    private Guid RoleId { get; set; } = roleId;
    
    public class Validator : AbstractValidator<DeleteRoleCommand>
    {
        public Validator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) : 
        BaseRequestHandler<DeleteRoleCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeleteRoleCommand request, CancellationToken ct)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId, ct);
            
            if (role is null) throw new DomainException(TogetherErrorCodes.Role.RoleNotFound);

            context.Roles.Remove(role);
            
            await context.SaveChangesAsync(ct);
            
            Message = "Deleted";
        }
    }
}