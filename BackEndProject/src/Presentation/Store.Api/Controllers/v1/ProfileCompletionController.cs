using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProfileCompletion.Commands;
using Edition.Application.Features.ProfileCompletion.Queries;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Models;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

[ApiVersion("1")]
[ControllerName("profile-completion")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class ProfileCompletionController(ISender mediator) : BaseApiController
{
    [HttpPost("config")]
    public async Task<ApiResult<GetProfileCompletionConfigResponse>> GetConfig()
        => await mediator.Send(new GetProfileCompletionConfigRequest());

    [HttpPost("my-state")]
    public async Task<ApiResult<GetMyProfileCompletionStateResponse>> GetMyState()
        => await mediator.Send(new GetMyProfileCompletionStateRequest());

    [HttpPut("my-answers")]
    public async Task<ApiResult<OperationResult>> SaveMyAnswers(SaveProfileCompletionAnswersRequest request)
        => await mediator.Send(request);

    [HttpPost("claim-reward")]
    public async Task<ApiResult<ClaimProfileCompletionRewardResponse>> ClaimReward()
        => await mediator.Send(new ClaimProfileCompletionRewardRequest());
}
