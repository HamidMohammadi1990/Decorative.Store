using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.CompanyPosDevices;

public record GetAllCompanyPosDeviceRequestDto : IContentPolicyQueryDto<CompanyPosDevice>
{
    [QueryFilter(MemberPath = "companyPosDevice.Name", Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter(MemberPath = "companyPosDevice.IsActive")]
    public bool? IsActive { get; init; } = true;

    [QueryFilter(MemberPath = "companyPosDevice.CompanyId")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "companyPosDevice.BankId")]
    public int? BankId { get; init; }

    [QueryFilter(MemberPath = "companyPosDevice.IP")]
    public string? IP { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyPosDevice, bool>>? ContentFilter { get; set; }
}
