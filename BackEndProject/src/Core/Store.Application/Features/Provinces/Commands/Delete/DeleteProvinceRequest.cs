using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Provinces.Commands;

public record DeleteProvinceRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProvinceEncryptor))]
    public int Id { get; init; }
}