using Edition.Application.Contracts.Localization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.ProductComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductCommentRepository
      (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<ProductComment>(context), IProductCommentRepository
{
    public async Task<PagedResult<GetAllProductCommentResponseDto>> GetAllAsync(GetAllProductCommentRequestDto request, CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var productCommentSource = Context.ProductComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productComments =
            from productComment in productCommentSource
            join topic in Context.CommentTopic on productComment.CommentTopicId equals topic.Id
            join product in Context.Product on productComment.ProductId equals product.Id
            join user in Context.User on productComment.UserId equals user.Id
            select new { productComment, topic, product, user };

        productComments = productComments.ApplyQueryFilters(request);

        var result = await
            productComments
            .AsNoTracking()
            .Select(x => new GetAllProductCommentResponseDto
            {
                Id = x.productComment.Id,
                UserId = x.productComment.UserId,
                IsActive = x.productComment.IsActive,
                ProductId = x.productComment.ProductId,
                Description = x.productComment.Description,
                CommentRate = x.productComment.CommentRate,
                ProductTitle = x.product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                QualityRating = x.productComment.CommentRate,
                CommentTopicId = x.productComment.CommentTopicId,
                AffordableRating = x.productComment.AffordableRating,
                CommentTopicTitle = x.topic.Title
            })
            .ToPagedAsync(request.Pagination, cancellationToken);

        return result;
    }

    public async Task<PagedResult<SearchProductCommentResponseDto>> SearchAsync(SearchProductCommentRequestDto request, CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var productCommentSource = Context.ProductComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productComments =
            from productComment in productCommentSource
            join topic in Context.CommentTopic on productComment.CommentTopicId equals topic.Id
            join product in Context.Product on productComment.ProductId equals product.Id
            join user in Context.User on productComment.UserId equals user.Id
            where productComment.IsActive
            select new { productComment, topic, product, user };

        productComments = productComments.ApplyQueryFilters(request);

        var result = await
            productComments
            .AsNoTracking()
            .Select(x => new SearchProductCommentResponseDto
            {
                Id = x.productComment.Id,
                UserId = x.productComment.UserId,
                ProductId = x.productComment.ProductId,
                Description = x.productComment.Description,
                CommentRate = x.productComment.CommentRate,
                ProductTitle = x.product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                QualityRating = x.productComment.CommentRate,
                CommentTopicId = x.productComment.CommentTopicId,
                AffordableRating = x.productComment.AffordableRating,
                CommentTopicTitle = x.topic.Title
            })
            .ToPagedAsync(request.Pagination, cancellationToken);

        return result;
    }

    public async Task<List<GetMyProductCommentDto>> GetByUserIdAsync(
        int userId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken = default)
    {
        return await
            (from productComment in Context.ProductComment.AsNoTracking()
             join product in Context.Product on productComment.ProductId equals product.Id
             where productComment.UserId == userId
             orderby productComment.Id descending
             select new GetMyProductCommentDto
             {
                 Id = productComment.Id,
                 ProductId = productComment.ProductId,
                 Description = productComment.Description,
                 CommentRate = productComment.CommentRate,
                 IsActive = productComment.IsActive,
                 ProductTitle = product.Translations
                         .Where(t => t.LanguageId == languageId)
                         .Select(t => t.Title)
                         .FirstOrDefault()
                     ?? product.Translations
                         .Where(t => t.LanguageId == defaultLanguageId)
                         .Select(t => t.Title)
                         .FirstOrDefault()
                     ?? string.Empty,
                 ProductSlug = product.Translations
                         .Where(t => t.LanguageId == languageId)
                         .Select(t => t.Slug)
                         .FirstOrDefault()
                     ?? product.Translations
                         .Where(t => t.LanguageId == defaultLanguageId)
                         .Select(t => t.Slug)
                         .FirstOrDefault()
                     ?? string.Empty,
             })
            .ToListAsync(cancellationToken);
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
