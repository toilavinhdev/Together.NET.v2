using Together.Domain.Enums;

namespace Together.Shared.ValueObjects;

public class IdentityPrivilege
{
    public Guid Id { get; set; }

    public long SubId { get; set; }

    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public UserStatus Status { get; set; }
    
    public string? Avatar { get; set; }

    public List<string> RoleClaims { get; set; } = default!;
}