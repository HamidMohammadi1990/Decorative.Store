using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Companies.Queries;

public class GetAllCompanyValidator : AbstractValidator<GetAllCompanyRequest>
{
    public GetAllCompanyValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ProvinceId).MustBeValidOptionalEntityId();
        RuleFor(x => x.CityId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.Company.Name);
        RuleFor(x => x.Code).MaximumLengthWhenNotEmpty(EntityFieldLengths.Company.Code);
        RuleFor(x => x.PostalCode).MaximumLengthWhenNotEmpty(EntityFieldLengths.Company.PostalCode);
    }
}
