using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Sections.Queries;
using Edition.Application.Features.Sections.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Sections For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("section")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageSection, PermissionType.ManageCmsGroup)]
public class SectionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListSection)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllSectionResponse>>> GetAll(GetAllSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetSectionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetSectionResponse?>> Get(GetSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateSection)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateSectionResponse>> Create(CreateSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateSection)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteSection)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteSectionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateSection)]
    [HttpPost("upload-image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ApiResult<UploadCmsImageResponse>> UploadImage([FromForm] IFormFile image)
        => await mediator.Send(new UploadCmsImageCommand(image));
}
