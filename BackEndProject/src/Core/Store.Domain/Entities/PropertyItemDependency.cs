namespace Store.Domain.Entities;

public class PropertyItemDependency
{
    public int ParentPropertyItemId { get; private set; }
    public int DependentPropertyItemId { get; private set; }


    public PropertyItem ParentPropertyItem { get; private set; } = default!;
    public PropertyItem DependentPropertyItem { get; private set; } = default!;
}