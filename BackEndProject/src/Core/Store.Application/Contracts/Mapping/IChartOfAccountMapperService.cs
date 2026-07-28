using Edition.Application.Features.ChartOfAccounts.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ChartOfAccounts;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IChartOfAccountMapperService : IMapper
{
    GetChartOfAccountResponse Map(ChartOfAccount model);
    GetAllChartOfAccountRequestDto Map(GetAllChartOfAccountRequest model);
    PagedResult<GetAllChartOfAccountResponse> Map(PagedResult<GetAllChartOfAccountDto> model);
}