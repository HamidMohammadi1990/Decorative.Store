using Edition.Application.Features.ProductQuestions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductQuestions;

namespace Edition.Application.Contracts.Mapping;

public interface IProductQuestionMapperService : IMapper
{
    SearchProductQuestionRequestDto Map(SearchProductQuestionRequest model);
    GetAllProductQuestionRequestDto Map(GetAllProductQuestionRequest model);
    PagedResult<SearchProductQuestionResponse> Map(PagedResult<SearchProductQuestionResponseDto> model);
    PagedResult<GetAllProductQuestionResponse> Map(PagedResult<GetAllProductQuestionResponseDto> model);
}
