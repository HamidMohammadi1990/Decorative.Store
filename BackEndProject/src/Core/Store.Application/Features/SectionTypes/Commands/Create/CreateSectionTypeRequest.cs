using Store.Common.Models;

namespace Edition.Application.Features.SectionTypes.Commands;

public record CreateSectionTypeRequest : IRequest<OperationResult<CreateSectionTypeResponse>>
{
    public int LanguageId { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
}
