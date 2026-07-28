using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public record GetCompanyPosDeviceRequest : IRequest<OperationResult<GetCompanyPosDeviceResponse?>>
{
    [JsonConverter(typeof(CompanyPosDeviceEncryptor))]
    public int Id { get; init; }
}