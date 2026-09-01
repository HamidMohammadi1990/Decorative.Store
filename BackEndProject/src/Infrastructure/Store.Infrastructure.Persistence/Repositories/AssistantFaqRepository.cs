using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.AssistantFaqs;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class AssistantFaqRepository
    (EditionDbContext context)
    : Repository<AssistantFaq>(context), IAssistantFaqRepository
{
    public async Task<PagedResult<GetAllAssistantFaqResponseDto>> GetAllAsync(GetAllAssistantFaqRequestDto request)
    {
        var source = Context.AssistantFaq
            .ApplyContentPolicyFilter(request.ContentFilter);

        return await source
            .ApplyQueryFilters(request)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .AsNoTracking()
            .Select(x => new GetAllAssistantFaqResponseDto
            {
                Id = x.Id,
                LanguageId = x.LanguageId,
                Question = x.Question,
                Answer = x.Answer,
                Priority = x.Priority,
                IsActive = x.IsActive,
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchAssistantFaqResponseDto>> SearchAsync(SearchAssistantFaqRequestDto request)
    {
        var query = Context.AssistantFaq.AsQueryable();

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        query = query.Where(x => x.LanguageId == request.LanguageId);

        return await query
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .AsNoTracking()
            .Select(x => new SearchAssistantFaqResponseDto
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Priority = x.Priority,
                IsActive = x.IsActive,
            })
            .ToPagedAsync(request.Pagination);
    }
}
