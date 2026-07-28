using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Companies.Commands;

public record CreateCompanyRequest : IRequest<OperationResult<CreateCompanyResponse>>
{
    [JsonConverter(typeof(CityEncryptor))]
    public int CityId { get; init; }
    public string CompanyName { get; init; } = default!;
    public string CompanyCode { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string? Email { get; init; }
    public string PostalCode { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string? Description { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}