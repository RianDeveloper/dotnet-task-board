using FluentValidation;

namespace SharpPet.Application.Projects.Queries.GetProjectsPaged;

public sealed class GetProjectsPagedQueryValidator : AbstractValidator<GetProjectsPagedQuery>
{
    public GetProjectsPagedQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
