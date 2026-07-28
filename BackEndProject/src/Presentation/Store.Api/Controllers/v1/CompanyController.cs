using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.Companies.Queries;
using Edition.Application.Features.Companies.Commands;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Companies
/// </summary>
[ApiVersion("1")]
[ControllerName("company")]
[ApiControllerCategory(ApiControllerCategory.Company)]
public class CompanyController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchCompanyResponse>>> Search(SearchCompanyRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCompanyResponse>> Create(CreateCompanyRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCompanyRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteCompanyRequest request)
        => await mediator.Send(request);
}