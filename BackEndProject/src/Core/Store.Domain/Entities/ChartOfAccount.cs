using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class ChartOfAccount : BaseEntity
{
    public int Level { get; set; }
    public int? ParentId { get; private set; }
    public string AccountCode { get; private set; } = default!;
    public string AccountTitle { get; private set; } = default!;
    public ChartOfAccountType AccountType { get; private set; } = default!;
    public ChartOfAccountDetailType AccountDetailType { get; private set; } = default!;


    public ChartOfAccount Parent { get; private set; } = default!;
    public ICollection<ChartOfAccount> Children { get; private set; } = [];
    public ICollection<FinancialDocumentDetail> FinancialDocumentDetails { get; private set; } = default!;


    public static ChartOfAccount Create(int level, string code, string title, ChartOfAccountType type, ChartOfAccountDetailType detailType, int? parentId = null)
        => new()
        {
            Level = level,
            ParentId = parentId,
            AccountType = type,
            AccountCode = code,
            AccountTitle = title,
            AccountDetailType = detailType,
        };

    public void Update(int level, string code, string title, ChartOfAccountType type, ChartOfAccountDetailType detailType, int? parentId)
    {
        Level = level;
        ParentId = parentId;
        AccountType = type;
        AccountCode = code;
        AccountTitle = title;
        AccountDetailType = detailType;
    }

    public void AddChildren(ICollection<ChartOfAccount> childs)
    {
        foreach (var child in childs)
            Children.Add(child);
    }
}