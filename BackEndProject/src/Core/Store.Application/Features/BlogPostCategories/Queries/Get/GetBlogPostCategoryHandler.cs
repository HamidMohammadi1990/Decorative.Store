using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public class GetBlogPostCategoryHandler
    (IBlogPostCategoryRepository blogPostCategoryRepository,
     IBlogPostCategoryMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetBlogPostCategoryRequest, OperationResult<GetBlogPostCategoryResponse?>>
{
    public async Task<OperationResult<GetBlogPostCategoryResponse?>> Handle(GetBlogPostCategoryRequest request, CancellationToken cancellationToken)
    {
        var blogPostCategory = await blogPostCategoryRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (blogPostCategory is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var (title, slug) = TranslationResolver.Resolve(blogPostCategory.Translations, languageId, defaultLanguage.Id);

        var result = mapper.Map(blogPostCategory, title, slug);
        return result;
    }
}
