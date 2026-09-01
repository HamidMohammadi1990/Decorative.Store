using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostFiles.Queries;

public record GetAllBlogPostFileRequest
    : ContentPolicyRequest<BlogPostFile>, IRequest<OperationResult<PagedResult<GetAllBlogPostFileResponse>>>
{
    public string? Title { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int? BlogPostId { get; init; }

    public bool? IsActive { get; init; }
    public bool? IsMain { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllBlogPostFileResponse
{
    [JsonConverter(typeof(BlogPostFileEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public string BlogPostTitle { get; init; } = default!;
    public string FileName { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsMain { get; init; }
}

public record GetBlogPostFileRequest : IRequest<OperationResult<GetBlogPostFileResponse?>>
{
    [JsonConverter(typeof(BlogPostFileEncryptor))]
    public int Id { get; init; }
}

public record GetBlogPostFileResponse
{
    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public string? Title { get; init; }
    public string FileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
}
