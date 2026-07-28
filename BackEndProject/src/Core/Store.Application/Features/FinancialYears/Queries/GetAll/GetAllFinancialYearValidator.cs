using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.FinancialYears.Queries;

public class GetAllFinancialYearValidator : AbstractValidator<GetAllFinancialYearRequest>
{
    public GetAllFinancialYearValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.FinancialYear.Title);
    }
}
