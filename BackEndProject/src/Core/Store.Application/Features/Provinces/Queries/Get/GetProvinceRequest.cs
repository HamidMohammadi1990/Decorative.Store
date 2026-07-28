using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Provinces.Queries;

public record GetProvinceRequest : IRequest<OperationResult<GetProvinceResponse>>
{
    [JsonConverter(typeof(ProvinceEncryptor))]
    public int Id { get; init; }
}