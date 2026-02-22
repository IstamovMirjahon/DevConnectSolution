using DevConnect.Domain.Enums;

namespace DevConnect.Domain.Entities;

public class BaseEntity : Base
{
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public State State { get; set; }
}