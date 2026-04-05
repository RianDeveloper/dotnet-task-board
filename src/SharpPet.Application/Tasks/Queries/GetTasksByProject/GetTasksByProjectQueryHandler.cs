using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Exceptions;
using SharpPet.Application.Common.Models;
using SharpPet.Application.Contracts;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Tasks.Queries.GetTasksByProject;

public sealed class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, PagedResult<TaskDto>>
{
    private readonly IProjectRepository _projects;
    private readonly ITaskRepository _tasks;

    public GetTasksByProjectQueryHandler(IProjectRepository projects, ITaskRepository tasks)
    {
        _projects = projects;
        _tasks = tasks;
    }

    public async Task<PagedResult<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        var (items, total) = await _tasks.GetByProjectIdPagedAsync(
            request.ProjectId,
            request.Page,
            request.PageSize,
            request.Status,
            cancellationToken);

        var dtos = items.Select(t => new TaskDto(t.Id, t.ProjectId, t.Title, t.Description, t.Status, t.DueDate)).ToList();
        return new PagedResult<TaskDto>(dtos, total, request.Page, request.PageSize);
    }
}
