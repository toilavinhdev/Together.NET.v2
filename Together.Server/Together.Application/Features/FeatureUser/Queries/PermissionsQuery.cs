namespace Together.Application.Features.FeatureUser.Queries;

public sealed class PermissionsQuery : IBaseRequest<List<string>>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<PermissionsQuery, List<string>>(httpContextAccessor)
    {
        protected override async Task<List<string>> HandleAsync(PermissionsQuery request, CancellationToken ct)
        {
            var roles = await context.UserRoles
                .Include(r => r.Role)
                .Where(ur => ur.UserId == CurrentUserClaims.Id)
                .Select(ur => ur.Role)
                .ToListAsync(ct);

            return roles
                .SelectMany(role => role.Claims)
                .Distinct()
                .OrderBy(claim => claim)
                .ToList();
        }
    }
}