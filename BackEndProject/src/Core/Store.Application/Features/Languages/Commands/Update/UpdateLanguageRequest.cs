using Store.Common.Models;

namespace Edition.Application.Features.Languages.Commands;

public record UpdateLanguageRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsRtl { get; init; }
}
