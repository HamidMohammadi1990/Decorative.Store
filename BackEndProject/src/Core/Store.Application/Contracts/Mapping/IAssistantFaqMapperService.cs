using Edition.Application.Features.AssistantFaqs.Queries;
using Store.Domain.Dtos.AssistantFaqs;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IAssistantFaqMapperService : IMapper
{
    GetAllAssistantFaqRequestDto Map(GetAllAssistantFaqRequest model);
    SearchAssistantFaqRequestDto Map(SearchAssistantFaqRequest model);
    PagedResult<GetAllAssistantFaqResponse> Map(PagedResult<GetAllAssistantFaqResponseDto> model);
    PagedResult<SearchAssistantFaqResponse> MapSearch(PagedResult<SearchAssistantFaqResponseDto> model);
    GetAssistantFaqResponse Map(AssistantFaq model);
}
