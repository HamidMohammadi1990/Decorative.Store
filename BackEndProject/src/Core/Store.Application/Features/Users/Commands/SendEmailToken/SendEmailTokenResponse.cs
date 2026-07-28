using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public record SendEmailTokenResponse
{
    public ErrorModel Message { get; set; } = default!;
    public ForgetPasswordOptionType OptionType { get; set; }
}