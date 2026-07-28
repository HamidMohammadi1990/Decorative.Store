using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ChartOfAccounts.Queries;

public record GetChartOfAccountRequest : IRequest<OperationResult<GetChartOfAccountResponse?>>
{
    [JsonConverter(typeof(ChartOfAccountEncryptor))]
    public int Id { get; init; }
}