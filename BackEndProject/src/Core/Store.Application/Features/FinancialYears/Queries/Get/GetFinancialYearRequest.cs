using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.FinancialYears.Queries;

public record GetFinancialYearRequest : IRequest<OperationResult<GetFinancialYearResponse?>>
{
    [JsonConverter(typeof(FinancialYearEncryptor))]
    public int Id { get; init; }
}