using Together.Application.Features.FeatureForum.Responses;

namespace Together.Application.Features.FeatureForum.Queries;

public class ListForumQuery : IBaseRequest<List<ForumViewModel>>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ListForumQuery, List<ForumViewModel>>(httpContextAccessor)
    {
        protected override async Task<List<ForumViewModel>> HandleAsync(ListForumQuery request, CancellationToken ct)
        {
            var data = await context.Forums
                .Include(f => f.Topics)!
                .ThenInclude(f => f.Posts)
                .Select(f => f.MapTo<ForumViewModel>())
                .ToListAsync(ct);

            return data;
        }
    }
}