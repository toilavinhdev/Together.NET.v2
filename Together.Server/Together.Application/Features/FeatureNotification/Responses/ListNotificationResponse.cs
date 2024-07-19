namespace Together.Application.Features.FeatureNotification.Responses;

public class ListNotificationResponse : PaginationResult<NotificationViewModel>;

public class NotificationViewModel : TimeTrackingEntity
{
    public Guid ActorId { get; set; }

    public string ActorUserName { get; set; } = default!;
    
    public string? ActorAvatar { get; set; }
    
    public string Activity { get; set; } = default!;
    
    public Guid SourceId { get; set; }
}