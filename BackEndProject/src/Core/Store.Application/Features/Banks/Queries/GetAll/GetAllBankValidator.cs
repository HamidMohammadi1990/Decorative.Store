using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Banks.Queries;

public class GetAllBankValidator : AbstractValidator<GetAllBankRequest>
{
    public GetAllBankValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Bank.Name);
    }
}
