using FluentValidation;

namespace Edition.Application.Features.WebSiteSettings.Queries;

public class GetWebSiteSettingValidator : AbstractValidator<GetWebSiteSettingRequest>
{
    public GetWebSiteSettingValidator()
    {
    }
}
