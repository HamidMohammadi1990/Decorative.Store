using Store.Common.Models;

namespace Edition.Application.Features.ProductComments.Queries;

public record GetMyProductCommentsRequest : IRequest<OperationResult<GetMyProductCommentsResponse>>;
