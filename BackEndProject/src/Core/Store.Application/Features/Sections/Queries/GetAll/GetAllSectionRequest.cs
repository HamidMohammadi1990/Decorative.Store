using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Sections.Queries;

public record GetAllSectionRequest : ContentPolicyRequest<Section>, IRequest<OperationResult<PagedResult<GetAllSectionResponse>>>
{
    [JsonConverter(typeof(SectionTypeNullableEncryptor))]
    public int? SectionTypeId { get; init; }
    [JsonConverter(typeof(SectionNullableEncryptor))]
    public int? ParentId { get; init; }
    public string? Title { get; init; }
    public string? Url { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
