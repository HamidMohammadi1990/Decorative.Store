using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Currency : BaseEntity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public bool IsDefault { get; set; }

    public static Currency Create(string code, string name, bool isDefault)
        => new()
        {
            Code = code,
            Name = name,
            IsDefault = isDefault
        };
}