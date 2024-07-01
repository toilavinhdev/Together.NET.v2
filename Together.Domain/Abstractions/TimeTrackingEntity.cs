namespace Together.Domain.Abstractions;

public abstract class TimeTrackingEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }

    public void MarkCreated()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkModified()
    {
        ModifiedAt = DateTimeOffset.UtcNow;
    }
}