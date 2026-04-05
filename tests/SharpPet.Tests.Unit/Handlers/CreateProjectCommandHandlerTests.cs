using FluentAssertions;
using Moq;
using SharpPet.Application.Abstractions;
using SharpPet.Application.Projects.Commands.CreateProject;
using SharpPet.Domain.Entities;
using Xunit;

namespace SharpPet.Tests.Unit.Handlers;

public sealed class CreateProjectCommandHandlerTests
{
    [Fact]
    public async Task Persists_project_and_returns_id()
    {
        var projects = new Mock<IProjectRepository>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateProjectCommandHandler(projects.Object, uow.Object);
        var id = await handler.Handle(new CreateProjectCommand("  Beta  "), CancellationToken.None);

        id.Should().NotBeEmpty();
        projects.Verify(
            x => x.Add(It.Is<Project>(p => p.Name == "Beta" && p.Id == id)),
            Times.Once);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
