using Store.Common.Models;

namespace Edition.Application.Features.Provinces.Commands;

public record CreateProvinceRequest : IRequest<OperationResult<CreateProvinceResponse>>
{
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string? Description { get; init; }
    public int Rate { get; init; }    
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
    public string? TelPrefix { get; set; }
}