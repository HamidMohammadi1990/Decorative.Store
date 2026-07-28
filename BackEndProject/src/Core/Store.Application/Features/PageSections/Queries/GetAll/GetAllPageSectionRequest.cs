using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.PageSections.Queries;

public record GetAllPageSectionRequest : ContentPolicyRequest<PageSection>, IRequest<OperationResult<PagedResult<GetAllPageSectionResponse>>>
{
    [JsonConverter(typeof(PageNullableEncryptor))]
    public int? PageId { get; init; }
    [JsonConverter(typeof(SectionNullableEncryptor))]
    public int? SectionId { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
