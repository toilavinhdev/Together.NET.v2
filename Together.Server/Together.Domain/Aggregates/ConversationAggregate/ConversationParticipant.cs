using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Domain.Aggregates.ConversationAggregate;

[PrimaryKey(nameof(ConversationId), nameof(UserId))]
public class ConversationParticipant
{
    public Guid ConversationId { get; set; }
    [ForeignKey(nameof(ConversationId))]
    public Conversation Conversation { get; set; } = default!;
    
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!;
}