using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ExpenseType : BaseEntity
{
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; } = default!;


    public ICollection<Expense> Expenses { get; set; } = default!;
}