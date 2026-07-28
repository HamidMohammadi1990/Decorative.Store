using Edition.Application.Features.FinancialYears.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.FinancialYears;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IFinancialYearMapperService : IMapper
{
    GetFinancialYearResponse Map(FinancialYear model);
    PagedResult<GetAllFinancialYearResponse> Map(PagedResult<FinancialYear> model);
    GetAllFinancialYearRequestDto Map(GetAllFinancialYearRequest model);
}