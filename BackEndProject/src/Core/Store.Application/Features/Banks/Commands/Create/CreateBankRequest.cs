using Store.Common.Models;

namespace Edition.Application.Features.Banks.Commands;

public record CreateBankRequest : IRequest<OperationResult<CreateBankResponse>>
{
    public string Title { get; init; } = default!;
    public string Icon { get; init; } = default!;
    public bool IsActive { get; init; }
}
