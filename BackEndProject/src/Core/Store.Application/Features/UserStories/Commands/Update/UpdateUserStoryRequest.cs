using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.UserStories.Commands;

public record UpdateUserStoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string? Caption { get; init; }
    public bool IsActive { get; init; }
    public string? ProductSlug { get; init; }
}
