using Store.Domain.Enums;

namespace Store.Domain.Dtos.Users;

public record GetAllUserDto
{
    public int Id { get; init; }
    public string UserName { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public bool EmailConfirmed { get; init; }
    public string? PhoneNumber { get; init; }
    public bool PhoneNumberConfirmed { get; init; }
    public bool LoginPermission { get; init; }
    public GenderType? Gender { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDateOnUtc { get; init; }
    public int AccessFailedCount { get; init; }
    public RefundMethodType RefundMethod { get; init; }    
    public int? CityId { get; init; }
    public string? CityName { get; set; }
    public string? EconomicCode { get; init; }
}