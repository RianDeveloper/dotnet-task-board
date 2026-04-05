using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using SharpPet.Application.Contracts;
using SharpPet.Domain.Enums;
using Xunit;

namespace SharpPet.Tests.Integration;

public sealed class TaskBoardApiTests : IClassFixture<ApiFixture>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private readonly HttpClient _client;

    public TaskBoardApiTests(ApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task Create_project_task_and_update_status_roundtrip()
    {
        var createProject = await _client.PostAsJsonAsync("/api/projects", new { name = "P1" });
        createProject.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createProject.Content.ReadFromJsonAsync<CreatedResponse>(JsonOptions);
        created.Should().NotBeNull();

        var project = await _client.GetFromJsonAsync<ProjectDto>($"/api/projects/{created!.id}", JsonOptions);
        project.Should().NotBeNull();
        project!.Name.Should().Be("P1");

        var createTask = await _client.PostAsJsonAsync(
            $"/api/projects/{created.id}/tasks",
            new { title = "T1", description = (string?)null, dueDate = (DateTimeOffset?)null, status = TaskStatus.Todo });
        createTask.StatusCode.Should().Be(HttpStatusCode.Created);
        var taskCreated = await createTask.Content.ReadFromJsonAsync<CreatedResponse>(JsonOptions);
        taskCreated.Should().NotBeNull();

        var patch = await _client.PatchAsJsonAsync(
            $"/api/tasks/{taskCreated!.id}/status",
            new { status = TaskStatus.Done });
        patch.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var tasksResponse = await _client.GetAsync($"/api/projects/{created.id}/tasks?page=1&pageSize=10&status=Done");
        tasksResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var tasks = await tasksResponse.Content.ReadFromJsonAsync<PagedResultDto<TaskDto>>(JsonOptions);
        tasks.Should().NotBeNull();
        tasks!.Items.Should().ContainSingle(t => t.Id == taskCreated.id && t.Status == TaskStatus.Done);
    }

    private sealed record CreatedResponse(Guid id);

    private sealed record PagedResultDto<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
}
