using Store.Common.Models;

namespace Edition.Application.Features.Users.Queries;

public record UserNameCheckRequest : IRequest<OperationResult<UserNameCheckResponse>>
{
    public string UserName { get; init; } = default!;
}