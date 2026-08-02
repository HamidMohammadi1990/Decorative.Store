using Store.Domain.Common;

namespace Store.Domain.Entities;

public class City : BaseEntity
{
    public int ProvinceId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? Description { get; private set; }
    public int Rate { get; private set; }
    public bool IsActive { get; private set; } = true;
    public float? Latitude { get; private set; }
    public float? Longitude { get; private set; }


    public Province Province { get; set; } = default!;
    public ICollection<UserAddress> UserAddresses { get; set; } = default!;


    public static City Create(int provinceId, string name, string slug, string? description, int rate, float? latitude, float? longitude)
        => new()
        {
            Name = name,
            Slug = slug,
            Rate = rate,
            Latitude = latitude,
            Longitude = latitude,
            ProvinceId = provinceId,
            Description = description
        };

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(int provinceId, string name, string slug, string? description, int rate, float? latitude, float? longitude)
    {
        Name = name;
        Slug = slug;
        Rate = rate;
        Latitude = latitude;
        Longitude = latitude;
        ProvinceId = provinceId;
        Description = description;
    }
}