using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.WebSiteSettings.Queries;

public class GetWebSiteSettingHandler
    (IWebSiteSettingRepository repository, IWebSiteSettingMapperService mapper)
    : IRequestHandler<GetWebSiteSettingRequest, OperationResult<GetWebSiteSettingResponse>>
{
    public async Task<OperationResult<GetWebSiteSettingResponse>> Handle(GetWebSiteSettingRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsNoTrackingAsync();
        if (model is null)
            return ErrorModel.Create("WebSiteSettingsNotFound");

        return mapper.Map(model);
    }
}
