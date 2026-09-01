using Edition.Application.Common.Directories;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Queries;

public class SearchBlogPostHandler
    (IBlogPostRepository blogPostRepository,
     IBlogPostMapperService mapper,
     IBlogPostFileRepository blogPostFileRepository)
    : IRequestHandler<SearchBlogPostRequest, OperationResult<PagedResult<SearchBlogPostResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchBlogPostResponse>>> Handle(
        SearchBlogPostRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var posts = await blogPostRepository.SearchAsync(requestModel);
        var result = mapper.Map(posts);

        var coverMap = await blogPostFileRepository.GetCoverFileNamesByBlogPostIdsAsync(
            result.Items.Select(x => x.Id).ToList(),
            cancellationToken);

        var enrichedItems = result.Items
            .Select(item => item with
            {
                CoverImageUrl = coverMap.TryGetValue(item.Id, out var fileName)
                    ? BlogPostDirectory.GetImageUrl(fileName)
                    : null,
            })
            .ToList();

        return PagedResult<SearchBlogPostResponse>.Create(enrichedItems, result);
    }
}
