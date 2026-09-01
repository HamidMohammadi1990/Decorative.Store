using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.AssistantFaqs.Queries;

public record GetAllAssistantFaqRequest
    : ContentPolicyRequest<AssistantFaq>, IRequest<OperationResult<PagedResult<GetAllAssistantFaqResponse>>>
{
    public int? LanguageId { get; init; }
    public string? Question { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllAssistantFaqResponse
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

public record GetAssistantFaqRequest : IRequest<OperationResult<GetAssistantFaqResponse?>>
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }
}

public record GetAssistantFaqResponse
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

public record SearchAssistantFaqRequest : IRequest<OperationResult<PagedResult<SearchAssistantFaqResponse>>>
{
    public int LanguageId { get; init; }
    public bool? IsActive { get; init; } = true;
    public PagedRequest Pagination { get; init; } = default!;
}

public record SearchAssistantFaqResponse
{
    [JsonConverter(typeof(AssistantFaqEncryptor))]
    public int Id { get; init; }

    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
