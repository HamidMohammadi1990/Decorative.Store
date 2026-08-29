using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductQuestions;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductQuestionRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<ProductQuestion>(context), IProductQuestionRepository
{
    public async Task<PagedResult<GetAllProductQuestionResponseDto>> GetAllAsync(
        GetAllProductQuestionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var productQuestionSource = Context.ProductQuestion
            .ApplyContentPolicyFilter(request.ContentFilter);

        var productQuestions =
            from productQuestion in productQuestionSource
            join product in Context.Product on productQuestion.ProductId equals product.Id
            join user in Context.User on productQuestion.UserId equals user.Id
            join answeredBy in Context.User on productQuestion.AnsweredByUserId equals answeredBy.Id into answeredUsers
            from answeredBy in answeredUsers.DefaultIfEmpty()
            select new { productQuestion, product, user, answeredBy };

        productQuestions = productQuestions.ApplyQueryFilters(request);

        return await productQuestions
            .AsNoTracking()
            .OrderByDescending(x => x.productQuestion.CreatedOnUtc)
            .Select(x => new GetAllProductQuestionResponseDto
            {
                Id = x.productQuestion.Id,
                UserId = x.productQuestion.UserId,
                ProductId = x.productQuestion.ProductId,
                Question = x.productQuestion.Question,
                Answer = x.productQuestion.Answer,
                CreatedOnUtc = x.productQuestion.CreatedOnUtc,
                IsActive = x.productQuestion.IsActive,
                ProductTitle = x.product.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.product.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                AnsweredByUserName = x.answeredBy != null ? x.answeredBy.UserName : null,
                AnsweredByFirstName = x.answeredBy != null ? x.answeredBy.FirstName : null,
                AnsweredByLastName = x.answeredBy != null ? x.answeredBy.LastName : null,
            })
            .ToPagedAsync(request.Pagination, cancellationToken);
    }

    public async Task<PagedResult<SearchProductQuestionResponseDto>> SearchAsync(
        SearchProductQuestionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var query = Context.ProductQuestion
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive);

        if (request.ProductId.HasValue)
            query = query.Where(x => x.ProductId == request.ProductId.Value);

        var joined =
            from question in query
            join user in Context.User on question.UserId equals user.Id
            join answeredBy in Context.User on question.AnsweredByUserId equals answeredBy.Id into answeredUsers
            from answeredBy in answeredUsers.DefaultIfEmpty()
            orderby question.CreatedOnUtc descending
            select new SearchProductQuestionResponseDto
            {
                Id = question.Id,
                ProductId = question.ProductId,
                UserId = question.UserId,
                Question = question.Question,
                Answer = question.Answer,
                CreatedOnUtc = question.CreatedOnUtc,
                UserName = user.UserName,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                AnsweredByUserName = answeredBy != null ? answeredBy.UserName : null,
                AnsweredByFirstName = answeredBy != null ? answeredBy.FirstName : null,
                AnsweredByLastName = answeredBy != null ? answeredBy.LastName : null
            };

        return await joined.ToPagedAsync(request.Pagination, cancellationToken);
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
