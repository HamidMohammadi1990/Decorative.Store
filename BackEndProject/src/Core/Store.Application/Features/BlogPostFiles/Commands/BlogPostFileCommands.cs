using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Microsoft.AspNetCore.Http;
using Store.Common.Models;

namespace Edition.Application.Features.BlogPostFiles.Commands;

public record CreateBlogPostFileRequest : IRequest<OperationResult<List<CreateBlogPostFileResponse>>>
{
    public List<BlogPostFileUploadRequest> Files { get; init; } = default!;
}

public record BlogPostFileUploadRequest
{
    public string BlogPostId { get; init; } = default!;
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public IFormFile Image { get; init; } = default!;
    public bool IsIndex { get; init; }
}

public record CreateBlogPostFileResponse
{
    [JsonConverter(typeof(BlogPostFileEncryptor))]
    public int Id { get; init; }

    public string? Title { get; init; }
    public string ImageUrl { get; init; } = default!;
}

public record DeleteBlogPostFileRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostFileEncryptor))]
    public int Id { get; init; }
}

public record UpdateStatusBlogPostFileRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostFileEncryptor))]
    public int Id { get; init; }
    public bool Status { get; init; }
}

public record UpdateSetMainBlogPostFileRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BlogPostFileEncryptor))]
    public int Id { get; init; }
}
