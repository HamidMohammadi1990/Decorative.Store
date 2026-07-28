using Store.Domain.Common;

namespace Store.Domain.Entities;

public class FinancialYear : BaseEntity
{
    public string Name { get; set; } = default!;
    public DateTime StartDate { get; set; } = default!;
    public DateTime EndDate { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public int CompanyId { get; set; } = default!;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;


    public Company Company { get; set; } = default!;
    public ICollection<FinancialDocument> FinancialDocuments { get; set; } = default!;


    public static FinancialYear Create(string name, DateTime startDate, DateTime endDate, int companyId)
         => new()
         {
             Name = name,
             EndDate = endDate,
             CompanyId = companyId,
             StartDate = startDate
         };

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(string name, DateTime startDate, DateTime endDate, bool isActive, int companyId)
    {
        Name = name;
        EndDate = endDate;
        IsActive = isActive;
        CompanyId = companyId;
        StartDate = startDate;
    }
}