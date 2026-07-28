using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.CompanyStoryComments.Queries;
using Edition.Application.Features.CompanyStoryComments.Commands;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Company Story Comment
/// </summary>
[ApiVersion("1")]
[ControllerName("company-story-comment")]
[ApiControllerCategory(ApiControllerCategory.Company)]
public class CompanyStoryCommentController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCompanyStoryCommentResponse>>> Search(SearchCompanyStoryCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCompanyStoryCommentResponse>> Create(CreateCompanyStoryCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCompanyStoryCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteCompanyStoryCommentRequest request)
        => await mediator.Send(request);
}
