using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostComments.Queries;

public class GetBlogPostCommentHandler
    (IBlogPostCommentRepository blogPostCommentRepository, IBlogPostCommentMapperService mapper)
    : IRequestHandler<GetBlogPostCommentRequest, OperationResult<GetBlogPostCommentResponse?>>
{
    public async Task<OperationResult<GetBlogPostCommentResponse?>> Handle(GetBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        var city = await blogPostCommentRepository.GetAsNoTrackingAsync(request.Id);
        if (city is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(city);
        return result;
    }
}