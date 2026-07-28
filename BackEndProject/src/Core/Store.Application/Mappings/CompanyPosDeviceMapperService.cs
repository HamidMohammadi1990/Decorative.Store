using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.CompanyPosDevices.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyPosDevices;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CompanyPosDeviceMapperService : ICompanyPosDeviceMapperService
{
    public GetCompanyPosDeviceResponse Map(CompanyPosDevice model)
    {
        return new GetCompanyPosDeviceResponse
        {
            Id = model.Id,
            IP = model.IP,
            Name = model.Name,
            BankId = model.BankId,
            IsActive = model.IsActive,
            CompanyId = model.CompanyId,
            Description = model.Description,
            CreationOnUtc = model.CreationOnUtc
        };
    }

    public GetAllCompanyPosDeviceRequestDto Map(GetAllCompanyPosDeviceRequest model)
    {
        return new GetAllCompanyPosDeviceRequestDto
        {
            IP = model.IP,
            Name = model.Name,
            BankId = model.BankId,
            IsActive = model.IsActive,
            CompanyId = model.CompanyId,
            Pagination = model.Pagination
        }.WithContentPolicy<CompanyPosDevice, GetAllCompanyPosDeviceRequestDto>(model);
    }

    public PagedResult<GetAllCompanyPosDeviceResponse> Map(PagedResult<GetAllCompanyPosDeviceDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCompanyPosDeviceResponse
            {
                Id = x.Id,
                IP = x.IP,
                Name = x.Name,
                BankId = x.BankId,
                IsActive = x.IsActive,
                CompanyId = x.CompanyId,
                BankName = x.BankName,
                CompanyName = x.CompanyName,
                Description = x.Description,
                CreationOnUtc = x.CreationOnUtc
            })
            .ToList();

        return PagedResult<GetAllCompanyPosDeviceResponse>.Create(items, model);
    }
}