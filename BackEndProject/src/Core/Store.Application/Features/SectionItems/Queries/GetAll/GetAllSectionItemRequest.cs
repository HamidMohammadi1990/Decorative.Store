using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.SectionItems.Queries;

public record GetAllSectionItemRequest : ContentPolicyRequest<SectionItem>, IRequest<OperationResult<PagedResult<GetAllSectionItemResponse>>>
{
    [JsonConverter(typeof(SectionNullableEncryptor))]
    public int? SectionId { get; init; }
    public string? Title { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
