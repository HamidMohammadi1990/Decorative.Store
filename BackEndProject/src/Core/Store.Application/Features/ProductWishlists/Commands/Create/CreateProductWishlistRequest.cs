using MediatR;
using Store.Common.Models;

namespace Edition.Application.Features.ProductWishlists.Commands;

public record CreateProductWishlistRequest : IRequest<OperationResult>
{
    public string Slug { get; init; } = default!;
}
