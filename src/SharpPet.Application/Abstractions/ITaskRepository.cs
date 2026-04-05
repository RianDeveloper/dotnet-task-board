using SharpPet.Domain.Entities;
using SharpPet.Domain.Enums;

namespace SharpPet.Application.Abstractions;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(TaskItem task);
    void Remove(TaskItem task);
    Task<(IReadOnlyList<TaskItem> Items, int Total)> GetByProjectIdPagedAsync(
        Guid projectId,
        int page,
        int pageSize,
        TaskStatus? statusFilter,
        CancellationToken cancellationToken = default);
}
