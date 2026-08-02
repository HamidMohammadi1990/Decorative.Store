using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.FinancialYears.Queries;

public record GetFinancialYearResponse
{
    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public bool IsActive { get; init; }
}