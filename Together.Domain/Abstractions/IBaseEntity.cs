using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Together.Domain.Abstractions;

public interface IBaseEntity
{
    Guid Id { get; set; }
    
    long SubId { get; set; }
}

[PrimaryKey(nameof(Id))]
[Index(nameof(SubId), IsUnique = true)]
public abstract class BaseEntity : IBaseEntity
{
    [Column(Order = 0)]
    public Guid Id { get; set; }
    
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SubId { get; set; }
}