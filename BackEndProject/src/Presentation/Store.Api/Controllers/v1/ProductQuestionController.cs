using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.ProductQuestions.Queries;
using Edition.Application.Features.ProductQuestions.Commands;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Product questions
/// </summary>
[ApiVersion("1")]
[ControllerName("product-question")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductQuestionController(ISender mediator) : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductQuestionResponse>>> Search(SearchProductQuestionRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductQuestionResponse>> Create(CreateProductQuestionRequest request)
        => await mediator.Send(request);
}
