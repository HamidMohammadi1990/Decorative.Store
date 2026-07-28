using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record SendEmailTokenRequest : IRequest<OperationResult<SendEmailTokenResponse>>
{
    public string UserName { get; set; } = default!;
}