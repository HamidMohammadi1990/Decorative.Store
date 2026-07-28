using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostComments.Queries;
using Store.Domain.Dtos.BlogPostComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BlogPostCommentMapperService : IBlogPostCommentMapperService
{
    public PagedResult<GetAllBlogPostCommentResponse> Map(PagedResult<GetAllBlogPostCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBlogPostCommentResponse
            {
                Id = x.Id,
                Content = x.Content,
                ParentId = x.ParentId,
                IsApproved = x.IsApproved,
                BlogPostId = x.BlogPostId,
                CreatedOnUtc = x.CreatedOnUtc,
                ApprovedOnUtc = x.ApprovedOnUtc,
                BlogPostTitle = x.BlogPostTitle,
                CreatedByUserFirstName = x.CreatedByUserFirstName!,
                CreatedByUserLastName = x.CreatedByUserLastName!,
                ApprovedByUserFirstName = x.ApprovedByUserFirstName,
                ApprovedByUserLastName = x.ApprovedByUserLastName,
                CreatedByUserId = x.CreatedByUserId,
                ApprovedByUserId = x.ApprovedByUserId,
            })
            .ToList();

        return PagedResult<GetAllBlogPostCommentResponse>.Create(items, model);
    }

    public PagedResult<SearchBlogPostCommentResponse> Map(PagedResult<SearchBlogPostCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchBlogPostCommentResponse
            {
                Id = x.Id,
                Content = x.Content,
                ParentId = x.ParentId,
                BlogPostId = x.BlogPostId,
                CreatedOnUtc = x.CreatedOnUtc,
                ApprovedOnUtc = x.ApprovedOnUtc,
                CreatedByUserFirstName = x.CreatedByUserFirstName!,
                CreatedByUserLastName = x.CreatedByUserLastName!,
                CreatedByUserId = x.CreatedByUserId
            })
            .ToList();

        return PagedResult<SearchBlogPostCommentResponse>.Create(items, model);
    }

    public GetBlogPostCommentResponse Map(BlogPostComment model)
    {
        return new GetBlogPostCommentResponse
        {
            Id = model.Id,
            Content = model.Content,
            ParentId = model.ParentId,
            BlogPostId = model.BlogPostId,
            IsApproved = model.IsApproved,
            CreatedOnUtc = model.CreatedOnUtc,
            ApprovedOnUtc = model.ApprovedOnUtc,
            CreatedByUserId = model.CreatedByUserId,
            ApprovedByUserId = model.ApprovedByUserId
        };
    }

    public GetAllBlogPostCommentRequestDto Map(GetAllBlogPostCommentRequest model)
    {
        return new GetAllBlogPostCommentRequestDto
        {
            IsApproved = model.IsApproved,
            BlogPostId = model.BlogPostId,
            Pagination = model.Pagination,
            CreatedByUserId = model.CreatedByUserId,
            ApprovedByUserId = model.ApprovedByUserId
        }.WithContentPolicy<BlogPostComment, GetAllBlogPostCommentRequestDto>(model);
    }

    public SearchBlogPostCommentRequestDto Map(SearchBlogPostCommentRequest model)
    {
        return new SearchBlogPostCommentRequestDto
        {
            BlogPostId = model.BlogPostId,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPostComment, SearchBlogPostCommentRequestDto>(model);
    }
}