using MediatR;
using SharpPet.Application.Abstractions;
using SharpPet.Domain.Entities;

namespace SharpPet.Application.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projects;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(IProjectRepository projects, IUnitOfWork unitOfWork)
    {
        _projects = projects;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };
        _projects.Add(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return project.Id;
    }
}
