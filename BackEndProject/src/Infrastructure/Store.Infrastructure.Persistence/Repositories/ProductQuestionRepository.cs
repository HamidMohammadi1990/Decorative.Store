using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductQuestions;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductQuestionRepository(EditionDbContext context)
    : Repository<ProductQuestion>(context), IProductQuestionRepository
{
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
}
