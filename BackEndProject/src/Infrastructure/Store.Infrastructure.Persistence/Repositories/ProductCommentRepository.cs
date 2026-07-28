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
      (EditionDbContext context)
    : Repository<ProductComment>(context), IProductCommentRepository
{    
    public async Task<PagedResult<GetAllProductCommentResponseDto>> GetAllAsync(GetAllProductCommentRequestDto request, CancellationToken cancellationToken = default)
    {
        var productCommentSource = Context.ProductComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productComments =
            from productComment in productCommentSource
            join topic in Context.CommentTopic on productComment.CommentTopicId equals topic.Id
            join company in Context.Company on productComment.CompanyId equals company.Id
            join product in Context.Product on productComment.ProductId equals product.Id
            join user in Context.User on productComment.UserId equals user.Id
            select new { productComment, topic, company, product, user };

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
                CompanyId = x.productComment.CompanyId,
                Description = x.productComment.Description,
                CommentRate = x.productComment.CommentRate,
                ProductTitle = x.product.Title,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                QualityRating = x.productComment.CommentRate,
                CommentTopicId = x.productComment.CommentTopicId,
                AffordableRating = x.productComment.AffordableRating,
                CommentTopicTitle = x.topic.Title,
                CompanyName = x.company.Name
            })
            .ToPagedAsync(request.Pagination, cancellationToken);

        return result;
    }

    public async Task<PagedResult<SearchProductCommentResponseDto>> SearchAsync(SearchProductCommentRequestDto request, CancellationToken cancellationToken = default)
    {
        var productCommentSource = Context.ProductComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productComments =
            from productComment in productCommentSource
            join topic in Context.CommentTopic on productComment.CommentTopicId equals topic.Id
            join company in Context.Company on productComment.CompanyId equals company.Id
            join product in Context.Product on productComment.ProductId equals product.Id
            join user in Context.User on productComment.UserId equals user.Id
            where productComment.IsActive
            select new { productComment, topic, company, product, user };

        productComments = productComments.ApplyQueryFilters(request);

        var result = await
            productComments
            .AsNoTracking()
            .Select(x => new SearchProductCommentResponseDto
            {
                Id = x.productComment.Id,
                UserId = x.productComment.UserId,                
                ProductId = x.productComment.ProductId,
                CompanyId = x.productComment.CompanyId,
                Description = x.productComment.Description,
                CommentRate = x.productComment.CommentRate,
                ProductTitle = x.product.Title,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                QualityRating = x.productComment.CommentRate,
                CommentTopicId = x.productComment.CommentTopicId,
                AffordableRating = x.productComment.AffordableRating,
                CommentTopicTitle = x.topic.Title,
                CompanyName = x.company.Name
            })
            .ToPagedAsync(request.Pagination, cancellationToken);

        return result;
    }
}