using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.UserAggregate;
using Together.Domain.Enums;

namespace Together.Domain.Aggregates.PostAggregate;

public class PostVote : ModifierTrackingEntity
{
    public Guid PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public Post Post { get; set; } = default!;
    
    public VoteType Type { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;
}