using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public record ForgetPasswordRequest : IRequest<OperationResult<ForgetPasswordResponse>>
{
    public string UserName { get; init; } = default!;
    public ForgetPasswordOptionType OptionType { get; init; }
}