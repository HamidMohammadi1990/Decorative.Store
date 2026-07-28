using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Infrastructure;
using Store.Common.Models;
using Store.Common.Extensions;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public class ChangePhoneNumberHandler
    : IRequestHandler<ChangePhoneNumberRequest, OperationResult>
{
    private readonly IUnitOfWork uow;
    private readonly ISmsService smsService;
    private readonly IUserRepository userRepository;
    private readonly ICurrentUserContext currentUser;

    public ChangePhoneNumberHandler(
        IUnitOfWork uow,
        ISmsService smsService,
        IUserRepository userRepository,
        ICurrentUserContext currentUser)
    {
        this.uow = uow;
        this.smsService = smsService;
        this.userRepository = userRepository;
        this.currentUser = currentUser;
    }

    public async Task<OperationResult> Handle(ChangePhoneNumberRequest request, CancellationToken cancellationToken)
    {
        var existOtherUser = await userRepository.VerifyRepeatPhoneAsync(request.PhoneNumber);
        if (existOtherUser)
            return ErrorModel.Create("MobileIsUsedByAnotherUser");

        var isConfirm = await smsService.VerifyTokenAsync(request.Token, request.PhoneNumber);
        if (!isConfirm)
            return ErrorModel.Create("ActivationCodeIsNotCorrect");

        var userId = currentUser.UserId;
        var user = await userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        user.UpdatePhoneNumber(request.PhoneNumber);
        user.ConfirmPhoneNumber();

        if (user.UserName.IsMobile())
            user.UpdateUserName(request.PhoneNumber);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}