using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Companies.Commands;

public record CreateCompanyResponse
{
    [JsonConverter(typeof(CompanyEncryptor))]
    public int Id { get; init; }
}