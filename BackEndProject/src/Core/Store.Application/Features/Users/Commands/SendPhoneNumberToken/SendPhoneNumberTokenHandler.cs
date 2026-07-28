using Edition.Application.Contracts;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public class SendPhoneNumberTokenHandler
    (IUserRepository userRepository, IAccountingService accountingService)
    : IRequestHandler<SendPhoneNumberTokenRequest, OperationResult<SendPhoneNumberTokenResponse>>
{
    public async Task<OperationResult<SendPhoneNumberTokenResponse>> Handle(SendPhoneNumberTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        if (string.IsNullOrEmpty(user.PhoneNumber))
            return ErrorModel.Create("ThereIsNoMobileRegisteredForYou");

        var smsSendResult = await accountingService.SendActivationCodeToPhoneAsync(user.PhoneNumber);
        if (smsSendResult.AlreadySend && smsSendResult.SuccessSent)
            return new SendPhoneNumberTokenResponse
            {
                Message = smsSendResult.Message!,
                OptionType = ForgetPasswordOptionType.Message
            };

        return new SendPhoneNumberTokenResponse
        {
            Message = ErrorModel.Create("VerificationCodeHasBeenSent"),
            OptionType = ForgetPasswordOptionType.Message
        };
    }
}