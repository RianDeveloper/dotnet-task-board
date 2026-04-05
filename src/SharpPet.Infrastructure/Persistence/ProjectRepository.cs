using Microsoft.EntityFrameworkCore;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Contracts;
using SharpPet.Domain.Entities;

namespace SharpPet.Infrastructure.Persistence;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;

    public ProjectRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Projects.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public void Add(Project project)
    {
        _db.Projects.Add(project);
    }

    public void Remove(Project project)
    {
        _db.Projects.Remove(project);
    }

    public async Task<(IReadOnlyList<ProjectSummaryDto> Items, int Total)> GetPagedSummariesAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Projects.AsNoTracking();
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectSummaryDto(p.Id, p.Name, p.CreatedAt, p.Tasks.Count))
            .ToListAsync(cancellationToken);
        return (items, total);
    }
}
