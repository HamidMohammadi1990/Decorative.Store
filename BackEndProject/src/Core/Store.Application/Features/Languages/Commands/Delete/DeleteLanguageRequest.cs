using Store.Common.Models;

namespace Edition.Application.Features.Languages.Commands;

public record DeleteLanguageRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
}
