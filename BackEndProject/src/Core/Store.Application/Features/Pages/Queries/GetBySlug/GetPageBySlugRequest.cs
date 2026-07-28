using Store.Common.Models;

namespace Edition.Application.Features.Pages.Queries;

public record GetPageBySlugRequest : IRequest<OperationResult<GetPageBySlugResponse>>
{
    public string Slug { get; init; } = default!;
}
