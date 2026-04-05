using SharpPet.Domain.Enums;

namespace SharpPet.Domain.Entities;

public sealed class TaskItem
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTimeOffset? DueDate { get; set; }
}
