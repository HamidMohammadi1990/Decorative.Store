using Edition.Application.Contracts;
using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public class SignOutUserHandler
    (IAccountingService accountingService)
    : IRequestHandler<SignOutUserRequest, OperationResult<bool>>
{
    public async Task<OperationResult<bool>> Handle(SignOutUserRequest request, CancellationToken cancellationToken)
    {
        var blockTokenResult = await accountingService.BlockTokenAsync(new(request.Token));
        if (!blockTokenResult.Result!.LoggedOut)
            return OperationResult<bool>.Fail();

        return true;
    }
}