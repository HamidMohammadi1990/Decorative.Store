using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.AssistantFaqs.Commands;

public record CreateAssistantFaqRequest : IRequest<OperationResult<CreateAssistantFaqResponse>>
{
    public int LanguageId { get; init; }
    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
}

public record CreateAssistantFaqResponse
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }
}

public record UpdateAssistantFaqRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

public record DeleteAssistantFaqRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }
}
