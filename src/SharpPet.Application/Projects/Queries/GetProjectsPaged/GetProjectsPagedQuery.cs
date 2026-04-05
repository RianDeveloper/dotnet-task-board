using MediatR;
using SharpPet.Application.Common.Models;
using SharpPet.Application.Contracts;

namespace SharpPet.Application.Projects.Queries.GetProjectsPaged;

public sealed record GetProjectsPagedQuery(int Page, int PageSize) : IRequest<PagedResult<ProjectSummaryDto>>;
