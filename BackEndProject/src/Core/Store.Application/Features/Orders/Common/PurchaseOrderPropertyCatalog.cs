using Store.Domain.Dtos.Products;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Common;

internal sealed class PurchaseOrderPropertyCatalog
{
    private readonly IReadOnlyList<ProductPropertyDto> rows;
    private readonly Dictionary<int, PropertyDefinition> definitions;
    private readonly Dictionary<int, HashSet<int>> itemIdsByPropertyId;

    private PurchaseOrderPropertyCatalog(
        IReadOnlyList<ProductPropertyDto> rows,
        Dictionary<int, PropertyDefinition> definitions,
        Dictionary<int, HashSet<int>> itemIdsByPropertyId)
    {
        this.rows = rows;
        this.definitions = definitions;
        MandatoryPropertyIds = [];
        this.itemIdsByPropertyId = itemIdsByPropertyId;
    }

    public HashSet<int> MandatoryPropertyIds { get; }

    public static PurchaseOrderPropertyCatalog Create(IReadOnlyList<ProductPropertyDto> rows)
    {
        var definitions = new Dictionary<int, PropertyDefinition>();
        var itemIdsByPropertyId = new Dictionary<int, HashSet<int>>();

        foreach (var group in rows.GroupBy(x => x.PropertyId))
        {
            var row = group.First();
            definitions[row.PropertyId] = new PropertyDefinition(false, row);
            AddItemIds(itemIdsByPropertyId, row.PropertyId, group.Select(x => x.PropertyItemId));
        }

        foreach (var group in rows.Where(x => x.ParentPropertyId.HasValue).GroupBy(x => x.ParentPropertyId!.Value))
        {
            var parentId = group.Key;
            if (definitions.ContainsKey(parentId))
                continue;

            var row = group.First();
            definitions[parentId] = new PropertyDefinition(true, row);
            AddItemIds(itemIdsByPropertyId, parentId, group.Select(x => x.ParentPropertyItemId));
        }

        return new PurchaseOrderPropertyCatalog(rows, definitions, itemIdsByPropertyId);
    }

    public bool TryGetDefinition(int propertyId, out PropertyDefinition definition)
        => definitions.TryGetValue(propertyId, out definition!);

    public bool IsValidItemId(int propertyId, int itemId)
        => itemIdsByPropertyId.TryGetValue(propertyId, out var itemIds) && itemIds.Contains(itemId);

    public ProductPropertyDto? FindItemRow(int propertyId, int itemId)
    {
        if (!TryGetDefinition(propertyId, out var definition))
            return null;

        return definition.IsParent
            ? rows.FirstOrDefault(x => x.ParentPropertyId == propertyId && x.ParentPropertyItemId == itemId)
            : rows.FirstOrDefault(x => x.PropertyId == propertyId && x.PropertyItemId == itemId);
    }

    private static void AddItemIds(
        Dictionary<int, HashSet<int>> itemIdsByPropertyId,
        int propertyId,
        IEnumerable<int?> itemIds)
    {
        foreach (var itemId in itemIds)
        {
            if (!itemId.HasValue)
                continue;

            if (!itemIdsByPropertyId.TryGetValue(propertyId, out var set))
            {
                set = [];
                itemIdsByPropertyId[propertyId] = set;
            }

            set.Add(itemId.Value);
        }
    }

    internal sealed class PropertyDefinition(bool isParent, ProductPropertyDto row)
    {
        public ProductPropertyDto Row { get; } = row;
        public bool IsParent { get; } = isParent;

        public PropertyType PropertyType
            => IsParent ? Row.ParentPropertyType ?? PropertyType.Boolean : Row.PropertyType;

        public bool IsMandatory => false;
    }
}
