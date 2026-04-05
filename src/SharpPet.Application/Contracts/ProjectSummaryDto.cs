namespace SharpPet.Application.Contracts;

public sealed record ProjectSummaryDto(Guid Id, string Name, DateTimeOffset CreatedAt, int TaskCount);
