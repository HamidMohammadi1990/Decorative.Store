using Edition.Application.Contracts.Localization;
using Edition.Application.Common.Localization;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductPropertyRules.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.ProductPropertyRules;

namespace Edition.Application.Contracts.Mapping;

public interface IProductPropertyRuleMapperService : IMapper
{
    GetProductPropertyRuleResponse Map(ProductPropertyRule model, string? description);
    GetAllProductPropertyRuleRequestDto Map(GetAllProductPropertyRuleRequest model);
    SearchProductPropertyRuleRequestDto Map(SearchProductPropertyRuleRequest model);
    PagedResult<GetAllProductPropertyRuleResponse> Map(PagedResult<ProductPropertyRule> model, int languageId, int defaultLanguageId);
    PagedResult<SearchProductPropertyRuleResponse> MapToSearch(PagedResult<ProductPropertyRule> model, int languageId, int defaultLanguageId);
}
