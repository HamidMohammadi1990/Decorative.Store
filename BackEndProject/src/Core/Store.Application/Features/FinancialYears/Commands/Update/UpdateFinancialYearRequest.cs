using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.FinancialYears.Commands;

public record UpdateFinancialYearRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string Name { get; init; } = default!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public bool IsActive { get; init; }
}