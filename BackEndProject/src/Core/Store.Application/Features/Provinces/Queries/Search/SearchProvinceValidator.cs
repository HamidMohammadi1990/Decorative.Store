using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Provinces.Queries;

public class SearchProvinceValidator : AbstractValidator<SearchProvinceRequest>
{
    public SearchProvinceValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.Province.Name);
    }
}
