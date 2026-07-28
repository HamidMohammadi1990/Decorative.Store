using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record SendPhoneNumberTokenRequest : IRequest<OperationResult<SendPhoneNumberTokenResponse>>
{
    public string UserName { get; init; } = default!;
}