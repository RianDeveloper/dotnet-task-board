using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Models;
using SharpPet.Application.Contracts;

namespace SharpPet.Application.Projects.Queries.GetProjectsPaged;

public sealed class GetProjectsPagedQueryHandler : IRequestHandler<GetProjectsPagedQuery, PagedResult<ProjectSummaryDto>>
{
    private readonly IProjectRepository _projects;

    public GetProjectsPagedQueryHandler(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<PagedResult<ProjectSummaryDto>> Handle(GetProjectsPagedQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _projects.GetPagedSummariesAsync(request.Page, request.PageSize, cancellationToken);
        return new PagedResult<ProjectSummaryDto>(items, total, request.Page, request.PageSize);
    }
}
