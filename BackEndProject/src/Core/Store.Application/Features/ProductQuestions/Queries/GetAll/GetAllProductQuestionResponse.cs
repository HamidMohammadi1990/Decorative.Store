using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductQuestions.Queries;

public record GetAllProductQuestionResponse
{
    [JsonConverter(typeof(ProductQuestionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = default!;
    public string Question { get; init; } = default!;
    public string? Answer { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }

    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public string UserName { get; init; } = default!;
    public string? AnsweredByFirstName { get; init; }
    public string? AnsweredByLastName { get; init; }
    public string? AnsweredByUserName { get; init; }
}
