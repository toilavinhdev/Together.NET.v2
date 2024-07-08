using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.PostAggregate;
using Together.Domain.Aggregates.ReplyAggregate;
using Together.Domain.Enums;

namespace Together.Domain.Aggregates.UserAggregate;

public class User : TimeTrackingEntity, IAggregateRoot
{
    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public UserStatus Status { get; set; }
    
    public string PasswordHash { get; set; } = default!;
    
    public Gender Gender { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Avatar { get; set; }
    
    public string? Biography { get; set; }
    
    [InverseProperty(nameof(UserRole.User))]
    public List<UserRole>? UserRoles { get; set; }
    
    [InverseProperty(nameof(Post.CreatedBy))]
    public List<Post>? Posts { get; set; }
    
    [InverseProperty(nameof(Reply.CreatedBy))]
    public List<Reply>? Replies { get; set; }
}