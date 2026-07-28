using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Companies.Queries;

public record GetCompanyRequest : IRequest<OperationResult<GetCompanyResponse?>>
{
    [JsonConverter(typeof(CompanyEncryptor))]
    public int Id { get; init; }
}