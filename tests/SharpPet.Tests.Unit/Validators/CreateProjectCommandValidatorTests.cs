using FluentAssertions;
using SharpPet.Application.Projects.Commands.CreateProject;
using Xunit;

namespace SharpPet.Tests.Unit.Validators;

public sealed class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator = new();

    [Fact]
    public void Empty_name_fails()
    {
        var result = _validator.Validate(new CreateProjectCommand(""));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Valid_name_passes()
    {
        var result = _validator.Validate(new CreateProjectCommand("Alpha"));
        result.IsValid.Should().BeTrue();
    }
}
