using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CommentTopics.Queries;
using Edition.Application.Features.CommentTopics.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Comment Topics For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("comment-topic")]
[ApiControllerCategory(ApiControllerCategory.Comments)]
[ControllerInfo(PermissionType.ManageCommentTopic, PermissionType.ManageCommentTopicGroup)]
public class CommentTopicController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCommentTopic)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCommentTopicResponse>>> GetAll(GetAllCommentTopicRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCommentTopicById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCommentTopicResponse>> Get(GetCommentTopicRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateCommentTopic)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCommentTopicResponse>> Create(CreateCommentTopicRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateCommentTopic)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCommentTopicRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteCommentTopic)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteCommentTopicRequest request)
        => await mediator.Send(request);
}
