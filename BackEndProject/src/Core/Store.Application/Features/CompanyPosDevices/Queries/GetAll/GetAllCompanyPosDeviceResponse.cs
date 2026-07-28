using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public record GetAllCompanyPosDeviceResponse
{
    [JsonConverter(typeof(CompanyPosDeviceEncryptor))]
    public int Id { get; init; } = default!;

    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; } = default!;

    public string CompanyName { get; init; } = default!;

    [JsonConverter(typeof(BankEncryptor))]
    public int BankId { get; init; } = default!;

    public string BankName { get; init; } = default!;
    public string IP { get; init; } = default!;
    public DateTime CreationOnUtc { get; init; }
}