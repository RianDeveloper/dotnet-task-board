using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharpPet.Api.Contracts;
using SharpPet.Api.Infrastructure;
using SharpPet.Application;
using SharpPet.Application.Projects.Commands.CreateProject;
using SharpPet.Application.Projects.Commands.DeleteProject;
using SharpPet.Application.Projects.Queries.GetProjectById;
using SharpPet.Application.Projects.Queries.GetProjectsPaged;
using SharpPet.Application.Tasks.Commands.CreateTask;
using SharpPet.Application.Tasks.Commands.DeleteTask;
using SharpPet.Application.Tasks.Commands.UpdateTaskStatus;
using SharpPet.Application.Tasks.Queries.GetTasksByProject;
using SharpPet.Domain.Enums;
using SharpPet.Infrastructure;
using SharpPet.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.Development.local.json", optional: true, reloadOnChange: true);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

var projects = app.MapGroup("/api/projects").WithTags("Projects");

projects.MapGet(
        "/",
        async (ISender sender, int page = 1, int pageSize = 20) =>
            Results.Ok(await sender.Send(new GetProjectsPagedQuery(page, pageSize))))
    .WithName("GetProjects");

projects.MapPost(
        "/",
        async (CreateProjectRequest body, ISender sender) =>
        {
            var id = await sender.Send(new CreateProjectCommand(body.Name));
            return Results.Created($"/api/projects/{id}", new { id });
        })
    .WithName("CreateProject");

projects.MapGet(
        "/{projectId:guid}",
        async (Guid projectId, ISender sender) =>
            Results.Ok(await sender.Send(new GetProjectByIdQuery(projectId))))
    .WithName("GetProject");

projects.MapDelete(
        "/{projectId:guid}",
        async (Guid projectId, ISender sender) =>
        {
            await sender.Send(new DeleteProjectCommand(projectId));
            return Results.NoContent();
        })
    .WithName("DeleteProject");

projects.MapPost(
        "/{projectId:guid}/tasks",
        async (Guid projectId, CreateTaskRequest body, ISender sender) =>
        {
            var status = body.Status ?? TaskStatus.Todo;
            var id = await sender.Send(
                new CreateTaskCommand(projectId, body.Title, body.Description, body.DueDate, status));
            return Results.Created($"/api/tasks/{id}", new { id });
        })
    .WithName("CreateTask");

projects.MapGet(
        "/{projectId:guid}/tasks",
        async (Guid projectId, ISender sender, int page = 1, int pageSize = 20, TaskStatus? status = null) =>
            Results.Ok(await sender.Send(new GetTasksByProjectQuery(projectId, page, pageSize, status))))
    .WithName("GetTasksByProject");

var tasks = app.MapGroup("/api/tasks").WithTags("Tasks");

tasks.MapPatch(
        "/{taskId:guid}/status",
        async (Guid taskId, UpdateTaskStatusRequest body, ISender sender) =>
        {
            await sender.Send(new UpdateTaskStatusCommand(taskId, body.Status));
            return Results.NoContent();
        })
    .WithName("UpdateTaskStatus");

tasks.MapDelete(
        "/{taskId:guid}",
        async (Guid taskId, ISender sender) =>
        {
            await sender.Send(new DeleteTaskCommand(taskId));
            return Results.NoContent();
        })
    .WithName("DeleteTask");

app.Run();

public partial class Program
{
}
