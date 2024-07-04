using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.ForumAggregate;
using Together.Domain.Aggregates.ReplyAggregate;
using Together.Domain.Aggregates.TopicAggregate;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Domain.Aggregates.PostAggregate;

public class Post : ModifierTrackingEntity, IAggregateRoot
{
    public Guid ForumId { get; set; }
    [ForeignKey(nameof(ForumId))]
    public Forum Forum { get; set; } = default!;
    
    public Guid TopicId { get; set; }
    [ForeignKey(nameof(TopicId))]
    public Topic Topic { get; set; } = default!;
    
    public Guid? PrefixId { get; set; }
    [ForeignKey(nameof(PrefixId))]
    public Prefix? Prefix { get; set; }
    
    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;
    
    [InverseProperty(nameof(Reply.Post))]
    public List<Reply>? Replies { get; set; }
    
    [InverseProperty(nameof(PostVote.Post))]
    public List<PostVote>? PostVotes { get; set; }
}