using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.FinancialYears.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.FinancialYears;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class FinancialYearMapperService : IFinancialYearMapperService
{
    public GetAllFinancialYearRequestDto Map(GetAllFinancialYearRequest model)
    {
        return new GetAllFinancialYearRequestDto
        {
            Name = model.Name,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<FinancialYear, GetAllFinancialYearRequestDto>(model);
    }

    public GetFinancialYearResponse Map(FinancialYear model)
    {
        return new GetFinancialYearResponse
        {
            Id = model.Id,
            Name = model.Name,
            IsActive = model.IsActive,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };
    }

    public PagedResult<GetAllFinancialYearResponse> Map(PagedResult<FinancialYear> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllFinancialYearResponse
            {
                Id = x.Id,
                Name = x.Name,
                EndDate = x.EndDate,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                CreatedOnUtc = x.CreatedOnUtc
            })
            .ToList();

        return PagedResult<GetAllFinancialYearResponse>.Create(items, model);
    }
}
