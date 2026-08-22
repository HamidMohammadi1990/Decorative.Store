using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.UserStories.Commands;

public record DeleteUserStoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int Id { get; init; }
}
