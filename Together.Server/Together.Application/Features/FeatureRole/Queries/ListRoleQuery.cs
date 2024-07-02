using System.Linq.Expressions;
using Together.Application.Features.FeatureRole.Responses;
using Together.Domain.Aggregates.RoleAggregate;

namespace Together.Application.Features.FeatureRole.Queries;

public sealed class ListRoleQuery : IBaseRequest<ListRoleResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public Guid? UserId { get; set; }
    
    public class Validator : AbstractValidator<ListRoleQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) :
        BaseRequestHandler<ListRoleQuery, ListRoleResponse>(httpContextAccessor)
    {
        protected override async Task<ListRoleResponse> HandleAsync(ListRoleQuery request, CancellationToken ct)
        {
            Expression<Func<Role, bool>> whereExpression = role => true;

            if (request.UserId is not null)
            {
                whereExpression = whereExpression.And(r => r.UserRoles!.Any(ur => ur.UserId == request.UserId));
            }

            var query = context.Roles
                .Where(whereExpression)
                .AsQueryable();

            var totalRecord = await query.LongCountAsync(ct);

            var data = await query
                .Select(role => role.MapTo<RoleViewModel>())
                .ToListAsync(ct);

            return new ListRoleResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data
            };
        }
    }
}