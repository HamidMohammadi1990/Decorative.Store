using MediatR;
using Asp.Versioning;
using Edition.Application.Features.RoomTypes.Queries;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Attributes;
using Store.Common.Enums;
using Store.WebFramework.Api;

namespace Store.Api.Controllers.v1;

[ApiVersion("1")]
[ControllerName("room-type")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
public class RoomTypeStoreController(ISender mediator) : BaseApiController
{
    [HttpPost("list")]
    public async Task<ApiResult<List<ListRoomTypeItemResponse>>> List(ListRoomTypesRequest request)
        => await mediator.Send(request);
}
