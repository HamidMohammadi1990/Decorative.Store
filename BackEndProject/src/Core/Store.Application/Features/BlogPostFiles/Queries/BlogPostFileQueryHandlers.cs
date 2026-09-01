using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostFiles.Queries;

public class GetAllBlogPostFileHandler
    (IBlogPostFileRepository blogPostFileRepository, IBlogPostFileMapperService mapper)
    : IRequestHandler<GetAllBlogPostFileRequest, OperationResult<PagedResult<GetAllBlogPostFileResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBlogPostFileResponse>>> Handle(
        GetAllBlogPostFileRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var blogPostFiles = await blogPostFileRepository.GetAllAsync(requestModel, cancellationToken);
        return mapper.Map(blogPostFiles);
    }
}

public class GetBlogPostFileHandler
    (IBlogPostFileRepository blogPostFileRepository,
     IBlogPostFileMapperService mapper,
     ICurrentLanguageContext languageContext,
     ILanguageRegistry languageRegistry)
    : IRequestHandler<GetBlogPostFileRequest, OperationResult<GetBlogPostFileResponse?>>
{
    public async Task<OperationResult<GetBlogPostFileResponse?>> Handle(
        GetBlogPostFileRequest request,
        CancellationToken cancellationToken)
    {
        var blogPostFile = await blogPostFileRepository.GetWithTranslationsAsNoTrackingAsync(request.Id, cancellationToken);
        if (blogPostFile is null)
            return ErrorModel.Create("InvalidId");

        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var title = ResolveTitle(blogPostFile.Translations, languageId, defaultLanguage.Id);

        return mapper.Map(blogPostFile, title);
    }

    private static string ResolveTitle(
        IEnumerable<Store.Domain.Entities.BlogPostFileTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<Store.Domain.Entities.BlogPostFileTranslation> ?? translations.ToList();
        return list.FirstOrDefault(t => t.LanguageId == languageId)?.Title
            ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)?.Title
            ?? string.Empty;
    }
}
