using System.Linq.Expressions;
using Together.Application.Features.FeatureUser.Responses;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Application.Features.FeatureUser.Queries;

public sealed class ListUserQuery : IBaseRequest<ListUserResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string? Search { get; set; }
    
    public class Validator : AbstractValidator<ListUserQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ListUserQuery, ListUserResponse>(httpContextAccessor)
    {
        protected override async Task<ListUserResponse> HandleAsync(ListUserQuery request, CancellationToken ct)
        {
            Expression<Func<User, bool>> whereExpression = u => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                whereExpression = whereExpression.And(u => u.UserName.Contains(request.Search.ToLower()));
            }

            var queryable = context.Users
                .Where(whereExpression)
                .AsQueryable();

            var totalRecord = await queryable.LongCountAsync(ct);

            var users = await queryable
                .Paging(request.PageIndex, request.PageSize)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    SubId = u.SubId,
                    UserName = u.UserName,
                    Avatar = u.Avatar
                })
                .ToListAsync(ct);

            return new ListUserResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = users
            };
        }
    }
}