using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Banks.Queries;
using Store.Domain.Dtos.Banks;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BankMapperService : IBankMapperService
{
    public PagedResult<GetAllBankResponse> Map(PagedResult<Bank> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBankResponse
            {
                Id = x.Id,
                Icon = x.Icon,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllBankResponse>.Create(items, model);
    }

    public PagedResult<SearchBankResponse> MapToSearch(PagedResult<Bank> model)
    {
        var items = model
            .Items
            .Select(x => new SearchBankResponse
            {
                Id = x.Id,
                Icon = x.Icon,
                Title = x.Title
            })
            .ToList();

        return PagedResult<SearchBankResponse>.Create(items, model);
    }

    public GetBankResponse Map(Bank model)
    {
        return new GetBankResponse
        {
            Id = model.Id,
            Icon = model.Icon,
            Title = model.Title,
            IsActive = model.IsActive
        };
    }

    public GetAllBankRequestDto Map(GetAllBankRequest model)
    {
        return new GetAllBankRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<Bank, GetAllBankRequestDto>(model);
    }

    public SearchBankRequestDto Map(SearchBankRequest model)
    {
        return new SearchBankRequestDto
        {
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<Bank, SearchBankRequestDto>(model);
    }
}
