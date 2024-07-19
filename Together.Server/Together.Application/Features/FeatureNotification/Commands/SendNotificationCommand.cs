using Together.Application.Sockets;
using Together.Application.Sockets.WebSocketMessages;
using Together.Domain.Aggregates.NotificationAggregate;

namespace Together.Application.Features.FeatureNotification.Commands;

public sealed class SendNotificationCommand : IBaseRequest
{
    public Guid ReceiverId { get; set; }
    
    public Guid ActorId { get; set; }
    
    public string Activity { get; set; } = default!;
    
    public Guid SourceId { get; set; }
    
    public class Validator : AbstractValidator<SendNotificationCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ReceiverId).NotEmpty();
            RuleFor(x => x.ActorId).NotEmpty();
            RuleFor(x => x.Activity).NotEmpty();
            RuleFor(x => x.SourceId).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, TogetherWebSocketHandler socket) 
        : BaseRequestHandler<SendNotificationCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(SendNotificationCommand request, CancellationToken ct)
        {
            var receiver = await context.Users.FirstOrDefaultAsync(u => u.Id == request.ReceiverId, ct);
            if (receiver is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound, request.ReceiverId.ToString());
            
            var actor = await context.Users.FirstOrDefaultAsync(u => u.Id == request.ActorId, ct);
            if (actor is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound, request.ActorId.ToString());
            
            var notification = request.MapTo<Notification>();
            notification.MarkCreated();

            await context.Notifications.AddAsync(notification, ct);

            await context.SaveChangesAsync(ct);

            await socket.SendMessageAsync(
                notification.ReceiverId.ToString(),
                new WebSocketMessage<ReceivedNotificationWebSocketMessage>
                {
                    Target = WebSocketClientTarget.ReceivedNotification,
                    Message = notification.MapTo<ReceivedNotificationWebSocketMessage>()
                });
        }
    }
}