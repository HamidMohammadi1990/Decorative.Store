using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Category : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<SubCategory> SubCategories { get; set; } = [];


    public static Category Create(string title, string slug, string code)
        => new()
        {
            Title = title,
            Slug = slug,
            Code = code
        };

    public void Update(string title, string slug, string code, bool isActive)
    {
        Title = title;
        Slug = slug;
        Code = code;
        IsActive = isActive;
    }

    public void AddSubCategories(List<SubCategory> subCategories)
    {
        foreach (var subCategory in subCategories)
            SubCategories.Add(subCategory);
    }
}