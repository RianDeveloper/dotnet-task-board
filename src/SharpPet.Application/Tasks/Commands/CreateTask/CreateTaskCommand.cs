using MediatR;
using SharpPet.Domain.Enums;

namespace SharpPet.Application.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand(
    Guid ProjectId,
    string Title,
    string? Description,
    DateTimeOffset? DueDate,
    TaskStatus Status) : IRequest<Guid>;
