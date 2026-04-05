using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Exceptions;
using SharpPet.Application.Contracts;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Projects.Queries.GetProjectById;

public sealed class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IProjectRepository _projects;

    public GetProjectByIdQueryHandler(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        return new ProjectDto(project.Id, project.Name, project.CreatedAt);
    }
}
