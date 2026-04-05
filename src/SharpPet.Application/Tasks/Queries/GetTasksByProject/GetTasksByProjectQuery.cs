using MediatR;
using SharpPet.Application.Common.Models;
using SharpPet.Application.Contracts;
using SharpPet.Domain.Enums;

namespace SharpPet.Application.Tasks.Queries.GetTasksByProject;

public sealed record GetTasksByProjectQuery(
    Guid ProjectId,
    int Page,
    int PageSize,
    TaskStatus? Status) : IRequest<PagedResult<TaskDto>>;
