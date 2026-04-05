using MediatR;

namespace SharpPet.Application.Tasks.Commands.DeleteTask;

public sealed record DeleteTaskCommand(Guid TaskId) : IRequest;
