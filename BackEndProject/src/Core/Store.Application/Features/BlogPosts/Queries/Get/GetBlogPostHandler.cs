using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetBlogPostHandler
    (IBlogPostRepository blogPostRepository,
     IBlogPostMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetBlogPostRequest, OperationResult<GetBlogPostResponse?>>
{
    public async Task<OperationResult<GetBlogPostResponse?>> Handle(GetBlogPostRequest request, CancellationToken cancellationToken)
    {
        var blogPost = await blogPostRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (blogPost is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var fields = TranslationResolver.Resolve(blogPost.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(blogPost, fields);
        return result;
    }
}
