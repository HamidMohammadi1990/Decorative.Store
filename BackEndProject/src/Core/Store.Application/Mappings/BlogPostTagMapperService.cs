using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostTags.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostTags;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BlogPostTagMapperService : IBlogPostTagMapperService
{
    public PagedResult<GetAllBlogPostTagResponse> Map(PagedResult<GetAllBlogPostTagDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBlogPostTagResponse
            {
                Id = x.Id,
                TagId = x.TagId,
                TagTitle = x.TagTitle,
                BlogPostId = x.BlogPostId,
                BlogPostTitle = x.BlogPostTitle
            })
            .ToList();

        return PagedResult<GetAllBlogPostTagResponse>.Create(items, model);
    }

    public PagedResult<SearchBlogPostTagResponse> Map(PagedResult<SearchBlogPostTagDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchBlogPostTagResponse
            {
                Id = x.Id,
                TagId = x.TagId,
                TagTitle = x.TagTitle,
                BlogPostId = x.BlogPostId,
                BlogPostTitle = x.BlogPostTitle
            })
            .ToList();

        return PagedResult<SearchBlogPostTagResponse>.Create(items, model);
    }

    public GetBlogPostTagResponse Map(BlogPostTag model)
    {
        return new GetBlogPostTagResponse
        {
            Id = model.Id,
            TagId = model.TagId,
            BlogPostId = model.BlogPostId
        };
    }

    public SearchBlogPostTagRequestDto Map(SearchBlogPostTagRequest model)
    {
        return new SearchBlogPostTagRequestDto
        {
            TagId = model.TagId,
            TagTitle = model.TagTitle,
            BlogPostId = model.BlogPostId,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPostTag, SearchBlogPostTagRequestDto>(model);
    }

    public GetAllBlogPostTagRequestDto Map(GetAllBlogPostTagRequest model)
    {
        return new GetAllBlogPostTagRequestDto
        {
            TagId = model.TagId,
            TagTitle = model.TagTitle,
            BlogPostId = model.BlogPostId,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPostTag, GetAllBlogPostTagRequestDto>(model);
    }
}