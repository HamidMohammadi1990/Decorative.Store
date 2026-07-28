using Edition.Application.Features.WebSiteSettings.Queries;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IWebSiteSettingMapperService : IMapper
{
    GetWebSiteSettingResponse Map(WebSiteSetting model);
}
