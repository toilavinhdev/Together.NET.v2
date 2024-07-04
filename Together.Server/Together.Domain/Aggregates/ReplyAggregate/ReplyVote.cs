using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.UserAggregate;
using Together.Domain.Enums;

namespace Together.Domain.Aggregates.ReplyAggregate;

public class ReplyVote : ModifierTrackingEntity
{
    public Guid ReplyId { get; set; }
    [ForeignKey(nameof(ReplyId))]
    public Reply Reply { get; set; } = default!;
    
    public VoteType Type { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;
}