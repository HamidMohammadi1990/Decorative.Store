namespace Store.Domain.Dtos.CompanyPosDevices;

public record GetAllCompanyPosDeviceDto
{
    public int Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
    public int CompanyId { get; init; } = default!;
    public string CompanyName { get; init; } = default!;
    public int BankId { get; init; } = default!;
    public string BankName { get; init; } = default!;
    public string IP { get; init; } = default!;
    public DateTime CreationOnUtc { get; init; }
}