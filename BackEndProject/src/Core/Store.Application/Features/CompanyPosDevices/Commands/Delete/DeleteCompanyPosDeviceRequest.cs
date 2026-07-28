using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public record DeleteCompanyPosDeviceRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyPosDeviceEncryptor))]
    public int Id { get; init; }
}