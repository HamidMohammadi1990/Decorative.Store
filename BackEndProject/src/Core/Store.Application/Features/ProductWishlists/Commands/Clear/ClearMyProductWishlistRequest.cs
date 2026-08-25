using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.ProductWishlists.Commands;

public record ClearMyProductWishlistRequest : IRequest<OperationResult>;
