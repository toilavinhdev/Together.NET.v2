using Microsoft.EntityFrameworkCore;
using Together.Domain.Aggregates.ForumAggregate;
using Together.Domain.Aggregates.PostAggregate;
using Together.Domain.Aggregates.ReplyAggregate;
using Together.Domain.Aggregates.RoleAggregate;
using Together.Domain.Aggregates.TopicAggregate;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Persistence;

public sealed class TogetherContext(DbContextOptions<TogetherContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; } = default!;

    public DbSet<Role> Roles { get; init; } = default!;

    public DbSet<UserRole> UserRoles { get; init; } = default!;
    
    public DbSet<Forum> Forums  { get; init; } = default!;
    
    public DbSet<Topic> Topics  { get; init; } = default!;
    
    public DbSet<Prefix> Prefixes  { get; init; } = default!;
    
    public DbSet<Post> Posts  { get; init; } = default!;
    
    public DbSet<PostVote> PostVotes  { get; init; } = default!;
    
    public DbSet<Reply> Replies  { get; init; } = default!;
    
    public DbSet<ReplyVote> ReplyVotes  { get; init; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(PersistenceAssembly.Assembly);
    }
}