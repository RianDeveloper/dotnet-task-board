using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Exceptions;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Tasks.Commands.DeleteTask;

public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(ITaskRepository tasks, IUnitOfWork unitOfWork)
    {
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException(nameof(TaskItem), request.TaskId);

        _tasks.Remove(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
