namespace Together.Shared.ValueObjects;

public sealed class CurrentUserClaims
{
    public Guid Id { get; set; }
    
    public long SubId { get; set; }

    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
}