using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.BlogPostComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostCommentRepository
    (EditionDbContext context)
    : Repository<BlogPostComment>(context), IBlogPostCommentRepository
{
    public async Task<PagedResult<GetAllBlogPostCommentResponseDto>> GetAllAsync(GetAllBlogPostCommentRequestDto request)
    {
        var blogPostCommentSource = Context.BlogPostComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var comments =
                   from blogPostComment in blogPostCommentSource
                   join blogPost in Context.BlogPost on blogPostComment.BlogPostId equals blogPost.Id
                   join createdByUser in Context.User on blogPostComment.CreatedByUserId equals createdByUser.Id
                   join approvedByUser in Context.User on blogPostComment.ApprovedByUserId equals approvedByUser.Id into joinApprovedByUser
                   from approvedByUser in joinApprovedByUser.DefaultIfEmpty()
                   select new { blogPostComment, blogPost, createdByUser, approvedByUser };

        comments = comments.ApplyQueryFilters(request);

        var result = await
            comments
            .Select(x => new GetAllBlogPostCommentResponseDto
            {
                Id = x.blogPostComment.Id,
                Content = x.blogPostComment.Content,
                ParentId = x.blogPostComment.ParentId,
                IsApproved = x.blogPostComment.IsApproved,
                BlogPostId = x.blogPostComment.BlogPostId,
                CreatedOnUtc = x.blogPostComment.CreatedOnUtc,
                ApprovedOnUtc = x.blogPostComment.ApprovedOnUtc,
                BlogPostTitle = x.blogPost.Translations.Select(t => t.Title).FirstOrDefault() ?? string.Empty,
                CreatedByUserFirstName = x.createdByUser.FirstName,
                CreatedByUserLastName = x.createdByUser.LastName,
                ApprovedByUserFirstName = x.approvedByUser.FirstName,
                ApprovedByUserLastName = x.approvedByUser.LastName,
                CreatedByUserId = x.blogPostComment.CreatedByUserId,
                ApprovedByUserId = x.blogPostComment.ApprovedByUserId,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchBlogPostCommentResponseDto>> SearchAsync(SearchBlogPostCommentRequestDto request)
    {
        var blogPostCommentSource = Context.BlogPostComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var comments =
                   from blogPostComment in blogPostCommentSource
                   join createdByUser in Context.User on blogPostComment.CreatedByUserId equals createdByUser.Id
                   where blogPostComment.IsApproved
                   select new { blogPostComment, createdByUser };

        comments = comments.ApplyQueryFilters(request);

        var result = await
            comments
            .Select(x => new SearchBlogPostCommentResponseDto
            {
                Id = x.blogPostComment.Id,
                Content = x.blogPostComment.Content,
                ParentId = x.blogPostComment.ParentId,
                BlogPostId = x.blogPostComment.BlogPostId,
                CreatedOnUtc = x.blogPostComment.CreatedOnUtc,
                ApprovedOnUtc = x.blogPostComment.ApprovedOnUtc,
                CreatedByUserFirstName = x.createdByUser.FirstName,
                CreatedByUserLastName = x.createdByUser.LastName,
                CreatedByUserId = x.blogPostComment.CreatedByUserId
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}