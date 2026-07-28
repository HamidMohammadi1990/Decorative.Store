using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyComments.Commands;

public class UpdateCompanyCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int Rate { get; init; }    
}