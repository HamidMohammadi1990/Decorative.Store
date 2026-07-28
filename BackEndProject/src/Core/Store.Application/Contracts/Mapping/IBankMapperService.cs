using Edition.Application.Features.Banks.Queries;
using Store.Domain.Dtos.Banks;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IBankMapperService : IMapper
{
    GetBankResponse Map(Bank model);
    GetAllBankRequestDto Map(GetAllBankRequest model);
    SearchBankRequestDto Map(SearchBankRequest model);
    PagedResult<GetAllBankResponse> Map(PagedResult<Bank> model);
    PagedResult<SearchBankResponse> MapToSearch(PagedResult<Bank> model);
}
