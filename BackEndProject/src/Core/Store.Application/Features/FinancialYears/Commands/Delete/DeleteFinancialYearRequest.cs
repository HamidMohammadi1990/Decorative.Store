using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.FinancialYears.Commands;

public record DeleteFinancialYearRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int Id { get; init; }
}