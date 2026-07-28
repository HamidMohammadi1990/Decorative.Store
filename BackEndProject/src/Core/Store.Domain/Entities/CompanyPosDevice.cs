using Store.Domain.Common;

namespace Store.Domain.Entities;

public class CompanyPosDevice : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int CompanyId { get; private set; } = default!;
    public int BankId { get; private set; } = default!;
    public string IP { get; private set; } = default!;
    public DateTime CreationOnUtc { get; private set; } = DateTime.UtcNow;


    public Bank Bank { get; private set; } = default!;
    public Company Company { get; private set; } = default!;
    public ICollection<PosTransaction> PosTransactions { get; private set; } = default!;


    public static CompanyPosDevice Create(string name, string? description, int companyId, int bankId, string ip)
        => new()
        {
            IP = ip,
            Name = name,
            BankId = bankId,
            CompanyId = companyId,
            Description = description
        };

    public void Update(string name, string? description, bool isActive, int companyId, int bankId, string ip)
    {
        IP = ip;
        Name = name;
        BankId = bankId;
        IsActive = isActive;
        CompanyId = companyId;
        Description = description;
    }

    public void DeActive()
    {
        IsActive = false;
    }
}