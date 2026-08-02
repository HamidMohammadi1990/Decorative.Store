using Store.Domain.Common;

namespace Store.Domain.Entities;

public class FinancialYear : BaseEntity
{
    public string Name { get; set; } = default!;
    public DateTime StartDate { get; set; } = default!;
    public DateTime EndDate { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    public ICollection<FinancialDocument> FinancialDocuments { get; set; } = default!;

    public static FinancialYear Create(string name, DateTime startDate, DateTime endDate)
         => new()
         {
             Name = name,
             EndDate = endDate,
             StartDate = startDate
         };

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(string name, DateTime startDate, DateTime endDate, bool isActive)
    {
        Name = name;
        EndDate = endDate;
        IsActive = isActive;
        StartDate = startDate;
    }
}
