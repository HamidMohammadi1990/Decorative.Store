using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Banks.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Banks
/// </summary>
[ApiVersion("1")]
[ControllerName("bank")]
[ApiControllerCategory(ApiControllerCategory.Financial)]
public class BankController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchBankResponse>>> Search(SearchBankRequest request)
        => await mediator.Send(request);
}
