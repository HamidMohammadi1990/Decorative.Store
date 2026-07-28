using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Companies.Commands;

public record DeleteCompanyRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyEncryptor))]
    public int Id { get; init; }
}