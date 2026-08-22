namespace Edition.Application.Features.ProductWishlists.Queries;

public record GetMyProductWishlistResponse
{
    public List<string> Slugs { get; init; } = [];
}
