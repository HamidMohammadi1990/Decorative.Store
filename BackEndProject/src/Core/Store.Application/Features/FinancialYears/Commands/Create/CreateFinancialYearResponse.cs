using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.FinancialYears.Commands;

public record CreateFinancialYearResponse
{
    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int Id { get; init; }
}