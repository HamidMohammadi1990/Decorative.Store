using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductQuestions.Commands;

public record CreateProductQuestionRequest : IRequest<OperationResult<CreateProductQuestionResponse>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string Question { get; init; } = default!;
}
