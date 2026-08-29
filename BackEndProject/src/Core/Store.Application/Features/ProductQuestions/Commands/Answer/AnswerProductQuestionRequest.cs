using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductQuestions.Commands;

public record AnswerProductQuestionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductQuestionEncryptor))]
    public int Id { get; init; }

    public string Answer { get; init; } = default!;
}
