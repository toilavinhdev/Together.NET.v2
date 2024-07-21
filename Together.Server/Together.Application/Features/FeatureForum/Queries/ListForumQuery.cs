using Together.Application.Features.FeatureForum.Responses;
using Together.Application.Features.FeaturePost.Responses;

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
                .ThenInclude(f => f.Posts)!
                .ThenInclude(f => f.Replies)
                .OrderBy(f => f.CreatedAt)
                .Select(f => new ForumViewModel
                {
                    Id = f.Id,
                    SubId = f.SubId,
                    Name = f.Name,
                    CreatedAt = f.CreatedAt,
                    Topics = f.Topics!.Select(t => new TopicViewModel
                    {
                        Id = t.Id,
                        SubId = t.SubId,
                        ForumId = t.ForumId,
                        Name = t.Name,
                        Description = t.Description,
                        CreatedAt = t.CreatedAt,
                        PostCount = t.Posts!.Count,
                        ReplyCount = t.Posts!.SelectMany(p => p.Replies!).LongCount(),
                    }).ToList()
                })
                .ToListAsync(ct);

            return data;
        }
    }
}