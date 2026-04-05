using MediatR;

namespace SharpPet.Application.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(string Name) : IRequest<Guid>;
