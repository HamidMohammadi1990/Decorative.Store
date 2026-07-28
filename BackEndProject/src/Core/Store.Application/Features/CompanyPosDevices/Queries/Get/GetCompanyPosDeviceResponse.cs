using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public record GetCompanyPosDeviceResponse
{
    [JsonConverter(typeof(CompanyPosDeviceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(BankEncryptor))]
    public int BankId { get; init; } = default!;

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; } = default!;

    public string IP { get; init; } = default!;
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
    public string? Description { get; init; }
    public DateTime CreationOnUtc { get; init; }
}