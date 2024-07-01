using FluentValidation;
using Together.Shared.ValueObjects;

namespace Together.Shared.Helpers;

public sealed class PaginationValidator : AbstractValidator<IPaginationRequest>
{
    public PaginationValidator()
    {
        RuleFor(x => x.PageIndex)
            .NotNull()
            .GreaterThan(0);
        
        RuleFor(x => x.PageSize)
            .NotNull()
            .GreaterThan(0);
    }
}