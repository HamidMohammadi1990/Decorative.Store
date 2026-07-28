using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.WebSiteSettings.Queries;

public record GetPublicWebSiteSettingRequest : IRequest<OperationResult<GetPublicWebSiteSettingResponse>>;

public record GetPublicWebSiteSettingResponse
{
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Telephone { get; init; }
    public string? Address { get; init; }
    public string? CartNumber { get; init; }
}

public class GetPublicWebSiteSettingHandler
    (IWebSiteSettingRepository repository)
    : IRequestHandler<GetPublicWebSiteSettingRequest, OperationResult<GetPublicWebSiteSettingResponse>>
{
    public async Task<OperationResult<GetPublicWebSiteSettingResponse>> Handle(GetPublicWebSiteSettingRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsNoTrackingAsync();
        if (model is null)
            return ErrorModel.Create("WebSiteSettingsNotFound");

        return new GetPublicWebSiteSettingResponse
        {
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Telephone = model.Telephone,
            Address = model.Address,
            CartNumber = model.CartNumber
        };
    }
}
