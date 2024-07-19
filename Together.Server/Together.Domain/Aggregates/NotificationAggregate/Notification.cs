using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Domain.Aggregates.NotificationAggregate;

public class Notification : TimeTrackingEntity, IAggregateRoot
{
    public Guid ReceiverId { get; set; }
    [ForeignKey(nameof(ReceiverId))]
    public User Receiver { get; set; } = default!;
    
    public Guid ActorId { get; set; }
    [ForeignKey(nameof(ActorId))]
    public User Actor { get; set; } = default!;
    
    public string Activity { get; set; } = default!;
    
    public Guid SourceId { get; set; }
}