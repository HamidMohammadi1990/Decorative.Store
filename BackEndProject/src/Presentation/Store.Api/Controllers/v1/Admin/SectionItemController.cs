using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.SectionItems.Queries;
using Edition.Application.Features.SectionItems.Commands;
using Edition.Application.Features.Sections.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management SectionItems For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("section-item")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageSectionItem, PermissionType.ManageCmsGroup)]
public class SectionItemController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListSectionItem)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllSectionItemResponse>>> GetAll(GetAllSectionItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetSectionItemById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetSectionItemResponse?>> Get(GetSectionItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateSectionItem)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateSectionItemResponse>> Create(CreateSectionItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateSectionItem)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateSectionItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteSectionItem)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteSectionItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateSectionItem)]
    [HttpPost("upload-image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ApiResult<UploadCmsImageResponse>> UploadImage([FromForm] IFormFile image)
        => await mediator.Send(new UploadCmsImageCommand(image));
}
