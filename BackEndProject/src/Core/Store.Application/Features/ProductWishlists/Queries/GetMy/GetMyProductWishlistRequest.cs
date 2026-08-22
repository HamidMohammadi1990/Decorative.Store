using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.ProductWishlists.Queries;

public record GetMyProductWishlistRequest : IRequest<OperationResult<GetMyProductWishlistResponse>>;
