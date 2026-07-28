using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record GetContentPolicyRuleOptionsRequest : IRequest<OperationResult<GetContentPolicyRuleOptionsResponse>>;
