namespace SharpPet.Application.Contracts;

public sealed record ProjectDto(Guid Id, string Name, DateTimeOffset CreatedAt);
