using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Exceptions;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Tasks.Commands.UpdateTaskStatus;

public sealed class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskStatusCommandHandler(ITaskRepository tasks, IUnitOfWork unitOfWork)
    {
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException(nameof(TaskItem), request.TaskId);

        task.Status = request.Status;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
