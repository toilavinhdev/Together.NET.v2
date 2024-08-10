using System.Linq.Expressions;
using Together.Application.Features.FeatureUser.Responses;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Application.Features.FeatureUser.Queries;

public sealed class ListUserQuery : IBaseRequest<ListUserResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string? Search { get; set; }
    
    public Guid? RoleId { get; set; }
    
    public class Validator : AbstractValidator<ListUserQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, IRedisService redisService) 
        : BaseRequestHandler<ListUserQuery, ListUserResponse>(httpContextAccessor)
    {
        protected override async Task<ListUserResponse> HandleAsync(ListUserQuery request, CancellationToken ct)
        {
            var queryable = context.Users.AsQueryable();
            
            Expression<Func<User, bool>> whereExpression = u => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                whereExpression = whereExpression.And(u => u.UserName.Contains(request.Search.ToLower()));
            }

            if (request.RoleId is not null)
            {
                queryable = queryable.Include(u => u.UserRoles);
                whereExpression = whereExpression.And(u => u.UserRoles!.Any(ur => ur.RoleId == request.RoleId));
            }

            queryable = queryable.Where(whereExpression);

            var totalRecord = await queryable.LongCountAsync(ct);

            var users = await queryable
                .Paging(request.PageIndex, request.PageSize)
                .Select(u => u.MapTo<UserViewModel>())
                .ToListAsync(ct);
            
            foreach (var user in users)
            {
                user.Online = await redisService.SetContainsAsync(TogetherRedisKeys.OnlineUserKeys(), user.Id.ToString());
            }

            return new ListUserResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = users
            };
        }
    }
}