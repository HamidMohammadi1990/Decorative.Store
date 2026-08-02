using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.FinancialYears.Queries;

public record GetAllFinancialYearResponse
{
    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int Id { get; set; } = default!;

    public string Name { get; set; } = default!;
    public DateTime StartDate { get; set; } = default!;
    public DateTime EndDate { get; set; } = default!;
    public bool IsActive { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}