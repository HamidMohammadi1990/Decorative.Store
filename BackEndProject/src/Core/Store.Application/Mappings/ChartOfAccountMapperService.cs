using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ChartOfAccounts.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ChartOfAccounts;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ChartOfAccountMapperService : IChartOfAccountMapperService
{
    public GetChartOfAccountResponse Map(ChartOfAccount model)
    {
        return new GetChartOfAccountResponse
        {
            Id = model.Id,
            Level = model.Level,
            ParentId = model.ParentId,
            AccountTitle = model.AccountTitle,
            AccountType = model.AccountType,
            DetailType = model.AccountDetailType,
            AccountCode = model.AccountCode
        };
    }

    public PagedResult<GetAllChartOfAccountResponse> Map(PagedResult<GetAllChartOfAccountDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllChartOfAccountResponse
            {
                Id = x.Id,
                Level = x.Level,
                ParentId = x.ParentId,
                AccountType = x.AccountType,
                AccountCode = x.AccountCode,
                AccountTitle = x.AccountTitle,
                AccountDetailType = x.AccountDetailType
            })
            .ToList();

        return PagedResult<GetAllChartOfAccountResponse>.Create(items, model);
    }

    public GetAllChartOfAccountRequestDto Map(GetAllChartOfAccountRequest model)
    {
        return new GetAllChartOfAccountRequestDto
        {
            Level = model.Level,
            ParentId = model.ParentId,
            AccountCode = model.AccountCode,
            AccountType = model.AccountType,
            AccountTitle = model.AccountTitle,
            AccountDetailType = model.AccountDetailType,
            Pagination = model.Pagination
        }.WithContentPolicy<ChartOfAccount, GetAllChartOfAccountRequestDto>(model);
    }
}