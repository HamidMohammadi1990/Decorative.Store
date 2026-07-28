using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Provinces.Queries;

public class GetAllProvinceValidator : AbstractValidator<GetAllProvinceRequest>
{
    public GetAllProvinceValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.Province.Name);
    }
}
