using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ChartOfAccounts.Commands;

public record DeleteChartOfAccountRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ChartOfAccountEncryptor))]
    public int Id { get; init; }
}