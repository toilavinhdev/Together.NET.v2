namespace Together.Application.Features.FeatureReport.Queries;

public sealed class StatisticQuery : IBaseRequest<object>
{
    internal class Handle(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<StatisticQuery, object>(httpContextAccessor)
    {
        protected override async Task<object> HandleAsync(StatisticQuery request, CancellationToken ct)
        {
            var totalTopic = await context.Topics.LongCountAsync(ct);

            var totalUser = await context.Users.LongCountAsync(ct);
            
            var totalPost = await context.Posts.LongCountAsync(ct);
            
            var totalReply = await context.Replies.LongCountAsync(ct);

            return new
            {
                TotalTopic = totalTopic,
                TotalUser = totalUser,
                TotalPost = totalPost,
                TotalReply = totalReply
            };
        }
    }
}