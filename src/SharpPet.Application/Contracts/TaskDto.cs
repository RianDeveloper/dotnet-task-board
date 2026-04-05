using SharpPet.Domain.Enums;

namespace SharpPet.Application.Contracts;

public sealed record TaskDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    TaskStatus Status,
    DateTimeOffset? DueDate);
