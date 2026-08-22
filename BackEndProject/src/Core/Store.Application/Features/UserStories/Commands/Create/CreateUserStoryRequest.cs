using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using MediatR;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.UserStories.Commands;

public record CreateUserStoryRequest : IRequest<OperationResult<CreateUserStoryResponse>>
{
    public string Title { get; init; } = default!;
    public string? Caption { get; init; }
    public StoryMediaType MediaType { get; init; }
    public string MediaPath { get; init; } = default!;
    public string MediaAlt { get; init; } = default!;
    public string? PosterPath { get; init; }
    public string? ProductSlug { get; init; }
}

public record CreateUserStoryResponse
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int Id { get; init; }
}
