using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Companies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CompanyRepository
    (EditionDbContext context)
    : Repository<Company>(context), ICompanyRepository
{
    public async Task<PagedResult<GetAllCompanyResponseDto>> GetAllAsync(GetAllCompanyRequestDto request)
    {
        var companySource = Context.Company
            .ApplyContentPolicyFilter(request.ContentFilter);

        var companies =
             from company in companySource
             join user in Context.User on company.UserId equals user.Id
             join city in Context.City on company.CityId equals city.Id
             join province in Context.Province on city.ProvinceId equals province.Id
             join companyProduct in Context.CompanyProduct on company.Id equals companyProduct.CompanyId
             select new { company, companyProduct, province, city, user };

        companies = companies.ApplyQueryFilters(request);

        var result =
            await companies
            .Select(x => new GetAllCompanyResponseDto
            {
                Id = x.company.Id,
                Name = x.company.Name,
                Code = x.company.Code,
                CityId = x.company.CityId,
                CityName = x.city.Name,
                IsActive = x.company.IsActive,
                Address = x.company.Address,
                Email = x.company.Email,
                Latitude = x.company.Latitude,
                Longitude = x.company.Longitude,
                PhoneNumber = x.company.PhoneNumber,
                PostalCode = x.company.PostalCode,
                ProvinceId = x.province.Id,
                ProvinceName = x.province.Name,
                UserId = x.company.UserId,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                Description = x.company.Description,
                CreatedOnUtc = x.company.CreatedOnUtc,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchCompanyResponseDto>> SearchAsync(SearchCompanyRequestDto request)
    {
        var companySource = Context.Company
            .ApplyContentPolicyFilter(request.ContentFilter);

        var companies =
             from company in companySource
             join user in Context.User on company.UserId equals user.Id
             join city in Context.City on company.CityId equals city.Id
             join province in Context.Province on city.ProvinceId equals province.Id
             join companyProduct in Context.CompanyProduct on company.Id equals companyProduct.CompanyId
             where company.IsActive
             select new { company, companyProduct, province, city, user };

        companies = companies.ApplyQueryFilters(request);

        var result =
            await companies
            .Select(x => new SearchCompanyResponseDto
            {
                Id = x.company.Id,
                Name = x.company.Name,
                Code = x.company.Code,
                CityId = x.company.CityId,
                CityName = x.city.Name,
                Address = x.company.Address,
                Email = x.company.Email,
                Latitude = x.company.Latitude,
                Longitude = x.company.Longitude,
                PhoneNumber = x.company.PhoneNumber,
                PostalCode = x.company.PostalCode,
                ProvinceId = x.province.Id,
                ProvinceName = x.province.Name,
                UserId = x.company.UserId,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                Description = x.company.Description,
                CreatedOnUtc = x.company.CreatedOnUtc,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}