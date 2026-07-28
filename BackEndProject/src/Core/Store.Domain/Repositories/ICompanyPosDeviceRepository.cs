using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyPosDevices;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICompanyPosDeviceRepository
{
    void Add(CompanyPosDevice companyPosDevice);
    Task<CompanyPosDevice?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<CompanyPosDevice?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCompanyPosDeviceDto>> GetAllAsync(GetAllCompanyPosDeviceRequestDto request);
}