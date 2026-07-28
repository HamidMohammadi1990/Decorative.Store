using Store.Common.Models;

namespace Edition.Application.Features.Tags.Commands;

public record CreateTagRequest : IRequest<OperationResult<CreateTagResponse>>
{
    public string Title { get; init; } = default!;
}