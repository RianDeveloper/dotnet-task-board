using MediatR;
using SharpPet.Domain.Enums;

namespace SharpPet.Application.Tasks.Commands.UpdateTaskStatus;

public sealed record UpdateTaskStatusCommand(Guid TaskId, TaskStatus Status) : IRequest;
