using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ChartOfAccounts.Commands;

public record CreateChartOfAccountResponse()
{
    [JsonConverter(typeof(ChartOfAccountEncryptor))]
    public int Id { get; init; }
}