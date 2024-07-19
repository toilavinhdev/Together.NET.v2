using Together.Application.Features.FeatureNotification.Commands;
using Together.Application.Sockets.WebSocketMessages;
using Together.Domain.Aggregates.NotificationAggregate;

namespace Together.Application.Mappings;

public sealed class NotificationMapping : Profile
{
    public NotificationMapping()
    {
        CreateMap<SendNotificationCommand, Notification>();
        
        CreateMap<Notification, ReceivedNotificationWebSocketMessage>();
    }
}