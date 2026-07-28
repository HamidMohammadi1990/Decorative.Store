namespace Store.Domain.Entities;

public class CompanyProduct
{
    public int CompanyId { get; private set; }
    public int ProductId { get; private set; }


    public Company Company { get; set; } = default!;
    public Product Product { get; set; } = default!;
}