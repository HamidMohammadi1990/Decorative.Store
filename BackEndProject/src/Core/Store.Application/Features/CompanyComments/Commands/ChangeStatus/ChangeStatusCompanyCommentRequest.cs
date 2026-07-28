using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.CompanyComments.Commands;

public record ChangeStatusCompanyCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }

    public CommentStatusType Status { get; init; }
}