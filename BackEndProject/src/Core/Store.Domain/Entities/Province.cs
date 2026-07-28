using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Province : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string? Description { get; private set; }
    public int Rate { get; private set; }
    public bool IsActive { get; private set; } = true;
    public float? Latitude { get; private set; }
    public float? Longitude { get; private set; }
    public string? TelPrefix { get; set; }


    public ICollection<City> Cities { get; private set; } = [];


    public static Province Create(string name, string slug, string? telPrefix, string? description, int rate, float? latitude, float? longitude)
        => new()
        {
            Name = name,
            Rate = rate,
            Slug = slug,
            Latitude = latitude,
            Longitude = longitude,
            TelPrefix = telPrefix,
            Description = description,
        };

    public void Update(string name, string slug, string? telPrefix, string? description, int rate, float? latitude, float? longitude)
    {
        Name = name;
        Rate = rate;
        Slug = slug;
        Latitude = latitude;
        Longitude = longitude;
        TelPrefix = telPrefix;
        Description = description;
    }

    public void AddCities(ICollection<City> cities)
    {
        foreach (var city in cities)
            Cities.Add(city);
    }

    public void DeActive()
    {
        IsActive = false;
    }
}