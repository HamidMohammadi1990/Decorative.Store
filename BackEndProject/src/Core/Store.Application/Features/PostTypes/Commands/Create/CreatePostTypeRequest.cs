using Store.Common.Models;

namespace Edition.Application.Features.PostTypes.Commands;

public record CreatePostTypeRequest : IRequest<OperationResult<CreatePostTypeResponse>>
{
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public int Priority { get; init; }
}