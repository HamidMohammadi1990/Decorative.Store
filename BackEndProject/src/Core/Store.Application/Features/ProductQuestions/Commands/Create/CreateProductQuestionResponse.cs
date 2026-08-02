using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductQuestions.Commands;

public record CreateProductQuestionResponse
{
    [JsonConverter(typeof(ProductQuestionEncryptor))]
    public int Id { get; init; }
}
