using Together.Application.Sockets;

namespace Together.Application.Features.FeatureReport.Queries;

public sealed class StatisticsQuery : IBaseRequest<Dictionary<string, object>>
{
    public List<string>? Metrics { get; set; }
    
    internal class Handle(IHttpContextAccessor httpContextAccessor, TogetherContext context, TogetherWebSocketHandler socket) 
        : BaseRequestHandler<StatisticsQuery, Dictionary<string, object>>(httpContextAccessor)
    {
        protected override async Task<Dictionary<string, object>> HandleAsync(StatisticsQuery request, CancellationToken ct)
        {
            var data = new Dictionary<string, object>();

            if (request.Metrics is null) return data;

            if (request.Metrics.Contains("totalTopic"))
            {
                var totalTopic = await context.Topics.LongCountAsync(ct);
                data.Add("totalTopic", totalTopic);
            }
            
            if (request.Metrics.Contains("totalUser"))
            {
                var totalUser = await context.Users.LongCountAsync(ct);
                data.Add("totalUser", totalUser);
            }
            
            if (request.Metrics.Contains("totalPost"))
            {
                var totalPost = await context.Posts.LongCountAsync(ct);
                data.Add("totalPost", totalPost);
            }
            
            if (request.Metrics.Contains("totalReply"))
            {
                var totalReply = await context.Replies.LongCountAsync(ct);
                data.Add("totalReply", totalReply);
            }

            if (request.Metrics.Contains("totalOnlineUser"))
            {
                var totalOnlineUser = socket.ConnectionManager.GetAll().Count;
                data.Add("totalOnlineUser", totalOnlineUser);
            }
            
            if (request.Metrics.Contains("newMember"))
            {
                var newMember = await context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .FirstOrDefaultAsync(ct);
                if(newMember is not null) data.Add("newMember", newMember.UserName);
            }
            
            return data;
        }
    }
}