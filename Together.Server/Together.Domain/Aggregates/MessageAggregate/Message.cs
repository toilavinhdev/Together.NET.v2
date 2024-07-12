using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.ConversationAggregate;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Domain.Aggregates.MessageAggregate;

public class Message : ModifierTrackingEntity, ISoftDeleteEntity
{
    public Guid ConversationId { get; set; }
    [ForeignKey(nameof(ConversationId))]
    public Conversation Conversation { get; set; } = default!;

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;

    public string Text { get; set; } = default!;
    
    public Guid? DeletedById { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}