using Store.Common.Models;

namespace Edition.Application.Features.Users.Queries;

public record GetForgetPasswordOptionRequest : IRequest<OperationResult<GetForgetPasswordOptionResponse>>
{
    public string UserName { get; init; } = default!;
}