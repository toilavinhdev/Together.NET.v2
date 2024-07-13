using Together.Application.Sockets;

namespace Together.Application.Features.FeatureReport.Queries;

public sealed class StatisticsQuery : IBaseRequest<Dictionary<string, object>>
{
    internal class Handle(IHttpContextAccessor httpContextAccessor, TogetherContext context, TogetherWebSocketHandler socket) 
        : BaseRequestHandler<StatisticsQuery, Dictionary<string, object>>(httpContextAccessor)
    {
        protected override async Task<Dictionary<string, object>> HandleAsync(StatisticsQuery request, CancellationToken ct)
        {
            var data = new Dictionary<string, object>();
            
            var totalTopic = await context.Topics.LongCountAsync(ct);
            data.Add("totalTopic", totalTopic);

            var totalUser = await context.Users.LongCountAsync(ct);
            data.Add("totalUser", totalUser);
            
            var totalPost = await context.Posts.LongCountAsync(ct);
            data.Add("totalPost", totalPost);
            
            var totalReply = await context.Replies.LongCountAsync(ct);
            data.Add("totalReply", totalReply);

            var totalOnlineUser = socket.ConnectionManager.GetAll().Count;
            data.Add("totalOnlineUser", totalOnlineUser);

            var newMember = await context.Users
                .OrderByDescending(u => u.CreatedAt)
                .FirstOrDefaultAsync(ct);
            if(newMember is not null) data.Add("newMember", newMember.UserName);

            return data;
        }
    }
}