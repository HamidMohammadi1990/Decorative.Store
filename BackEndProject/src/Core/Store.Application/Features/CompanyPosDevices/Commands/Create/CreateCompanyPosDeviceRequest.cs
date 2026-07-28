using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public record CreateCompanyPosDeviceRequest : IRequest<OperationResult<CreateCompanyPosDeviceResponse>>
{
    [JsonConverter(typeof(BankEncryptor))]
    public int BankId { get; init; } = default!;

    [JsonConverter(typeof(CompanyPosDeviceEncryptor))]
    public int CompanyId { get; init; } = default!;

    public string IP { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}