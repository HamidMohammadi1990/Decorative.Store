using Store.Domain.Enums;
using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record GetContentPolicyEntitySchemaRequest : IRequest<OperationResult<GetContentPolicyEntitySchemaResponse>>
{
    public string EntityType { get; init; }
    public string? ParentPath { get; init; }
}