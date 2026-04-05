using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Common.Exceptions;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Projects.Commands.DeleteProject;

public sealed class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projects;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IProjectRepository projects, IUnitOfWork unitOfWork)
    {
        _projects = projects;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        _projects.Remove(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
