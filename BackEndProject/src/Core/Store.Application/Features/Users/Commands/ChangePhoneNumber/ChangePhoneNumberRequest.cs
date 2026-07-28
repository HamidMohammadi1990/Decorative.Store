using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record ChangePhoneNumberRequest : IRequest<OperationResult>
{
    public string Token { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
}