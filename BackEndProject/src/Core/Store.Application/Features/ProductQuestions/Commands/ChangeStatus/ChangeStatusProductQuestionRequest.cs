using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductQuestions.Commands;

public record ChangeStatusProductQuestionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductQuestionEncryptor))]
    public int Id { get; init; }

    public bool IsActive { get; init; }
}
