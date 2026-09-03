using MediatR;
using Asp.Versioning;
using Edition.Application.Features.RoomTypes.Commands;
using Edition.Application.Features.RoomTypes.Queries;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Attributes;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.WebFramework.Api;

namespace Store.Api.Controllers.v1.Admin;

[ApiVersion("1")]
[ControllerName("room-type")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageRoomType, PermissionType.ManageRoomTypeGroup)]
public class RoomTypeController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListRoomType)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllRoomTypeResponse>>> GetAll(GetAllRoomTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetRoomTypeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetRoomTypeResponse?>> Get(GetRoomTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateRoomType)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateRoomTypeResponse>> Create(CreateRoomTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateRoomType)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateRoomTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteRoomType)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteRoomTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateRoomType)]
    [HttpPost("upload-image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ApiResult<UploadRoomTypeImageResponse>> UploadImage([FromForm] IFormFile image)
        => await mediator.Send(new UploadRoomTypeImageCommand(image));
}
