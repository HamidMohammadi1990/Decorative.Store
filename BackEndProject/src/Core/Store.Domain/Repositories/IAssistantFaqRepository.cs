using System.Linq.Expressions;
using Store.Domain.Dtos.AssistantFaqs;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IAssistantFaqRepository
{
    Task<PagedResult<GetAllAssistantFaqResponseDto>> GetAllAsync(GetAllAssistantFaqRequestDto request);
    Task<PagedResult<SearchAssistantFaqResponseDto>> SearchAsync(SearchAssistantFaqRequestDto request);
    void Add(AssistantFaq entity);
    Task<bool> AnyAsync(Expression<Func<AssistantFaq, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<AssistantFaq?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<AssistantFaq?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Remove(AssistantFaq entity);
}
