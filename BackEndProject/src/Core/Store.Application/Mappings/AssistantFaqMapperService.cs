using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.AssistantFaqs.Queries;
using Store.Domain.Dtos.AssistantFaqs;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class AssistantFaqMapperService : IAssistantFaqMapperService
{
    public GetAllAssistantFaqRequestDto Map(GetAllAssistantFaqRequest model)
        => new GetAllAssistantFaqRequestDto
        {
            LanguageId = model.LanguageId,
            Question = model.Question,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<AssistantFaq, GetAllAssistantFaqRequestDto>(model);

    public SearchAssistantFaqRequestDto Map(SearchAssistantFaqRequest model)
        => new()
        {
            LanguageId = model.LanguageId,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        };

    public PagedResult<GetAllAssistantFaqResponse> Map(PagedResult<GetAllAssistantFaqResponseDto> model)
    {
        var items = model.Items
            .Select(x => new GetAllAssistantFaqResponse
            {
                Id = x.Id,
                LanguageId = x.LanguageId,
                Question = x.Question,
                Answer = x.Answer,
                Priority = x.Priority,
                IsActive = x.IsActive,
            })
            .ToList();

        return PagedResult<GetAllAssistantFaqResponse>.Create(items, model);
    }

    public PagedResult<SearchAssistantFaqResponse> MapSearch(PagedResult<SearchAssistantFaqResponseDto> model)
    {
        var items = model.Items
            .Select(x => new SearchAssistantFaqResponse
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Priority = x.Priority,
                IsActive = x.IsActive,
            })
            .ToList();

        return PagedResult<SearchAssistantFaqResponse>.Create(items, model);
    }

    public GetAssistantFaqResponse Map(AssistantFaq model)
        => new()
        {
            Id = model.Id,
            LanguageId = model.LanguageId,
            Question = model.Question,
            Answer = model.Answer,
            Priority = model.Priority,
            IsActive = model.IsActive,
        };
}
