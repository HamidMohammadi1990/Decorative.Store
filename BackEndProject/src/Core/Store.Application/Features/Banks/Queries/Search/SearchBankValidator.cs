using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Banks.Queries;

public class SearchBankValidator : AbstractValidator<SearchBankRequest>
{
    public SearchBankValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Bank.Name);
    }
}
