using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public record CreateCompanyPosDeviceResponse
{
    [JsonConverter(typeof(CompanyPosDeviceEncryptor))]
    public int Id { get; init; }
}