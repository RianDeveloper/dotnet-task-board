using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Exceptions;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IProjectRepository _projects;
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(
        IProjectRepository projects,
        ITaskRepository tasks,
        IUnitOfWork unitOfWork)
    {
        _projects = projects;
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Status = request.Status,
            DueDate = request.DueDate
        };
        _tasks.Add(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return task.Id;
    }
}
