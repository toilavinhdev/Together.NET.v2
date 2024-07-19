using Together.Application.Features.FeatureNotification.Responses;

namespace Together.Application.Features.FeatureNotification.Queries;

public sealed class ListNotificationQuery : IBaseRequest<ListNotificationResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public class Validator : AbstractValidator<ListNotificationQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ListNotificationQuery, ListNotificationResponse>(httpContextAccessor)
    {
        protected override async Task<ListNotificationResponse> HandleAsync(ListNotificationQuery request, CancellationToken ct)
        {
            var queryable = context.Notifications
                .Where(n => n.ReceiverId == CurrentUserClaims.Id)
                .AsQueryable();

            var totalRecord = await queryable.LongCountAsync(ct);

            var data = await queryable
                .Include(n => n.Actor)
                .OrderByDescending(n => n.CreatedAt)
                .Paging(request.PageIndex, request.PageSize)
                .Select(n => new NotificationViewModel
                {
                    Id = n.Id,
                    SubId = n.SubId,
                    ActorId = n.ActorId,
                    ActorUserName = n.Actor.UserName,
                    ActorAvatar = n.Actor.Avatar,
                    Activity = n.Activity,
                    SourceId = n.SourceId,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(ct);
            
            return new ListNotificationResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data
            };
        }
    }
}