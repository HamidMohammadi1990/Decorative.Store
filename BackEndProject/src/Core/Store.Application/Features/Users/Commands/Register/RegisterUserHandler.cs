using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Infrastructure;
using Store.Common.Models;
using Store.Common.Extensions;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Users.Commands;

public class RegisterUserHandler
    : IRequestHandler<RegisterUserRequest, OperationResult>
{
    private readonly IUnitOfWork uow;
    private readonly ISmsService smsService;
    private readonly IEmailService emailService;
    private readonly IUserRepository userRepository;

    public RegisterUserHandler(
        IUnitOfWork uow,
        ISmsService smsService,
        IEmailService emailService,
        IUserRepository userRepository)
    {
        this.uow = uow;
        this.smsService = smsService;
        this.emailService = emailService;
        this.userRepository = userRepository;
    }

    public async Task<OperationResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var isExistsUser = await userRepository.VerifyRepeatPhoneAsync(request.UserName);
        if (isExistsUser)
            return ErrorModel.Create("UserNameIsNotValid");

        var isMobile = request.UserName.IsMobile();
        var verifyTokenResult = isMobile
            ? await smsService.VerifyTokenAsync(request.Token, request.UserName)
            : await emailService.VerifyTokenAsync(request.Token, request.UserName);

        if (!verifyTokenResult)
            return ErrorModel.Create("VerificationCodeIsNotValid");

        var user = User.Create(request.UserName, isMobile ? null : request.UserName, isMobile ? request.UserName : null);
        user.EnsureSecurityStamp();

        if (isMobile)
            user.ConfirmPhoneNumber();

        if (!isMobile)
            user.ConfirmEmail();

        userRepository.Add(user);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}