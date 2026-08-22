using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetBlogPostDetailHandler(IBlogPostRepository blogPostRepository)
    : IRequestHandler<GetBlogPostDetailRequest, OperationResult<GetBlogPostDetailResponse>>
{
    public async Task<OperationResult<GetBlogPostDetailResponse>> Handle(
        GetBlogPostDetailRequest request,
        CancellationToken cancellationToken)
    {
        var detail = await blogPostRepository.GetDetailBySlugAsync(request.Slug, cancellationToken);

        if (detail is null)
        {
            return new GetBlogPostDetailResponse { NotFound = true };
        }

        return new GetBlogPostDetailResponse
        {
            NotFound = false,
            Post = new BlogPostDetailPostResponse
            {
                Id = detail.Post.Id,
                Title = detail.Post.Title,
                Slug = detail.Post.Slug,
                CategoryTitle = detail.Post.CategoryTitle,
                CategorySlug = detail.CategorySlug,
                CategoryId = detail.Post.CategoryId,
                MetaDescription = detail.Post.MetaDescription,
                SeoKeywords = detail.Post.SeoKeywords,
                Content = detail.Post.Content,
                UserFirstName = detail.Post.UserFirstName,
                UserLastName = detail.Post.UserLastName,
                UserId = detail.Post.UserId,
                ReadingTimeInMinutes = detail.Post.ReadingTimeInMinutes,
                CreatedOnUtc = detail.Post.CreatedOnUtc,
                UpdatedOnUtc = detail.Post.UpdatedOnUtc,
                PublishedOnUtc = detail.Post.PublishedOnUtc,
                CommentCount = detail.Comments.Count,
            },
            Comments = detail.Comments
                .Select(comment => new BlogPostDetailCommentResponse
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedByUserFirstName = comment.CreatedByUserFirstName ?? string.Empty,
                    CreatedByUserLastName = comment.CreatedByUserLastName ?? string.Empty,
                    CreatedOnUtc = comment.CreatedOnUtc,
                    ApprovedOnUtc = comment.ApprovedOnUtc,
                })
                .ToList(),
            RelatedPosts = detail.RelatedPosts
                .Select(post => new BlogPostDetailRelatedResponse
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    CategoryTitle = post.CategoryTitle,
                    CategorySlug = post.CategorySlug,
                    CategoryId = post.CategoryId,
                    MetaDescription = post.MetaDescription,
                    SeoKeywords = post.SeoKeywords,
                    ReadingTimeInMinutes = post.ReadingTimeInMinutes,
                    CreatedOnUtc = post.CreatedOnUtc,
                    UpdatedOnUtc = post.UpdatedOnUtc,
                    PublishedOnUtc = post.PublishedOnUtc,
                })
                .ToList(),
            CategoryLabels = detail.CategoryLabels,
        };
    }
}
