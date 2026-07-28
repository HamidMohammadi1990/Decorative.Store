using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Companies.Commands;

public class CreateCompanyValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyValidator(ICompanyRepository companyRepository)
    {
        RuleFor(x => new { x.CompanyName, x.CompanyCode, x.PostalCode })
            .MustAsync(async (x, CancellationToken)
                   => !await companyRepository
            .AnyAsync(c => c.Name == x.CompanyName.Trim() ||
                      c.PostalCode == x.PostalCode.Trim() ||
                      c.Code == x.CompanyCode.Trim()))
            .WithMessage(MessageKeys.DuplicateInformation);

        RuleFor(x => x.CityId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidCityId);

        RuleFor(x => x.CompanyName)
            .MinimumLength(5)
            .WithMessage(MessageKeys.MinLength5Characters)
            .MaximumLength(30)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => x.CompanyCode)
            .MinimumLength(5)
            .WithMessage(MessageKeys.MinLength5Characters)
            .MaximumLength(12)
            .WithMessage(MessageKeys.MaxLength12Characters);

        RuleFor(x => x.PhoneNumber)
            .MinimumLength(11)
            .WithMessage(MessageKeys.MinLength11Characters)
            .MaximumLength(11)
            .WithMessage(MessageKeys.MaxLength11Characters);

        RuleFor(x => x.Email)
            .MinimumLength(10)
            .WithMessage(MessageKeys.MinLength10Characters)
            .MaximumLength(35)
            .WithMessage(MessageKeys.MaxLength35Characters);

        RuleFor(x => x.PostalCode)
            .MinimumLength(10)
            .WithMessage(MessageKeys.MinLength10Characters)
            .MaximumLength(10)
            .WithMessage(MessageKeys.MaxLength10Characters);

        RuleFor(x => x.Address)
            .MinimumLength(15)
            .WithMessage(MessageKeys.MinLength15Characters)
            .MaximumLength(120)
            .WithMessage(MessageKeys.MaxLength120Characters);

        RuleFor(x => x.Description)
            .MaximumLength(300)
            .WithMessage(MessageKeys.MaxLength300Characters);

        RuleFor(x => x.Latitude)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidLatitude);

        RuleFor(x => x.Longitude)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidLongitude);
    }
}
