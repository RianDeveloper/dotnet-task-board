using Microsoft.EntityFrameworkCore;
using SharpPet.Application.Abstractions;
using SharpPet.Domain.Entities;
using SharpPet.Domain.Enums;

namespace SharpPet.Infrastructure.Persistence;

public sealed class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public void Add(TaskItem task)
    {
        _db.TaskItems.Add(task);
    }

    public void Remove(TaskItem task)
    {
        _db.TaskItems.Remove(task);
    }

    public async Task<(IReadOnlyList<TaskItem> Items, int Total)> GetByProjectIdPagedAsync(
        Guid projectId,
        int page,
        int pageSize,
        TaskStatus? statusFilter,
        CancellationToken cancellationToken = default)
    {
        var query = _db.TaskItems.AsNoTracking().Where(t => t.ProjectId == projectId);
        if (statusFilter.HasValue)
            query = query.Where(t => t.Status == statusFilter.Value);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(t => t.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, total);
    }
}
