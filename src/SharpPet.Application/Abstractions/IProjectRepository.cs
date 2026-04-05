using SharpPet.Application.Contracts;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Abstractions;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Project project);
    void Remove(Project project);
    Task<(IReadOnlyList<ProjectSummaryDto> Items, int Total)> GetPagedSummariesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
