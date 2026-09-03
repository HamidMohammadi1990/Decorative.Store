namespace Store.Infrastructure.Persistence.SeedData;

internal static class RoomTypeSeedData
{
    internal sealed record RoomSeed(string Code, string ImagePath, string FaTitle, string EnTitle, int Priority);

    internal static readonly RoomSeed[] Items =
    [
        new("living", "/images/home/living-room.svg", "نشیمن", "Living room", 0),
        new("bedroom", "/images/home/bedroom.svg", "اتاق خواب", "Bedroom", 1),
        new("dining", "/images/home/dining.svg", "ناهارخوری", "Dining room", 2),
    ];
}
