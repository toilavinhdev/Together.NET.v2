using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Domain.Aggregates.RoleAggregate;

public class Role : ModifierTrackingEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;

    public bool IsDefault { get; set; }
    
    public string? Description { get; set; }

    public List<string> Claims { get; set; } = default!;
    
    [InverseProperty(nameof(UserRole.Role))]
    public List<UserRole>? UserRoles { get; set; }
}