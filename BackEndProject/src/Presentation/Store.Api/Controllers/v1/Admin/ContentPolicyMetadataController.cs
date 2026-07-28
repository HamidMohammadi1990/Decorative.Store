using MediatR;
using Asp.Versioning;
using Edition.Common.Models;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ContentPolicyMetadata.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

[ApiVersion("1")]
[ControllerName("content-policy-metadata")]
[ApiControllerCategory(ApiControllerCategory.ContentPolicy)]
[ControllerInfo(PermissionType.ManageContentPolicyMetadata, PermissionType.ManageContentPolicyMetadataGroup)]
public class ContentPolicyMetadataController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.ContentPolicy_GetEntityTypes)]
    [HttpPost("entity-types")]
    public async Task<ApiResult<GetContentPolicyEntityTypesResponse>> GetEntityTypes()
        => await mediator.Send(new GetContentPolicyEntityTypesRequest());

    [ActionInfo(PermissionType.ContentPolicy_GetEntitySchema)]
    [HttpPost("entity-schema")]
    public async Task<ApiResult<GetContentPolicyEntitySchemaResponse>> GetEntitySchema(GetContentPolicyEntitySchemaRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ContentPolicy_GetRuleOptions)]
    [HttpPost("rule-options")]
    public async Task<ApiResult<GetContentPolicyRuleOptionsResponse>> GetRuleOptions()
        => await mediator.Send(new GetContentPolicyRuleOptionsRequest());

    [ActionInfo(PermissionType.ContentPolicy_GetPropertyOperators)]
    [HttpPost("property-operators")]
    public async Task<ApiResult<GetContentPolicyPropertyOperatorsResponse>> GetPropertyOperators(
        GetContentPolicyPropertyOperatorsRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ContentPolicy_ValidateRules)]
    [HttpPost("validate-rules")]
    public async Task<ApiResult<ValidateContentPolicyRulesResponse>> ValidateRules(ValidateContentPolicyRulesRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ContentPolicy_Preview)]
    [HttpPost("preview")]
    public async Task<ApiResult<PreviewContentPolicyResponse>> Preview(PreviewContentPolicyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ContentPolicy_CompareMerge)]
    [HttpPost("compare-merge")]
    public async Task<ApiResult<CompareContentPolicyMergeResponse>> CompareMerge(CompareContentPolicyMergeRequest request)
        => await mediator.Send(request);
}
