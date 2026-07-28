using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductFile : BaseEntity
{
    public string Title { get; private set; } = null!;
    public int ProductId { get; private set; }
    public string FileName { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public bool IsMain { get; private set; }


    public Product Product { get; private set; } = default!;


    public static ProductFile Create(string title, int productId, string fileName, bool isMain)
        => new()
        {
            Title = title,
            ProductId = productId,
            FileName = fileName,
            IsMain = isMain
        };

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }
}