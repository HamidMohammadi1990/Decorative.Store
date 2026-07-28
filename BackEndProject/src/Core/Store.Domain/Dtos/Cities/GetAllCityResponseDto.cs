namespace Store.Domain.Dtos.Cities;

public record GetAllCityResponseDto
{
    public int Id { get; init; }
    public int ProvinceId { get; init; }
    public string ProvinceName { get; init; } = default!;
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? Description { get; init; }
    public int Rate { get; init; }    
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
    public bool IsActive { get; init; }
}