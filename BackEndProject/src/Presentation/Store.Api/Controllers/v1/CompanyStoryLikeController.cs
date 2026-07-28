using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Dtos.Pagination;
using Edition.Application.Features.CompanyStoryLikes.Queries;
using Edition.Application.Features.CompanyStoryLikes.Commands;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Company Story Like
/// </summary>
[ApiVersion("1")]
[ControllerName("company-story-like")]
[ApiControllerCategory(ApiControllerCategory.Company)]
public class CompanyStoryLikeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("create")]
    public async Task<ApiResult<CreateCompanyStoryLikeResponse>> Create(CreateCompanyStoryLikeRequest request)
        => await mediator.Send(request);
}
