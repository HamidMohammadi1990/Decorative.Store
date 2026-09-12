using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.Users.Queries;

public record GetMyPermissionsRequest : IRequest<OperationResult<GetMyPermissionsResponse>>;

public record GetMyPermissionsResponse
{
    public List<string> Permissions { get; init; } = [];
}
