using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductFiles.Queries;

public record GetAllProductFileRequest : ContentPolicyRequest<ProductFile>, IRequest<OperationResult<PagedResult<GetAllProductFileResponse>>>
{
    public string? Title { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int? ProductId { get; init; }

    public bool? IsActive { get; init; }
    public bool? IsMain { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}