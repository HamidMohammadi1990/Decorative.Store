using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ChartOfAccounts.Commands;

public record CreateChartOfAccountRequest : IRequest<OperationResult<CreateChartOfAccountResponse>>
{
    [JsonConverter(typeof(ChartOfAccountNullableEncryptor))]
    public int? ParentId { get; init; }

    public int Level { get; init; }
    public string AccountCode { get; init; } = default!;
    public string AccountTitle { get; init; } = default!;
    public ChartOfAccountType AccountType { get; init; }
    public ChartOfAccountDetailType DetailType { get; init; }
}