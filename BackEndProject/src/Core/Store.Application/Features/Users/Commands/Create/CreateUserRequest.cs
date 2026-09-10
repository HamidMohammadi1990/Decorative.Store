using Store.Domain.Enums;
using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record CreateUserRequest : IRequest<OperationResult<CreateUserResponse>>
{
    public string UserName { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string? Email { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public string Password { get; init; } = default!;
    public GenderType Gender { get; init; }
    public string? ProfileImageFileName { get; init; }
}