using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ChartOfAccounts;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ChartOfAccountRepository
    (EditionDbContext context)
    : Repository<ChartOfAccount>(context), IChartOfAccountRepository
{
    public async Task<PagedResult<GetAllChartOfAccountDto>> GetAllAsync(GetAllChartOfAccountRequestDto request)
    {
        var accounts = Context.ChartOfAccount
            .ApplyContentPolicyFilter(request.ContentFilter);

        var result =
            await accounts
            .Select(x => new GetAllChartOfAccountDto
            {
                Id = x.Id,
                Level = x.Level,
                ParentId = x.ParentId,
                AccountType = x.AccountType,
                AccountCode = x.AccountCode,
                AccountTitle = x.AccountTitle,
                AccountDetailType = x.AccountDetailType
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}