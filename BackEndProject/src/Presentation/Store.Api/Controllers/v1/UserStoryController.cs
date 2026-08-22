using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.UserStories.Queries;
using Edition.Application.Features.UserStories.Commands;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;

namespace Store.Api.Controllers.v1;

/// <summary>
/// User stories management
/// </summary>
[ApiVersion("1")]
[ControllerName("user-story")]
[ApiControllerCategory(ApiControllerCategory.Users)]
public class UserStoryController(ISender mediator) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("search-active")]
    public async Task<ApiResult<SearchActiveUserStoriesResponse>> SearchActive(SearchActiveUserStoriesRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpGet("my")]
    public async Task<ApiResult<GetMyUserStoriesResponse>> My()
        => await mediator.Send(new GetMyUserStoriesRequest());

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateUserStoryResponse>> Create(CreateUserStoryRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateUserStoryRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteUserStoryRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("upload-media")]
    [RequestSizeLimit(8_388_608)]
    public async Task<ApiResult<UploadUserStoryMediaResponse>> UploadMedia([FromForm] IFormFile file)
        => await mediator.Send(new UploadUserStoryMediaRequest(file));
}
