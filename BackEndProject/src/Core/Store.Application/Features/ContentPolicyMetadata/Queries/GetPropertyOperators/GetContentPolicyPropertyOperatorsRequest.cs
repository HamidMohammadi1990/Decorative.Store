using Store.Domain.Enums;
using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record GetContentPolicyPropertyOperatorsRequest : IRequest<OperationResult<GetContentPolicyPropertyOperatorsResponse>>
{
    public string EntityType { get; init; }
    public string FieldPath { get; init; } = default!;
}