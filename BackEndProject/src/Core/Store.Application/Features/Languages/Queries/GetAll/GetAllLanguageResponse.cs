namespace Edition.Application.Features.Languages.Queries;

public record GetAllLanguageResponse
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsDefault { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsRtl { get; init; }
}
