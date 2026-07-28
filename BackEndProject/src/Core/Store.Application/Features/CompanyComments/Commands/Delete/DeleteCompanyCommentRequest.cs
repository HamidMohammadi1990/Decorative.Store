using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyComments.Commands;

public class DeleteCompanyCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }
}