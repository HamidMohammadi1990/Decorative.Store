using Edition.Application.Common.Directories;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostFiles.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostFiles;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BlogPostFileMapperService : IBlogPostFileMapperService
{
    public GetBlogPostFileResponse Map(BlogPostFile model, string title)
    {
        return new GetBlogPostFileResponse
        {
            Title = title,
            ImageUrl = BlogPostDirectory.GetImageUrl(model.FileName),
            FileName = model.FileName,
            BlogPostId = model.BlogPostId,
        };
    }

    public GetAllBlogPostFileRequestDto Map(GetAllBlogPostFileRequest model)
    {
        return new GetAllBlogPostFileRequestDto
        {
            Title = model.Title,
            IsMain = model.IsMain,
            IsActive = model.IsActive,
            BlogPostId = model.BlogPostId,
            Pagination = model.Pagination,
        }.WithContentPolicy<BlogPostFile, GetAllBlogPostFileRequestDto>(model);
    }

    public PagedResult<GetAllBlogPostFileResponse> Map(PagedResult<GetAllBlogPostFileResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBlogPostFileResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsMain = x.IsMain,
                IsActive = x.IsActive,
                FileName = x.FileName,
                BlogPostId = x.BlogPostId,
                BlogPostTitle = x.BlogPostTitle,
            })
            .ToList();

        return PagedResult<GetAllBlogPostFileResponse>.Create(items, model);
    }
}
