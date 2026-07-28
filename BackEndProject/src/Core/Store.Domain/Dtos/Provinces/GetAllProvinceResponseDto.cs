namespace Store.Domain.Dtos.Provinces;

public record GetAllProvinceResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string? Description { get; init; }
    public int Rate { get; init; }
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
    public bool IsActive { get; init; }
}