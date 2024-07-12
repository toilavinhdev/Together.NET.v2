using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.MessageAggregate;
using Together.Domain.Enums;

namespace Together.Domain.Aggregates.ConversationAggregate;

public class Conversation : ModifierTrackingEntity, IAggregateRoot
{
    public string? Name { get; set; }
    
    public ConversationType Type { get; set; }

    [InverseProperty(nameof(ConversationParticipant.Conversation))]
    public List<ConversationParticipant> ConversationParticipants { get; set; } = default!;
    
    [InverseProperty(nameof(Message.Conversation))]
    public List<Message>? Messages { get; set; }
}