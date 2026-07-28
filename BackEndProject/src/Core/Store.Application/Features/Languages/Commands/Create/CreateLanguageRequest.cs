using Store.Common.Models;

namespace Edition.Application.Features.Languages.Commands;

public record CreateLanguageRequest : IRequest<OperationResult<CreateLanguageResponse>>
{
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; } = true;
    public int DisplayOrder { get; init; }
    public bool IsRtl { get; init; }
}
