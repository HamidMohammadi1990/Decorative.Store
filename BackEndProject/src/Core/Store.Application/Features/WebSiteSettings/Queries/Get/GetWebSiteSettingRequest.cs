using Store.Common.Models;

namespace Edition.Application.Features.WebSiteSettings.Queries;

public record GetWebSiteSettingRequest : IRequest<OperationResult<GetWebSiteSettingResponse>>;
