using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record PreviewContentPolicyRequest : IRequest<OperationResult<PreviewContentPolicyResponse>>
{
    public int UserId { get; init; }
    public string EntityType { get; init; }
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.GetAll;
    public int SampleSize { get; init; } = 10;
}
