using SharpPet.Domain.Enums;

namespace SharpPet.Api.Contracts;

public sealed record CreateProjectRequest(string Name);

public sealed record CreateTaskRequest(
    string Title,
    string? Description,
    DateTimeOffset? DueDate,
    TaskStatus? Status);

public sealed record UpdateTaskStatusRequest(TaskStatus Status);
