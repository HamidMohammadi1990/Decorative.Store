using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProfileCompletion.Commands;
using Edition.Application.Features.ProfileCompletion.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management profile completion questions and campaign settings for admin
/// </summary>
[ApiVersion("1")]
[ControllerName("profile-completion")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageProfileCompletion, PermissionType.ManageProfileCompletionGroup)]
public class ProfileCompletionController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.GetProfileCompletionConfig)]
    [HttpPost("get-config")]
    public async Task<ApiResult<GetProfileCompletionConfigResponse>> GetConfig()
        => await mediator.Send(new GetProfileCompletionConfigRequest());

    [ActionInfo(PermissionType.UpdateProfileCompletionConfig)]
    [HttpPut("update-config")]
    public async Task<ApiResult<OperationResult>> UpdateConfig(UpdateProfileCompletionConfigRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ListProfileCompletionUserState)]
    [HttpPost("user-states/get-all")]
    public async Task<ApiResult<PagedResult<GetAllProfileCompletionUserStateResponse>>> GetUserStates(
        GetAllProfileCompletionUserStateRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProfileCompletionUserState)]
    [HttpPost("user-states/get")]
    public async Task<ApiResult<GetProfileCompletionUserStateResponse?>> GetUserState(
        GetProfileCompletionUserStateRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProfileCompletionUserState)]
    [HttpPut("user-states/update")]
    public async Task<ApiResult<OperationResult>> UpdateUserState(UpdateProfileCompletionUserStateRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProfileCompletionUserState)]
    [HttpDelete("user-states/delete")]
    public async Task<ApiResult<OperationResult>> DeleteUserState(DeleteProfileCompletionUserStateRequest request)
        => await mediator.Send(request);
}
