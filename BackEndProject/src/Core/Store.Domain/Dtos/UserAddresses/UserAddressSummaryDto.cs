namespace Store.Domain.Dtos.UserAddresses;

public record UserAddressSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;
}