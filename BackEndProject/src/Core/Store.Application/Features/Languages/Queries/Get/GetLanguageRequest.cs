using Store.Common.Models;

namespace Edition.Application.Features.Languages.Queries;

public record GetLanguageRequest : IRequest<OperationResult<GetLanguageResponse?>>
{
    public int Id { get; init; }
}
