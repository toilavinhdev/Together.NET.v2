using Microsoft.EntityFrameworkCore;
using Together.Domain.Aggregates.RoleAggregate;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Persistence;

public sealed class TogetherContext(DbContextOptions<TogetherContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; } = default!;

    public DbSet<Role> Roles { get; init; } = default!;

    public DbSet<UserRole> UserRoles { get; init; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(PersistenceAssembly.Assembly);
    }
}