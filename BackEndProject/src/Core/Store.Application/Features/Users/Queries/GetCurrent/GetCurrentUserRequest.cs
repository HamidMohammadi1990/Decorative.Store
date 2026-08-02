using Store.Common.Models;

namespace Edition.Application.Features.Users.Queries;

public record GetCurrentUserRequest : IRequest<OperationResult<GetUserResponse?>>;
