using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Dtos.CompanyPosDevices;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CompanyPosDeviceRepository
    (EditionDbContext context)
    : Repository<CompanyPosDevice>(context), ICompanyPosDeviceRepository
{
    public async Task<PagedResult<GetAllCompanyPosDeviceDto>> GetAllAsync(GetAllCompanyPosDeviceRequestDto request)
    {
        var companyPosDeviceSource = Context.CompanyPosDevice
            .ApplyContentPolicyFilter(request.ContentFilter);

        var devices =
            from companyPosDevice in companyPosDeviceSource
            join company in Context.Company on companyPosDevice.CompanyId equals company.Id
            join bank in Context.Bank on companyPosDevice.BankId equals bank.Id
            select new { companyPosDevice, company, bank };

        devices = devices.ApplyQueryFilters(request);

        var result =
            await devices.Select(x => new GetAllCompanyPosDeviceDto
            {
                Id = x.companyPosDevice.Id,
                IP = x.companyPosDevice.IP,
                Name = x.companyPosDevice.Name,
                BankId = x.companyPosDevice.BankId,
                BankName = x.bank.Title,
                IsActive = x.companyPosDevice.IsActive,
                CompanyId = x.companyPosDevice.CompanyId,
                CompanyName = x.company.Name,
                Description = x.companyPosDevice.Description,
                CreationOnUtc = x.companyPosDevice.CreationOnUtc,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}