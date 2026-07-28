using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class GetContentPolicyRuleOptionsHandler
    (IContentPolicyMetadataRepository metadataRepository, IContentPolicyMapperService mapper)
    : IRequestHandler<GetContentPolicyRuleOptionsRequest, OperationResult<GetContentPolicyRuleOptionsResponse>>
{
    public Task<OperationResult<GetContentPolicyRuleOptionsResponse>> Handle(
        GetContentPolicyRuleOptionsRequest request,
        CancellationToken cancellationToken)
        => Task.FromResult<OperationResult<GetContentPolicyRuleOptionsResponse>>(
            new GetContentPolicyRuleOptionsResponse
            {
                Operators = mapper.MapEnumOptions<ContentPolicyOperator>(),
                Effects = mapper.MapEnumOptions<ContentPolicyEffect>(),
                ValueTypes = mapper.MapEnumOptions<ContentPolicyValueType>(),
                QueryActions = mapper.MapEnumOptions<ContentPolicyQueryAction>(),
                RuleGroups = mapper.MapEnumOptions<ContentPolicyRuleGroup>(),
                MergeModes = mapper.MapEnumOptions<ContentPolicyMergeMode>(),
                ContextPaths = metadataRepository.GetContextPaths()
            });
}
