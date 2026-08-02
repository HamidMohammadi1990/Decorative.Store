using Store.Common.Models;

namespace Edition.Application.Features.FinancialYears.Commands;

public record CreateFinancialYearRequest : IRequest<OperationResult<CreateFinancialYearResponse>>
{
    public string Name { get; init; } = default!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
