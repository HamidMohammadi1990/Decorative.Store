using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductQuestions.Queries;

public record SearchProductQuestionRequest
    : ContentPolicyRequest<ProductQuestion>,
      IRequest<OperationResult<PagedResult<SearchProductQuestionResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
