namespace Together.Domain.Abstractions;

public interface ISoftDeleteEntity
{
    Guid? DeletedById { get; set; }
    
    DateTime? DeletedAt { get; set; }
}