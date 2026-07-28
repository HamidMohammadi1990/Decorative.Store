using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.CompanyComments.Queries;
using Edition.Application.Features.CompanyComments.Commands;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Company Comments
/// </summary>
[ApiVersion("1")]
[ControllerName("company-comment")]
[ApiControllerCategory(ApiControllerCategory.Company)]
public class CompanyCommentController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCompanyCommentResponse>>> Search(SearchCompanyCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCompanyCommentResponse>> Create(CreateCompanyCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete(DeleteCompanyCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("update")]
    public async Task<ApiResult> Update(UpdateCompanyCommentRequest request)
        => await mediator.Send(request);
}