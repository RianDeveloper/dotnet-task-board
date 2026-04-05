using MediatR;
using SharpPet.Application.Contracts;

namespace SharpPet.Application.Projects.Queries.GetProjectById;

public sealed record GetProjectByIdQuery(Guid ProjectId) : IRequest<ProjectDto>;
